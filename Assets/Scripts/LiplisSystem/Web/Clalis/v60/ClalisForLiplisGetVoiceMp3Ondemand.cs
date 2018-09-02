﻿//====================================================================
//  ClassName : ClalisForLiplisGetVoiceMp3
//  概要      : 音声データ取得
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Web.Clalis.v60
{
    public class ClalisForLiplisGetVoiceMp3Ondemand
    {
        /// <summary>
        /// 音声おしゃべり
        /// </summary>
        /// <returns></returns>
        public static IEnumerator GetAudioClip(MsgSentence Sentence)
        {
            if (Sentence.AllocationId < 0 || Sentence.AllocationId > 3)
            {
                //何もしない
            }
            else
            {
                //ソース指定し音楽流す
                //音楽ファイルロード
                using (WWW www = new WWW(UtilVoiceByOS.GetOndemandVoiceApiUrl(), CreateRequest(Sentence)))
                {
                    ////読み込み完了まで待機
                    //yield return www;

                    // 画像ダウンロード完了を待機
                    while (www.MoveNext())
                    {// コルーチンの終了を待つ
                        yield return null;
                    }

                    //ダウンロードサイズが5kb以下ならエラーと判断
                    if (www.bytesDownloaded > 5000)
                    {
                        yield return www.GetAudioClip(false, false, UtilVoiceByOS.GetVoiceFileTypeByOs());
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
        }

        /// <summary>
        /// リクエストを生成する
        /// </summary>
        /// <param name="NowLoadTopic"></param>
        /// <param name="SubId"></param>
        /// <returns></returns>
        public static WWWForm CreateRequest(MsgSentence Sentence)
        {
            WWWForm ps = new WWWForm();

            //レックトピック生成
            string json = JsonConvert.SerializeObject(TopicUtil.CreateReqTopicVoiceOndemand(Sentence.TalkSentence, Sentence.AllocationId));

            //パラメータ生成
            ps.AddField("reqmsg", json); //トーンURL

            //結果を返す
            return ps;
        }

        /// <summary>
        /// 初期のボイスデータをセットする
        /// </summary>
        /// <param name="NowLoadTopic"></param>
        /// <returns></returns>
        public static IEnumerator SetVoiceDataStart(MsgTopic NowLoadTopic)
        {
            if (NowLoadTopic.TalkSentenceList.Count < 1)
            {
                goto End;
            }

            //センテンス状態チェック
            if (NowLoadTopic.TalkSentenceList[0].VoiceData != null)
            {
                //すでにデータがあれば何もしない
            }
            else if (NowLoadTopic.TalkSentenceList[0].AllocationId < 0 || NowLoadTopic.TalkSentenceList[0].AllocationId > 3)
            {
                //何もしない
            }
            else
            {
                //ソース指定し音楽流す
                //音楽ファイルロード
                using (WWW www = new WWW(UtilVoiceByOS.GetOndemandVoiceApiUrl(), CreateRequest(NowLoadTopic.TalkSentenceList[0])))
                {
                    // 画像ダウンロード完了を待機
                    while (www.MoveNext())
                    {// コルーチンの終了を待つ
                        yield return null;
                    }

                    //ダウンロードサイズが5kb以下ならエラーと判断
                    if (www.bytesDownloaded > 5000)
                    {
                        NowLoadTopic.TalkSentenceList[0].VoiceData = www.GetAudioClip(false, false, UtilVoiceByOS.GetVoiceFileTypeByOs());
                    }
                    else
                    {
                        NowLoadTopic.TalkSentenceList[0].VoiceData = null;
                    }
                }
            }

            //終了ラベル
            End:;
        }
    }
}