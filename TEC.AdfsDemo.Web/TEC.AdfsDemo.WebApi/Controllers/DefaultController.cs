using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TEC.Internal.Utils.WebApi.Attributes;

namespace TEC.AdfsDemo.WebApi.Controllers
{
    public class DefaultController : ApiController
    {
        [OAuthAuthorize]
        public void index() {

        }
    }
}
