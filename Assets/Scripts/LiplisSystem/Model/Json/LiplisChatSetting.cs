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
        public List<ChatSetting> ChatList { get; set; }

        public LiplisChatSetting()
        {
            ChatList = new List<ChatSetting>();
        }
    }
    public class ChatSetting
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string sentence { get; set; }
        public string emotion { get; set; }
        public string rangeStart { get; set; }
        public string rangeEnd { get; set; }

        /// <summary>
        /// エモーションを取得する
        /// </summary>
        /// <returns></returns>
        public int GetEmotion()
        {
            try
            {
                return int.Parse(emotion);
            }
            catch
            {
                return 0;
            }
        }



    }

}