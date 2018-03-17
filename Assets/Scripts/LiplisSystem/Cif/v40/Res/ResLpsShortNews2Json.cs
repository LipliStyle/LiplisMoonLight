//=======================================================================
//  ClassName : ResLpsShortNews2Json
//  概要      : レスポンス ショートニュースニュース Json
//
//  Liplis5.0
//
//  Copyright(c) 2010-2016 LipliStyle.Sachin
//=======================================================================
using System;
using System.Runtime.InteropServices;

namespace Clalis.v40.Res
{
    [SerializableAttribute]
    [ComVisibleAttribute(true)]
    public class ResLpsShortNews2Json
    {
        ///=============================
        ///プロパティ
        public string url { get; set; }
        public string result { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        #region resShortNews
        public ResLpsShortNews2Json()
        {
            this.url = "";
            this.result = "";
        }
        public ResLpsShortNews2Json(string url, string result)
        {
            this.url = url;
            this.result = result;
        }
        #endregion
    }
}
