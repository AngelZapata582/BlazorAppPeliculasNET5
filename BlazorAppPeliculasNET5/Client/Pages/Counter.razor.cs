using BlazorAppPeliculasNET5.Client.Repositorios;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Pages
{
	public partial class Counter
	{
		[Inject] ServicioSingleton singleton { get; set; } = null!;
		[Inject] ServicioTransient transient { get; set; } = null!;
		[Inject] IJSRuntime js { get; set; } = null!;
		private int currentCount = 0;
		private static int CurrentCountStatic = 0;
		IJSObjectReference modulo;

		[JSInvokable]
		public  async Task IncrementCount()
		{
			modulo = await js.InvokeAsync<IJSObjectReference>("import", "./js/CounterJs.js");
			currentCount++;
			CurrentCountStatic = currentCount;
			singleton.Valor = currentCount;
			transient.Valor = currentCount;
			await js.InvokeVoidAsync("pruebaNetStatic");
			await modulo.InvokeVoidAsync("mostrarAlerta",$"Cuanta en: {currentCount}");
		}

		private async Task IncrementCountJavaScript()
		{
			await js.InvokeVoidAsync("pruebaNetInstancia",DotNetObjectReference.Create(this));
		}

		[JSInvokable]
		public static Task<int> ObtenerCurrentCount()
		{
			return Task.FromResult(CurrentCountStatic);
		}
	}
}
