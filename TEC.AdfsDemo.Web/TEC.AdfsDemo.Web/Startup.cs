using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TEC.AdfsDemo.Web.Startup))]
namespace TEC.AdfsDemo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}