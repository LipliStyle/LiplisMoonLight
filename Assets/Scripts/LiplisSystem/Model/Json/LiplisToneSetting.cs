//====================================================================
//  ClassName : LiplisToneSetting
//  概要      : 口調設定
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Model.Json
{
    public class LiplisToneSetting
    {
        public List<ToneSetting> ToneList { get; set; }

        public LiplisToneSetting()
        {
            ToneList = new List<ToneSetting>();
        }
    }

    public class ToneSetting
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sentence { get; set; }
        public string befor { get; set; }
        public string after { get; set; }
        public string type { get; set; }
    }

}
