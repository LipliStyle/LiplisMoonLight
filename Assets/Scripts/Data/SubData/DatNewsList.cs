//====================================================================
//  ClassName : DatNewTopic
//  概要      : 最新話題データ
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using Assets.Scripts.Com;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.Msg;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatNewsList
    {
        ///=============================
        ///プロパティ
        public uint LastUpdateTime;

        ///=============================
        ///最新ニュースデータ
        public ResLpsBaseNewsList LastNewsList;

        ///=============================
        ///表示用ニュースキュー
        public ResLpsBaseNewsList LastNewsQ;



        //====================================================================
        //
        //                            データセット
        //                         
        //====================================================================
        #region データセット
        /// <summary>
        /// データをセットする
        /// </summary>
        /// <param name="dataList"></param>
        public void SetData(ResLpsBaseNewsList DataList)
        {
            //NULLチェック
            if (DataList == null)
            {
                return;
            }

            //最新ニュースリストセット
            if(LastNewsList == null)
            {
                this.LastNewsList = DataList;
            }
            else
            {
                this.LastNewsList.UpdateList(DataList);
            }

            //ニュースキューを更新する
            SetQ(DataList);
        }
        #endregion

        //====================================================================
        //
        //                            ニュースキュー
        //                         
        //====================================================================
        #region ニュースキュー
        /// <summary>
        /// データをセットする
        /// </summary>
        /// <param name="dataList"></param>
        public void SetQ(ResLpsBaseNewsList DataList)
        {
            if (this.LastNewsQ == null)
            {
                this.LastNewsQ = new ResLpsBaseNewsList();
            }

            RebuildTargetList(ref this.LastNewsQ.NewsList, DataList.NewsList);
            RebuildTargetList(ref this.LastNewsQ.MatomeList, DataList.MatomeList);
            RebuildTargetList(ref this.LastNewsQ.ReTweetList, DataList.ReTweetList);
            RebuildTargetList(ref this.LastNewsQ.PictureList, DataList.PictureList);
            RebuildTargetList(ref this.LastNewsQ.HashList, DataList.HashList);
            RebuildTargetList(ref this.LastNewsQ.HotWordList, DataList.HotWordList);
        }

        /// <summary>
        /// ニュースリストをリビルドする
        /// </summary>
        /// <param name="targetMyList"></param>
        /// <param name="NewDataList"></param>
        public void RebuildTargetList(ref List<MsgBaseNewsData> targetMyList, List<MsgBaseNewsData> NewDataList)
        {
            //NULLチェック
            if (targetMyList == null)
            {
                targetMyList = new List<MsgBaseNewsData>();
            }

            //結果リスト
            List<MsgBaseNewsData> ExistsList = new List<MsgBaseNewsData>();
            List<MsgBaseNewsData> NewList = new List<MsgBaseNewsData>();
            List<MsgBaseNewsData> ResList = new List<MsgBaseNewsData>();

            //既存データを回し、新しいリストに無いものを除去する
            foreach (var data in targetMyList)
            {
                if(ContainsList(NewDataList, data.DATAKEY))
                {
                    ExistsList.Add(data);
                }
            }

            //新しいデータを末尾に追加する
            foreach (var data in NewDataList)
            {
                //新しいニュースリストに存在した場合は、Existsリスト、存在しない場合はNewListに入れる
                if (!ContainsList(targetMyList, data.DATAKEY))
                {
                    NewList.Add(data);
                }
            }

            //既存データを残し、新しいデータを末尾にに入れる
            ResList.AddRange(ExistsList);
            ResList.AddRange(NewList);
 

            //ニュースリストを更新
            targetMyList = ResList;
        }

        /// <summary>
        /// データキーをデキューする。
        /// </summary>
        /// <param name="targetMyList"></param>
        /// <returns></returns>
        public string DequeueKey(ref List<MsgBaseNewsData> targetMyList)
        {
            //先頭から1個取り出す
            MsgBaseNewsData data = targetMyList.Dequeue();

            //最後尾に追加
            targetMyList.Add(data);

            //データキーを返す
            return data.DATAKEY;
        }

        /// <summary>
        /// ニュースキーを取得する
        /// </summary>
        /// <returns></returns>
        public string DequeueKeyNewsList()
        {
            return DequeueKey(ref LastNewsQ.NewsList);
        }
        public string DequeueKeyMatomeList()
        {
            return DequeueKey(ref LastNewsQ.MatomeList);
        }
        public string DequeueKeyReTweetList()
        {
            return DequeueKey(ref LastNewsQ.ReTweetList);
        }
        public string DequeueKeyPictureList()
        {
            return DequeueKey(ref LastNewsQ.PictureList);
        }
        public string DequeueKeyHashList()
        {
            return DequeueKey(ref LastNewsQ.HashList);
        }
        public string DequeueKeyHotWordList()
        {
            return DequeueKey(ref LastNewsQ.HotWordList);
        }

        /// <summary>
        /// 対象データを最後尾に付ける
        /// </summary>
        /// <param name="targetMyList"></param>
        /// <returns></returns>
        public void DequeueKey(ref List<MsgBaseNewsData> targetMyList, string DATAKEY)
        {
            //ターゲットデータ
            MsgBaseNewsData targetData = null;

            //新リスト
            List<MsgBaseNewsData> newList = new List<MsgBaseNewsData>();

            foreach (MsgBaseNewsData newsData in targetMyList)
            {
                //HITチェック
                if(newsData.DATAKEY == DATAKEY)
                {
                    targetData = newsData;
                }
                else
                {
                    newList.Add(newsData);
                }
            }

            //NULLチェック
            if(targetData == null)
            {
                return;
            }

            //最後尾に追加
            newList.Add(targetData);

            //リスト設定
            targetMyList = newList;
        }
        /// <summary>
        /// ニュースキーを取得する
        /// </summary>
        /// <returns></returns>
        public void DequeueKeyNewsList(string DATAKEY)
        {
            DequeueKey(ref LastNewsQ.NewsList,DATAKEY);
        }
        public void DequeueKeyMatomeList(string DATAKEY)
        {
            DequeueKey(ref LastNewsQ.MatomeList, DATAKEY);
        }
        public void DequeueKeyReTweetList(string DATAKEY)
        {
            DequeueKey(ref LastNewsQ.ReTweetList, DATAKEY);
        }
        public void DequeueKeyPictureList(string DATAKEY)
        {
            DequeueKey(ref LastNewsQ.PictureList, DATAKEY);
        }
        public void DequeueKeyHashList(string DATAKEY)
        {
            DequeueKey(ref LastNewsQ.HashList, DATAKEY);
        }
        public void DequeueKeyHotWordList(string DATAKEY)
        {
            DequeueKey(ref LastNewsQ.HotWordList, DATAKEY);
        }
        public void DequeueKeyNewsList(ContentCategoly NewsSource,string DATAKEY)
        {
            if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.news)
            {
                DequeueKey(ref LastNewsQ.NewsList, DATAKEY);
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.matome)
            {
                DequeueKey(ref LastNewsQ.MatomeList, DATAKEY);
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.retweet)
            {
                DequeueKey(ref LastNewsQ.ReTweetList, DATAKEY);
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.hotPicture)
            {
                DequeueKey(ref LastNewsQ.PictureList, DATAKEY);
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.hotHash)
            {
                DequeueKey(ref LastNewsQ.HashList, DATAKEY);
            }
        }


        /// <summary>
        /// ニュースデータを検索する
        /// </summary>
        /// <param name="targetMyList"></param>
        /// <param name="RemoveKey"></param>
        /// <returns></returns>
        public MsgBaseNewsData SearchList(List<MsgBaseNewsData> targetMyList, string RemoveKey)
        {
            //ニューリスト
            List<MsgBaseNewsData> newList = new List<MsgBaseNewsData>();

            //ターゲットリストを回し、対象データを検索する
            foreach (var item in targetMyList)
            {
                if (item.DATAKEY == RemoveKey)
                {
                    return item;
                }
            }

            //見つからなければNULLを返す
            return null;
        }

        /// <summary>
        /// ニュースリストのキー存在チェック
        /// </summary>
        /// <param name="DATAKEY"></param>
        /// <returns></returns>
        public bool ContainsNewsList(string DATAKEY)
        {
            return ContainsList(this.LastNewsQ.NewsList, DATAKEY);
        }

        /// <summary>
        /// まとめサイトリストのキー存在チェック
        /// </summary>
        /// <param name="DATAKEY"></param>
        /// <returns></returns>
        public bool ContainsMatomeList(string DATAKEY)
        {
            return ContainsList(this.LastNewsQ.MatomeList, DATAKEY);
        }

        /// <summary>
        /// リツイートのキー存在チェック
        /// </summary>
        /// <param name="DATAKEY"></param>
        /// <returns></returns>
        public bool ContainsReTweetList(string DATAKEY)
        {
            return ContainsList(this.LastNewsQ.ReTweetList, DATAKEY);
        }

        /// <summary>
        /// ピクチャーの存在チェック
        /// </summary>
        /// <param name="DATAKEY"></param>
        /// <returns></returns>
        public bool ContainsPictureList(string DATAKEY)
        {
            return ContainsList(this.LastNewsQ.PictureList, DATAKEY);
        }

        /// <summary>
        /// ハッシュリストの存在チェック
        /// </summary>
        /// <param name="DATAKEY"></param>
        /// <returns></returns>
        public bool ContainsHashList(string DATAKEY)
        {
            return ContainsList(this.LastNewsQ.HashList, DATAKEY);
        }

        /// <summary>
        /// ホットワードの存在チェック
        /// </summary>
        /// <param name="DATAKEY"></param>
        /// <returns></returns>
        public bool ContainsHotWordList(string DATAKEY)
        {
            return ContainsList(this.LastNewsQ.HotWordList, DATAKEY);
        }

        /// <summary>
        /// 対象リストのキーチェックを行う
        /// </summary>
        /// <param name="targetList"></param>
        /// <param name="DATAKEY"></param>
        /// <returns></returns>
        public bool ContainsList(List<MsgBaseNewsData> targetList, string DATAKEY)
        {
            foreach (var item in targetList)
            {
                if (item.DATAKEY == DATAKEY)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
