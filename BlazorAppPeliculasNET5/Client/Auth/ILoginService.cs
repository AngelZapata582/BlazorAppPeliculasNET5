using BlazorAppPeliculasNET5.Shared.DTOs;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Auth
{
    public interface ILoginService
    {
        Task Login(UserToken tokenDTO);
        Task Logout();
        Task ManejarRenovacion();
    }
}
