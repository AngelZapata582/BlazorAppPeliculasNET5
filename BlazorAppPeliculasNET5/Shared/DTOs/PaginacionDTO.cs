using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Shared.DTOs
{
    public class PaginacionDTO
    {
        public int pagina { get; set; } = 1;
        public int registros { get; set; } = 1;
    }
}
