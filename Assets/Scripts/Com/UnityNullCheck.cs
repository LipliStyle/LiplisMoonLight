//====================================================================
//  ClassName : UnityNullCheck
//  概要      : ユニティオブジェクトのNULLチェックを行う。
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Com
{
    public class UnityNullCheck
    {
        /// <summary>
        /// ユニティオブジェクトのNULLチェック
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(Object obj)
        {
            if (obj is UnityEngine.Object)
            {
                if ((UnityEngine.Object)obj != null)
                {
                    // 元気なUnityオブジェクト
                    return false;
                }
                else
                {
                    // 死んだフリしているUnityオブジェクト
                    return false;
                }
            }
            else
            {
                if (obj != null)
                {
                    // 普通のnullでないオブジェクト
                    return false;
                }
                else
                {
                    // ガチnull
                    return true;    
                }
            }
        }

    }
}
