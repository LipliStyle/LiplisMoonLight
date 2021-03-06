﻿//====================================================================
//  ClassName : UtilUnityPath
//  概要      : Unityでよく使うパスを取得する
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//====================================================================

using Assets.Scripts.LiplisSystem.Model.Setting;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class UtilUnityPath
    {
        ///=============================
        ///リソースパス
        public const string RESOURCES = "/Resources";

        /// <summary>
        /// アセットパスを返す
        /// </summary>
        /// <returns></returns>
        public static string GetAssetPath()
        {
            return Application.dataPath;
        }

        /// <summary>
        /// リソースパスを返す
        /// </summary>
        /// <returns></returns>
        public static string GetResourcesPath()
        {
            return Application.dataPath + RESOURCES;
        }

        /// <summary>
        /// リソースパスを返す
        /// </summary>
        /// <returns></returns>
        public static string GetStreamingAssetsPath()
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath;
#endif

#if UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets";
#else
            return Application.streamingAssetsPath;
#endif
        }

    }
}
