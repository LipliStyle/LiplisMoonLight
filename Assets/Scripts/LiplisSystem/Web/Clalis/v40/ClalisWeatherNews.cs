//====================================================================
//  ClassName : ClalisWeatherNews
//  概要      : 天気ニュース取得
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Cif.v40.Res;
using Assets.Scripts.LiplisSystem.Com;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;

namespace Assets.Scripts.LiplisSystem.Web.Clalis.v40
{
    public static class ClalisWeatherNews
    {
        /// <summary>
        /// 対象地域の天気を取得する
        /// 現在未使用
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static int getNowWeather(string regionId)
        {
            try
            {
                NameValueCollection ps = new NameValueCollection();
                ps.Add("region", regionId);          //地域コードの指定

                //Jsonで結果取得
                string jsonText = HttpPost.sendPost(LpsDefine.LIPLIS_API_WEATHER_NEWS, ps);

                //APIの結果受け取り用クラス
                ResLpsWeatherInfo result = JsonConvert.DeserializeObject<ResLpsWeatherInfo>(jsonText);

                //結果を返す
                return ConvertWetherToCode(result.weather);
            }
            catch(Exception ex)
            {
                return 4;
            }
        }
        
        /// <summary>
        /// 天気をウェザーコードに変換する
        /// 現在未使用
        /// </summary>
        /// <param name="wether"></param>
        /// <returns></returns>
        private static int ConvertWetherToCode(string wether)
        {
            switch(wether)
            {
                case "":
                    return 0;
                default:
                    return 4;
            }
        }

    }
}
