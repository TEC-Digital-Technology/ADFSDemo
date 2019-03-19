using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEC.AdfsDemo.Web.Enums
{
    /// <summary>
    /// ADFS Demo 網站的設定檔列舉
    /// </summary>
    public enum DemoWebSettingEnum
    {
        /// <summary>
        /// 使用 AES-256 加解密時需要的 Key，屬於<see cref="String"/>
        /// </summary>
        AES256Key,
        /// <summary>
        /// 使用 AES-256 加解密時需要的 IV，屬於<see cref="String"/>
        /// </summary>
        AES256IV,
        /// <summary>
        /// 用於身分識別的應用程式金鑰，屬於 <see cref="String"/>
        /// </summary>
        AppKey,
        /// <summary>
        /// 認證提供者位址，屬於 <see cref="String"/>
        /// </summary>
        Authority,
        /// <summary>
        /// 信賴識別碼，屬於 <see cref="String"/>
        /// </summary>
        AllowedAudience,
        /// <summary>
        /// 簽署憑證路徑，屬於 <see cref="Uri"/>
        /// </summary>
        SigningCertPath,
        /// <summary>
        /// 客戶端識別碼，屬於 <see cref="String"/>
        /// </summary>
        ClientId,
        /// <summary>
        /// 取得資源識別碼，屬於 <see cref="String"/>
        /// </summary>
        GraphResourceId,
        /// <summary>
        /// 平台名稱，用於介接內部系統使用(TEC Internal Utils)，屬於 <see cref="String"/>
        /// </summary>
        PlatformName,
        /// <summary>
        /// 帳號資料伺服器位址，用於介接內部系統使用(TEC Internal Utils)，屬於 <see cref="Uri"/>
        /// </summary>
        AccountServiceApiBaseLocation,
        /// <summary>
        /// 登出路徑的 Url 格式，屬於 <see cref="String"/>
        /// <para>
        /// 回傳值類似 https://domain/logout?post_logout_redirect_uri={0} ，{0} 為登出完成後要導向的頁面，須經由格式化處理
        /// </para>
        /// </summary>
        LogoutUrlFormat,
    }
}