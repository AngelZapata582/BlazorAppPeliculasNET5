function pruebaNetStatic() {
    DotNet.invokeMethodAsync("BlazorAppPeliculasNET5.Client", "ObtenerCurrentCount")
        .then(resultado => {
            console.log('contenido desde javascript ' + resultado);
        })
}

function pruebaNetInstancia(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("IncrementCount");
}