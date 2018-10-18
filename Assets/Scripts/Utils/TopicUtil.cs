//=======================================================================
//  ClassName : TopicUtil
//  概要      : 話題ユーティリティ
//
//
//  (c) Live2D Inc.All rights reserved.
//=======================================================================﻿﻿
using Assets.Scripts.LiplisSystem.Cif.v60.Req;
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.LiplisSystem.Msg;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class TopicUtil
    {
        /// <summary>
        /// トピックにアロケーションIDを設定する
        /// </summary>
        /// <param name="topic"></param>
        public static void SetAllocationIdAndTone(MsgTopic topic, List<LiplisModel> ModelList)
        {
            int allocationId = 0;

            //アロケーションID設定
            foreach (MsgSentence sentence in topic.TalkSentenceList)
            {
                //アロケーションID設定
                sentence.AllocationId = allocationId;

                sentence.SetTone(ModelList[allocationId].Tone);

                //インクリメント
                allocationId++;

                //先頭に戻す
                if (allocationId == ModelList.Count)
                {
                    allocationId = 0;
                }

            }
        }

        /// <summary>
        /// リクエストを生成する
        /// </summary>
        /// <returns></returns>
        public static ReqTopic CreateReqTopic()
        {
            ReqTopic result = new ReqTopic();

            result.VersionInfo = ((int)Application.platform).ToString();

            return result;
        }
        public static ReqTopicVoice CreateReqTopicVoice()
        {
            ReqTopicVoice result = new ReqTopicVoice();

            result.VersionInfo = ((int)Application.platform).ToString();

            return result;
        }
        public static ReqTopicVoiceOndemand CreateReqTopicVoiceOndemand()
        {
            ReqTopicVoiceOndemand result = new ReqTopicVoiceOndemand();

            result.VersionInfo = ((int)Application.platform).ToString();

            return result;
        }

        /// <summary>
        /// レックトピックを生成する
        /// </summary>
        /// <param name="toneUrlList"></param>
        /// <returns></returns>
        public static ReqTopic CreateReqTopic(List<string> toneUrlList)
        {
            ReqTopic result = CreateReqTopic();

            result.ToneUrlList = toneUrlList;

            return result;
        }
        public static ReqTopic CreateReqTopic(List<string> toneUrlList, string DataKey,string NewsSource)
        {
            ReqTopic result = CreateReqTopic();

            result.DataKey = DataKey;
            result.NewsSource = NewsSource;
            result.ToneUrlList = toneUrlList;

            return result;
        }

        /// <summary>
        /// 音声データ要求メッセージを生成する
        /// </summary>
        /// <param name="DataKey"></param>
        /// <param name="SubId"></param>
        /// <param name="NewsSource"></param>
        /// <returns></returns>
        public static ReqTopicVoice CreateReqTopicVoice(string DataKey,int SubId, string NewsSource)
        {
            ReqTopicVoice result = CreateReqTopicVoice();

            result.DataKey = DataKey;
            result.SubId = SubId.ToString();
            result.NewsSource = NewsSource;

            return result;
        }

        /// <summary>
        /// 音声データ要求メッセージを生成する
        /// </summary>
        /// <param name="DataKey"></param>
        /// <param name="SubId"></param>
        /// <param name="NewsSource"></param>
        /// <returns></returns>
        public static ReqTopicVoiceOndemand CreateReqTopicVoiceOndemand(string Sentence, int AllocationId)
        {
            ReqTopicVoiceOndemand result = CreateReqTopicVoiceOndemand();

            result.Sentence = Sentence;
            result.AllocationId = AllocationId;
            result.Bitrate = 64;

            return result;
        }


    }
}
