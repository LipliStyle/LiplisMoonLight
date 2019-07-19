//====================================================================
//  ClassName : LiplisWeather
//  概要      : 天気文章を生成するクラス
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.LiplisSystem.Msg;
using System;
using System.Collections.Generic;
namespace Assets.Scripts.LiplisSystem.Sentece
{
    public class LiplisWeather
    {
        //====================================================================
        // 
        //                           天気情報関連
        //                         
        //====================================================================
        #region 天気情報関連
        /// <summary>
        /// ランダムにその日の情報を取得する
        /// </summary>
        /// <returns></returns>
        public static void SetWetherSentence(List<MsgSentence> q, LiplisTone Tone, int AllocationId)
        {
            //NULLチェック
            if (LiplisStatus.Instance.InfoWether.WetherDtlList == null)
            {
                return;
            }

            if (LiplisStatus.Instance.InfoWether.WetherDtlList.Count < 1)
            {
                return;
            }

            //現在時刻取得
            DateTime dt = DateTime.Now;

            //天気コード取得
            MsgDayWether todayWether = LiplisStatus.Instance.InfoWether.GetWetherSentenceToday(dt);

            //0～12 今日 午前、午後、夜の天気
            if (dt.Hour >= 0 && dt.Hour <= 18)
            {
                CreateWetherMessage("今日の天気は", todayWether, q, Tone, AllocationId);
            }

            //19～23 明日の天気
            else if (dt.Hour >= 19 && dt.Hour <= 23)
            {
                //明日の天気も取得
                MsgDayWether tomorrowWether = LiplisStatus.Instance.InfoWether.GetWetherSentenceTommorow(dt);

                CreateWetherMessage("", todayWether, q, Tone, AllocationId);
                CreateWetherMessage("明日の天気は", tomorrowWether, q, Tone, AllocationId);

            }
        }



        /// <summary>
        /// メッセージを生成する
        /// </summary>
        /// <returns></returns>
        public static void CreateWetherMessage(string dayString, MsgDayWether result, List<MsgSentence> q, LiplisTone Tone, int AllocationId)
        {
            //全入力
            if (result.wether1Roughly != "" && result.wether2Roughly != "" && result.wether3Roughly != "")
            {
                CreateWetherMessage3(dayString, result, q, Tone, AllocationId);
            }
            //2個入力
            else if (result.wether2Roughly != "" && result.wether3Roughly != "")
            {
                CreateWetherMessage2(dayString, result, q, Tone, AllocationId);
            }
            //1個入力
            else if (result.wether3Roughly != "")
            {
                CreateWetherMessage1(dayString, result, q, Tone, AllocationId);
            }
        }


