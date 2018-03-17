//=======================================================================
//  ClassName : MsgGreet
//  概要      : あいさつメッセージ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿

using Assets.Scripts.DataChar.CharacterTalk;
using System;

namespace Assets.Scripts.LiplisSystem.Msg
{
    public class MsgGreet
    {
        public DateTime SrtTime;
        public DateTime EndTime;
        public MsgTopic message;

        /// <summary>
        /// デフォルトコンストラクター
        /// </summary>
        public MsgGreet()
        {

        }

        /// <summary>
        /// グリートのセット
        /// </summary>
        /// <param name="SrtTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="message"></param>
        public MsgGreet(DateTime SrtTime,DateTime EndTime,MsgTopic message)
        {
            this.SrtTime = SrtTime;
            this.EndTime = EndTime;
            this.message = message;
        }

        /// <summary>
        /// グリートのセット
        /// </summary>
        /// <param name="SrtTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="message"></param>
        public MsgGreet(CharDataTone Tone, DateTime SrtTime, DateTime EndTime, string message, int Emotion, int Point, int AllocationId)
        {
            this.SrtTime = SrtTime;
            this.EndTime = EndTime;
            this.message = new MsgTopic(Tone,message, message,Emotion,Point,1, AllocationId);
        }

        /// <summary>
        /// トピックを取得する
        /// </summary>
        /// <returns></returns>
        public MsgTopic GetTopic()
        {
            return message;
        }

    }
}
