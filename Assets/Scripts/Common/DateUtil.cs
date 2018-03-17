//=======================================================================
//  ClassName : DateUtil
//  概要      : 日付に関するユーティリティ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using System;

namespace Assets.Scripts.Common
{
    public class DateUtil
    {

        /// <summary>
        /// 時分秒からデイトタイムを生成する
        /// </summary>
        /// <param name="h"></param>
        /// <param name="m"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime CreateDatetime(int h, int m, int s)
        {
            return new DateTime(1900, 1, 1, h, m, s);
        }

    }
}
