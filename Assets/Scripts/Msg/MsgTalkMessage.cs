//=======================================================================
//  ClassName : MsgTalkMessage
//  概要      : トークメッセージ
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿
using Assets.Scripts.Com;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Msg
{
    public class MsgTalkMessage
    {
        ///=============================
        ///プロパティ
        public string url { get; set; }
        public string title { get; set; }
        public string result { get; set; }
        public string sorce { get; set; }
        public string jpgUrl { get; set; }
        public int newsEmotion { get; set; }
        public int newsPoint { get; set; }
        public List<string> nameList { get; set; }
        public List<int> emotionList { get; set; }
        public List<int> pointList { get; set; }

        public int joyP { get; set; }
        public int joyM { get; set; }
        public int admirationP { get; set; }
        public int admirationM { get; set; }
        public int peaceP { get; set; }
        public int peaceM { get; set; }
        public int ecstasyP { get; set; }
        public int ecstasyM { get; set; }
        public int amazementP { get; set; }
        public int amazementM { get; set; }
        public int rageP { get; set; }
        public int rageM { get; set; }
        public int interestP { get; set; }
        public int interestM { get; set; }
        public int respectP { get; set; }
        public int respectM { get; set; }
        public int calmlyP { get; set; }
        public int calmlyM { get; set; }
        public int proudP { get; set; }
        public int proudM { get; set; }

        public int widgetIndex { get; set; }    //ガールズトーク識別用ID

        /// <summary>
        /// コンストラクター
        /// このコンストラクターを使用する場合は、リザルトとソースは必ず設定する必要がある！！
        ///
        /// 
        /// 
        /// </summary>
        #region コンストラクター
        public MsgTalkMessage()
        {
            url = "";
            title = "";
            result = "";
            sorce = "";
            jpgUrl = "";
            newsEmotion = 0;
            newsPoint = 0;
            nameList = new List<string>();
            emotionList = new List<int>();
            pointList = new List<int>();
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="name"></param>
        /// <param name="emotion"></param>
        /// <param name="point"></param>
        public MsgTalkMessage(string name, int emotion, int point)
        {
            url = "";
            result = name;
            sorce = name;
            title = name;
            jpgUrl = "";
            newsEmotion = 0;
            newsPoint = 0;
            nameList = new List<string>();
            emotionList = new List<int>();
            pointList = new List<int>();

            nameList.Add(name);
            emotionList.Add(emotion);
            pointList.Add(point);
        }

        /// <summary>
        /// コンマ分割のクラリス処理データからインスタンスを生成する
        /// </summary>
        /// <param name="sentenceData"></param>
        public MsgTalkMessage(string sentenceData)
        {
            url = "";
            jpgUrl = "";
            newsEmotion = 0;
            newsPoint = 0;
            nameList = new List<string>();
            emotionList = new List<int>();
            pointList = new List<int>();

            //リストを生成する
            createList(sentenceData);

            //ニュースエモーションも算出
            calcNewsEmotion();
        }

        #endregion

        /// <summary>
        /// リストを生成する
        /// </summary>
        /// <param name="sentenceData"></param>
        public void createList(string sentenceData)
        {
            StringBuilder sbSource = new StringBuilder();

            //コロン分解
            foreach (var parts in sentenceData.Split(';'))
            {
                try
                {
                    //コンマ分解
                    string[] buf = parts.Split(',');

                    //文末が来たら抜ける
                    if (buf[0] == "EOS")
                    {
                        break;
                    }

                    nameList.Add(buf[0]);
                    sbSource.Append(buf[0]);
                    emotionList.Add(LpsLiplisUtil.stringToInt(buf[1]));
                    pointList.Add(LpsLiplisUtil.stringToInt(buf[2]));
                }
                catch
                {

                }
            }

            result = sbSource.ToString();
            sorce = sbSource.ToString();
            title = sbSource.ToString();

        }

        /// <summary>
        /// calcNewsEmotion
        ///	ニュースエモーションを計算する
        /// </summary>
        #region calcNewsEmotion
        public void calcNewsEmotion()
        {
            int idx = 0;
            int maxEmo = 0;
            int maxPoint = 0;

            joyP = 0;
            joyM = 0;
            admirationP = 0;
            admirationM = 0;
            peaceP = 0;
            peaceM = 0;
            ecstasyP = 0;
            ecstasyM = 0;
            amazementP = 0;
            amazementM = 0;
            rageP = 0;
            rageM = 0;
            interestP = 0;
            interestM = 0;
            respectP = 0;
            respectM = 0;
            calmlyP = 0;
            calmlyM = 0;
            proudP = 0;
            proudM = 0;

            try
            {
                //名前リストが入っていたら
                if (nameList != null)
                {
                    //エモーションリストを回して集計
                    foreach (int emo in emotionList)
                    {
                        switch (emo)
                        {
                            case 1:
                                if (pointList[idx] >= 0)
                                {
                                    joyP += pointList[idx];
                                }
                                else
                                {
                                    joyM += pointList[idx];
                                }
                                break;
                            case 2:
                                if (pointList[idx] >= 0)
                                {
                                    admirationP += pointList[idx];
                                }
                                else
                                {
                                    admirationM += pointList[idx];
                                }
                                break;
                            case 3:
                                if (pointList[idx] >= 0)
                                {
                                    peaceP += pointList[idx];
                                }
                                else
                                {
                                    peaceM += pointList[idx];
                                }
                                break;
                            case 4:
                                if (pointList[idx] >= 0)
                                {
                                    ecstasyP += pointList[idx];
                                }
                                else
                                {
                                    ecstasyM += pointList[idx];
                                }
                                break;
                            case 5:
                                if (pointList[idx] >= 0)
                                {
                                    amazementP += pointList[idx];
                                }
                                else
                                {
                                    amazementM += pointList[idx];
                                }
                                break;
                            case 6:
                                if (pointList[idx] >= 0)
                                {
                                    rageP += pointList[idx];
                                }
                                else
                                {
                                    rageM += pointList[idx];
                                }
                                break;
                            case 7:
                                if (pointList[idx] >= 0)
                                {
                                    interestP += pointList[idx];
                                }
                                else
                                {
                                    interestM += pointList[idx];
                                }
                                break;
                            case 8:
                                if (pointList[idx] >= 0)
                                {
                                    respectP += pointList[idx];
                                }
                                else
                                {
                                    respectM += pointList[idx];
                                }
                                break;
                            case 9:
                                if (pointList[idx] >= 0)
                                {
                                    calmlyP += pointList[idx];
                                }
                                else
                                {
                                    calmlyM += pointList[idx];
                                }
                                break;
                            case 10:
                                if (pointList[idx] >= 0)
                                {
                                    proudP += pointList[idx];
                                }
                                else
                                {
                                    proudM += pointList[idx];
                                }
                                break;
                            default: break;
                        }

                        idx++;
                    }
                }
                //最大値の検索

                if (Math.Abs(maxPoint) < Math.Abs(joyP)) { maxPoint = joyP; maxEmo = 1; }
                if (Math.Abs(maxPoint) < Math.Abs(joyM)) { maxPoint = joyM; maxEmo = 1; }
                if (Math.Abs(maxPoint) < Math.Abs(admirationP)) { maxPoint = admirationP; maxEmo = 2; }
                if (Math.Abs(maxPoint) < Math.Abs(admirationM)) { maxPoint = admirationM; maxEmo = 2; }
                if (Math.Abs(maxPoint) < Math.Abs(peaceP)) { maxPoint = peaceP; maxEmo = 3; }
                if (Math.Abs(maxPoint) < Math.Abs(peaceM)) { maxPoint = peaceM; maxEmo = 3; }
                if (Math.Abs(maxPoint) < Math.Abs(ecstasyP)) { maxPoint = ecstasyP; maxEmo = 4; }
                if (Math.Abs(maxPoint) < Math.Abs(ecstasyM)) { maxPoint = ecstasyM; maxEmo = 4; }
                if (Math.Abs(maxPoint) < Math.Abs(amazementP)) { maxPoint = amazementP; maxEmo = 5; }
                if (Math.Abs(maxPoint) < Math.Abs(amazementM)) { maxPoint = amazementM; maxEmo = 5; }
                if (Math.Abs(maxPoint) < Math.Abs(rageP)) { maxPoint = rageP; maxEmo = 6; }
                if (Math.Abs(maxPoint) < Math.Abs(rageM)) { maxPoint = rageM; maxEmo = 6; }
                if (Math.Abs(maxPoint) < Math.Abs(interestP)) { maxPoint = interestP; maxEmo = 7; }
                if (Math.Abs(maxPoint) < Math.Abs(interestM)) { maxPoint = interestM; maxEmo = 7; }
                if (Math.Abs(maxPoint) < Math.Abs(respectP)) { maxPoint = respectP; maxEmo = 8; }
                if (Math.Abs(maxPoint) < Math.Abs(respectM)) { maxPoint = respectM; maxEmo = 8; }
                if (Math.Abs(maxPoint) < Math.Abs(calmlyP)) { maxPoint = calmlyP; maxEmo = 9; }
                if (Math.Abs(maxPoint) < Math.Abs(calmlyM)) { maxPoint = calmlyM; maxEmo = 9; }
                if (Math.Abs(maxPoint) < Math.Abs(proudP)) { maxPoint = proudP; maxEmo = 10; }
                if (Math.Abs(maxPoint) < Math.Abs(proudM)) { maxPoint = proudM; maxEmo = 10; }

                //最大値取得
                newsEmotion = maxEmo;
                newsPoint = maxPoint;
            }
            catch
            {
                return;
            }
        }
        #endregion


        /// <summary>
        /// getNameFirst
        ///	１個目のネームを返す
        /// </summary>
        /// <returns>1個目のname</returns>
        #region getMessage
        public string getMessage()
        {
            if (nameList.Count > 0)
            {
                return nameList[0];
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
