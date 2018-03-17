//=======================================================================
//  ClassName : TalkDataGreet
//  概要      : あいさつデータ
//              シングルトン
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Msg;

namespace Assets.Scripts.Data.Talk
{
    public class TalkDataGreet : TalkDataBase
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
        private static TalkDataGreet instance;
        public static TalkDataGreet Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TalkDataGreet();
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
        //                           あいさつ取得
        //                         
        //====================================================================
        #region あいさつ取得
        /// <summary>
        /// あいさつ文章を取得する
        /// </summary>
        /// <returns></returns>
        public MsgSentence GetGreetMessage()
        {
            MsgSentence result = new MsgSentence();



            return result;
        }
        #endregion
    }
}
