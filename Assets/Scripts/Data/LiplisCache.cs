//=======================================================================
//  ClassName : LiplisCache
//  概要      : キャッシュデータ
//              シングルトン
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data.Cache;
using Assets.Scripts.Data.SubData;
using Assets.Scripts.DataChar;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    public class LiplisCache
    {
        //テクスチャーキャッシュ
        //public Dictionary<string, CacheDataTexture> CacheThumbnail;



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
            //this.CacheThumbnail = new Dictionary<string, CacheDataTexture>();
        }

        #endregion
    }
}
