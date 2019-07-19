//====================================================================
//  ClassName : ClalisWeatherNews
//  概要      : 天気ニュース取得
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using Assets.Scripts.Com;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.LiplisSystem.Web.Clalis.v60
{
    public class ClalisLocationWetherList
    {
        /// <summary>
        /// 対象地域の天気を取得する
        /// 現在未使用
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static IEnumerator GetWetherList(int days)
        {
            WWWForm ps = new WWWForm();
            string jsonText = "";

            using (UnityWebRequest request = UnityWebRequest.Post(LpsDefine.LIPLIS_API_LOCATION_WEATHER, ps))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    if (request.responseCode == 200)
                    {
                        // UTF8文字列として取得する
                        jsonText = LpsGzipUtil.Decompress(request.downloadHandler.data);

                    }

                }
            }

            //APIの結果受け取り用クラス
            yield return JsonConvert.DeserializeObject<ResLpsWeatherInfo60List>(jsonText);
        }
    }
}
