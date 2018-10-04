//====================================================================
//  ClassName : LiplisChatSentence
//  概要      : チャットセンテンス
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
//TODO LiplisChatSentence クラス構造、ファイル構造見直しせよ！
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Model.Json
{
    public class LiplisChatSetting
    {
        public Chat chat { get; set; }
    }
    public class ChatDiscription
    {
        public string name { get; set; }
        public string type { get; set; }
        public string discription { get; set; }
        public int emotion { get; set; }
        public string prerequisite { get; set; }
    }

    public class Chat
    {
        public List<ChatDiscription> chatDiscription { get; set; }
    }

}