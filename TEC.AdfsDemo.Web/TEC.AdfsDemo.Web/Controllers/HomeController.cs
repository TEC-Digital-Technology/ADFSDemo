using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEC.AdfsDemo.Web.Attributes;

namespace TEC.AdfsDemo.Web.Controllers
{
    [NotifyRefreshAccessToken]
    public class HomeController : TEC.Internal.Utils.Mvc.Controllers.ControllerBase
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<ActionResult> RequiredAuthorizationPage()
        {
            //取得登入者帳號資料
            var currentUserData = base.AccountCliamsInfo;

            //TEC.Internal.Utils.Core.ExternalDataSource.AccountService.AccountApiHandler accountApiHandler = new Internal.Utils.Core.ExternalDataSource.AccountService.AccountApiHandler();
            ////取得帳號資料，須先開通帳號使用權限後，才得以呼叫成功，否則會出現 0006 錯誤代號。
            //var accountInfo = await accountApiHandler.getAccountInfoByEmailAsync("Test03@tecyt.com");

            return View();
        }
    }
}