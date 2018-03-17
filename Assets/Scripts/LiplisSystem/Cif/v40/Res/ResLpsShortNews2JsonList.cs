//=======================================================================
//  ClassName : ResLpsShortNews2JsonList
//  概要      : レスポンス ショートニュース Json リスト
//
//  Liplis5.0
//
//  Copyright(c) 2010-2016 LipliStyle.Sachin
//=======================================================================
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Clalis.v40.Res
{
    [SerializableAttribute]
    [ComVisibleAttribute(true)]
    public class ResLpsShortNews2JsonList
    {
        ///=============================
        ///プロパティ
        public List<ResLpsShortNews2Json> lstNews { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        #region ResLpsShortNews2JsonList
        public ResLpsShortNews2JsonList()
        {
            this.lstNews = new List<ResLpsShortNews2Json>();
        }
        public ResLpsShortNews2JsonList(string url, List<ResLpsShortNews2Json> lstNews)
        {
            this.lstNews = lstNews;
        }
        #endregion
    }
}
