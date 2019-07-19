//=======================================================================
//  ClassName : TextureUtil
//  概要      : テクスチャユーティル
//              
//
//  LiplisMoonlight
//  Create 2019/05/05
//
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿

using System.IO;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class TextureUtil
    {
        /// <summary>
        /// ファイルからテクスチャを生成して返す
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D GetTextureFromFile(string path)
        {
            return GetTextureFromFile(ReadPngFile(path));
        }
        public static Texture2D GetTextureFromFile(byte[] readBinary)
        {
            //int pos = 16; // 16バイトから開始

            //int width = 0;
            //for (int i = 0; i < 4; i++)
            //{
            //    width = width * 256 + readBinary[pos++];
            //}

            //int height = 0;
            //for (int i = 0; i < 4; i++)
            //{
            //    height = height * 256 + readBinary[pos++];
            //}

            //Texture2D texture = new Texture2D(width, height);

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(readBinary);

            return texture;
        }


        /// <summary>
        /// Pngの読み込み
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadPngFile(string path)
        {
            using (BinaryReader bin = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
            {
                return bin.ReadBytes((int)bin.BaseStream.Length);
            }
        }

        /// <summary>
        /// pngで保存する
        /// </summary>
        public static void SavePng(Texture2D texture, string path)
        {
            //PNGで保存
            var png = texture.EncodeToPNG();
            File.WriteAllBytes(path, png);
        }
    }
}
