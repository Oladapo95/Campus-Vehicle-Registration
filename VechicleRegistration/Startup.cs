using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VechicleRegistration.Startup))]
namespace VechicleRegistration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
