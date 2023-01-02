using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Shared.DTOs
{
    public class UserToken
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
