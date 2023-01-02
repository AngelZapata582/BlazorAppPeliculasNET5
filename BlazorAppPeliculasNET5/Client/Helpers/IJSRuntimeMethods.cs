using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Helpers
{
	public static class IJSRuntimeMethods
	{
		public static async ValueTask<bool> Confirm(this IJSRuntime js, string mensaje)
		{
			return await js.InvokeAsync<bool>("confirm", mensaje);
		}

		public static ValueTask<object> SetItemLocalStorage(this IJSRuntime js, string key, string content)
		=> js.InvokeAsync<object>("localStorage.setItem", key, content);

		public static ValueTask<string> GetItemLocalStorage(this IJSRuntime js, string key)
		=> js.InvokeAsync<string>("localStorage.getItem", key);

        public static ValueTask<string> RemoveItemLocalStorage(this IJSRuntime js, string key)
        => js.InvokeAsync<string>("localStorage.removeItem", key);
    }
}
