using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TEC.AdfsDemo.Web.Enums;
using TEC.AdfsDemo.Web.Settings;
using TEC.Internal.Utils.Core.Info;

namespace TEC.AdfsDemo.Web.Attributes
{
    /// <summary>
    /// 驗證目前登入者的 Access Token 是否即將過期，若即將過期，則自動更新 AccessToken，更新失敗時會將回應資料的 Header 加上需要更新 Access Token 的欄位
    /// </summary>
    public class NotifyRefreshAccessTokenAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 初始化自動更新 AccessToken 失敗時，會將回應資料的 Header 加上需要更新 Access Token 的欄位
        /// </summary>
        public NotifyRefreshAccessTokenAttribute() : base()
        {
            base.Order = 0;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //有認證成功才需要 Access Token 的檢核
                TEC.Internal.Utils.Mvc.Controllers.ControllerBase controllerBase = filterContext.Controller as TEC.Internal.Utils.Mvc.Controllers.ControllerBase;

                //取得宣告資料，接著準備檢查有效期
                AccountCliamsInfo accountCliamsInfo = controllerBase.AccountCliamsInfo;
                DateTime expiredDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(accountCliamsInfo.Exp).ToLocalTime();
                //過期五分鐘內，自動更新 AccessToken
                if (DateTime.Now.AddMinutes(5).CompareTo(expiredDateTime) >= 0)
                {
                    try
                    {
                        ClaimsIdentity currentClaimsIdentity = controllerBase.User.Identity as ClaimsIdentity;
                        Claim tokenCacheClaim = currentClaimsIdentity.Claims.SingleOrDefault(t => t.Type == "TokenCache");
                        if (tokenCacheClaim != null)
                        {
                            ClientCredential credential = new ClientCredential(
                              SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.ClientId].ToString(),
                              SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.AppKey].ToString());
                            TokenCache tokenCache = TokenCache.DefaultShared;
                            tokenCache.Deserialize(Convert.FromBase64String(tokenCacheClaim.Value));
                            //移除舊的快取資料
                            var tokenCacheItems = tokenCache.ReadItems().ToList();
                            if (tokenCache.Count > 1)
                            {
                                tokenCache.ReadItems().OrderByDescending(t => t.ExpiresOn).Skip(1).ToList().ForEach(t => tokenCache.DeleteItem(t));
                            }
                            AuthenticationContext authContext = new AuthenticationContext(SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.Authority].ToString(), false, tokenCache);
                            var authenticationResultTask = authContext.AcquireTokenSilentAsync(SettingCollectionFactoryInternal.DemoWebSettingCollection[DemoWebSettingEnum.GraphResourceId].ToString(), credential, new UserIdentifier(accountCliamsInfo.UPN, UserIdentifierType.RequiredDisplayableId)).ConfigureAwait(false);
                            AuthenticationResult authenticationResult = authenticationResultTask.GetAwaiter().GetResult();
                            var authenticationTicket = Startup.OAuthBearerAuthenticationOptions.AccessTokenFormat.Unprotect(authenticationResult.AccessToken);
                            authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("AccessToken", authenticationResult.AccessToken));
                            authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("IdToken", authenticationResult.IdToken));
                            authenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("TokenCache", Convert.ToBase64String(authContext.TokenCache.Serialize())));
                            #region 目前階段
                            currentClaimsIdentity.TryRemoveClaim(currentClaimsIdentity.FindFirst("AccessToken"));
                            currentClaimsIdentity.TryRemoveClaim(currentClaimsIdentity.FindFirst("IdToken"));
                            currentClaimsIdentity.TryRemoveClaim(currentClaimsIdentity.FindFirst("TokenCache"));
                            currentClaimsIdentity.AddClaim(new System.Security.Claims.Claim("AccessToken", authenticationResult.AccessToken));
                            currentClaimsIdentity.AddClaim(new System.Security.Claims.Claim("IdToken", authenticationResult.IdToken));
                            currentClaimsIdentity.AddClaim(new System.Security.Claims.Claim("TokenCache", Convert.ToBase64String(authContext.TokenCache.Serialize())));
                            #endregion
                            var ctx = controllerBase.Request.GetOwinContext();
                            ctx.Authentication.AuthenticationResponseGrant = new Microsoft.Owin.Security.AuthenticationResponseGrant(new ClaimsIdentity(authenticationTicket.Identity.Claims, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie), new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = true });
                            controllerBase.overrideAccountCliamsInfo(authenticationTicket.Identity);
                        }
                    }
                    catch
                    {
                        //因為無法自動更新 Token，要讓使用者手動更新
                        // 前端收到本 Header 後，應該要引導使用者重新登入
                        filterContext.HttpContext.Response.Headers.Add("AccessTokenExpired", "true");
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
