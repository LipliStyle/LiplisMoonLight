﻿//=======================================================================
//  ClassName : TalkDataBase
//  概要      : トークデータ ベース メッセージ
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data.Talk
{
    public class TalkDataBase
    {
        ///=============================
        /// おしゃべりリスト
        List<MsgSentence> TalkList;

        /// <summary>
        /// コンストラクター
        /// </summary>
        protected TalkDataBase()
        {
            initList();
        }

        /// <summary>
        /// リストを初期化する
        /// </summary>
        protected virtual void initList()
        {
            TalkList = new List<MsgSentence>();
        }
    }
}
