//=======================================================================
//  ClassName : MsgSentence
//  概要      : おしゃべり文章メッセージ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.DataChar.CharacterTalk;
using Assets.Scripts.LiplisSystem.Com;
using System;

namespace Assets.Scripts.LiplisSystem.Msg
{
    [Serializable]
    public class MsgSentence
    {
        ///=============================
        /// ベースデータ
        public uint CreateTime;
        public string DataKey;
        public int SubId;
        public int AllocationId = -1;

        ///=============================
        /// おしゃべりデータ
        public string BaseSentence ;    //ベース文章
        public string TalkSentence;     //おしゃべり文章
        public int Emotion;             //エモーション
        public int Point;               //ポイント
        public int Mode;
        public bool FlgAddMessge;          //追加メッセージ

        ///=============================
        /// 口調変換するかどうか
        public bool FlgToneConvert = true;  //トーンコンバートするかどうか

        /// <summary>
        /// デフォルトコンストラクター
        /// </summary>
        public MsgSentence()
        {

        }

        /// <summary>
        /// ベースセンテンスを設定するコンストラクター
        /// モード0:口調変換する
        /// モード1:口調変換しない 
        /// </summary>
        /// <param name="BaseSentence"></param>
        public MsgSentence(CharDataTone Tone, string BaseSentence, int Emotion, int Point, int Mode)
        {
            this.BaseSentence = BaseSentence;
            this.Emotion = Emotion;
            this.Point = Point;
            this.Mode = Mode;

            ToneConvert(Tone);
        }
        public MsgSentence(CharDataTone Tone, string BaseSentence, int Emotion, int Point, int Mode, bool FlgAddMessge)
        {
            this.BaseSentence = BaseSentence;
            this.Emotion = Emotion;
            this.Point = Point;
            this.Mode = Mode;
            this.FlgAddMessge = FlgAddMessge;

            ToneConvert(Tone);
        }
        public MsgSentence(CharDataTone Tone, string BaseSentence, int Emotion, int Point, int Mode, bool FlgAddMessge, int AllocationId)
        {
            this.BaseSentence = BaseSentence;
            this.Emotion = Emotion;
            this.Point = Point;
            this.Mode = Mode;
            this.FlgAddMessge = FlgAddMessge;
            this.AllocationId = AllocationId;

            ToneConvert(Tone);
        }
        public MsgSentence(CharDataTone Tone, string BaseSentence, string TalkSentence, int Emotion, int Point, int Mode)
        {
            this.BaseSentence = BaseSentence;
            this.TalkSentence = TalkSentence;
            this.Emotion = Emotion;
            this.Point = Point;
            this.Mode = Mode;

            ToneConvert(Tone);
        }

        public MsgSentence(CharDataTone Tone, string BaseSentence, string TalkSentence, int Emotion, int Point, int Mode, int AllocationId)
        {
            this.BaseSentence = BaseSentence;
            this.TalkSentence = TalkSentence;
            this.Emotion = Emotion;
            this.Point = Point;
            this.Mode = Mode;
            this.AllocationId = AllocationId;

            ToneConvert(Tone);
        }

        /// <summary>
        /// トーンコンバートする
        /// </summary>
        public void ToneConvert(CharDataTone Tone)
        {
            if (Mode != 1)
            {
                this.TalkSentence = Tone.Convert(this.BaseSentence);
            }
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public MsgSentence Clone()
        {
            MsgSentence msg = new MsgSentence();

            msg.CreateTime   = this.CreateTime;
            msg.DataKey      = this.DataKey;
            msg.SubId        = this.SubId;
            msg.AllocationId = this.AllocationId = -1;
            msg.BaseSentence = this.BaseSentence;    //ベース文章
            msg.TalkSentence = this.TalkSentence;     //おしゃべり文章
            msg.Emotion      = this.Emotion;             //エモーション
            msg.Point        = this.Point;               //ポイント
            msg.Mode         = this.Mode;
            msg.FlgAddMessge = this.FlgAddMessge;          //追加メッセージ

            return msg;
    }


    }



}
