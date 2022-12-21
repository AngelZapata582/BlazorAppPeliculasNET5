namespace BlazorAppPeliculasNET5.Client
{
	public class ServicioSingleton
	{
		//se mantiene vivo en la sesion de la pestaña del usuario
		//si se cambia de componente sigue vivo pero si se duplica la pestaña no influye el nuevo singleton en el anterior
		public int Valor { get; set; } //se mantiene vivo en la sesion del usuario
	}

	public class ServicioTransient
	{
		//cada cambio de pestaña o componente se crea una nueva isntancia del servicio
		public int Valor { set; get; } //se genera diferente en cada instancia
	}
}
