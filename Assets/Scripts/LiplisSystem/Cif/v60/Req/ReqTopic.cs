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
        public string DataKey;
        public string NewsSource;   //MenuUiManager.ContentCategolyに準拠
        public string VersionInfo;

        ///=============================
        /// トーンURLリスト
        public List<string> ToneUrlList = new List<string>();
    }
}
