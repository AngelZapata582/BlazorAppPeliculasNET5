using System;
using System.Timers;

namespace BlazorAppPeliculasNET5.Client.Auth
{
    public class RenovadorToken : IDisposable
    {
        private readonly ILoginService loginService;
        private Timer timer;
        
        public RenovadorToken(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public void Iniciar()
        {
            timer = new Timer();
            timer.Interval = 1000 * 60 * 50;
            timer.Elapsed += Timer_Elasped;
            timer.Start();
        }

        private void Timer_Elasped(object? sender, ElapsedEventArgs e)
        {
            loginService.ManejarRenovacion();
        }
        public void Dispose()
        {
            timer.Dispose();
        }
    }
}
