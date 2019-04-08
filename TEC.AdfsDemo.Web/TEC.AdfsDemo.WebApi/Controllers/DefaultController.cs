using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TEC.Internal.Utils.Core.Info;
using TEC.Internal.Utils.WebApi.Attributes;

namespace TEC.AdfsDemo.WebApi.Controllers
{
    public class DefaultController : TEC.Internal.Utils.WebApi.Controller.ApiControllerBase
    {
        [OAuthAuthorize]
        public void index()
        {
            //取得帳號資料
            AccountCliamsInfo accountCliamsInfo = base.AccountCliamsInfo;
        }
    }
}
