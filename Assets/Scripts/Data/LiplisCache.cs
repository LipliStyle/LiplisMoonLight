//=======================================================================
//  ClassName : LiplisCache
//  概要      : キャッシュデータ
//              シングルトン
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.Data.SubData;

namespace Assets.Scripts.Data
{
    public class LiplisCache
    {
        //テクスチャーキャッシュ
        public DatImagePath ImagePath;

        //====================================================================
        //
        //                          シングルトン管理
        //                         
        //====================================================================
        #region シングルトン管理

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static LiplisCache instance;
        public static LiplisCache Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LiplisCache();
                }

                return instance;
            }
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public LiplisCache()
        {
            DataLoad();
        }

        /// <summary>
        /// インスタンスをセットする
        /// </summary>
        /// <param name="Instance"></param>
        public static void SetInstance(LiplisCache Instance)
        {
            //インスタンスセット
            instance = Instance;

            //リストが初期化されない可能性があるので、データロードを呼んでおく。
            instance.DataLoad();
        }

        /// <summary>
        /// データロードする
        /// </summary>
        public void DataLoad()
        {
            //インスタンス化
            if (this.ImagePath == null) { this.ImagePath = new DatImagePath(); }

            //リストからディクショナリを復元する
            this.ImagePath.Recovery();
        }

        #endregion
    }
}
