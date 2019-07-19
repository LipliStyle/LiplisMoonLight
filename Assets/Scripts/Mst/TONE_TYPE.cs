//====================================================================
//  ClassName : TONE_TYPE
//  概要      : トーンタイプ
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿

namespace Assets.Scripts.Define
{
    public enum TONE_TYPE
    {
        NONE = 0,                   //設定なし。ディクショナリに登録しない
        REPLACE = 1,
        REGEX = 2,
        END = 10,
        END_CHINESE_CHARACTER = 11,
        END_NUMBER = 12,
        END_ALPHABET = 13,
        END_SYMBOL = 14,
        END_KATAKANA = 15,
        END_HIRAGANA = 16,
    }
}
