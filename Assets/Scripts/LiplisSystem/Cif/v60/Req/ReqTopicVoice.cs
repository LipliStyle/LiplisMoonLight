//=======================================================================
//  ClassName : ReqTopicVoice
//  概要      : トピック音声取得要求メッセージ
//
//
//  LiplisSystemシステム      
//  Copyright(c) 2010-2018 sachin.
//=======================================================================

namespace Assets.Scripts.LiplisSystem.Cif.v60.Req
{
    public class ReqTopicVoice
    {
        ///=============================
        /// 設定
        public string DataKey;
        public string SubId;        //ニュースIDで要求する場合は-1を指定する
        public string NewsSource;   //MenuUiManager.ContentCategolyに準拠
        public string VersionInfo;
    }
}
