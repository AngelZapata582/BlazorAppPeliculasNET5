using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Helpers
{
    public static class HttpContextExtension
    {
        public async static Task InsertarParametrosPaginacionRespuesta<T> (
            this HttpContext context, IQueryable<T> queryable, int cantidad)
        {
            if(context is null) { throw new ArgumentNullException(nameof(context)); }
            double conteo = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(conteo / cantidad);
            context.Response.Headers.Add("conteo", conteo.ToString());
            context.Response.Headers.Add("paginas", totalPaginas.ToString());
        }
    }
}
