//=======================================================================
//  ClassName : MsgMotion
//  概要      : モーションメッセージ
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿

namespace Assets.Scripts.Msg
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
