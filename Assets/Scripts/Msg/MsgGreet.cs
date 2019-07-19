//=======================================================================
//  ClassName : MsgGreet
//  概要      : あいさつメッセージ
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.LiplisSystem.Model.Json;
using Assets.Scripts.Utils;
using System;

namespace Assets.Scripts.Msg
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
                this.message = new MsgTopic(Tone, chat.sentence, chat.sentence, chat.GetEmotion(), chat.GetEmotion(), true, AllocationId);

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
        /// トピックを取得する
        /// </summary>
        /// <returns></returns>
        public MsgTopic GetTopic()
        {
            return message;
        }

    }
}
