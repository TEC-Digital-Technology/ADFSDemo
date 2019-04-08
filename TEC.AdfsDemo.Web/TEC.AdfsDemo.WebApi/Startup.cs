using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(TEC.AdfsDemo.WebApi.Startup))]
namespace TEC.AdfsDemo.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureOAuth(app);
        }
    }
}