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

        ///=============================
        /// URLリスト
        public List<string> THUMBNAIL_URL_LIST;
        public List<string> VIDEO_URL_LIST;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public MsgTopic()
        {
            TalkSentenceList = new List<MsgSentence>();
            THUMBNAIL_URL_LIST = new List<string>();
            VIDEO_URL_LIST = new List<string>();
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


        /// <summary>
        /// コピーインスタンスを生成する
        /// </summary>
        /// <returns></returns>
        public MsgTopic Clone()
        {
            //結果データ
            MsgTopic copy = new MsgTopic();

            //基本データのコピー
            copy.CreateTime = this.CreateTime;
            copy.DataKey = this.DataKey;
            copy.Title = this.Title;
            copy.Url = this.Url;
            copy.ThumbnailUrl = this.ThumbnailUrl;
            copy.TopicClassification = this.TopicClassification;
            copy.FlgNotAddChatted = this.FlgNotAddChatted;

            //センテンスリストのコピー
            foreach (var sentence in this.TalkSentenceList)
            {
                copy.TalkSentenceList.Add(sentence.Clone());
            }

            //URLリスト、サムネリストのコピー
            copy.THUMBNAIL_URL_LIST = new List<string>(this.THUMBNAIL_URL_LIST);
            copy.VIDEO_URL_LIST = new List<string>(this.VIDEO_URL_LIST);

            return copy;
        }

    }



}
