//====================================================================
//  ClassName : ClalisForLiplisGetNewTopic2Mlt
//  概要      : 最新話題データ取得
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.LiplisSystem.Web.Clalis.v60
{
    public class ClalisForLiplisGetNewTopic2MltLight
    {
        /// <summary>
        /// 最新話題データ取得
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static IEnumerator GetNewTopic(List<string> toneUrlList)
        {
            WWWForm ps = new WWWForm();

            //レックトピック生成
            string json = JsonConvert.SerializeObject(TopicUtil.CreateReqTopic(toneUrlList));

            //パラメータ生成
            ps.AddField("reqmsg", json); //トーンURL

            string jsonText = "";

            using (UnityWebRequest request = UnityWebRequest.Post(LpsDefine.LIPLIS_API_NEW_TOPIC_2_MLT_LIGHT, ps))
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