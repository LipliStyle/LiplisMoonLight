//====================================================================
//  ClassName : AssetLoader
//  概要      : アセットローダー
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Model.Priset;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class AssetLoader
    {

        /// <summary>
        /// Loads asset.
        /// </summary>
        /// <param name="assetType">Asset type.</param>
        /// <param name="absolutePath">Path to asset.</param>
        /// <returns>The asset on success; <see langword="null"/> otherwise.</returns>
        public static T LoadAsset<T>(string absolutePath) where T : class
        {
            return LoadAsset(typeof(T), absolutePath) as T;
        }

        /// <summary>
        /// Loads asset.
        /// </summary>
        /// <param name="assetType">Asset type.</param>
        /// <param name="absolutePath">Path to asset.</param>
        /// <returns>The asset on success; <see langword="null"/> otherwise.</returns>
        public static object LoadAsset(Type assetType, string absolutePath)
        {
            if (assetType == typeof(byte[]))
            {
                return PrisetModelSettingLoader.LoadBinary(absolutePath);
            }
            else if (assetType == typeof(string))
            {
                return PrisetModelSettingLoader.LoadText(absolutePath);
            }
            else if (assetType == typeof(Texture2D))
            {
                return PrisetModelSettingLoader.LoadTexture(absolutePath);
            }

            // Fail hard.
            throw new NotSupportedException();
        }
    }
}
