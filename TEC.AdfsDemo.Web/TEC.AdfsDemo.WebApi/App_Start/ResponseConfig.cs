using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TEC.AdfsDemo.WebApi.Enums;
using TEC.AdfsDemo.WebApi.Messaging;
using TEC.Core.Web.WebApi.Common.Messaging;

namespace TEC.AdfsDemo.WebApi.App_Start
{
    /// <summary>
    /// 回應設定
    /// </summary>
    public static class ResponseConfig
    {
        /// <summary>
        /// 相關範例可參考 IResultCodeDefinition 之說明
        /// </summary>
        public static IResultCodeDefinition<ResultCodeSettingEnum> ResultCodeDefinition => new ResultCodeDefinition();

        /// <summary>
        /// 相關範例皆可參考 ResultCodeFormatterBase 之說明
        /// </summary>
        public static ResultCodeFormatterBase<ResultCodeSettingEnum> ResultCodeFormatter => new ResultCodeFormatter();
    }
}