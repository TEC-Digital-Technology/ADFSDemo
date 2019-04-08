using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TEC.AdfsDemo.WebApi.Enums;
using TEC.Core.Web.WebApi.Common.Messaging;

namespace TEC.AdfsDemo.WebApi.Messaging
{
    /// <summary>
    /// 訊息回傳定義
    /// </summary>
    public class ResultCodeDefinition : IResultCodeDefinition<ResultCodeSettingEnum>
    {
        /// <summary>
        /// 取得預設例外的訊息列舉
        /// </summary>
        public ResultCodeSettingEnum DefaultExceptionResultCode => ResultCodeSettingEnum.SystemError;

        /// <summary>
        /// 取得預設的回傳訊息列舉
        /// </summary>
        public ResultCodeSettingEnum DefaultResultCode => ResultCodeSettingEnum.Success;

        /// <summary>
        /// 取得當參數驗證錯誤時，從指定索引鍵取得驗證錯誤的欄位，通常為"ModelState"，此索引建/值必須設定至擲出的例外中
        /// </summary>
        public string InvalidArgumentExceptionDataKey => "ModelState";

        /// <summary>
        /// 取得當參數驗證失敗時要傳回的訊息列舉
        /// </summary>
        public ResultCodeSettingEnum InvalidArgumentResultCode => ResultCodeSettingEnum.InvalidArgument;
    }
}