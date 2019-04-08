using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using TEC.AdfsDemo.WebApi.App_Start;
using TEC.AdfsDemo.WebApi.Enums;
using TEC.AdfsDemo.WebApi.Exceptions;
using TEC.Core.Web.WebApi.Common.Messaging;
using TEC.Core.Web.WebApi.Common.Response;

namespace TEC.AdfsDemo.WebApi.Models
{
    /// <summary>
    /// 用於擴充例外的靜態類別
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// 格式化已知例外的訊息
        /// </summary>
        /// <param name="operationFailedException">要格式化的例外</param>
        /// <param name="formatType">格式化方式</param>
        /// <param name="cultureInfo">與此例外回傳訊息相關的文化特性</param>
        /// <returns>格式化後的結果</returns>
        public static string getFormattedString(this OperationFailedException operationFailedException, ResultCodeFormatType formatType, CultureInfo cultureInfo)
        {
            return ResponseConfig.ResultCodeFormatter.Format(formatType, operationFailedException.ResultCodeSettingEnum, cultureInfo);
        }

        /// <summary>
        /// 使用已知例外的UI執行緒的文化特性，格式化已知例外的訊息
        /// </summary>
        /// <param name="operationFailedException">要格式化的例外</param>
        /// <param name="formatType">格式化方式</param>
        /// <returns>格式化後的結果</returns>
        public static string getFormattedString(this OperationFailedException operationFailedException, ResultCodeFormatType formatType)
        {
            return ResponseConfig.ResultCodeFormatter.Format(formatType, operationFailedException.ResultCodeSettingEnum, operationFailedException.CultureInfo);
        }

        /// <summary>
        /// 使用已知的例外(繼承[OperationFailedException]之類別)為API回傳結果
        /// </summary>
        /// <param name="operationFailedException">繼承[OperationFailedException]之例外</param>
        /// <returns>回傳的例外訊息</returns>
        public static ExceptionResponse<ResultCodeSettingEnum> getResponse(this OperationFailedException operationFailedException)
        {
            return operationFailedException.getResponse(operationFailedException.CultureInfo);
        }

        /// <summary>
        /// 轉換已知的例外(繼承[OperationFailedException]之類別)為API回傳結果
        /// </summary>
        /// <param name="operationFailedException">繼承[OperationFailedException]之例外</param>
        /// <param name="cultureInfo">與此例外回傳訊息相關的文化特性</param>
        /// <returns>回傳的例外訊息</returns>
        public static ExceptionResponse<ResultCodeSettingEnum> getResponse(this OperationFailedException operationFailedException,
            CultureInfo cultureInfo)
        {
            return new ExceptionResponse<ResultCodeSettingEnum>(operationFailedException, ResponseConfig.ResultCodeFormatter, ResponseConfig.ResultCodeDefinition);
        }

        /// <summary>
        /// 轉換未知的例外(非繼承[OperationFailedException]之類別)為API回傳結果
        /// </summary>
        /// <param name="exception">非繼承[OperationFailedException]之例外</param>
        /// <param name="cultureInfo">與此例外回傳訊息相關的文化特性</param>
        /// <param name="resultCodeSettingEnum">與此錯誤資訊相關的訊息列舉</param>
        /// <returns>回傳的例外訊息</returns>
        public static ExceptionResponse<ResultCodeSettingEnum> getResponse(this Exception exception,
            CultureInfo cultureInfo, ResultCodeSettingEnum resultCodeSettingEnum = ResultCodeSettingEnum.SystemError)
        {
            return new ExceptionResponse<ResultCodeSettingEnum>(exception, cultureInfo, resultCodeSettingEnum, ResponseConfig.ResultCodeFormatter, ResponseConfig.ResultCodeDefinition);
        }

        /// <summary>
        /// 使用目前UI執行緒的文化特性([Thread.CurrentThread].[Thread.CurrentUICulture])，
        /// 轉換未知的例外(非繼承[OperationFailedException]之類別)為API回傳結果
        /// </summary>
        /// <param name="exception">非繼承[OperationFailedException]之例外</param>
        /// <param name="resultCodeSettingEnum">與此錯誤資訊相關的訊息列舉</param>
        /// <returns>回傳的例外訊息</returns>
        public static ExceptionResponse<ResultCodeSettingEnum> getResponse(this Exception exception, ResultCodeSettingEnum resultCodeSettingEnum = ResultCodeSettingEnum.SystemError)
        {
            return exception.getResponse(Thread.CurrentThread.CurrentUICulture, resultCodeSettingEnum);
        }
    }
}