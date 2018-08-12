//====================================================================
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
    public  class ClalisForLiplisGetVoiceMp3
    {
        /// <summary>
        /// 音声おしゃべり
        /// </summary>
        /// <returns></returns>
        public static IEnumerator GetAudioClip(MsgTopic NowLoadTopic, int AllocationId, int SubId)
        {
            if (AllocationId < 0 || AllocationId > 3)
            {
                //何もしない
            }
            else
            {
                //ソース指定し音楽流す
                //音楽ファイルロード
                using (WWW www = new WWW(UtilVoiceByOS.GetVoiceApiUrl(), CreateRequest(NowLoadTopic, SubId)))
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
                        yield return www.GetAudioClip(true, true, UtilVoiceByOS.GetVoiceFileTypeByOs());
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
        public static WWWForm CreateRequest(MsgTopic NowLoadTopic, int SubId)
        {
            WWWForm ps = new WWWForm();

            //レックトピック生成
            string json = JsonConvert.SerializeObject(TopicUtil.CreateReqTopicVoice(NowLoadTopic.DataKey, SubId, NowLoadTopic.TopicClassification));
            
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
                using (WWW www = new WWW(UtilVoiceByOS.GetVoiceApiUrl(), CreateRequest(NowLoadTopic, NowLoadTopic.TalkSentenceList[0].SubId)))
                {
                    // 画像ダウンロード完了を待機
                    while (www.MoveNext())
                    {// コルーチンの終了を待つ
                        yield return null;
                    }

                    //ダウンロードサイズが5kb以下ならエラーと判断
                    if (www.bytesDownloaded > 5000)
                    {
                        NowLoadTopic.TalkSentenceList[0].VoiceData = www.GetAudioClip(true, true, UtilVoiceByOS.GetVoiceFileTypeByOs());
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
