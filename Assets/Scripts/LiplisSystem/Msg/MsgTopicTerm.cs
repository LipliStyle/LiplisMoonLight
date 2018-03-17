//=======================================================================
//  ClassName : MsgTopicTerm
//  概要      : 条件付き話題メッセージ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿

namespace Assets.Scripts.LiplisSystem.Msg
{
    public class MsgTopicTerm : MsgTopic
    {
        ///=============================
        /// おしゃべり条件
        public TargetTerm Term;        //対象条件

        /// <summary>
        /// コンストラクター
        /// </summary>
        public MsgTopicTerm():base()
        {
            //条件の初期化
            Term = new TargetTerm();
        }

    }

    /// <summary>
    /// 対象条件
    /// </summary>
    public class TargetTerm
    {
        ///=============================
        /// おしゃべり時刻範囲
        public int TimeStart = -1;
        public int TimeEnd = -1;

        public string weather = "";
        public int tempMax = -1;
        public int tempMin = -1;
        public int chanceOfRain = -1;
        public string wind = "";
        public string wave = "";

        //=============================
        // おしゃべり条件

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="TimeStart"></param>
        /// <param name="TimeEnd"></param>
        public TargetTerm(int TimeStart, int TimeEnd)
        {
            this.TimeStart = TimeStart;
            this.TimeEnd = TimeEnd;
        }
        public TargetTerm()
        {

        }

    }
}
