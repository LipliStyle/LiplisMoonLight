//====================================================================
//  ClassName : DatEnvironmentInfomation
//  概要      : 環境情報
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================

using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Com;
using System;
using UnityEngine;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatEnvironmentInfomation
    {
        ///=============================
        ///プロパティ
        public float ScreanWidth;
        public float ScreanHeight;

        ///=============================
        ///タイトル座標
        public float TITLE_LOCATION_X;
        public float TITLE_LOCATION_Y;
        public float TITLE_LOCATION_Z;

        ///=============================
        ///選択モード
        public ContentCategoly SelectMode;

        ///=============================
        ///ペンディング
        public bool FlgTalkPending = false;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DatEnvironmentInfomation()
        {
            this.ScreanHeight = Screen.height;
            this.ScreanWidth = Screen.width;
            this.SelectMode = 0;
        }

        /// <summary>
        /// 差分倍率 横幅を返す
        /// </summary>
        /// <returns></returns>
        public float GetDifferentialMagnificationWidth()
        {
            return ScreanWidth / LpsDefine.SCREAN_DEFAULT_WIDTH;
        }
        public float GetDifferentialMagnificationWidthR()
        {
            return LpsDefine.SCREAN_DEFAULT_WIDTH/ ScreanWidth;
        }

        /// <summary>
        /// 差分倍率 横幅を返す
        /// </summary>
        /// <returns></returns>
        public float GetDifferentialMagnificationHeight()
        {
            return ScreanHeight / LpsDefine.SCREAN_SIZE_DEFAULT_HEIGTH;
        }
        public float GetDifferentialMagnificationHeightR()
        {
            return LpsDefine.SCREAN_SIZE_DEFAULT_HEIGTH / ScreanHeight;
        }

        /// <summary>
        /// ペンディング設定
        /// </summary>
        public void SetPendingOn()
        {
            FlgTalkPending = true;
        }
        public void SetPendingOff()
        {
            FlgTalkPending = false;
        }
    }
}
