//====================================================================
//  ClassName : CharDataBase
//  概要      : キャラクタデータベースクラス
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.DataChar.CharacterTalk;
using Assets.Scripts.LiplisSystem.Mst;
using UnityEngine;

namespace Assets.Scripts.DataChar.Rabiits
{
    public abstract class CharDataBase
    {
        /// <summary>
        /// キャラクターデータを生成する
        /// </summary>
        /// <param name="key"></param>
        /// <param name="allocationID"></param>
        /// <param name="defaultPosition"></param>
        /// <returns></returns>
        public CharacterData CreateCharData(string FrontModelName, string RightModelName, string LeftModelName, int allocationID, MST_CARACTER_POSITION defaultPosition, string WindowName, float LocationY)
        {
            CharDataTone Tone = CreateCharDataTone();
            CharDataGreet Greet = CreateCharDataGreet(Tone,allocationID);
            CharacterData charData = new CharacterData(FrontModelName, RightModelName, LeftModelName, allocationID, Greet, Tone, defaultPosition, WindowName, LocationY);
            
            return charData;
        }

        /// <summary>
        /// 挨拶データ作成
        /// </summary>
        protected abstract CharDataGreet CreateCharDataGreet(CharDataTone Tone,int allocationID);

        /// <summary>
        /// トーンデータ作成
        /// </summary>
        protected abstract CharDataTone CreateCharDataTone();

    }
}
