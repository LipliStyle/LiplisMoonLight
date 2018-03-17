//=======================================================================
//  ClassName : ResLpsSummaryNews2JsonList
//  概要      : レスポンス サマリーニュース Json リスト
//
//  Liplis5.0
//
//  Copyright(c) 2010-2016 LipliStyle.Sachin
//=======================================================================
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

namespace Clalis.v40.Res
{
    [SerializableAttribute]
    [ComVisibleAttribute(true)]
    public class ResLpsSummaryNews2JsonList
    {
        ///=============================
        ///プロパティ
        public List<ResLpsSummaryNews2Json> lstNews { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        #region ResLpsShortNews2JsonList
        public ResLpsSummaryNews2JsonList()
        {
            this.lstNews = new List<ResLpsSummaryNews2Json>();
        }
        public ResLpsSummaryNews2JsonList(string url, List<ResLpsSummaryNews2Json> lstNews)
        {
            this.lstNews = lstNews;
        }
        #endregion
    }
}
