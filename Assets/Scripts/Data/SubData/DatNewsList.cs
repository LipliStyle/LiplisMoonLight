//====================================================================
//  ClassName : DatNewTopic
//  概要      : 最新話題データ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.SubData
{
    public class DatNewsList
    {
        ///=============================
        ///プロパティ
        public uint LastUpdateTime;

        ///=============================
        ///最新ニュースデータ
        public ResLpsBaseNewsList LastNewsList;


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
            this.LastNewsList = DataList;
        }
        #endregion

    }
}
