//=======================================================================
//  ClassName : DateUtil
//  概要      : 日付に関するユーティリティ
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using System;

namespace Assets.Scripts.Utils
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
