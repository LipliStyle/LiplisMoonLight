//====================================================================
//  ClassName : WebImage
//  概要      : ウェブ上の画像インスタンス
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2019 sachin.
//====================================================================
using Assets.Scripts.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.LiplisSystem.Web.Clalis
{
    public  class WebImage
    {
        /// <summary>
        /// URLに指定された画像をダウンロードし、テクスチャとして返す
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static IEnumerator GetImage(string url)
        {
            float addTime = 0f;//タイムアウト監視

            WWWForm ps = new WWWForm();

            using (UnityWebRequest request = UnityWebRequest.Post(url, ps))
            {
                AsyncOperation op = request.SendWebRequest();
                while (true)
                {
                    if (op.isDone == false)
                    {
                        //通信中
                        yield return null;

                        addTime += Time.deltaTime;

                        if ((int)addTime >= 3)
                        {
                            //3秒経過したらループを抜ける

                            //通信を切断する
                            request.Abort();
                            break;
                        }

                    }
                    else
                    {
                        //通信完了でループを抜ける
                        break;
                    }

                }

                if (request.responseCode == 200)
                {
                    yield return TextureUtil.GetTextureFromFile(request.downloadHandler.data);
                }
                else
                {
                    yield return null;
                }

                //yield return request.SendWebRequest();

                //if (request.isNetworkError)
                //{
                //    Debug.Log(request.error);

                //    yield return null;
                //}
                //else
                //{
                //    if (request.responseCode == 200)
                //    {
                //        yield return TextureUtil.GetTextureFromFile(request.downloadHandler.data);
                //    }
                //    else
                //    {
                //        yield return null;
                //    }
                //}
            }
        }
    }
}
