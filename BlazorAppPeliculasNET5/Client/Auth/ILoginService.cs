using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Auth
{
    public interface ILoginService
    {
        Task Login(string token);
        Task Logout();
    }
}
