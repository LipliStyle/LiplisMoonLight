//=======================================================================
//  ClassName : MsgTopic
//  概要      : 話題メッセージ
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Model;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Msg
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
        public MsgTopic(LiplisTone Tone, string BaseSentence, string TalkSentence, int Emotion, int Point, bool FlgConvert, int AllocationId)
        {
            TalkSentenceList = new List<MsgSentence>();
            TalkSentenceList.Add(new MsgSentence(Tone, BaseSentence, TalkSentence, Emotion, Point, FlgConvert, AllocationId));
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
            copy.CreateTime          = this.CreateTime;
            copy.DataKey             = this.DataKey;
            copy.Title               = this.Title;
            copy.Url                 = this.Url;
            copy.ThumbnailUrl        = this.ThumbnailUrl;
            copy.TopicClassification = this.TopicClassification;
            copy.FlgNotAddChatted    = this.FlgNotAddChatted;

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

        /// <summary>
        /// ベースニュースデータとして返す
        /// </summary>
        /// <returns></returns>
        public MsgBaseNewsData Convert2BaseNewsData()
        {
            MsgBaseNewsData data = new MsgBaseNewsData();

            data.CREATE_TIME      = this.CreateTime;
            data.DATA_TYPE        = this.TopicClassification;
            data.RANK             = 0;
            data.DATAKEY          = this.DataKey;
            data.TITLE            = this.Title;
            data.URL              = this.Url;
            data.THUMBNAIL_URL    = this.ThumbnailUrl;
            data.THUMBNAIL_HEIGHT = 0;
            data.THUMBNAIL_WIDTH  = 0;
            data.THUMBNAIL_SIZE   = 0;
            data.THUMBNAIL_DATA = null; 

            return data;

        }
    }



}
