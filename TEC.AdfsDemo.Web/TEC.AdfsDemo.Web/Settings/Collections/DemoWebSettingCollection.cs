using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TEC.AdfsDemo.Web.Enums;
using TEC.Core.Settings.Collections;

namespace TEC.AdfsDemo.Web.Settings.Collections
{
    /// <summary>
    /// ADFS Demo 網站的設定檔集合
    /// </summary>
    public class DemoWebSettingCollection : SettingCollectionBase<DemoWebSettingEnum, object, string>
    {
        /// <summary>
        /// 初始化 ADFS Demo 網站的設定檔集合
        /// </summary>
        public DemoWebSettingCollection()
            : base("DemoWebSettingCollection")
        { }

        public override object getDefaultValue(DemoWebSettingEnum key)
        {
            switch (key)
            {
                default:
                    throw new NotImplementedException("不支援以此方法取得預設設定。");
            }
        }
    }
}