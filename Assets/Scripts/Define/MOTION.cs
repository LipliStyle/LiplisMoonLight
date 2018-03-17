//====================================================================
//  ClassName : MOTION
//  概要      : モーション定義
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿


namespace Assets.Scripts.Define
{
    public class MOTION
    {
        public const string IDLE         = "MOTION_IDLE";
        public const string NORMAL       = "MOTION_NORMAL";
        public const string JOY_P        = "MOTION_JOY_P";
        public const string JOY_M        = "MOTION_JOY_M";
        public const string ADMIRATION_P = "MOTION_ADMIRATION_P";
        public const string ADMIRATION_M = "MOTION_ADMIRATION_M";
        public const string PEACE_P      = "MOTION_PEACE_P";
        public const string PEACE_M      = "MOTION_PEACE_M";
        public const string ECSTASY_P    = "MOTION_ECSTASY_P";
        public const string ECSTASY_M    = "MOTION_ECSTASY_M";
        public const string AMAZEMENT_P  = "MOTION_AMAZEMENT_P";
        public const string AMAZEMENT_M  = "MOTION_AMAZEMENT_M";
        public const string RAGE_P       = "MOTION_RAGE_P";
        public const string RAGE_M       = "MOTION_RAGE_M";
        public const string INTEREST_P   = "MOTION_INTEREST_P";
        public const string INTEREST_M   = "MOTION_INTEREST_M";
        public const string RESPECT_P    = "MOTION_RESPECT_P";
        public const string RESPECT_M    = "MOTION_RESPECT_M";
        public const string CLAMLY_P     = "MOTION_CLAMLY_P";
        public const string CLAMLY_M     = "MOTION_CLAMLY_M";
        public const string PROUD_P      = "MOTION_PROUD_P";
        public const string PROUD_M      = "MOTION_PROUD_M";

        public const string SHAKE        = "MOTION_SHAKE";
        public const string FLICK_HEAD   = "MOTION_FLICK_HEAD";
        public const string PITATCH      = "MOTION_PITATCH";

        /// <summary>
        /// 感情を取得する
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public static string GetMotion(int emotion, int point)
        {
            if (emotion      == EMOTION.NORMAL) { return NORMAL; }
            else if (emotion == EMOTION.JOY && point >= 0) { return JOY_P; }
            else if (emotion == EMOTION.JOY && point < 0) { return JOY_M; }
            else if (emotion == EMOTION.ADMIRATION && point >= 0) { return ADMIRATION_P; }
            else if (emotion == EMOTION.ADMIRATION && point < 0) { return ADMIRATION_M; }
            else if (emotion == EMOTION.PEACE && point >= 0) { return PEACE_P; }
            else if (emotion == EMOTION.PEACE && point < 0) { return PEACE_M; }
            else if (emotion == EMOTION.ECSTASY && point >= 0) { return ECSTASY_P; }
            else if (emotion == EMOTION.ECSTASY && point < 0) { return ECSTASY_M; }
            else if (emotion == EMOTION.AMAZEMENT && point >= 0) { return AMAZEMENT_P; }
            else if (emotion == EMOTION.AMAZEMENT && point < 0) { return AMAZEMENT_M; }
            else if (emotion == EMOTION.RAGE && point >= 0) { return RAGE_P; }
            else if (emotion == EMOTION.RAGE && point < 0) { return RAGE_M; }
            else if (emotion == EMOTION.INTEREST && point >= 0) { return INTEREST_P; }
            else if (emotion == EMOTION.INTEREST && point < 0) { return INTEREST_M; }
            else if (emotion == EMOTION.RESPECT && point >= 0) { return RESPECT_P; }
            else if (emotion == EMOTION.RESPECT && point < 0) { return RESPECT_M; }
            else if (emotion == EMOTION.CLAMLY && point >= 0) { return CLAMLY_P; }
            else if (emotion == EMOTION.CLAMLY && point < 0) { return CLAMLY_M; }
            else if (emotion == EMOTION.PROUD && point >= 0) { return PROUD_P; }
            else if (emotion == EMOTION.PROUD && point < 0) { return PROUD_M; }
            else { return IDLE; }
        }
    }
}
