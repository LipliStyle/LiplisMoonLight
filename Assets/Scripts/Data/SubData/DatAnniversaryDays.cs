//====================================================================
//  ClassName : DatAnniversaryDays
//  概要      : 記念日データリスト
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================

using Assets.Scripts.Com;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.Msg;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatAnniversaryDays
    {
        ///=============================
        ///プロパティ
        public uint LastUpdateTime;

        ///=============================
        /// アニバーサーリーデイズリスト
        //public List<MsgTopic> AnniversaryDaysList;
        public ResWhatDayIsToday DataList;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DatAnniversaryDays()
        {
            //AnniversaryDaysList = new List<MsgTopic>();

            //1時間前の時刻をセット(最初に強制取得するため)
            this.LastUpdateTime = LpsDatetimeUtil.enc(DateTime.Now.AddHours(-1));
        }

        /// <summary>
        /// データリストを生成する
        /// </summary>
        /// <param name="DataList"></param>
        public void SetData(ResWhatDayIsToday DataList)
        {
            this.DataList = DataList;
        }

        /// <summary>
        /// 保持データに本日データがあるかどうかチェックする
        /// </summary>
        /// <returns></returns>
        public bool CheckTodayDataExists()
        {
            //nullチェック
            if (DataList == null)
            {
                return false;
            }
            if (DataList.AnniversaryDaysList == null)
            {
                return false;
            }

            foreach (MsgTopic data in DataList.AnniversaryDaysList)
            {
                if (LpsDatetimeUtil.dec(data.CreateTime).Day == DateTime.Now.Day)
                {
                    return true;
                }
            }

            //本日データが無かった。
            return false;
        }













    }
}
