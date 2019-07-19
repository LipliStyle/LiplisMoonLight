//=======================================================================
//  ClassName : MODEL_POS
//  概要      : モデルポジション
//
//              会話ウインドウのポジションはWINDOW_POSによって決定されている。
//              画像ウインドウのポジション設定はImageWindow.SetIMageWindowLocation
//              インフォウインドウのポジション設定はInfoWindow.SetMoveTarget
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿﻿﻿

using Assets.Scripts.Data;
using System;
using UnityEngine;

namespace Assets.Scripts.Define
{
    public class MODEL_POS
    {
        ///=============================
        ///Z座標デフォルト
        public const float LOCATION_Z = 000f;

        ///=============================
        ///各配置のX座標
        public const float CHAR_DISTANCE_CENTER = 210;
        public const float CHAR_DISTANCE_LEFT_FITGH = 190;
        public const float CENTER_X_ODD = -35;
        public const float CENTER_X_EVEN =-100;

        ///=============================
        ///左右配置のX座標
        public const float LEFT_X = -340;
        public const float RIGHT_X = 340;

        /// <summary>
        /// 位置取得
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charLocationY"></param>
        /// <returns></returns>
        public static Vector3 GetPosLive2d30(Int32 position, float charLocationY, int modelNum)
        {
            if(LiplisSetting.Instance.Setting.CharArrangement == SETTING_CHAR_ARRANGEMENT.CENTER)
            {
                //真ん中配置
                return GetPosLive2d30Center(position, charLocationY, modelNum);
            }
            else
            {
                //デフォルトは左右配置とする。
                return GetPosLive2d30LeftRight(position, charLocationY, modelNum);
            }

           
        }

        //====================================================================
        //
        //                             左右配列
        //                         
        //====================================================================
        #region 左右配列

        public static Vector3 GetPosLive2d30LeftRight(Int32 position, float charLocationY, int modelNum)
        {
            if (modelNum % 2 == 0)
            {
                return new Vector3(GetPosLive2d30LeftRightEvenX(position, modelNum), charLocationY, LOCATION_Z);
            }
            else
            {
                return new Vector3(GetPosLive2d30LeftRightOddX(position, modelNum), charLocationY, LOCATION_Z);
            }
        }

        /// <summary>
        /// モデル数が偶数の場合のX算出
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charLocationY"></param>
        /// <param name="modelNum"></param>
        /// <returns></returns>
        public static float GetPosLive2d30LeftRightEvenX(int position, int modelNum)
        {
            //センター算出
            int center = modelNum / 2;

            //右端チェック
            if (IsRight(position))
            {
                return RIGHT_X;
            }

            //左端チェック
            if (IsLeft(position, modelNum))
            {
                return LEFT_X;
            }

            if(position >= center)
            {
                //センターより左
                //左端からのリスト距離×実距離70引く
                return LEFT_X + (((modelNum - 1) - position) * CHAR_DISTANCE_LEFT_FITGH);
            }
            else
            {
                //センターより右
                //右端からのリスト距離×実距離70引く
                return RIGHT_X - (position * CHAR_DISTANCE_LEFT_FITGH);
            }
        }

        /// <summary>
        /// モデル数が奇数の場合のX算出
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charLocationY"></param>
        /// <param name="modelNum"></param>
        /// <returns></returns>
        public static float GetPosLive2d30LeftRightOddX(int position, int modelNum)
        {
            //センター算出
            int center = (modelNum - 1) / 2;

            //右端チェック
            if (IsRight(position))
            {
                return RIGHT_X;
            }

            //左端チェック
            if (IsLeft(position, modelNum))
            {
                return LEFT_X;
            }

            if (position >= center)
            {
                //センターより左
                //左端からのリスト距離×実距離70引く
                return LEFT_X + (((modelNum - 1) - position) * CHAR_DISTANCE_LEFT_FITGH);
            }
            else
            {
                //センターより右
                //右端からのリスト距離×実距離70引く
                return RIGHT_X - (position * CHAR_DISTANCE_LEFT_FITGH);
            }
        }
        #endregion

        //====================================================================
        //
        //                             中央配列
        //                         
        //====================================================================
        #region 中央配列

        /// <summary>
        /// 中央整列の座標計算
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charLocationY"></param>
        /// <param name="modelNum"></param>
        /// <returns></returns>
        public static Vector3 GetPosLive2d30Center(Int32 position, float charLocationY, int modelNum)
        {
            if (modelNum % 2 == 0)
            {
                return new Vector3(GetPosLive2d30CenterEvenX(position, modelNum), charLocationY, LOCATION_Z);
            }
            else
            {
                return new Vector3(GetPosLive2d30CenterOddX(position, modelNum), charLocationY, LOCATION_Z);
            }
        }

        /// <summary>
        /// モデル数が偶数の場合のX算出
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charLocationY"></param>
        /// <param name="modelNum"></param>
        /// <returns></returns>
        public static float GetPosLive2d30CenterEvenX(int position, int modelNum)
        {
            //センター算出
            int center = modelNum / 2;

            //センターからのリスト距離×実距離70
            return CENTER_X_EVEN + ((center -position) * CHAR_DISTANCE_CENTER);
        }

        /// <summary>
        /// モデル数が奇数の場合のX算出
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charLocationY"></param>
        /// <param name="modelNum"></param>
        /// <returns></returns>
        public static float GetPosLive2d30CenterOddX(int position, int modelNum)
        {
            //センター算出
            int center = (modelNum -1)/ 2;
            
            //センターからのリスト距離×実距離70
            return CENTER_X_ODD + ((center - position) * CHAR_DISTANCE_CENTER);
        }
        #endregion


        //====================================================================
        //
        //                             共通処理
        //                         
        //====================================================================
        #region 共通処理
        /// <summary>
        /// 右端か?
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private static bool IsRight(int position)
        {
            return position == 0;
        }

        /// <summary>
        /// 左端か?
        /// </summary>
        /// <param name="position"></param>
        /// <param name="modelNum"></param>
        /// <returns></returns>
        private static bool IsLeft(int position, int modelNum)
        {
            return position == (modelNum - 1);
        }
        #endregion
    }
}
