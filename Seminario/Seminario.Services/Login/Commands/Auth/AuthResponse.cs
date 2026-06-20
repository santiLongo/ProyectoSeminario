using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminario.Services.Login.Response
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Token { get; set; }
    }
}
