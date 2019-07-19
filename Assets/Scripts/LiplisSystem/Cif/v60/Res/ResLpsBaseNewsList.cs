//=======================================================================
//  ClassName : ResLpsBaseNewsList
//  概要      : ベースニュースリスト
//
//  SatelliteServer
//  Copyright(c) 2009-2017 sachin.
//=======================================================================
using Assets.Scripts.Msg;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Cif.v60.Res
{
    [Serializable]
    public class ResLpsBaseNewsList
    {
        ///=============================
        /// ニュースリスト
        public List<MsgBaseNewsData> NewsList;          //ニュースリスト
        public List<MsgBaseNewsData> MatomeList;        //2chまとめリスト
        public List<MsgBaseNewsData> ReTweetList;       //リツイートリスト
        public List<MsgBaseNewsData> PictureList;       //ピクチャーリスト
        public List<MsgBaseNewsData> HashList;          //ハッシュリスト
        public List<MsgBaseNewsData> HotWordList;       //ホットワードリスト

        ///=============================
        /// ニュースリストインデックス
        public int NewsListIdx;
        public int MatomeListIdx;
        public int ReTweetListIdx;
        public int PictureListIdx;
        public int HashListIdx;
        public int HotWordListIdx;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public ResLpsBaseNewsList()
        {
            NewsList = new List<MsgBaseNewsData>();
            MatomeList = new List<MsgBaseNewsData>();
            ReTweetList = new List<MsgBaseNewsData>();
            PictureList = new List<MsgBaseNewsData>();
            HashList = new List<MsgBaseNewsData>();
            HotWordList = new List<MsgBaseNewsData>();

            NewsListIdx = 0;
            MatomeListIdx = 0;
            ReTweetListIdx = 0;
            PictureListIdx = 0;
            HashListIdx = 0;
            HotWordListIdx = 0;
        }

        /// <summary>
        /// すべてのリストを取得する
        /// </summary>
        /// <returns></returns>
        //public List<MsgBaseNewsData> GetAllList()
        //{
        //    List<MsgBaseNewsData> resultList = new List<MsgBaseNewsData>();

        //    resultList.AddRange(NewsList);
        //    resultList.AddRange(MatomeList);
        //    resultList.AddRange(ReTweetList);
        //    resultList.AddRange(PictureList);
        //    resultList.AddRange(HashList);
        //    resultList.AddRange(HotWordList);

        //    return resultList;
        //}



        /// <summary>
        /// リストの更新
        /// 
        /// 中身のあるものだけ更新する
        /// </summary>
        /// <param name="NewList"></param>
        public void UpdateList(ResLpsBaseNewsList NewList)
        {
            if (NewList.NewsList.Count > 0)
            {
                this.NewsList = NewList.NewsList;
            }

            if (NewList.MatomeList.Count > 0)
            {
                this.MatomeList = NewList.MatomeList;
            }

            if (NewList.ReTweetList.Count > 0)
            {
                this.ReTweetList = NewList.ReTweetList;
            }

            if (NewList.PictureList.Count > 0)
            {
                this.PictureList = NewList.PictureList;
            }

            if (NewList.HashList.Count > 0)
            {
                this.HashList = NewList.HashList;
            }

            if (NewList.HotWordList.Count > 0)
            {
                this.HotWordList = NewList.HotWordList;
            }
        }

        /// <summary>
        /// ニュースリストからIDを取得する
        /// </summary>
        /// <returns></returns>
        public string GetNewsKeyFromNewsList()
        {
            return GetNewsKeyFromNewsList(ref NewsListIdx, NewsList);
        }
        public string GetNewsKeyFromMatomeList()
        {
            return GetNewsKeyFromNewsList(ref MatomeListIdx, MatomeList);
        }
        public string GetNewsKeyFromReTweetList()
        {
            return GetNewsKeyFromNewsList(ref ReTweetListIdx, ReTweetList);
        }
        public string GetNewsKeyFromPictureList()
        {
            return GetNewsKeyFromNewsList(ref PictureListIdx, PictureList);
        }
        public string GetNewsKeyFromHashList()
        {
            return GetNewsKeyFromNewsList(ref HashListIdx, HashList);
        }
        public string GetNewsKeyFromHotWordList()
        {
            return GetNewsKeyFromNewsList(ref HotWordListIdx, HotWordList);
        }

        /// <summary>
        /// 対象インデックスの
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="targetList"></param>
        /// <returns></returns>
        public string GetNewsKeyFromNewsList(ref int idx, List<MsgBaseNewsData> targetList)
        {
            if (targetList.Count > 0)
            {
                if (idx > targetList.Count - 1)
                {
                    //次のニュースリストidxを1に設定
                    idx = 1;

                    //0番目のキーを返す
                    return targetList[0].DATAKEY;
                }
                else
                {
                    //現在のインデックスのキーを返し、インクリメント
                    return targetList[idx++].DATAKEY;
                }
            }
            else
            {
                //空を返す
                return "";
            }
        }
    }
}
