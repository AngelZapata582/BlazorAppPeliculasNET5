function pruebaNetStatic() {
    DotNet.invokeMethodAsync("BlazorAppPeliculasNET5.Client", "ObtenerCurrentCount")
        .then(resultado => {
            console.log('contenido desde javascript ' + resultado);
        })
}

function pruebaNetInstancia(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("IncrementCount");
}

function timerInactivo(dotnetHelper) {
    var timer;
    //reinicia el timer al mover el mouse o al presionar una tecla
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, 1000 * 60 * 30);
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}