//=======================================================================
//  ClassName : CacheDataTexture
//  概要      : DataTexture
//              テクスチャーキャッシュ
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿

using System;
using UnityEngine;

namespace Assets.Scripts.Data.Cache
{
    public class CacheDataTexture
    {
        public string Url;
        public DateTime RegisterDateTime;
        public Texture2D Tex;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="RegisterDateTime"></param>
        /// <param name="Tex"></param>
        public CacheDataTexture(string Url,DateTime RegisterDateTime,Texture2D Tex)
        {
            this.Url = Url;
            this.RegisterDateTime = RegisterDateTime;
            this.Tex = Tex;
        }

    }
}
