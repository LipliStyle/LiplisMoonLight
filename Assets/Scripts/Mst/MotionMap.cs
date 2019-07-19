using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Define
{
    public class MotionMap
    {

        /// <summary>
        /// 感情を取得する
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public static MOTION GetMotion(int emotion, int point)
        {
            //絶対値に補正
            emotion = Math.Abs(emotion);

            if (emotion == EMOTION.NORMAL)                          { return MOTION.MOTION_NORMAL; }
            else if (emotion == EMOTION.JOY && point >= 0)          { return MOTION.MOTION_JOY_P; }
            else if (emotion == EMOTION.JOY && point < 0)           { return MOTION.MOTION_JOY_M; }
            else if (emotion == EMOTION.ADMIRATION && point >= 0)   { return MOTION.MOTION_ADMIRATION_P; }
            else if (emotion == EMOTION.ADMIRATION && point < 0)    { return MOTION.MOTION_ADMIRATION_M; }
            else if (emotion == EMOTION.PEACE && point >= 0)        { return MOTION.MOTION_PEACE_P; }
            else if (emotion == EMOTION.PEACE && point < 0)         { return MOTION.MOTION_PEACE_M; }
            else if (emotion == EMOTION.ECSTASY && point >= 0)      { return MOTION.MOTION_ECSTASY_P; }
            else if (emotion == EMOTION.ECSTASY && point < 0)       { return MOTION.MOTION_ECSTASY_M; }
            else if (emotion == EMOTION.AMAZEMENT && point >= 0)    { return MOTION.MOTION_AMAZEMENT_P; }
            else if (emotion == EMOTION.AMAZEMENT && point < 0)     { return MOTION.MOTION_AMAZEMENT_M; }
            else if (emotion == EMOTION.RAGE && point >= 0)         { return MOTION.MOTION_RAGE_P; }
            else if (emotion == EMOTION.RAGE && point < 0)          { return MOTION.MOTION_RAGE_M; }
            else if (emotion == EMOTION.INTEREST && point >= 0)     { return MOTION.MOTION_INTEREST_P; }
            else if (emotion == EMOTION.INTEREST && point < 0)      { return MOTION.MOTION_INTEREST_M; }
            else if (emotion == EMOTION.RESPECT && point >= 0)      { return MOTION.MOTION_RESPECT_P; }
            else if (emotion == EMOTION.RESPECT && point < 0)       { return MOTION.MOTION_RESPECT_M; }
            else if (emotion == EMOTION.CLAMLY && point >= 0)       { return MOTION.MOTION_CLAMLY_P; }
            else if (emotion == EMOTION.CLAMLY && point < 0)        { return MOTION.MOTION_CLAMLY_M; }
            else if (emotion == EMOTION.PROUD && point >= 0)        { return MOTION.MOTION_PROUD_P; }
            else if (emotion == EMOTION.PROUD && point < 0)         { return MOTION.MOTION_PROUD_M; }
            else { return MOTION.MOTION_IDLE; }
        }

        /// <summary>
        /// デフォルトモーションを返す
        /// </summary>
        /// <returns></returns>
        public static MOTION GetDefaultMotion()
        {
            return EMOTION.NORMAL;
        }
    }
}
