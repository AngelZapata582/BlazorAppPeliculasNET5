using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Shared.DTOs
{
    public class EditarUser
    {
        public string UsuarioId { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
