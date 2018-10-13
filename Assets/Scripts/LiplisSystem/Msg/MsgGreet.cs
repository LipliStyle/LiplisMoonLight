//=======================================================================
//  ClassName : MsgGreet
//  概要      : あいさつメッセージ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿

using Assets.Scripts.Common;
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.LiplisSystem.Model.Json;
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
        public MsgGreet(LiplisTone Tone, ChatSetting chat, int AllocationId)
        {
            try
            {
                this.message = new MsgTopic(Tone, chat.sentence, chat.sentence, chat.GetEmotion(), chat.GetEmotion(), 0, AllocationId);

                //Chatに設定があれば、時間範囲を設定する
                if (chat.rangeStart != "" || chat.rangeEnd != "")
                {
                    //スタート時刻をセット
                    string[] start = chat.rangeStart.Split(':');
                    this.SrtTime = DateUtil.CreateDatetime(int.Parse(start[0]), int.Parse(start[1]), 0);

                    //終了時刻をセット
                    string[] end = chat.rangeEnd.Split(':');
                    this.EndTime = DateUtil.CreateDatetime(int.Parse(end[0]), int.Parse(end[1]), 0);
                }
            }
            catch
            {
                //エラーでも処理を継続
            }
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
        public MsgGreet(LiplisTone Tone, DateTime SrtTime, DateTime EndTime, string message, int Emotion, int Point, int AllocationId)
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
