using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TEC.Core.ComponentModel;

namespace TEC.AdfsDemo.WebApi.Enums
{
    /// <summary>
    /// 回傳訊息列舉
    /// </summary>
    [DescriptiveEnumEnforcement(DescriptiveEnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)]
    public enum ResultCodeSettingEnum
    {
        #region 系統(0000-0999)
        /// <summary>
        /// 成功
        /// </summary>
        [EnumDescription("0000")]
        Success,

        /// <summary>
        /// 參數驗證錯誤
        /// </summary>
        [EnumDescription("0001")]
        InvalidArgument,

        /// <summary>
        /// 無法辨識的語系
        /// </summary>
        [EnumDescription("0002")]
        UnrecognizedLanguage,
        #endregion 系統(0000-0999)

        #region 驗證 (1000-2999)
        /// <summary>
        /// 欄位為必填
        /// </summary>
        [EnumDescription("1000")]
        FieldRequired,

        /// <summary>
        /// 值區間不符規定
        /// </summary>
        [EnumDescription("1001")]
        InvalidRange,

        /// <summary>
        /// 必須先登入
        /// </summary>
        [EnumDescription("1002")]
        LoginRequired,

        /// <summary>
        /// 登入票證已過期，請重新登入
        /// </summary>
        [EnumDescription("1003")]
        AccessTokenExpired,

        /// <summary>
        /// 登入票證不合法
        /// </summary>
        [EnumDescription("1004")]
        InvalidAccessToken,

        /// <summary>
        /// 登入的帳號或密碼錯誤
        /// </summary>
        [EnumDescription("1005")]
        LoginFailed,

        /// <summary>
        /// 查無此資料
        /// </summary>
        [EnumDescription("1006")]
        DataNotFound,

        /// <summary>
        /// 授權發生錯誤，詳細資料請參考相關訊息：{0}
        /// </summary>
        [EnumDescription("1007")]
        AuthorizationFailed,
        #endregion

        /// <summary>
        /// 系統錯誤
        /// </summary>
        [EnumDescription("9999")]
        SystemError
    }
}