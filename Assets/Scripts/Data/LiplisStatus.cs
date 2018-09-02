//=======================================================================
//  ClassName : DataLiplisStatus
//  概要      : ステータスデータ
//              シングルトン
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data.SubData;
using Assets.Scripts.DataChar;
using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class LiplisStatus
    {
        //====================================================================
        //
        //                          シングルトン管理
        //                         
        //====================================================================
        #region シングルトン管理

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static LiplisStatus instance;
        public static LiplisStatus Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LiplisStatus();
                }

                return instance;
            }
        }

        /// <summary>
        /// インスタンスをセットする
        /// </summary>
        /// <param name="Instance"></param>
        public static void SetInstance(LiplisStatus Instance)
        {
            //インスタンスセット
            instance = Instance;

            //リストが初期化されない可能性があるので、データロードを呼んでおく。
            instance.DataLoad();
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public LiplisStatus()
        {
            DataLoad();
        }

        /// <summary>
        /// データロードする
        /// </summary>
        public void DataLoad()
        {
            if (this.InfoLocation     == null) { this.InfoLocation = new DatLocation(); }
            if (this.InfoWether       == null) {this.InfoWether = new DatWether(); }
            if (this.NewTopic         == null) { this.NewTopic = new　DatNewTopic(); }
            if (this.InfoAnniversary  == null) { this.InfoAnniversary = new DatAnniversaryDays(); }

            if (this.CharDataList     == null) { this.CharDataList = new CharacterDataList(); }
            if (this.NewsList         == null) { this.NewsList = new DatNewsList(); }

            this.EnvironmentInfo = new DatEnvironmentInfomation();
        }

        #endregion

        //環境情報
        public DatEnvironmentInfomation EnvironmentInfo;

        //キャラクターデータ
        public CharacterDataList CharDataList;

        //最終挨拶
        //最終ニュース
        public DatNewTopic NewTopic;

        //最終発言
        //最終起動
        //前回位置
        //移動平均
        //移動中

        //位置情報
        public DatLocation InfoLocation;

        //天気情報
        public DatWether InfoWether;

        //本日情報
        public DatAnniversaryDays InfoAnniversary;

        //流行りデータ
        //口調設定
        //口調変換処理
        //感情値
        //非同期処理

        //ニュースリスト
        public DatNewsList NewsList;

        //リソース解放最終日時
        public DateTime LastRunReleaseProcessing;
    }
}
