using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UdemAuthServer.Core.Configuration;
using UdemAuthServer.Core.Dtos;
using UdemAuthServer.Core.Models;
using UdemAuthServer.Core.Services;

namespace UdemyAuthServer.Service.Services
{
   
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOptions _customTokenOptions;

        public TokenService(IOptions<CustomTokenOptions> customTokenOptions, UserManager<UserApp> userManager)
        {
            _customTokenOptions = customTokenOptions.Value;
            _userManager = userManager;
        }
        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private IEnumerable<Claim> GetClaims(UserApp userApp,List<String> audiences)
        {
            var userList = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };

            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());
            return claims;
        }
        public TokenDto CreateToken(UserApp userApp)
        {
            var accesTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccesTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricKey(_customTokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: _customTokenOptions.Issuer,
                expires: accesTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(userApp, _customTokenOptions.Audience),
                signingCredentials: signingCredentials

                );
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new TokenDto
            {
                AccesToken = token
                ,
                RefreshToken = CreateRefreshToken(),
                AccesTokenExpiration = accesTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            return tokenDto;
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accesTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccesTokenExpiration);
           
            var securityKey = SignService.GetSymmetricKey(_customTokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: _customTokenOptions.Issuer,
                expires: accesTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials

                ); ;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new ClientTokenDto
            {
                AccesToken = token
                ,
                
                AccesTokenExpiration = accesTokenExpiration,
                
            };
            return tokenDto;
        }
    }
}
