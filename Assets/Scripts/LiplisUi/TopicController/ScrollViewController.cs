//=======================================================================
//  ClassName : ScrollViewController
//  概要      : スクロールビューコントローラー
//              
//
//  LiplisMoonlight
//  Create 2019/06/27
//
//  Copyright(c) 2017-2019 sachin.
//=======================================================================﻿
using Assets.Scripts.Msg;
using Assets.Scripts.Util.Ugui.DynamicScrollView.ScrollView;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LiplisUi.TopicController
{
    public class ScrollViewController : MonoBehaviour
    {

        public static ScrollViewController sc;

        public DynamicScrollView scrollView;

        public List<MsgBaseNewsData> NewsDataList;

        //====================================================================
        //
        //                           初期化処理
        //                         
        //====================================================================
        #region 初期化処理

        private void Awake()
        {
            //シングルトン
            sc = this;

            //レクト取得
            ScrollRect scrollRect = this.scrollView.GetComponent<ScrollRect>();

            ///ポジション設定
            scrollRect.verticalNormalizedPosition = 1.0f;
        }


        private void Start()
        {
            //アイテム数 デフォルト100
            this.scrollView.totalItemCount = 100;
        }

        #endregion

        //====================================================================
        //
        //                        ニュースをセットする
        //                         
        //====================================================================
        #region ニュースをセットする
        public void SetNews(List<MsgBaseNewsData> NewsList)
        {
            //ニュースリストを設定する
            this.NewsDataList = NewsList;

            //スクロールビューカウントを設定する
            this.scrollView.totalItemCount = sc.NewsDataList.Count;

            //0のいちにスクロールする
            this.scrollView.scrollByItemIndex(0);

            //リフレッシュ
            this.scrollView.refresh();
        }

        /// <summary>
        /// 位置を更新しない
        /// </summary>
        /// <param name="NewsList"></param>
        public void UpdateNews(List<MsgBaseNewsData> NewsList)
        {
            //ニュースリストを設定する
            this.NewsDataList = NewsList;

            //スクロールビューカウントを設定する
            this.scrollView.totalItemCount = sc.NewsDataList.Count;

            //リフレッシュ
            this.scrollView.refresh();
        }

        /// <summary>
        /// リフレッシュ
        /// </summary>
        public void Refresh()
        {
            //リフレッシュ
            this.scrollView.refresh();
        }
        #endregion
    }
}
