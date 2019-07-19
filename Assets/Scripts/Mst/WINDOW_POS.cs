//=======================================================================
//  ClassName : WINDOW_POS
//  概要      : ウインドウポジション
//
//              画像ウインドウのポジション設定はImageWindow.SetIMageWindowLocation
//              インフォウインドウのポジション設定はInfoWindow.SetMoveTarget

//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿﻿﻿

using UnityEngine;
namespace Assets.Scripts.Define
{
    public class WINDOW_POS
    {
        ///=============================
        ///Z座標デフォルト
        public const float LOCATION_Z = 0f;
        public const float LOCATION_Y = 50f;

        ///=============================
        ///各配置のX座標
        public const float LOCATION_X_MODERATOR = 250f;
        public const float LOCATION_X_RIGHT = -80f;
        public const float LOCATION_X_CENTER = -230f;
        public const float LOCATION_X_LEFT = -380f;

        public static Vector3 GetPos(Vector3 pos, float offset)
        {
            return new Vector3(pos.x - 100, pos.y + offset, 0);
        }
    }
}
