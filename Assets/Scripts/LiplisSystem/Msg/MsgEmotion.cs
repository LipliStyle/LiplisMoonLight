//=======================================================================
//  ClassName : MsgEmotion
//  概要      : 感情メッセージ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
namespace Assets.Scripts.LiplisSystem.Msg
{
    public class MsgEmotion
    {
        public int EMOTION;
        public int POINT;

        public MsgEmotion(int EMOTION,int POINT)
        {
            this.EMOTION = EMOTION;
            this.POINT = POINT;
        }
    }
}
