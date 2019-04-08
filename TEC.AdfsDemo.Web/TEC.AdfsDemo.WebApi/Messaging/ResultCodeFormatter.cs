using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TEC.AdfsDemo.WebApi.Enums;
using TEC.Core.Web.WebApi.Common.Messaging;

namespace TEC.AdfsDemo.WebApi.Messaging
{
    /// <summary>
    /// 用於格式化訊息資訊的類別
    /// </summary>
    public class ResultCodeFormatter : ResultCodeFormatterBase<ResultCodeSettingEnum>
    {
        #region MessageCodeFormatterBase 成員

        /// <summary>
        /// 取得指定文化特性中指定列舉的訊息
        /// </summary>
        /// <param name="cultureInfo">文化特性</param>
        /// <param name="enumType">要取得訊息的列舉</param>
        /// <returns></returns>
        protected override string getResultCodeContent(CultureInfo cultureInfo, ResultCodeSettingEnum enumType)
            => enumType.ToString();

        #endregion
    }
}