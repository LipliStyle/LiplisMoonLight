//=======================================================================
//  ClassName : MsgWeatherInfoDtl60
//  概要      : 天気データ ウェザーインフォインターフェースのラッパー
//              シングルトン
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Cif.v60.Res;

namespace Assets.Scripts.LiplisSystem.Msg
{
    public class MsgWeatherInfoDtl60 : ResLpsWeatherInfoDtl60
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="rwi"></param>
        public MsgWeatherInfoDtl60(ResLpsWeatherInfoDtl60 rwi) : base(rwi)
        {

        }

        public MsgWeatherInfoDtl60(MsgWeatherInfoDtl60 rwi) : base(rwi)
        {

        }
    }
}
