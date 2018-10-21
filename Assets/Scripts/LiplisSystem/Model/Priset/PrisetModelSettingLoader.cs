//====================================================================
//  ClassName : PrisetModelSettingLoader
//  概要      : プリセットモデルの設定を読み込む
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Model.Priset
{
    public class PrisetModelSettingLoader
    {
        /// <summary>
        /// 対象パスのJsonからクラスにロードする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathResources"></param>
        /// <returns></returns>
        public static T LoadClassFromJson<T>(string pathResources)
        {
            return JsonConvert.DeserializeObject<T>(LoadText(pathResources));
        }

        /// <summary>
        /// テクスチャロード
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="pathResources"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string pathResources)
        {
            var tex = new Texture2D(1, 1);
            tex.LoadImage(LoadBinary(pathResources));
            return tex;
        }

        /// <summary>
        /// 対象パスからテキストでデータを取得する
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string LoadText(string filePath)
        {
            if (filePath.Contains(":/"))
            {
                WWW www = new WWW(filePath);

                while (!www.isDone) { }

                string jsonData = Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);

                return jsonData;
            }
            else
            {
                return File.ReadAllText(filePath, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 対象パスからバイナリでデータを取得する
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] LoadBinary(string filePath)
        {
            if (filePath.Contains(":/"))
            {
                WWW www = new WWW(filePath);

                while (!www.isDone) { }

                return www.bytes;
            }
            else
            {
                return File.ReadAllBytes(filePath);
            }
        }

    }
}
