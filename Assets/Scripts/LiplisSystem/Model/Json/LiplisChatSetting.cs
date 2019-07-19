//====================================================================
//  ClassName : LiplisChatSentence
//  概要      : チャットセンテンス
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//====================================================================
using System;
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Model.Json
{
    [Serializable]
    public class LiplisChatSetting
    {
        public List<ChatSetting> ChatList;

        public LiplisChatSetting()
        {
            ChatList = new List<ChatSetting>();
        }
    }

    [Serializable]
    public class ChatSetting
    {
        public int id;
        public string name;
        public string type;
        public string sentence;
        public string emotion;
        public string rangeStart;
        public string rangeEnd;

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