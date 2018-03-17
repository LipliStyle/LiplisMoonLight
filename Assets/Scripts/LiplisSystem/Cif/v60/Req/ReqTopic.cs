//=======================================================================
//  ClassName : ReqTopic
//  概要      : トピック取得要求メッセージ
//
//  入力      : tone="トーン定義URL"
//
//  LiplisSystemシステム      
//  Copyright(c) 2010-2017 sachin. All Rights Reserved. 
//=======================================================================
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Cif.v60.Req
{
    public class ReqTopic
    {
        ///=============================
        /// 設定

        ///=============================
        /// トーンURLリスト
        public List<string> ToneUrlList = new List<string>();

    }
}
