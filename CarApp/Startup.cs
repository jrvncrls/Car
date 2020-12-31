using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarApp.Startup))]
namespace CarApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
