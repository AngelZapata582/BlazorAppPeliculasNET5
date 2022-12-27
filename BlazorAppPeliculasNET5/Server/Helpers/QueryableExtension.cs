using BlazorAppPeliculasNET5.Shared.DTOs;
using System.Linq;

namespace BlazorAppPeliculasNET5.Server.Helpers
{
    public static class QueryableExtension
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacion)
        {
            return queryable.Skip((paginacion.pagina - 1) * paginacion.registros)
                .Take(paginacion.registros);
        }
    }
}
