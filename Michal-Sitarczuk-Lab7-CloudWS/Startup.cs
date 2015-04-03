using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Michal_Sitarczuk_Lab7_CloudWS.Startup))]
namespace Michal_Sitarczuk_Lab7_CloudWS
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
