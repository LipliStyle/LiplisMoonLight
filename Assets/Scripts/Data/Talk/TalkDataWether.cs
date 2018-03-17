//=======================================================================
//  ClassName : TalkDataWether
//  概要      : 天気おしゃべりデータ
//              シングルトン
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Msg;

namespace Assets.Scripts.Data.Talk
{
    public class TalkDataWether : TalkDataBase
    {
        //====================================================================
        //
        //                          シングルトン管理
        //                         
        //====================================================================
        #region シングルトン管理

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static TalkDataWether instance;
        public static TalkDataWether Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TalkDataWether();
                }

                return instance;
            }
        }

        /// <summary>
        /// リストを初期化する
        /// </summary>
        protected override void initList()
        {
            //ベースイニット実行
            base.initList();

            //リストを作成する

        }

        #endregion



        //====================================================================
        //
        //                           天気文章取得
        //                         
        //====================================================================
        #region 天気文章取得
        /// <summary>
        /// あいさつ文章を取得する
        /// </summary>
        /// <returns></returns>
        public MsgSentence GetWetherMessage()
        {
            MsgSentence result = new MsgSentence();



            return result;
        }
        #endregion

    }
}
