//=======================================================================
//  ClassName : TopicUtil
//  概要      : 話題ユーティリティ
//
//
//  (c) Live2D Inc.All rights reserved.
//=======================================================================﻿﻿
using Assets.Scripts.LiplisSystem.Cif.v60.Req;
using Assets.Scripts.LiplisSystem.Msg;
using System.Collections.Generic;

namespace Assets.Scripts.Utils
{
    public class TopicUtil
    {
        /// <summary>
        /// トピックにアロケーションIDを設定する
        /// </summary>
        /// <param name="topic"></param>
        public static void SetAllocationId(MsgTopic topic)
        {
            int allocationId = 0;

            //アロケーションID設定
            foreach (MsgSentence sentence in topic.TalkSentenceList)
            {
                sentence.AllocationId = allocationId;

                //インクリメント
                allocationId++;

                //先頭に戻す
                if (allocationId == 4)
                {
                    allocationId = 0;
                }

            }
        }


        /// <summary>
        /// レックトピックを生成する
        /// </summary>
        /// <param name="toneUrlList"></param>
        /// <returns></returns>
        public static ReqTopic CreateReqTopic(List<string> toneUrlList)
        {
            ReqTopic result = new ReqTopic();

            result.ToneUrlList = toneUrlList;

            return result;
        }
    }
}
