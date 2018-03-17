//====================================================================
//  ClassName : DatLocation
//  概要      : 位置データ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Web.Clalis.v60;
using System;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatLocation
    {
        ///=============================
        ///プロパティ
        public uint LastUpdateTime;

        //国名
        public string CountryIsoCode;
        public string CountryName;
        public string CountryNameJp;

        //県名
        public string MostSpecificSubdivisionIsoCode;
        public string MostSpecificSubdivisionName;
        public string MostSpecificSubdivisionNameJp;

        //都市名
        public string CityName;
        public string CityNameJp;

        //郵便番号
        public string PostalCode;

        //経度/緯度
        public double? Latitude;
        public double? Longitude;

        /// <summary>
        /// データをセットする
        /// </summary>
        public void SetData(ResLpsLocationInfo location)
        {
            //取得失敗なら更新しない
            if(location == null)
            {
                return;
            }

            //データ更新
            this.CountryIsoCode                 = location.CountryIsoCode;
            this.CountryName                    = location.CountryName;
            this.CountryNameJp                  = location.CountryNameJp;
            this.MostSpecificSubdivisionIsoCode = location.MostSpecificSubdivisionIsoCode;
            this.MostSpecificSubdivisionName    = location.MostSpecificSubdivisionName;
            this.MostSpecificSubdivisionNameJp  = location.MostSpecificSubdivisionNameJp;
            this.CityName                       = location.CityName;
            this.CityNameJp                     = location.CityNameJp;
            this.PostalCode                     = location.PostalCode;
            this.Latitude                       = location.Latitude;
            this.Longitude                      = location.Longitude;

        }


    }
}
