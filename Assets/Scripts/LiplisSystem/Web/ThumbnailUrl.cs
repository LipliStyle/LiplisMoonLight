//=======================================================================
//  ClassName : ThumbnailUrl
//  概要      : サムネイルURL生成クラス
//              
//  LiplisMoonlight
//  Create 2018/06/23
//
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Com;

namespace Assets.Scripts.LiplisSystem.Web
{
    public class ThumbnailUrl
    {
        /// <summary>
        /// サムネイル生成
        /// </summary>
        /// <param name="ThumbnailUrl"></param>
        /// <returns></returns>
        public static string CreateThumbnailUrl(string ThumbnailUrl)
        {
            return LpsDefine.LIPLIS_API_THUMBNAIL_PROXY + "?url=" + ThumbnailUrl;
        }

        /// <summary>
        /// サムネイル生成
        /// </summary>
        /// <param name="ThumbnailUrl"></param>
        /// <returns></returns>
        public static string CreateThumbnailSmallUrl(string ThumbnailUrl)
        {
            return LpsDefine.LIPLIS_API_THUMBNAIL_PROXY_SMALL + "?url=" + ThumbnailUrl;
        }

        /// <summary>
        /// リスト用サムネイルURL生成
        /// </summary>
        /// <param name="ThumbnailUrl"></param>
        /// <returns></returns>
        public static string CreateListThumbnailUrl(string ThumbnailUrl)
        {
            return LpsDefine.LIPLIS_API_THUMBNAIL_PROXY_LIST + "?url=" + ThumbnailUrl;
        }


    }
}
