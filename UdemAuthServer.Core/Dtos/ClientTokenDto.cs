using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemAuthServer.Core.Dtos
{
    public class ClientTokenDto
    {
        public string AccesToken { get; set; }
        public DateTime AccesTokenExpiration { get; set; }
    }
}
