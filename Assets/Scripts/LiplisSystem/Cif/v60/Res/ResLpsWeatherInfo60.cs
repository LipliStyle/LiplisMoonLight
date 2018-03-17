//=======================================================================
//  Clalis6.0
//  ClassName : ResLpsWeatherInfo60
//  概要      : レスポンス天気情報改
//
//  SatelliteServer
//  Copyright(c) 2009-2017 sachin. All Rights Reserved. 
//=======================================================================
using System;

namespace Assets.Scripts.LiplisSystem.Cif.v60.Res
{
    [Serializable]
    public class ResLpsWeatherInfo60
    {
        ///=============================
        ///プロパティ
        public DateTime? date;
        public string weather;
        public int tempMax;
        public int tempMin;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="rwi"></param>
        public ResLpsWeatherInfo60(ResLpsWeatherInfo60 rwi)
        {
            if (rwi != null)
            {
                this.date = rwi.date;
                this.weather = rwi.weather;
                this.tempMax = rwi.tempMax;
                this.tempMin = rwi.tempMin;
            }
            else
            {
                this.date = null;
                this.weather = "";
                this.tempMax = 0;
                this.tempMin = 0;
            }

        }

    }
}