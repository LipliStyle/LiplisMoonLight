//=======================================================================
//  ClassName : ResLpsWeatherInfoList
//  概要      : ロケーションインフォリスト
//
//  LiplisSystemシステム      
//  Copyright(c) 2010-2017 sachin. All Rights Reserved. 
//=======================================================================
using Assets.Scripts.LiplisSystem.Cif.v40.Res;
using System.Collections.Generic;


namespace Assets.Scripts.LiplisSystem.Cif.v60.Res
{
    public class ResLpsWeatherInfo60List
    {
        ///=============================
        ///プロパティ
        public string isoCode { get; set; }
        public string regionCode { get; set; }

        ///=============================
        //天気リスト
        public List<ResLpsWeatherInfo60> WetherList { get; set; }
        public List<ResLpsWeatherInfoDtl60> WetherDescriptionList { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public ResLpsWeatherInfo60List()
        {
            WetherList = new List<ResLpsWeatherInfo60>();
            WetherDescriptionList = new List<ResLpsWeatherInfoDtl60>();
        }
    }
}