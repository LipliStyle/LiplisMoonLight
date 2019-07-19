//=======================================================================
//  ClassName : ResLpsTopic
//  概要      : 話題結果メッセージ
//
//  SatelliteServer
//  Copyright(c) 2009-2017 sachin.
//=======================================================================
using System.Collections.Generic;
using System;
using Assets.Scripts.Msg;

namespace Assets.Scripts.LiplisSystem.Cif.v60.Res
{
    [Serializable]
    public class ResLpsTopicList
    {
        public List<MsgTopic> topicList;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public ResLpsTopicList()
        {
            topicList = new List<MsgTopic>();
        }
    }
}
