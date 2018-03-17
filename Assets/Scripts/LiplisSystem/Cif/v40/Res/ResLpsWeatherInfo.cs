//=======================================================================
//  Clalis4.0
//  ClassName : ResLpsWeatherInfo
//  概要      : レスポンスウェザーニュースJsonオブジェクトリスト
//
//  SatelliteServer
//  Copyright(c) 2009-2017 sachin. All Rights Reserved. 
//=======================================================================
using System;
using System.Runtime.InteropServices;

namespace Assets.Scripts.LiplisSystem.Cif.v40.Res
{
    [SerializableAttribute]
    [ComVisibleAttribute(true)]
    public class ResLpsWeatherInfo
    {
        ///=============================
        ///プロパティ
        public string isoCode { get; set; }
        public string regionCode { get; set; }
        public DateTime? date { get; set; }
        public string weather { get; set; }
        public int tempMax { get; set; }
        public int tempMin { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        #region ResLpsWeatherInfo
        public ResLpsWeatherInfo()
        {
            this.isoCode = "";
            this.regionCode = "";
            this.date = null;
            this.weather = "";
            this.tempMax = 0;
            this.tempMin = 0;
        }
        public ResLpsWeatherInfo(string isoCode, string regionCode, DateTime? date, string weather, int tempMax, int tempMin)
        {
            this.isoCode = isoCode;
            this.regionCode = regionCode;
            this.date = date;
            this.weather = weather;
            this.tempMax = tempMax;
            this.tempMin = tempMin;
        }
        public ResLpsWeatherInfo(ResLpsWeatherInfo rwi)
        {
            if(rwi != null)
            {
                this.isoCode = rwi.isoCode;
                this.regionCode = rwi.regionCode;
                this.date = rwi.date;
                this.weather = rwi.weather;
                this.tempMax = rwi.tempMax;
                this.tempMin = rwi.tempMin;
            }
            else
            {
                this.isoCode = "";
                this.regionCode = "";
                this.date = null;
                this.weather = "";
                this.tempMax = 0;
                this.tempMin = 0;
            }

        }
        #endregion
    }
}
