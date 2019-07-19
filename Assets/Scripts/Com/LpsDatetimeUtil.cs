//====================================================================
//  ClassName : LpsDatetimeUtil
//  概要      : 日付ユーティリティ
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using System;

namespace Assets.Scripts.Com
{
    public class LpsDatetimeUtil
    {
        /// <summary>
        /// デコード
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime dec(uint s)
        {
            long serial = s;

            long yearr = (long)((serial) / level(5));
            long monthr = (long)((serial -= level(5) * yearr) / level(4));
            long dayr = (long)((serial -= level(4) * monthr) / level(3));
            long hourr = (long)((serial -= level(3) * dayr) / level(2));
            long minuter = (long)((serial -= level(2) * hourr) / level(1));
            long secondr = (long)((serial -= level(1) * minuter));

            yearr += 2006;
            monthr += 1;
            dayr += 1;

            DateTime result = new DateTime((int)yearr, (int)monthr, (int)dayr, (int)hourr, (int)minuter, (int)secondr);

            return result;
        }

        /// <summary>
        /// エンコード
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static uint enc(DateTime now)
        {
            long year = (now.Year - 2006) * level(5);
            long month = (now.Month - 1) * level(4);
            long day = (now.Day - 1) * level(3);
            long hour = now.Hour * level(2);
            long minute = now.Minute * level(1);
            long second = now.Second;
            uint result = (uint)(year + month + day + hour + minute + second);

            return result;
        }

        /// <summary>
        /// 平準化
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static int level(int lv)
        {
            int[] levels = { 12, 31, 24, 60, 60 };
            Array.Reverse(levels);

            int res = 1;
            for (int i = 0; i < lv; i++)
            {
                res *= levels[i];
            }

            return res;
        }

        /// <summary>
        /// 現在時刻のオブジェクトを取得する
        /// </summary>
        /// <returns></returns>
        public static uint Now
        {
            get
            {
                return LpsDatetimeUtil.enc(DateTime.Now);
            }
        }

    }
}
