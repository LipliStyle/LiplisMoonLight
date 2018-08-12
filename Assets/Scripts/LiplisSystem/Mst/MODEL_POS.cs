//=======================================================================
//  ClassName : MODEL_POS
//  概要      : モデルポジション
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿﻿﻿

using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Mst
{
    public class MODEL_POS
    {
        ///=============================
        ///Z座標デフォルト
        public const float LOCATION_Z = 000f;

        ///=============================
        ///各配置のX座標
        public const float LOCATION_X_MODERATOR = 310f;
        public const float LOCATION_X_RIGHT = -30f;
        public const float LOCATION_X_CENTER = -170f;
        public const float LOCATION_X_LEFT = -310f;

        /// <summary>
        /// 位置取得
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charLocationY"></param>
        /// <returns></returns>
        public static Vector3 GetPos(MST_CARACTER_POSITION position, float charLocationY)
        {
            //指定された位置から、配置すべき位置を示すベクターを返す
            if(position == MST_CARACTER_POSITION.Right)
            {
                //右配置
                return new Vector3(LOCATION_X_RIGHT, charLocationY, LOCATION_Z);
            }
            else if (position == MST_CARACTER_POSITION.Center)
            {
                //真ん中配置
                return new Vector3(LOCATION_X_CENTER, charLocationY, LOCATION_Z);
            }
            else if (position == MST_CARACTER_POSITION.Left)
            {
                //左配置
                return new Vector3(LOCATION_X_LEFT, charLocationY, LOCATION_Z);
            }
            else
            {
                //司会配置
                return new Vector3(LOCATION_X_MODERATOR, charLocationY, LOCATION_Z);
            }
        }
    }
}
