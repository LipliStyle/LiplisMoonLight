//====================================================================
//  ClassName : CubismAutoMouthInputEx
//  概要      : 自動口パクのオーバーライド
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using Live2D.Cubism.Framework.MouthMovement;

namespace Assets.Scripts.Live2dEx
{
    public static class CubismAutoMouthInputEx
    {
        public static void LipSyncOn(this CubismAutoMouthInput e)
        {
            e.Timescale = 10;
        }


        public static void LipSyncOff(this CubismAutoMouthInput e)
        {
            e.Reset();
        }


    }
}
