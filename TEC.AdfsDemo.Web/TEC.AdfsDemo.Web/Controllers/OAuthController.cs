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
        public ActionResult RedirectToSignInPage()
        {
            AuthenticationContext authContext = new AuthenticationContext(SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.Authority].ToString(), false);
            Uri redirectUri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority).ToString() + "/OAuth/Login");
            Uri authorizationUrl = authContext.GetAuthorizationRequestUrlAsync(
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.GraphResourceId].ToString(),
                  SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.ClientId].ToString(), redirectUri, UserIdentifier.AnyUser, null).Result;
            return base.Redirect(authorizationUrl.AbsoluteUri);
        }
        public async Task<ActionResult> Login(string code, string error, string error_description, string resource, string state)
        {
            // Redeem the authorization code from the response for an access token and refresh token.
            if (String.IsNullOrWhiteSpace(code))
            {
                ErrorModel errorModel = new ErrorModel();
                if (String.Compare(error, "access_denied", true) == 0)
                {
                    errorModel.Title = "存取被拒";
                    errorModel.MessageHtml = "此帳號沒有權限存取網站，請更換帳號後重新嘗試。";

                }
                else
                {
                    errorModel.Title = error;
                    errorModel.MessageHtml = error_description;
                }
                return this.View("~/Views/Shared/Error.cshtml", errorModel);
            }
            try
            {
                ClientCredential credential = new ClientCredential(
                    SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.ClientId].ToString(),
                    SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.AppKey].ToString());
                AuthenticationContext authContext = new AuthenticationContext(SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.Authority].ToString(), false);
                AuthenticationResult authenticationResult = await authContext.AcquireTokenByAuthorizationCodeAsync(
                    code, new Uri(Request.Url.GetLeftPart(UriPartial.Path)), credential, SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.GraphResourceId].ToString());
                var authenticationTicket = Startup.OAuthBearerAuthenticationOptions.AccessTokenFormat.Unprotect(authenticationResult.AccessToken);
                authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("AccessToken", authenticationResult.AccessToken));
                authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("IdToken", authenticationResult.IdToken));
                authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("TokenCache", Convert.ToBase64String(authContext.TokenCache.Serialize())));

                var ctx = base.Request.GetOwinContext();
                ctx.Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalBearer);
                var applicationCookieIdentity = new ClaimsIdentity(authenticationTicket.Identity.Claims, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                ctx.Authentication.SignIn(applicationCookieIdentity);

                // Return to the originating page where the user triggered the sign-in
                return RedirectToAction("RequiredAuthorizationPage", new RouteValueDictionary(new { controller = "Home", action = "RequiredAuthorizationPage", state = state }));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult SignOut(string redirectPath = "/OAuth2/SignedOut")
        {
            base.Response.Buffer = true;
            var authenticationManager = base.HttpContext.GetOwinContext().Authentication;
            string idToken = authenticationManager.User.FindFirst("IdToken")?.Value;
            authenticationManager.SignOut(base.HttpContext.GetOwinContext().Authentication.GetAuthenticationTypes().Select(t => t.AuthenticationType).ToArray());
            string siteUrl = Request.Url.GetLeftPart(UriPartial.Authority).ToString();
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
            Uri redirectUri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority).ToString() + "/OAuth/Login");
            Uri authorizationUrl = await authContext.GetAuthorizationRequestUrlAsync(
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.GraphResourceId].ToString(),
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.ClientId].ToString(), redirectUri, UserIdentifier.AnyUser, null);
            return this.Content(authorizationUrl.AbsoluteUri);
        }
    }
}