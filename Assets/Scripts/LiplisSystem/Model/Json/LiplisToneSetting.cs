//====================================================================
//  ClassName : LiplisToneSetting
//  概要      : 口調設定
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
//TODO LiplisToneSetting クラス構造、ファイル構造見直しせよ！
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Model.Json
{
    public class LiplisToneSetting
    {
        public Tone tone { get; set; }
    }

    public class ToneDiscription
    {
        public string name { get; set; }
        public string befor { get; set; }
        public string after { get; set; }
        public string type { get; set; }
    }

    public class Tone
    {
        public List<ToneDiscription> toneDiscription { get; set; }
    }

}
