using Assets.Scripts.Com;
using Assets.Scripts.LiplisSystem.Com;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.LiplisSystem.Web
{
    public static class HttpPost
    {
        const int WEB_POST_TIMEOUT = 30000;
        const string WEB_POST_METHOD = "POST";
        const string WEB_POST_CONTENT_TYPE = "application/x-www-form-urlencoded";

        /// <summary>
        /// ポストを送信する
        /// (UTF-8のみ)
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string sendPost(string url, NameValueCollection postData)
        {
            //受信したデータを表示する
            return sendPost(url, getParamDataByte(postData));
        }
        public static string sendPost(string url, byte[] data)
        {
            //ウェブリクエストの取得
            HttpWebRequest req = getWebRequest(url, data.Length, WEB_POST_TIMEOUT);

            req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

            // ポスト・データの書き込み
            sendPostRequest(req, data);

            //受信したデータを表示する
            return getWebResponse(req);
        }
        public static string sendPost(string url, NameValueCollection postData, int postTimeout)
        {
            //受信したデータを表示する
            return sendPost(url, getParamDataByte(postData), postTimeout);
        }
        public static string sendPost(string url, byte[] data, int postTimeout)
        {
            //ウェブリクエストの取得
            HttpWebRequest req = getWebRequest(url, data.Length, postTimeout);

            req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

            // ポスト・データの書き込み
            sendPostRequest(req, data);

            //受信したデータを表示する
            return getWebResponse(req);
        }


        /// <summary>
        /// ポストした結果をストリームで取得する
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static Stream sendPostGetStream(string url, NameValueCollection postData)
        {
            byte[] data = getParamDataByte(postData);

            //ウェブリクエストの取得
            HttpWebRequest req = getWebRequest(url, data.Length, WEB_POST_TIMEOUT);

            req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

            // ポスト・データの書き込み
            sendPostRequest(req, data);

            return req.GetResponse().GetResponseStream();
        }

        /// <summary>
        /// ポストして、帰ってきたGzip圧縮ストリームをUTF8で解凍して返す
        /// 
        /// Gzip圧縮データ受け用のメソッド
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string sendPostAndDecompressUtf8(string url, NameValueCollection postData)
        {
            string t = LpsGzipUtil.Decompress(sendPostGetStream(url, postData));


            //解凍して返す
            return LpsGzipUtil.Decompress(sendPostGetStream(url, postData));
        }

        /// <summary>
        /// ポストを送信する
        /// (送りっぱなし)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        public static void throwPost(string url, NameValueCollection postData)
        {
            throwPost(url, getParamDataByte(postData));
        }
        public static void throwPost(string url, byte[] data)
        {
            //ウェブリクエストの取得
            HttpWebRequest req = getWebRequest(url, data.Length, WEB_POST_TIMEOUT);

            // ポスト・データの書き込み
            sendPostRequest(req, data);
        }

        ///====================================================================
        ///
        ///                       POSTの実行メソッド
        ///                         
        ///====================================================================

        /// <summary>
        /// getParamDataByte
        /// パラメータをバイトで取得する
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static byte[] getParamDataByte(NameValueCollection postData)
        {
            string param = "";

            //バラメータの取得
            foreach (string k in postData)
            {
                param += String.Format("{0}={1}&", k, postData[k]);
            }

            //パラメータをバイト変換
            return Encoding.UTF8.GetBytes(param);
        }

        /// <summary>
        /// getWebRequest
        /// ウェブリクエストを取得する
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static HttpWebRequest getWebRequest(string url, long paramLength, int postTimeout)
        {
            // リクエストの作成
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = postTimeout;
            req.Method = WEB_POST_METHOD;
            req.ContentType = WEB_POST_CONTENT_TYPE;
            req.ContentLength = paramLength;

            return req;
        }

        /// <summary>
        /// getWebResponse
        /// ウェブレスポンスを取得する
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string getWebResponse(HttpWebRequest req)
        {
            using (Stream resStream = req.GetResponse().GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(resStream, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }


        /// <summary>
        /// getWebRequest
        /// ウェブリクエストを取得する
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>]
        private static void sendPostRequest(HttpWebRequest req, byte[] pramData)
        {
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(pramData, 0, pramData.Length);
            }
        }
    }
}
