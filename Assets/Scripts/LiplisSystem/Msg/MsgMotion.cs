//=======================================================================
//  ClassName : MsgMotion
//  概要      : モーションメッセージ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿

namespace Assets.Scripts.LiplisSystem.Msg
{
    public class MsgMotion
    {
        public string GROUP_NAME;
        public int NO;

        public MsgMotion(string GROUP_NAME, int NO)
        {
            this.GROUP_NAME = GROUP_NAME;
            this.NO = NO;
        }
    }
}
