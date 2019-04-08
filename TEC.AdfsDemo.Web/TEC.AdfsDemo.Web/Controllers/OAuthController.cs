using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TEC.AdfsDemo.Web.Enums;
using TEC.AdfsDemo.Web.Models.Shared;
using TEC.AdfsDemo.Web.Settings;

namespace TEC.AdfsDemo.Web.Controllers
{
    public class OAuthController : Controller
    {
        /// <summary>
        /// OAuth 登入後重新導向位址
        /// </summary>
        /// <param name="code">授權碼</param>
        /// <param name="error">錯誤</param>
        /// <param name="error_description">錯誤描述</param>
        /// <param name="resource"></param>
        /// <param name="state">如果請求中包含 state 參數，響應中就應該出現相同的值。應用程式最好在使用回應之前確認要求和回應中的狀態值完全相同。這有助於偵測對用戶端發動的 跨網站偽造要求 (CSRF) 攻擊 。</param>
        /// <returns></returns>
        public async Task<ActionResult> Login(string code, string error, string error_description, string resource, string state)
        {
            // Redeem the authorization code from the response for an access token and refresh token.
            if (String.IsNullOrWhiteSpace(code))
            {
                ErrorModel errorModel = new ErrorModel();
                if (String.Compare(error, "access_denied", true) == 0)
                {
                    errorModel.Title = "存取被拒";
                    errorModel.MessageHtml = "此帳號沒有權限存取[EPP 易點]後台，請更換帳號後重新嘗試。";
                }
                else
                {
                    errorModel.Title = error;
                    errorModel.MessageHtml = error_description;
                }
                return this.View("~/Views/Shared/Error.cshtml", errorModel);
            }

            ClientCredential credential = new ClientCredential(
                SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.ClientId].ToString(),
                SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.AppKey].ToString()
                );
            AuthenticationContext authContext = new AuthenticationContext(SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.Authority].ToString(), false);
            AuthenticationResult authenticationResult = await authContext.AcquireTokenByAuthorizationCodeAsync(
                code, new Uri(base.Request.Url.GetLeftPart(UriPartial.Path)), credential, SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.GraphResourceId].ToString());
            var authenticationTicket = Startup.OAuthBearerAuthenticationOptions.AccessTokenFormat.Unprotect(authenticationResult.AccessToken);
            authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("AccessToken", authenticationResult.AccessToken));
            authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("IdToken", authenticationResult.IdToken));
            authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("TokenCache", Convert.ToBase64String(authContext.TokenCache.Serialize())));

            var owinContext = base.Request.GetOwinContext();
            owinContext.Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalBearer);
            var applicationCookieIdentity = new ClaimsIdentity(authenticationTicket.Identity.Claims, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
            owinContext.Authentication.SignIn(applicationCookieIdentity);

            return RedirectToAction("RequiredAuthorizationPage", new RouteValueDictionary(new { controller = "Home", action = "RequiredAuthorizationPage", state = state }));
        }
        /// <summary>
        /// OAuth 登出後重新導向位址
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            return base.RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        }
        /// <summary>
        /// 導向 OAuth 登入頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectToSignInPage()
        {
            AuthenticationContext authContext = new AuthenticationContext(SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.Authority].ToString(), false);
            Uri redirectUri = new Uri(base.Request.Url.GetLeftPart(UriPartial.Authority).ToString() + "/OAuth/Login");
            Uri authorizationUrl = authContext.GetAuthorizationRequestUrlAsync(
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.GraphResourceId].ToString(),
                  SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.ClientId].ToString(), redirectUri, UserIdentifier.AnyUser, null).Result;
            return base.Redirect(authorizationUrl.AbsoluteUri);
        }
        /// <summary>
        /// 導向 OAuth 登出頁面
        /// </summary>
        /// <param name="redirectPath">OAuth 登出後重新導向的網址</param>
        /// <returns></returns>
        public ActionResult RedirectToSignOutPage(string redirectPath = "/OAuth/Logout")
        {
            base.Response.Buffer = true;
            var authenticationManager = base.HttpContext.GetOwinContext().Authentication;
            string idToken = authenticationManager.User.FindFirst("IdToken")?.Value;
            authenticationManager.SignOut(base.HttpContext.GetOwinContext().Authentication.GetAuthenticationTypes().Select(t => t.AuthenticationType).ToArray());
            string siteUrl = base.Request.Url.GetLeftPart(UriPartial.Authority).ToString();
            string logoutUrlFormat = SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.LogoutUrlFormat].ToString();

            return base.Redirect(String.Format(logoutUrlFormat, base.Url.Encode(siteUrl + redirectPath), idToken));
        }
        /// <summary>
        /// 連接至 OAuth IDP ，並取得重新要求 Access Token 的 URL 位址
        /// </summary>
        /// <returns></returns>
        [Authorize, HttpPost]
        public async Task<ActionResult> GetRefreshTokenUrl()
        {
            AuthenticationContext authContext = new AuthenticationContext(SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.Authority].ToString(), false);

            Uri redirectUri = new Uri(base.Request.Url.GetLeftPart(UriPartial.Authority).ToString() + "/OAuth/Login");
            Uri authorizationUrl = await authContext.GetAuthorizationRequestUrlAsync(
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.GraphResourceId].ToString(),
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.ClientId].ToString(), redirectUri, UserIdentifier.AnyUser, null);
            return this.Content(authorizationUrl.AbsoluteUri);
        }
    }
}