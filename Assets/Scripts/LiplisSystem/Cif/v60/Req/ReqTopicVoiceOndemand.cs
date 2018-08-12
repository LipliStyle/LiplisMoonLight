//=======================================================================
//  ClassName : ReqTopicVoice
//  概要      : トピック音声取得要求メッセージ
//
//
//  LiplisSystemシステム      
//  Copyright(c) 2010-2018 sachin. All Rights Reserved. 
//=======================================================================
namespace Assets.Scripts.LiplisSystem.Cif.v60.Req
{
    public class ReqTopicVoiceOndemand
    {
        public string Sentence { get; set; }
        public int AllocationId { get; set; }
        public int Bitrate { get; set; }
        public string VersionInfo { get; set; }
    }
}
