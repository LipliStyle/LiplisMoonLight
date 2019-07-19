//=======================================================================
//  ClassName : MsgNewsThumbnail
//  概要      : ニュースサムネイルメッセージ
//              
//
//  LiplisMoonlight
//  Create 2018/04/09
//
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿
using UnityEngine.UI;

namespace Assets.Scripts.Msg
{
    public class MsgNewsThumbnail
    {
        ///=============================
        ///プロパティ
        public string Url;
        public Image image;

        /// <summary>
        /// サムネイルメッセージ
        /// </summary>
        public MsgNewsThumbnail(string Url, Image image)
        {
            this.Url = Url;
            this.image = image;
        }
    }
}