        /// <summary>
        /// ウェザー入力3つの場合の入力
        /// </summary>
        /// <returns></returns>
        private static void CreateWetherMessage3(string dayString, MsgDayWether result, List<MsgSentence> q, LiplisTone Tone, int AllocationId)
        {
            //	同一
            if (result.wether1Roughly == result.wether2Roughly && result.wether1Roughly == result.wether3Roughly)
            {
                MsgSentence sentence1 = CreateWetherSentence(dayString + "1日を通して" + GetWetherWording2(result.wether1Roughly) + "でしょう。", result.wether1Roughly, false, Tone, AllocationId);
                q.Add(sentence1);
            }
            //	前2つ同
            else if (result.wether1Roughly == result.wether2Roughly)
            {
                //1文目生成
                MsgSentence sentence1 = CreateWetherSentence(dayString + "朝から昼にかけて" + GetWetherWording1(result.wether1Roughly) + "ますが、", result.wether1Roughly, false, Tone, AllocationId);
                q.Add(sentence1);

                //2文目生成
                MsgSentence sentence2 = CreateWetherSentence(GetWetherWording2(result.wether3Roughly) + "でしょう。", result.wether3Roughly, true, Tone, AllocationId);
                q.Add(sentence2);

                return;
            }
            //	後2つ同
            else if (result.wether2Roughly == result.wether3Roughly)
            {
                //1文目生成
                MsgSentence sentence1 = CreateWetherSentence(dayString + "朝は" + GetWetherWording1(result.wether1Roughly) + "ますが、", result.wether1Roughly, false, Tone, AllocationId);
                q.Add(sentence1);

                //2文目生成
                MsgSentence sentence2 = CreateWetherSentence("昼過ぎから今夜まで" + GetWetherWording2(result.wether2Roughly) + "でしょう。", result.wether2Roughly, true, Tone, AllocationId);
                q.Add(sentence2);

                return;
            }
            //	全違
            else if (result.wether1Roughly != result.wether2Roughly && result.wether1Roughly != result.wether3Roughly)
            {
                //1文目生成
                MsgSentence sentence1 = CreateWetherSentence(dayString + "朝は" + GetWetherWording1(result.wether1Roughly) + "ますが、", result.wether1Roughly, false, Tone, AllocationId);
                q.Add(sentence1);

                //2文目生成
                MsgSentence sentence2 = CreateWetherSentence("昼過ぎから" + GetWetherWording1(result.wether2Roughly) + "、", result.wether2Roughly, true, Tone, AllocationId);
                q.Add(sentence2);

                //2文目生成
                MsgSentence sentence3 = CreateWetherSentence("今夜は" + GetWetherWording2(result.wether3Roughly) + "でしょう。", result.wether3Roughly, true, Tone, AllocationId);
                q.Add(sentence3);

                return;
            }
            //	前後同じ
            else if (result.wether1Roughly == result.wether3Roughly)
            {
                //1文目生成
                MsgSentence sentence1 = CreateWetherSentence(dayString + "朝は" + GetWetherWording1(result.wether1Roughly) + "ますが、", result.wether1Roughly, false, Tone, AllocationId);
                q.Add(sentence1);

                //2文目生成
                MsgSentence sentence2 = CreateWetherSentence("昼過ぎから" + GetWetherWording1(result.wether2Roughly) + "、", result.wether2Roughly, true, Tone, AllocationId);
                q.Add(sentence2);

                //2文目生成
                MsgSentence sentence3 = CreateWetherSentence("今夜は" + GetWetherWording2(result.wether3Roughly) + "でしょう。", result.wether3Roughly, true, Tone, AllocationId);
                q.Add(sentence3);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// センテンスを生成する
        /// </summary>
        /// <param name="message"></param>
        /// <param name="wetherCode"></param>
        /// <param name="addMessage"></param>
        /// <returns></returns>
        private static MsgSentence CreateWetherSentence(string message, string wetherCode, bool addMessage, LiplisTone Tone, int AllocationId)
        {
            //エモーションをセットする
            MsgEmotion emotion = GetWetherEmotion(wetherCode);

            //インスタンス生成
            MsgSentence sentence = new MsgSentence(Tone, message, message, emotion.EMOTION, emotion.POINT, false, AllocationId, addMessage);

            //トーンコンバート
            sentence.ToneConvert();

            //生成したセンテンスを返す
            return sentence;
        }


        /// <summary>
        /// ウェザー入力2つの場合の入力
        /// </summary>
        /// <returns></returns>
        private static void CreateWetherMessage2(string dayString, MsgDayWether result, List<MsgSentence> q, LiplisTone Tone, int AllocationId)
        {
            //同じ
            if (result.wether2Roughly == result.wether3Roughly)
            {
                MsgSentence sentence1 = CreateWetherSentence(dayString + "午後から夜を通して" + GetWetherWording2(result.wether2Roughly) + "でしょう。", result.wether2Roughly, false, Tone, AllocationId);
                q.Add(sentence1);
                return;
            }
            //違う
            else
            {
                //1文目生成
                MsgSentence sentence1 = CreateWetherSentence(dayString + "午後から夜にかけては" + GetWetherWording1(result.wether2Roughly) + "ますが、", result.wether2Roughly, false, Tone, AllocationId);
                q.Add(sentence1);

                //2文目生成
                MsgSentence sentence2 = CreateWetherSentence("今夜は" + GetWetherWording2(result.wether3Roughly) + "でしょう。", result.wether2Roughly, true, Tone, AllocationId);
                q.Add(sentence2);

                return;
            }
        }

        /// <summary>
        /// ウェザー入力1つの場合の入力
        /// </summary>
        /// <returns></returns>
        private static void CreateWetherMessage1(string dayString, MsgDayWether result, List<MsgSentence> q, LiplisTone Tone, int AllocationId)
        {
            MsgSentence sentence1 = CreateWetherSentence("今夜は" + GetWetherWording2(result.wether3Roughly) + "でしょう。", result.wether3Roughly, false, Tone, AllocationId);
            q.Add(sentence1);
        }

        /// <summary>
        /// 天気の言い回し取得 パターン1
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetWetherWording1(string code)
        {
            if (code == WETHER_CODE.SUNNY)
            {
                return "晴れ";
            }
            else if (code == WETHER_CODE.CLOUDY)
            {
                return "曇り";
            }
            else if (code == WETHER_CODE.RAIN)
            {
                return "雨が振り";
            }
            else if (code == WETHER_CODE.SNOW)
            {
                return "雪が振り";
            }
            else
            {
                return "該当ワードなし";
            }
        }

        /// <summary>
        /// 天気の言い回し取得 パターン2
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetWetherWording2(string code)
        {
            if (code == WETHER_CODE.SUNNY)
            {
                return "晴れる";
            }
            else if (code == WETHER_CODE.CLOUDY)
            {
                return "曇る";
            }
            else if (code == WETHER_CODE.RAIN)
            {
                return "雨が降る";
            }
            else if (code == WETHER_CODE.SNOW)
            {
                return "雪が降る";
            }
            else
            {
                return "該当ワードなし";
            }
        }

        /// <summary>
        /// 天気エモーションを取得する
        /// </summary>
        /// <param name="wetherCode"></param>
        /// <returns></returns>
        public static MsgEmotion GetWetherEmotion(string wetherCode)
        {
            switch (wetherCode)
            {
                case WETHER_CODE.SUNNY: return new MsgEmotion(2, 3);
                case WETHER_CODE.CLOUDY: return new MsgEmotion(0, 0);
                case WETHER_CODE.RAIN: return new MsgEmotion(2, -3);
                case WETHER_CODE.SNOW: return new MsgEmotion(2, -3);
                default: return new MsgEmotion(0, 0);
            }
        }

        public MsgSentence GetWetherMessage(string timeString, string wetherCode)
        {
            MsgSentence result = new MsgSentence();

            switch (wetherCode)
            {
                case WETHER_CODE.SUNNY: result.BaseSentence = timeString + "の天気は" + "晴れでしょう。"; result.Emotion = EMOTION.JOY; result.Point = 3; break;
                case WETHER_CODE.SUNNY_THEN_CLOUDY: result.BaseSentence = timeString + "の天気は" + "晴れのち曇りでしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.SUNNY_THEN_RAIN: result.BaseSentence = timeString + "の天気は" + "晴れのち雨でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.SUNNY_THEN_SNOW: result.BaseSentence = timeString + "の天気は" + "晴れのち雪でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.SUNNY_WITH_OCCASIONAL_CLOUDY: result.BaseSentence = timeString + "の天気は" + "晴れ時々曇りでしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.SUNNY_WITH_OCCASIONAL_RAIN: result.BaseSentence = timeString + "の天気は" + "晴れ時々雨でしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.SUNNY_WITH_OCCASIONAL_SNOW: result.BaseSentence = timeString + "の天気は" + "晴れ時々雪でしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.CLOUDY: result.BaseSentence = timeString + "の天気は" + "曇りでしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.CLOUDY_THEN_SUNNY: result.BaseSentence = timeString + "の天気は" + "曇りのち晴れでしょう。"; result.Emotion = EMOTION.JOY; result.Point = 3; break;
                case WETHER_CODE.CLOUDY_THEN_RAIN: result.BaseSentence = timeString + "の天気は" + "曇りのち雨でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.CLOUDY_THEN_SNOW: result.BaseSentence = timeString + "の天気は" + "曇りのち雪でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_SUNNY: result.BaseSentence = timeString + "の天気は" + "曇り時々晴れでしょう。"; result.Emotion = EMOTION.JOY; result.Point = 3; break;
                case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_RAIN: result.BaseSentence = timeString + "の天気は" + "曇り時々雨でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_SNOW: result.BaseSentence = timeString + "の天気は" + "曇り時々雪でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.RAIN: result.BaseSentence = timeString + "の天気は" + "雨でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.RAIN_THEN_SUNNY: result.BaseSentence = timeString + "の天気は" + "雨のち晴れでしょう。"; result.Emotion = EMOTION.JOY; result.Point = 3; break;
                case WETHER_CODE.RAIN_THEN_CLOUDY: result.BaseSentence = timeString + "の天気は" + "雨のち曇でしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.RAIN_THEN_SNOW: result.BaseSentence = timeString + "の天気は" + "雨のち雪でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.RAIN_WITH_OCCASIONAL_SUNNY: result.BaseSentence = timeString + "の天気は" + "雨時々晴れでしょう。"; result.Emotion = EMOTION.JOY; result.Point = 3; break;
                case WETHER_CODE.RAIN_WITH_OCCASIONAL_CLOUDY: result.BaseSentence = timeString + "の天気は" + "雨時々曇りでしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.RAIN_WITH_OCCASIONAL_SNOW: result.BaseSentence = timeString + "の天気は" + "雨時々雪でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.SNOW: result.BaseSentence = timeString + "の天気は" + "雪でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.SNOW_THEN_SUNNY: result.BaseSentence = timeString + "の天気は" + "雪のち晴れでしょう。"; result.Emotion = EMOTION.JOY; result.Point = 3; break;
                case WETHER_CODE.SNOW_THEN_CLOUDY: result.BaseSentence = timeString + "の天気は" + "雪のち曇りでしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.SNOW_THEN_RAIN: result.BaseSentence = timeString + "の天気は" + "雪のち雨でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.SNOW_WITH_OCCASIONAL_SUNNY: result.BaseSentence = timeString + "の天気は" + "雪時々晴れでしょう。"; result.Emotion = EMOTION.JOY; result.Point = 3; break;
                case WETHER_CODE.SNOW_WITH_OCCASIONAL_CLOUDY: result.BaseSentence = timeString + "の天気は" + "雪時々曇りでしょう。"; result.Emotion = EMOTION.NORMAL; result.Point = 0; break;
                case WETHER_CODE.SNOW_WITH_OCCASIONAL_RAIN: result.BaseSentence = timeString + "の天気は" + "雪時々雨でしょう。"; result.Emotion = EMOTION.JOY; result.Point = -3; break;
                case WETHER_CODE.WIND_STOM: result.BaseSentence = timeString + "の天気は" + "暴風雨でしょう。"; result.Emotion = EMOTION.AMAZEMENT; result.Point = 3; break;

                default: result.BaseSentence = ""; result.Emotion = 0; result.Point = 0; break;
            }

            //データセット
            result.TalkSentence = result.BaseSentence;

            return result;
        }


        #endregion
    }
}
