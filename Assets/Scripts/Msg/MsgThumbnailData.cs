//=======================================================================
//  ClassName : MsgThumbnailData
//  概要      : サムネイルデータ
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.Util.Ugui;
using System;
using UnityEngine;

namespace Assets.Scripts.Msg
{
    public class MsgThumbnailData
    {
        ///=============================
        ///画面制御
        public string thumbnailUrl;
        public Texture2D tex;      //最大表示テクスチャ
        public float MaxWidth = 0;    //幅
        public float MaxHeight = 0;   //高さ
        public float TexWidth = 0;    //幅
        public float TexHeight = 0;   //高さ

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="thumbnailUrl"></param>
        public MsgThumbnailData(string thumbnailUrl)
        {
            this.thumbnailUrl = thumbnailUrl;
        }

        /// <summary>
        /// テクスチャ生成
        /// </summary>
        /// <param name="tex"></param>
        public void CreateTex(Texture2D tex)
        {
            try
            {
                //テクスチャのロード
                this.tex = tex;
                this.TexWidth = tex.width;
                this.TexHeight = tex.height;

                //比率
                float rate = 0;

                //マックス値の計算
                //-----------------------------------------
                //スクリーン高さ
                float maxScreanHeight = LpsDefine.SCREAN_SIZE_DEFAULT_HEIGTH;

                //縦に合わせる
                rate = maxScreanHeight / (float)tex.height;

                //求めた比率から横算出
                this.MaxWidth = (float)tex.width * rate;

                //高さ固定
                this.MaxHeight = maxScreanHeight;

                //サイズ調整
                TextureScale.Bilinear(this.tex, (int)MaxWidth, (int)MaxHeight);
                //-----------------------------------------


                //表示サイズの計算
                //-----------------------------------------

                //スクリーン幅の33%
                float screanWidt = LpsDefine.SCREAN_DEFAULT_WIDTH / 3;

                //横に合わせる
                rate = screanWidt / (float)tex.width;

                //横固定 スクリーン幅の半分に固定
                this.TexWidth = screanWidt;

                //求めた比率から高さ算出
                this.TexHeight = (float)tex.height * rate;

                if (tex.width >= tex.height)
                {
                    //横長の場合は計算どおりのサイズでOK
                }
                else
                {
                    //972以上なら、高さをベースに設定
                    if (this.TexHeight > LpsDefine.SCREAN_SIZE_DEFAULT_HEIGTH80)
                    {
                        //縮小率を求める
                        float ReductionRatio = LpsDefine.SCREAN_SIZE_DEFAULT_HEIGTH80 / this.TexHeight;

                        //高さ固定
                        this.TexHeight = LpsDefine.SCREAN_SIZE_DEFAULT_HEIGTH80;

                        //横を更に縮小する
                        this.TexWidth = this.TexWidth * ReductionRatio;
                    }
                }
                //-----------------------------------------
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}
