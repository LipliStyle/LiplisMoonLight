//=======================================================================
//  ClassName : GameObjectUtils
//  概要      : ゲームオブジェクトユーティリティ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class GameObjectUtils
    {
        /// <summary>
        /// 指定された GameObject を複製して返します
        /// </summary>
        public static GameObject Clone(GameObject go)
        {
            var clone = GameObject.Instantiate(go) as GameObject;
            clone.transform.parent = go.transform.parent;
            clone.transform.localPosition = go.transform.localPosition;
            clone.transform.localScale = go.transform.localScale;
            return clone;
        }

        /// <summary>
        /// テクスチャをコピーする
        /// </summary>
        /// <param name="tex"></param>
        /// <returns></returns>
        public static Texture2D Clone(Texture2D tex)
        {
            Texture2D texCopy = new Texture2D(tex.width, tex.height, tex.format, tex.mipmapCount > 1);

            texCopy.LoadRawTextureData(tex.GetRawTextureData());

            texCopy.Apply();

            return texCopy;
        }

    }
}
