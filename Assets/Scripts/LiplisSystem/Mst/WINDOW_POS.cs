﻿//=======================================================================
//  ClassName : WINDOW_POS
//  概要      : ウインドウポジション
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿﻿﻿

using UnityEngine;
namespace Assets.Scripts.LiplisSystem.Mst
{
    public class WINDOW_POS
    {
        ///=============================
        ///Z座標デフォルト
        public const float LOCATION_Z = 0f;
        public const float LOCATION_Y = 50f;

        ///=============================
        ///各配置のX座標
        public const float LOCATION_X_MODERATOR = 300f;
        public const float LOCATION_X_RIGHT = -30f;
        public const float LOCATION_X_CENTER = -180f;
        public const float LOCATION_X_LEFT = -330f;

        /// <summary>
        /// 位置取得
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charLocationY"></param>
        /// <returns></returns>
        public static Vector3 GetPos(MST_CARACTER_POSITION position)
        {
            //指定された位置から、配置すべき位置を示すベクターを返す
            if (position == MST_CARACTER_POSITION.Right)
            {
                //右配置
                return new Vector3(LOCATION_X_RIGHT, LOCATION_Y, LOCATION_Z);
            }
            else if (position == MST_CARACTER_POSITION.Center)
            {
                //真ん中配置
                return new Vector3(LOCATION_X_CENTER, LOCATION_Y, LOCATION_Z);
            }
            else if (position == MST_CARACTER_POSITION.Left)
            {
                //左配置
                return new Vector3(LOCATION_X_LEFT, LOCATION_Y, LOCATION_Z);
            }
            else
            {
                //司会配置
                return new Vector3(LOCATION_X_MODERATOR, LOCATION_Y, LOCATION_Z);
            }
        }
    }
}
