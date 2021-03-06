//=======================================================================
//  ClassName : FileManager
//  概要      : ファイルマネージャー
//
//
//  (c) Live2D Inc.All rights reserved.
//=======================================================================﻿﻿
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace Assets.Scripts.Utils
{
    public class FileManager
    {
        /// <summary>
        /// テキストアセットを読み込む
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TextAsset LoadTextAsset(string path)
        {
            return (TextAsset)Resources.Load(path, typeof(TextAsset));
        }

        /// <summary>
        /// テクスチャを読み込む
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string path)
        {
            return (Texture2D)Resources.Load(path, typeof(Texture2D));
        }

        /// <summary>
        /// 音声アセットを読み込む
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static AudioClip LoadAssetsSound(string filename)
        {
            AudioClip player = null;

            try
            {
                player = (AudioClip)(Resources.Load(filename)) as AudioClip;

            }
            catch (IOException e)
            {
                Debug.Log(e.StackTrace);
            }

            return player;
        }

        /// <summary>
        /// バイナリを読み込む
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] LoadBin(string path)
        {
            try
            {
                TextAsset ta = (TextAsset)Resources.Load(path, typeof(TextAsset));
                byte[] buf = ta.bytes;
                return buf;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return new byte[] { };
            }

        }

        /// <summary>
        /// 文字列を読み込む
        /// (主にjsonファイル?)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String LoadString(string path)
        {
            return System.Text.Encoding.GetEncoding("UTF-8").GetString(LoadBin(path));
        }

        /// <summary>
        /// ファイル名を取得する
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getFilename(string url)
        {
            return Regex.Replace(url, ".*/", "");
        }

        /// <summary>
        /// ディレクトリ名を取得する
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getDirName(string url)
        {
            return Regex.Replace(url, "(.*/)(.+)", "$1");
        }
    }
}

