//=======================================================================
//  ClassName : LpsImageLoader
//  概要      : イメージローダー
//
//  SatelliteServer
//  Copyright(c) 2009-2017 sachin. All Rights Reserved. 
//=======================================================================
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Com
{
    public static class LpsImageLoader
    {
        /// <summary>
        /// バイナリからテクスチャーを生成する
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Texture2D CreateTextureFromBinary(byte[] bytes)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            return texture;
        }
    }
}
