using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEC.AdfsDemo.Web.Models.Shared
{
    /// <summary>
    /// 用於 Error View 的 ViewModel
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// 設定或取得 Title
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 設定或取得錯誤代號
        /// </summary>
        public string ErrorCode { set; get; }
        /// <summary>
        /// 設定或取得訊息內容的 HTML 文字
        /// </summary>
        public string MessageHtml { set; get; } = "<strong>Oops!</strong><br/> 有些邏輯貌似壞掉了，請與技術支援小組聯絡唷!";
    }
}