//====================================================================
//  ClassName : LiplisMoonlightSetting
//  概要      : 設定データ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using System;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Setting
{
    public class LiplisMoonlightSetting
    {
        //サイズ
        //0  160  × 90   
        //1  320  × 180  
        //2  480  × 270  
        //3  640  × 360  
        //4  800  × 450  ☆
        //5  960  × 540  
        //6  1120 × 630  
        //7  1280 × 720  
        //8  1440 × 810  
        //9  1600 × 900  
        //10 1760 × 990  
        //11 1920 × 1080 

        public Int32 SIZE_CODE = 4;

        /// <summary>
        /// サイズを取得する
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSize()
        {
            switch (SIZE_CODE)
            {
                case 0: return new  Vector2(160.0f, 90.0f);
                case 1: return new  Vector2(320.0f  , 180.0f );
                case 2: return new  Vector2(480.0f  , 270.0f );
                case 3: return new  Vector2(640.0f  , 360.0f );
                case 4: return new  Vector2(800.0f  , 450.0f );
                case 5: return new  Vector2(960.0f  , 540.0f );
                case 6: return new  Vector2(1120.0f , 630.0f );
                case 7: return new  Vector2(1280.0f , 720.0f );
                case 8: return new  Vector2(1440.0f , 810.0f );
                case 9: return new  Vector2(1600.0f , 900.0f );
                case 10: return new Vector2(1760.0f , 990.0f );
                case 11: return new Vector2(1920.0f , 1080.0f);
                default:return new  Vector2(800.0f  , 450.0f );
            }

        }





    }









}
