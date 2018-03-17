//====================================================================
//  ClassName : DatNewTopic
//  概要      : 最新話題データ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatNewTopic
    {
        ///=============================
        ///プロパティ
        public uint LastUpdateTime;

        ///=============================
        ///最新ニュースデータ
        public ResLpsTopicList LastData;

        ///=============================
        /// メッセージリスト
        public List<MsgTopic> TalkTopicList;            //トークメッセージリスト
        public List<MsgTopic> InterruptTopicList;       //割り込みトークメッセージリスト
        public List<MsgTopic> ChattedKeyList;           //おしゃべり済みトピックリスト

        ///=============================
        /// トーンURLリスト
        public List<string> ToneUrlList;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DatNewTopic()
        {
            TalkTopicList = new List<MsgTopic>();
            InterruptTopicList = new List<MsgTopic>();
            ChattedKeyList = new List<MsgTopic>();

            //トーンURLの更新
            InitToneUrlList();
        }

        /// <summary>
        /// トーンURLリストの更新
        /// </summary>
        private void InitToneUrlList()
        {
            ToneUrlList = new List<string>();

            ToneUrlList.Add(LpsDefine.LIPLIS_TONE_URL_HAZUKI);      //アロケーションID : 0
            ToneUrlList.Add(LpsDefine.LIPLIS_TONE_URL_SHIROHA);     //アロケーションID : 1
            ToneUrlList.Add(LpsDefine.LIPLIS_TONE_URL_KUROHA);      //アロケーションID : 2
            ToneUrlList.Add(LpsDefine.LIPLIS_TONE_URL_MOMOHA);      //アロケーションID : 3
        }

        /// <summary>
        /// キーリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyList()
        {
            List<string> keyList = new List<string>();

            foreach (MsgTopic topic in TalkTopicList)
            {
                keyList.Add(topic.DataKey);
            }

            //重複回避挿入
            foreach (MsgTopic topic in ChattedKeyList)
            {
                keyList.AddToNotDuplicate(topic.DataKey);
            }

            return keyList;
        }

    }
}
