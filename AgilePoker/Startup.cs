using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(AgilePoker.Startup))]
namespace AgilePoker
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}