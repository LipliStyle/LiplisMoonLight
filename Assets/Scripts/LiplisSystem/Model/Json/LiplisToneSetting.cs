//====================================================================
//  ClassName : LiplisToneSetting
//  概要      : 口調設定
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
    public class LiplisToneSetting
    {
        public List<ToneSetting> ToneList;

        public LiplisToneSetting()
        {
            ToneList = new List<ToneSetting>();
        }
    }

    [Serializable]
    public class ToneSetting
    {
        public int id;
        public string name;
        public string sentence;
        public string befor;
        public string after;
        public string type;
    }

}
