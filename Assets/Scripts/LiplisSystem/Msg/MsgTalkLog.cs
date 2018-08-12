//=======================================================================
//  ClassName : MsgTalkLog
//  概要      : ログメッセージ
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.LiplisSystem.Msg
{
    public class MsgTalkLog
    {
        public uint CREATE_TIME;
        public string DATA_TYPE;
        public string DATAKEY;
        public string TITLE;
        public string URL;
        public string THUMBNAIL_URL;

        public string PANEL_KEY;

        ///=============================
        /// おしゃべりセンテンスリスト
        public List<MsgSentence> TalkSentenceList;
    }
}
