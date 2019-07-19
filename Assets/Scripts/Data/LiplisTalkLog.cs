//=======================================================================
//  ClassName : LiplisTalkLog
//  概要      : おしゃべりログデータ
//              シングルトン
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.Msg;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Data
{
    public class LiplisTalkLog
    {
        //=============================
        // ログリスト
        public Queue<MsgTopic> LogList;

        //====================================================================
        //
        //                          シングルトン管理
        //                         
        //====================================================================
        #region シングルトン管理

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static LiplisTalkLog instance;
        public static LiplisTalkLog Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LiplisTalkLog();
                }

                return instance;
            }
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public LiplisTalkLog()
        {
            LogList = new Queue<MsgTopic>();
        }

        /// <summary>
        /// ログデータ追加
        /// 過去100件のログを記録する
        /// </summary>
        public void AddLog(MsgTopic topic)
        {
            //ログを追加する
            LogList.Enqueue(topic);

            //100件以上になったら古いものを捨てる
            if(LogList.Count > 100)
            {
                LogList.Dequeue();
            }
        }

        /// <summary>
        /// ログリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<MsgBaseNewsData> GetLogList()
        {
            List<MsgBaseNewsData> resList = new List<MsgBaseNewsData>();

            //逆順で回す
            foreach (var log in Enumerable.Reverse(LogList).ToList())
            {
                if(log != null)
                {
                    resList.Add(log.Convert2BaseNewsData());
                }
            }

            return resList;
        }
        #endregion
    }
}
