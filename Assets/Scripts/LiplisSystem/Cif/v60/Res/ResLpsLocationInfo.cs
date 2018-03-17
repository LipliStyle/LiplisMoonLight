//=======================================================================
//  ClassName : ResLpsLocationInfo
//  概要      : ロケーションインフォ
//
//  LiplisSystemシステム      
//  Copyright(c) 2010-2017 sachin. All Rights Reserved. 
//=======================================================================
namespace Assets.Scripts.LiplisSystem.Cif.v60.Res
{
    public class ResLpsLocationInfo
    {
        //国名
        public string CountryIsoCode { get; set; }
        public string CountryName { get; set; }
        public string CountryNameJp { get; set; }

        //県名
        public string MostSpecificSubdivisionIsoCode { get; set; }
        public string MostSpecificSubdivisionName { get; set; }
        public string MostSpecificSubdivisionNameJp { get; set; }

        //都市名
        public string CityName { get; set; }
        public string CityNameJp { get; set; }

        //郵便番号
        public string PostalCode { get; set; }

        //経度/緯度
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int? AccuracyRadius { get; set; }
        public int? AverageIncome { get; set; }
        public bool HasCoordinates { get; set; }
        public int? MetroCode { get; set; }
        public int? PopulationDensity { get; set; }
        public string TimeZone { get; set; }
    }
}
