//=======================================================================
//  ClassName : MsgSentence
//  概要      : おしゃべり文章メッセージ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Model;
using System;
using UnityEngine;

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
        public bool FlgToneConvertSkip;         //コンバート
        public bool FlgAddMessge;          //追加メッセージ

        ///=============================
        /// 音声データ
        public AudioClip VoiceData;

        ///=============================
        /// トーンデータ
        private LiplisTone Tone;

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
        public MsgSentence(LiplisTone Tone, string BaseSentence, string TalkSentence, int Emotion, int Point, bool FlgToneConvertSkip, int AllocationId, bool FlgAddMessge)
        {
            this.BaseSentence = BaseSentence;
            this.Emotion = Emotion;
            this.Point = Point;
            this.FlgToneConvertSkip = FlgToneConvertSkip;
            this.FlgAddMessge = FlgAddMessge;
            this.AllocationId = AllocationId;
            this.Tone = Tone;
        }
        public MsgSentence(LiplisTone Tone, string BaseSentence, string TalkSentence, int Emotion, int Point, bool FlgToneConvertSkip, int AllocationId)
        {
            this.BaseSentence = BaseSentence;
            this.TalkSentence = TalkSentence;
            this.Emotion = Emotion;
            this.Point = Point;
            this.FlgToneConvertSkip = FlgToneConvertSkip;
            this.AllocationId = AllocationId;
            this.FlgAddMessge = false;
            this.Tone = Tone;
        }

        /// <summary>
        /// トーンコンバートする
        /// </summary>
        public void ToneConvert()
        {
            try
            {
                //トーンコンバーターがNULLの場合はエラー回避
                if (this.Tone == null)
                {
                    this.TalkSentence = this.BaseSentence;
                }

                //スキップフラグがOFFでかつ未コンバートの場合 
                //必ずコンバート処理するように変更。理由は、アロケーションIDが変更されている可能性があるから。
                //if (!FlgToneConvertSkip && (this.TalkSentence == null || this.TalkSentence == ""))
                if (!FlgToneConvertSkip && (this.TalkSentence == null || this.TalkSentence == ""))
                {
                    this.TalkSentence = this.Tone.Convert(this.BaseSentence);
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// トーン設定をセットする
        /// </summary>
        /// <param name="Tone"></param>
        public void SetTone(LiplisTone Tone)
        {
            this.Tone = Tone;
            ToneConvert();
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public MsgSentence Clone()
        {
            MsgSentence msg = new MsgSentence();

            msg.CreateTime           = this.CreateTime;
            msg.DataKey              = this.DataKey;
            msg.SubId                = this.SubId;
            msg.AllocationId         = this.AllocationId;
            msg.BaseSentence         = this.BaseSentence;    //ベース文章
            msg.TalkSentence         = this.TalkSentence;     //おしゃべり文章
            msg.Emotion              = this.Emotion;             //エモーション
            msg.Point                = this.Point;               //ポイント
            msg.FlgToneConvertSkip   = this.FlgToneConvertSkip;
            msg.FlgAddMessge         = this.FlgAddMessge;          //追加メッセージ

            return msg;
    }


    }



}
