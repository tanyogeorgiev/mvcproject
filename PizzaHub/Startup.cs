using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PizzaHub.Startup))]
namespace PizzaHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
