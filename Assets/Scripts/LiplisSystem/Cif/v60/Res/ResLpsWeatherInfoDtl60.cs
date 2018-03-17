//=======================================================================
//  ClassName : ResLpsWeatherInfoList
//  概要      : 天気明細データ
//
//  LiplisSystemシステム      
//  Copyright(c) 2010-2017 sachin. All Rights Reserved. 
//=======================================================================
using System;

namespace Assets.Scripts.LiplisSystem.Cif.v60.Res
{
    [Serializable]
    public class ResLpsWeatherInfoDtl60
    {
        ///=============================
        ///プロパティ
        public DateTime? date;
        public string timeId    ;
        public string dayOfWeek ;
        public string weather   ;
        public int tempMax      ;
        public int tempMin      ;
        public int chanceOfRain ;
        public string wind      ;
        public string wave      ;
        public string comment   ;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="rwi"></param>
        public ResLpsWeatherInfoDtl60(ResLpsWeatherInfoDtl60 rwi)
        {
            if (rwi != null)
            {
                this.date = rwi.date;
                this.weather = rwi.weather;
                this.tempMax = rwi.tempMax;
                this.tempMin = rwi.tempMin;
                this.chanceOfRain = rwi.chanceOfRain;
                this.wind = rwi.wind;
                this.wave = rwi.wave;
                this.comment = rwi.comment;
            }
            else
            {
                this.date         = null;
                this.weather      = "";
                this.tempMax      = 0;
                this.tempMin      = 0;
                this.chanceOfRain = 0;
                this.wind         = "";
                this.wave         = "";
                this.comment      = "";
            }
        }
    }
}