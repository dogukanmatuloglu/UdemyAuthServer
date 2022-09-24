using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemAuthServer.Core.Configuration;
using UdemAuthServer.Core.Dtos;
using UdemAuthServer.Core.Models;

namespace UdemAuthServer.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto CreateTokenByClient(Client client)
    }
}
