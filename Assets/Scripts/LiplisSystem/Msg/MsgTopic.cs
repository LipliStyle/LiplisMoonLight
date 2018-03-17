//=======================================================================
//  ClassName : MsgTopic
//  概要      : 話題メッセージ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.DataChar.CharacterTalk;
using Assets.Scripts.LiplisSystem.Com;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Msg
{
    [Serializable]
    public class MsgTopic
    {
        ///=============================
        /// ベース情報
        public uint CreateTime;
        public string DataKey;
        public string Title;
        public string Url;
        public string ThumbnailUrl;
        public string TopicClassification;
        public bool FlgNotAddChatted = false;           //フラグONでおしゃべり済みに追加しない

        ///=============================
        /// おしゃべりセンテンスリスト
        public List<MsgSentence> TalkSentenceList;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public MsgTopic()
        {
            TalkSentenceList = new List<MsgSentence>();
        }


        /// <summary>
        /// コンストラクター
        /// </summary>
        public MsgTopic(CharDataTone Tone,string BaseSentence, int Emotion, int Point)
        {
            TalkSentenceList = new List<MsgSentence>();
            TalkSentenceList.Add(new MsgSentence(Tone, BaseSentence, Emotion, Point, 1));
        }
        public MsgTopic(CharDataTone Tone, string BaseSentence,string TalkSentence, int Emotion, int Point)
        {
            TalkSentenceList = new List<MsgSentence>();
            TalkSentenceList.Add(new MsgSentence(Tone, BaseSentence, TalkSentence, Emotion, Point, 1));
        }
        public MsgTopic(CharDataTone Tone, string BaseSentence, string TalkSentence, int Emotion, int Point, int Mode)
        {
            TalkSentenceList = new List<MsgSentence>();
            TalkSentenceList.Add(new MsgSentence(Tone, BaseSentence, TalkSentence, Emotion, Point, Mode));
        }

        public MsgTopic(CharDataTone Tone, string BaseSentence, string TalkSentence, int Emotion, int Point, int Mode, int AllocationId)
        {
            TalkSentenceList = new List<MsgSentence>();
            TalkSentenceList.Add(new MsgSentence(Tone, BaseSentence, TalkSentence, Emotion, Point, Mode,AllocationId));
        }
    }



}
