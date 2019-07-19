//=======================================================================
//  ClassName : ResWhatDayIsToday
//  概要      : アニバーサーリーデイズ
//
//  SatelliteServer
//  Copyright(c) 2009-2017 sachin.
//=======================================================================
using Assets.Scripts.Msg;
using System.Collections.Generic;

namespace Assets.Scripts.LiplisSystem.Cif.v60.Res
{
    public class ResWhatDayIsToday
    {
        ///=============================
        /// アニバーサーリーデイズリスト
        public List<MsgTopic> AnniversaryDaysList;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public ResWhatDayIsToday()
        {
            AnniversaryDaysList = new List<MsgTopic>();
        }


    }
}