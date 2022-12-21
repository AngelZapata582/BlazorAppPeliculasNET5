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
	}
}
