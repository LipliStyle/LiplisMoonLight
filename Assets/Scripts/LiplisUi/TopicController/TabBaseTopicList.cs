//=======================================================================
//  ClassName : TabBaseTopicList
//  概要      : ニュースベース
//              一覧UIの操作を行う
//              
//
//  LiplisMoonlight
//  Create 2018/04/09
//
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Web;
using Assets.Scripts.Msg;
using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LiplisUi.TopicController
{
    public class TabBaseTopicList
    {
        ///=============================
        ///コンテンツ
        protected GameObject Parent;


        //=====================================
        // 描画制御用FPS計算
        protected int frameCount = -1;

        //=====================================
        // イメージリスト
        private List<TopicPanel> panelList;

        //=====================================
        // 本コントロールのID
        protected ContentCategoly Categoly;

        //=====================================
        // spriteEmpty
        private Sprite empty;

        ///=============================
        ///プレファブ
        private GameObject PrefabNewsPanel;

        //====================================================================
        //
        //                           初期化処理
        //                         
        //====================================================================
        #region 初期化処理
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="Parent"></param>
        public TabBaseTopicList(GameObject Parent)
        {
            //親コントロール取得
            this.Parent = Parent;

            //クラスの初期化
            InitClass();
        }

        /// <summary>
        /// クラスの初期化
        /// </summary>
        protected void InitClass()
        {
            empty = Sprite.Create(new Texture2D(0, 0), new Rect(), new Vector2());

            this.PrefabNewsPanel = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_NEWS_PANEL);
        }
        #endregion

        //====================================================================
        //
        //  コンテント操作
        //                         
        //====================================================================
        #region コンテント操作

        /// <summary>
        /// 子要素のクリア
        /// </summary>
        public void Clear()
        {
            foreach (Transform n in Parent.transform)
            {
                UnityEngine.Object.Destroy(n.gameObject);
            }

            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// ニュースを生成する
        /// </summary>
        /// <param name="NewsList"></param>
        /// <returns></returns>
        public IEnumerator CreateNewsList(List<MsgBaseNewsData> NewsList)
        {
            if (ScrollViewController.sc != null)
            {
                ScrollViewController.sc.SetNews(NewsList);
            }

            yield return 0;
        }

        /// <summary>
        /// ニュースを更新する
        /// </summary>
        /// <param name="NewsList"></param>
        /// <returns></returns>
        public IEnumerator UpdateNewsList(List<MsgBaseNewsData> NewsList)
        {
            if (ScrollViewController.sc != null)
            {
                ScrollViewController.sc.UpdateNews(NewsList);
            }

            yield return 0;
        }

        #endregion
    }
}