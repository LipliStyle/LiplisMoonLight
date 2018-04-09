//=======================================================================
//  ClassName : ResLpsBaseNewsList
//  概要      : ベースニュースリスト
//
//  SatelliteServer
//  Copyright(c) 2009-2017 sachin. All Rights Reserved. 
//=======================================================================
using Assets.Scripts.LiplisSystem.Msg;
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Cif.v60.Res
{
    public class ResLpsBaseNewsList
    {
        public List<MsgBaseNewsData> NewsList;          //ニュースリスト
        public List<MsgBaseNewsData> MatomeList;        //2chまとめリスト
        public List<MsgBaseNewsData> ReTweetList;       //リツイートリスト
        public List<MsgBaseNewsData> PictureList;       //ピクチャーリスト
        public List<MsgBaseNewsData> HashList;          //ハッシュリスト
        public List<MsgBaseNewsData> HotWordList;       //ホットワードリスト

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
        }

        /// <summary>
        /// すべてのリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<MsgBaseNewsData> GetAllList()
        {
            List<MsgBaseNewsData> resultList = new List<MsgBaseNewsData>();

            resultList.AddRange(NewsList);
            resultList.AddRange(MatomeList);
            resultList.AddRange(ReTweetList);
            resultList.AddRange(PictureList);
            resultList.AddRange(HashList);
            resultList.AddRange(HotWordList);

            return resultList;
        }
    }
}
