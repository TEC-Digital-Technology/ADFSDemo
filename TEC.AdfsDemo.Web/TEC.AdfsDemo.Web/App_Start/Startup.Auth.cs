using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web;
using TEC.AdfsDemo.Web.Settings;
using TEC.Internal.Utils.Core.Info.ExternalDataSource;
using TEC.Internal.Utils.Core.Security;

namespace TEC.AdfsDemo.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/OAuth2/SignIn"),
            });

            OAuthBearerAuthenticationOptions oAuthBearerAuthenticationOptions = Startup.OAuthBearerAuthenticationOptions;
            oAuthBearerAuthenticationOptions.AuthenticationMode = AuthenticationMode.Passive;
            oAuthBearerAuthenticationOptions.AuthenticationType = DefaultAuthenticationTypes.ExternalBearer;
            app.UseOAuthBearerAuthentication(oAuthBearerAuthenticationOptions);
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();
            #region Utils Config
            Internal.Utils.Core.UtilsCoreConfig.configUtil(accountServiceConfigInfo: new AccountServiceConfigInfo(
                 ((Uri)SettingCollectionFactoryInternal.DemoWebSettingCollection[Enums.DemoWebSettingEnum.AccountServiceApiBaseLocation]).AbsoluteUri,
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[Enums.DemoWebSettingEnum.PlatformName].ToString(),
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[Enums.DemoWebSettingEnum.AES256Key].ToString(),
                 SettingCollectionFactoryInternal.DemoWebSettingCollection[Enums.DemoWebSettingEnum.AES256IV].ToString()),
                 licensingServiceConfigInfo: null,
                 getAuthorizationFunc: () =>
                 {
                     return "Bearer " + (HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity).FindFirst("AccessToken").Value;
                 });
            #endregion
        }

        /// <summary>
        /// 取得應用程式中介驗證選項
        /// </summary>
        internal static OAuthBearerAuthenticationOptions OAuthBearerAuthenticationOptions { get; } = OAuthConfigUtil.OAuthBearerAuthenticationOptions;
    }
}