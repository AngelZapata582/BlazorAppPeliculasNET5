using BlazorAppPeliculasNET5.Client.Repositorios;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Pages
{
	public partial class Counter
	{
		private int currentCount = 0;
		[Inject] public IJSRuntime js { get; set; } = null!;
		[CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

		public  async void IncrementCount()
		{
			var authState = await authenticationState;
			var user = authState.User;
			if (user.Identity.IsAuthenticated)
			{
				currentCount++;
			}
			else
			{
				currentCount--;
			}
		}

	}
}
