//====================================================================
//  ClassName : DatSetting
//  概要      : 設定情報
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//====================================================================
using Assets.Scripts.Define;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatSetting
    {
        ///=============================
        ///有効 話題
        public bool FlgTopicNews;
        public bool FlgTopicSummary;
        public bool FlgTopicRetweet;
        public bool FlgTopicHash;

        ///=============================
        ///有効 話題
        public bool FlgVoice;
        public float TalkSpeed;
        public int TalkNum;
        public int GraphicLevel;
        public int DrawingFps;
        public int SpeechBallonNum;
        public int CharArrangement;

        ///=============================
        ///デバッグモード
        public bool FlgDebug;

        ///=============================
        ///舞台上に居るキャラクターのリスト
        public List<string> LstModelOnTheStage;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DatSetting()
        {
            this.FlgTopicNews = true;
            this.FlgTopicSummary = true;
            this.FlgTopicRetweet = true;
            this.FlgTopicHash = false;

            this.TalkSpeed = 5.0f;
            this.TalkNum = 1;
            this.GraphicLevel = 1;
            this.DrawingFps = 0;
            this.SpeechBallonNum = 2;
            this.CharArrangement = 0;

            this.FlgVoice = false;
            this.FlgDebug = false;
        }

        /// <summary>
        /// すべてチェックされているか、全てチェックが外されている場合
        /// </summary>
        /// <returns></returns>
        public bool GetCheckAll()
        {
            return (FlgTopicNews && FlgTopicSummary && FlgTopicRetweet && FlgTopicHash)
                || (!FlgTopicNews && !FlgTopicSummary && !FlgTopicRetweet && !FlgTopicHash);
        }

        /// <summary>
        /// カテゴリーチェック
        /// </summary>
        /// <param name="ContentCat"></param>
        /// <returns></returns>
        public bool CatCheck(string ContentCat)
        {
            if (GetCheckAll())
            {
                return true;
            }
            else if (ContentCat == "")
            {
                //空は許可
                return true;
            }
            else if (ContentCat == null)
            {
                //空は許可
                return true;
            }
            else if (ContentCat == ((int)ContentCategoly.news).ToString() && LiplisSetting.Instance.Setting.FlgTopicNews)
            {
                return true;
            }
            else if (ContentCat == ((int)ContentCategoly.matome).ToString() && LiplisSetting.Instance.Setting.FlgTopicSummary)
            {
                return true;
            }
            else if (ContentCat == ((int)ContentCategoly.retweet).ToString() && LiplisSetting.Instance.Setting.FlgTopicRetweet)
            {
                return true;
            }
            else if (ContentCat == ((int)ContentCategoly.hotHash).ToString() && LiplisSetting.Instance.Setting.FlgTopicHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// トークスピードの取得
        /// </summary>
        /// <returns></returns>
        public float GetTalkSpeed()
        {
            if(TalkSpeed != 0)
            {
                return 0.2f / TalkSpeed;
            }
            else
            {
                return 0.2f;
            }
        }

        /// <summary>
        /// おしゃべり量を取得する
        /// </summary>
        /// <returns></returns>
        public int GetTalkNum()
        {
            if (this.TalkNum == 0)
            {
                //少ない
                return 11;
            }
            else if (this.TalkNum == 1)
            {
                //ふつう
                return 23;
            }
            else if (this.TalkNum == 2)
            {
                //多い
                return 35;
            }
            else if (this.TalkNum == 3)
            {
                //MAX(ほぼ無限)
                return 999;
            }
            else
            {
                //ふつうをデフォルト
                return 24;
            }
        }

        /// <summary>
        /// FPSを取得する
        /// </summary>
        /// <returns></returns>
        public int GetFps()
        {
            if(this.DrawingFps == 0)
            {
                return 60;
            }
            else if (this.DrawingFps == 1)
            {
                return 30;
            }
            else
            {
                this.DrawingFps = 0;
                return 60;
            }
        }

        /// <summary>
        /// 吹き出し数を取得する
        /// </summary>
        /// <returns></returns>
        public int GetSpeechBallonNum()
        {
            if (this.SpeechBallonNum == 0)
            {
                return 3;
            }
            else if (this.SpeechBallonNum == 1)
            {
                return 2;
            }
            else if (this.SpeechBallonNum == 2)
            {
                return 1;
            }
            else
            {
                this.SpeechBallonNum = 0;
                return 3;
            }
        }
    }
}
