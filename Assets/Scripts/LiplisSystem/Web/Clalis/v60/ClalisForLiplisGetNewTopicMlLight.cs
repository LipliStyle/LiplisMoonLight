//====================================================================
//  ClassName : ClalisForLiplisGetNewTopicMlLight
//  概要      : 最新話題データ取得
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.LiplisSystem.Web.Clalis.v60
{
    public class ClalisForLiplisGetNewTopicMlLight
    {
        /// <summary>
        /// 最新話題データ取得
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static IEnumerator GetNewTopic()
        {
            WWWForm ps = new WWWForm();

            string jsonText = "";

            using (UnityWebRequest request = UnityWebRequest.Post(LpsDefine.LIPLIS_API_NEW_TOPIC_ML_LIGHT, ps))
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
            yield return JsonConvert.DeserializeObject<ResLpsTopicList>(jsonText);
        }
    }
}
