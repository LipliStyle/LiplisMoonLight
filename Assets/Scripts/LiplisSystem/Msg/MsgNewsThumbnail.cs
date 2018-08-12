//=======================================================================
//  ClassName : MsgNewsThumbnail
//  概要      : ニュースサムネイルメッセージ
//              
//
//  LiplisLive2D
//  Create 2018/04/09
//
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine.UI;

namespace Assets.Scripts.LiplisSystem.Msg
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
