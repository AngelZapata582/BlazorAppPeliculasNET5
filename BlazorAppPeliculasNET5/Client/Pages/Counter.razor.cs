using BlazorAppPeliculasNET5.Client.Repositorios;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Pages
{
	public partial class Counter
	{
		private int currentCount = 0;

		public  void IncrementCount()
		{
			currentCount++;
		}

	}
}
