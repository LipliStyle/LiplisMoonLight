//=======================================================================
//  ClassName : TouchManager
//  概要      : タッチマネージャ
//              ピンチイン、ピンチアウト、2本指ドラッグなどによる画面操作を管理する
//
//  (c) Live2D Inc.All rights reserved.
//=======================================================================﻿﻿
using System;

namespace Assets.Scripts.Utils
{
    public class TouchManager
    {
        ///=============================
        ///タッチ制御プロパティ
        private float startY;// タッチを開始した位置
        private float startX;

        private float lastX = 0;// シングルタッチ時のxの値
        private float lastY = 0;// シングルタッチ時のyの値
        private float lastX1 = 0;// ダブルタッチ時の一つ目のxの値
        private float lastY1 = 0;// ダブルタッチ時の一つ目のyの値
        private float lastX2 = 0;// ダブルタッチ時の二つ目のxの値
        private float lastY2 = 0;// ダブルタッチ時の二つ目のyの値

        private float lastTouchDistance = -1;// 2本以上でタッチしたときの指の距離

        private float moveX;// 前回の値から今回の値へのxの移動距離。論理座標。
        private float moveY;// 前回の値から今回の値へのyの移動距離。論理座標。

        private float scale;// このフレームで掛け合わせる拡大率。拡大操作中以外は1。

        private bool touchSingle;// シングルタッチ時はtrue
        private bool flipAvailable;// フリップが有効かどうか


        ///====================================================================
        ///
        ///                         タッチ関連イベント
        ///                         
        ///====================================================================
        #region タッチ関連イベント

        /// <summary>
        /// タッチ開始時イベント。
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void TouchBegan(float x1, float y1, float x2, float y2)
        {
            float dist = Distance(x1, y1, x2, y2);
            float centerX = (lastX1 + lastX2) * 0.5f;
            float centerY = (-lastY1 - lastY2) * 0.5f;

            lastX = centerX;
            lastY = centerY;
            startX = centerX;
            startY = centerY;
            lastTouchDistance = dist;
            flipAvailable = true;
            touchSingle = false;
        }


        /// <summary>
        /// タッチ開始時イベント
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void TouchBegan(float x, float y)
        {
            lastX = x;
            lastY = -y;
            startX = x;
            startY = -y;
            lastTouchDistance = -1;
            flipAvailable = true;
            touchSingle = true;
        }


        /// <summary>
        /// ドラッグ時のイベント
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void TouchesMoved(float x, float y)
        {
            lastX = x;
            lastY = -y;
            lastTouchDistance = -1;
            touchSingle = true;
        }


        /// <summary>
        /// ドラッグ時のイベント
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void TouchesMoved(float x1, float y1, float x2, float y2)
        {
            float dist = Distance(x1, y1, x2, y2);
            float centerX = (x1 + x2) * 0.5f;
            float centerY = (-y1 + -y2) * 0.5f;

            if (lastTouchDistance > 0)
            {
                scale = (float)Math.Pow(dist / lastTouchDistance, 0.75);
                moveX = CalcShift(x1 - lastX1, x2 - lastX2);
                moveY = CalcShift(-y1 - lastY1, -y2 - lastY2);
            }
            else
            {
                scale = 1;
                moveX = 0;
                moveY = 0;
            }

            lastX = centerX;
            lastY = centerY;
            lastX1 = x1;
            lastY1 = -y1;
            lastX2 = x2;
            lastY2 = -y2;
            lastTouchDistance = dist;
            touchSingle = false;
        }

        #endregion

        ///====================================================================
        ///
        ///                         プロパティゲッター
        ///                         
        ///====================================================================
        #region プロパティゲッター

        public float GetCenterX()
        {
            return lastX;
        }


        public float GetCenterY()
        {
            return lastY;
        }


        public float GetDeltaX()
        {
            return moveX;
        }


        public float GetDeltaY()
        {
            return moveY;
        }


        public float GetStartX()
        {
            return startX;
        }


        public float GetStartY()
        {
            return startY;
        }


        public float GetScale()
        {
            return scale;
        }


        public float GetX()
        {
            return lastX;
        }


        public float GetY()
        {
            return lastY;
        }


        public float GetX1()
        {
            return lastX1;
        }


        public float GetY1()
        {
            return lastY1;
        }


        public float GetX2()
        {
            return lastX2;
        }


        public float GetY2()
        {
            return lastY2;
        }

        #endregion

        //====================================================================
        //
        //                         タッチ関連イベント
        //                         
        //====================================================================
        #region タッチ関連イベント
        /// <summary>
        /// 点1から点2への距離を求める
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        private float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }


        /// <summary>
        /// 二つの値から、移動量を求める。
        /// 違う方向の場合は移動量０。同じ方向の場合は、絶対値が小さい方の値を参照する
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private float CalcShift(float v1, float v2)
        {
            if ((v1 > 0) != (v2 > 0)) return 0;

            float fugou = v1 > 0 ? 1 : -1;
            float a1 = Math.Abs(v1);
            float a2 = Math.Abs(v2);
            return fugou * ((a1 < a2) ? a1 : a2);
        }

        /// <summary>
        /// フリックした距離
        /// </summary>
        /// <returns></returns>
        public float GetFlickDistance()
        {
            return Distance(startX, startY, lastX, lastY);
        }
        #endregion

        //====================================================================
        //
        //                         タッチ・フリック関連処理
        //                         
        //====================================================================
        #region タッチ・フリック関連処理
        /// <summary>
        /// シングルタッチかどうか？
        /// </summary>
        /// <returns></returns>
        public bool IsSingleTouch()
        {
            return touchSingle;
        }

        /// <summary>
        /// フリックが有効かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsFlickAvailable()
        {
            return flipAvailable;
        }

        /// <summary>
        /// フリックを無効化
        /// </summary>
        public void DisableFlick()
        {
            flipAvailable = false;
        }
        #endregion
    }
}

