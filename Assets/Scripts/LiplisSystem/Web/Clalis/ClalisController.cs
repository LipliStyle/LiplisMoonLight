//=======================================================================
//  ClassName : ClalisController
//  概要      : クラリスコントローラー
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.Com;
using Assets.Scripts.Data;
using Assets.Scripts.Data.SubData;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Web.Clalis.v60;
using Assets.Scripts.Msg;
using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Web.Clalis
{
    public class ClalisController
    {
        //====================================================================
        //
        //                        パブリックメソッド
        //                         
        //====================================================================
        #region データ保存

        /// <summary>
        /// データセーブ
        /// </summary>
        public IEnumerator Save()
        {
            //指定キー「LiplisStatus」でリプリスステータスのインスタンスを保存する
            SaveDataClalis.SetClass(LpsDefine.SETKEY_LIPLIS_STATUS, LiplisStatus.Instance);

            //セーブ発動
            SaveDataClalis.Save();

            yield return null;
        }
        #endregion


        //====================================================================
        //
        //                        位置情報収集
        //                         
        //====================================================================
        #region 位置情報収集

        /// <summary>
        /// 位置情報収集
        /// </summary>
        public IEnumerator DataCollectLocation()
        {
            //トークインスタンス取得
            DatLocation Location = LiplisStatus.Instance.InfoLocation;

            //指定時間経過していなければ抜ける
            if (LpsDatetimeUtil.dec(Location.LastUpdateTime).AddMinutes(10) > DateTime.Now)
            {
                goto End;
            }

            //最新データをダウンロードする
            yield return CoroutineHandler.StartStaticCoroutine(SetLocation(Location));

        //終了ラベル
        End:;
        }

        /// <summary>
        /// 最新データをダウンロードする
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetLocation(DatLocation Location)
        {
            //最新ニュースデータ取得
            var Async = ClalisLocationInfomation.GetLocation();

            //非同期実行
            yield return Async;

            //データ取得
            ResLpsLocationInfo location = (ResLpsLocationInfo)Async.Current;

            //データセット
            Location.SetData(location);

            //最終更新時刻設定
            yield return Location.LastUpdateTime = LpsDatetimeUtil.Now;

        }


        #endregion


        //====================================================================
        //
        //                        天気情報収集
        //                         
        //====================================================================
        #region 天気情報収集

        /// <summary>
        /// 天気情報収集
        /// </summary>
        public IEnumerator DataCollectWether()
        {
            //トークインスタンス取得
            DatWether Wether = LiplisStatus.Instance.InfoWether;

            //指定時間経過していなければ抜ける
            if (LpsDatetimeUtil.dec(Wether.LastUpdateTime).AddMinutes(60) > DateTime.Now)
            {
                goto End;
            }

            //最新データをダウンロードする
            yield return CoroutineHandler.StartStaticCoroutine(SetWether(Wether));

        //終了ラベル
        End:;
        }


        /// <summary>
        /// 最新データをダウンロードする
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetWether(DatWether Wether)
        {
            //最新ニュースデータ取得
            var Async = ClalisLocationWetherList.GetWetherList(7);

            //非同期実行
            yield return Async;

            //データ取得
            ResLpsWeatherInfo60List DataList = (ResLpsWeatherInfo60List)Async.Current;

            //データセット
            Wether.SetData(DataList);

            //最終更新時刻設定
            yield return Wether.LastUpdateTime = LpsDatetimeUtil.Now;
        }



        #endregion

        //====================================================================
        //
        //                        本日情報データ取得
        //                         
        //====================================================================
        #region 本日情報データ取得

        /// <summary>
        /// 本日情報データ取得
        /// </summary>
        public IEnumerator DataCollectAnniversaryDays()
        {
            //トークインスタンス取得
            DatAnniversaryDays InfoAnniversary = LiplisStatus.Instance.InfoAnniversary;

            //指定時間経過していなければ抜ける
            if (LpsDatetimeUtil.dec(InfoAnniversary.LastUpdateTime).AddMinutes(10) > DateTime.Now)
            {
                goto End;
            }

            //本日データがすでに入っていれば抜ける
            if (InfoAnniversary.CheckTodayDataExists())
            {
                goto End;
            }

            //最新データをダウンロードする
            yield return CoroutineHandler.StartStaticCoroutine(SetAnniversaryDays(InfoAnniversary));

        //終了ラベル
        End:;
        }

        /// <summary>
        /// 最新データをダウンロードする
        /// </summary>
        /// <returns></returns>
        public IEnumerator SetAnniversaryDays(DatAnniversaryDays InfoAnniversary)
        {
            //最新ニュースデータ取得
            var Async = ClalisWhatDayIsToday.GetAnniversaryDaysList();

            //非同期実行
            yield return Async;

            //データ取得
            ResWhatDayIsToday DataList = (ResWhatDayIsToday)Async.Current;

            //データリスト生成
            InfoAnniversary.SetData(DataList);

            //最終更新時刻設定
            yield return InfoAnniversary.LastUpdateTime = LpsDatetimeUtil.Now;
        }

        #endregion


        //====================================================================
        //
        //                        ニュースデータ収集関連
        //                         
        //====================================================================
        #region ニュースデータ収集関連
        /// <summary>
        /// データをセットする
        /// </summary>
        /// <param name="dataList"></param>
        public IEnumerator DataCollectNewTopic()
        {
            //指定時間経過していなければ抜ける
            if (LpsDatetimeUtil.dec(LiplisStatus.Instance.NewTopic.LastUpdateTime).AddMinutes(10) > DateTime.Now)
            {
                goto End;
            }

            //最終更新時刻設定
            yield return LiplisStatus.Instance.NewTopic.LastUpdateTime = LpsDatetimeUtil.Now;

            //最新データをダウンロードする
            yield return CoroutineHandler.StartStaticCoroutine(SetLastTopicMltData());

            //要求データセット
            LiplisCache.Instance.ImagePath.SetRequestUrlQ();

            //古いデータを削除する
            CoroutineHandler.StartStaticCoroutine(DeleteOldData());

        //終了ラベル
        End:;
        }

        private bool FlgRunSetLastTopicMltData = false;

        /// <summary>
        /// 最新データをダウンロードする
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetLastTopicMltData()
        {
            if (FlgRunSetLastTopicMltData)
            {
                goto End;
            }

            //実行中
            FlgRunSetLastTopicMltData = true;

            //トークインスタンス取得
            DatNewTopic NewTopic = LiplisStatus.Instance.NewTopic;

            //アシンク取得
            IEnumerator Async = DownloadData(NewTopic);

            //非同期実行
            yield return Async;

            //データ取得
            ResLpsTopicList DownloadLastData = (ResLpsTopicList)Async.Current;

            //NULLチェック
            if (DownloadLastData == null) { goto End; }
            if (DownloadLastData.topicList == null) { goto End; }

            //データ取得
            NewTopic.LastData = DownloadLastData;

            //キーリスト取得
            List<string> keyList = NewTopic.GetKeyList();

            //スライス
            yield return new WaitForSeconds(1f);

            //キーが無ければ入れる。
            foreach (MsgTopic topic in NewTopic.LastData.topicList)
            {
                //話題を積む条件は精査必要
                if (!keyList.Contains(topic.DataKey))
                {
                    if (LiplisSetting.Instance.Setting.CatCheck(topic.TopicClassification))
                    {
                        //対象カテゴリならトピックリストに追加する
                        NewTopic.TalkTopicList.InsertToNotDuplicate(topic.Clone(), 0);
                    }
                    else
                    {
                        //おしゃべり済みに追加
                        NewTopic.ChattedKeyList.AddToNotDuplicate(topic.Clone());
                    }
                }

                //1フレームスキップ
                yield return null;
            }

            //取得した結果、120件以下ならおかわり
            if (NewTopic.TalkTopicList.Count <= 120)
            {
                //終了にする
                FlgRunSetLastTopicMltData = false;

                CoroutineHandler.StartStaticCoroutine(SetLastTopicMltData());
            }

            //重複排除
            NewTopic.RemoveDuplicateTopicList();

        //終了ラベル
        End:;

            //終了にする
            FlgRunSetLastTopicMltData = false;
        }

        private IEnumerator DownloadData(DatNewTopic NewTopic)
        {
            //最新ニュースデータ取得
            IEnumerator Async;

            //件数が少ない場合はライト版で取得する
            if (NewTopic == null)
            {
                NewTopic = new DatNewTopic();
                NewTopic.LastData = new ResLpsTopicList();
                Async = ClalisForLiplisGetNewTopic2MltLight.GetNewTopic(new List<string>());
            }
            else if (NewTopic.LastData == null)
            {
                NewTopic.LastData = new ResLpsTopicList();
                Async = ClalisForLiplisGetNewTopic2MltLight.GetNewTopic(new List<string>());
            }
            else if (NewTopic.TalkTopicList.Count <= 25)
            {
                Async = ClalisForLiplisGetNewTopic2MltLight.GetNewTopic(new List<string>());
            }
            else
            {
                Async = ClalisForLiplisGetNewTopic2Mlt.GetNewTopic(new List<string>());
            }

            return Async;
        }


        /// <summary>
        /// 古いデータを削除する
        /// </summary>
        /// <returns></returns>
        private IEnumerator DeleteOldData()
        {
            //指定条件に合致するデータを削除する
            LiplisStatus.Instance.NewTopic.DeleteOldData();

            //削除まで終わったらセーブする
            CoroutineHandler.StartStaticCoroutine(Save());

            //古いサムネイルを削除する
            LiplisCache.Instance.ImagePath.Clean();

            //繰越
            yield return new WaitForSeconds(1f);
        }



        /// <summary>
        /// 最新のニュースリストを取得する
        /// </summary>
        /// <returns></returns>
        public IEnumerator SetLastNewsList()
        {
            //トークインスタンス取得
            DatNewsList newsList = LiplisStatus.Instance.NewsList;

            //指定時間経過していなければ抜ける
            if (LpsDatetimeUtil.dec(newsList.LastUpdateTime).AddMinutes(60) > DateTime.Now)
            {
                goto End;
            }

            //最新データをダウンロードする
            yield return CoroutineHandler.StartStaticCoroutine(SetLastNews());

        //終了ラベル
        End:;
        }

        /// <summary>
        /// 最新データをダウンロードする
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetLastNews()
        {
            //最新ニュースデータ取得
            var Async = ClalisForLiplisGetNewsList.GetNewsList();

            //非同期実行
            yield return Async;

            //データ取得
            ResLpsBaseNewsList DataList = (ResLpsBaseNewsList)Async.Current;

            //データセット
            LiplisStatus.Instance.NewsList.SetData(DataList);

            //取得ニュースデータ保存
            CoroutineHandler.StartStaticCoroutine(Save());

            //最終更新時刻設定
            LiplisStatus.Instance.NewsList.LastUpdateTime = LpsDatetimeUtil.Now;

            //要求データセット
            LiplisCache.Instance.ImagePath.SetRequestUrlQ();

            yield return null;
        }

        #endregion




    }
}
