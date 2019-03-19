using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TEC.AdfsDemo.Web.Enums;
using TEC.AdfsDemo.Web.Settings.Collections;
using TEC.Core.Settings.Providers;

namespace TEC.AdfsDemo.Web.Settings
{
    /// <summary>
    /// 設定檔集合工廠
    /// </summary>
    internal static class SettingCollectionFactoryInternal
    {
        private static readonly object sync_obj = new object();
        /// <summary>
        /// 取得應用程式的設定檔集合
        /// </summary>
        public static DemoWebSettingCollection DemoWebSettingCollection
        {
            get
            {
                ApplicationSettingProvider<DemoWebSettingCollection, DemoWebSettingEnum, object, string> provider =
                    new ApplicationSettingProvider<DemoWebSettingCollection, DemoWebSettingEnum, object, string>();

                if (provider.load() == null)
                {
                    lock (SettingCollectionFactoryInternal.sync_obj)
                    {
                        if (provider.load() == null)
                        {
                            DemoWebSettingCollection demoWebSettingCollection = new DemoWebSettingCollection
                            {
                                [DemoWebSettingEnum.AccountServiceApiBaseLocation] = new Uri(ConfigurationManager.AppSettings["AccountServiceApiBaseLocation"]),
                                [DemoWebSettingEnum.AllowedAudience] = ConfigurationManager.AppSettings["ida:AllowedAudience"],
                                [DemoWebSettingEnum.AppKey] = ConfigurationManager.AppSettings["ida:AppKey"],
                                [DemoWebSettingEnum.Authority] = ConfigurationManager.AppSettings["ida:Authority"],
                                [DemoWebSettingEnum.ClientId] = ConfigurationManager.AppSettings["ida:ClientId"],
                                [DemoWebSettingEnum.GraphResourceId] = ConfigurationManager.AppSettings["ida:GraphResourceId"],
                                [DemoWebSettingEnum.SigningCertPath] = new Uri(ConfigurationManager.AppSettings["ida:SingingCertPath"]),
                                [DemoWebSettingEnum.PlatformName] = ConfigurationManager.AppSettings["PlatformName"],
                                [DemoWebSettingEnum.AES256Key] = ConfigurationManager.AppSettings["AES256Key"],
                                [DemoWebSettingEnum.AES256IV] = ConfigurationManager.AppSettings["AES256IV"],
                                [DemoWebSettingEnum.LogoutUrlFormat] = ConfigurationManager.AppSettings["ida:LogoutUrlFormat"],
                            };

                            provider.save(demoWebSettingCollection);
                        }
                    }
                }

                return provider.load();
            }
        }
    }
}