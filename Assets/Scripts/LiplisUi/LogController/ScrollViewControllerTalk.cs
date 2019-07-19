//=======================================================================
//  ClassName : ScrollViewControllerLog
//  概要      : スクロールビューコントローラー
//              
//
//  LiplisMoonlight
//  Create 2019/06/27
//
//  Copyright(c) 2017-2019 sachin.
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.Msg;
using Assets.Scripts.Util.Ugui.DynamicScrollView.ScrollView;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.LiplisUi.LogController
{
   public class ScrollViewControllerTalk : MonoBehaviour
    {

        public static ScrollViewControllerTalk sc;

        public DynamicScrollView scrollView;

        public List<MsgSentence> TalkSentenceList;

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
            this.scrollView.totalItemCount = 0;
        }

        #endregion

        //====================================================================
        //
        //                        ニュースをセットする
        //                         
        //====================================================================
        #region ニュースをセットする
        public void SetSentence(List<MsgSentence> TalkSentenceList)
        {
            //ニュースリストを設定する
            this.TalkSentenceList = TalkSentenceList;

            //スクロールビューカウントを設定する
            this.scrollView.totalItemCount = sc.TalkSentenceList.Count;

            //リフレッシュ
            this.scrollView.refresh();
        }

        public void SetSentence(int dataIndex)
        {
            //自データをロードする
            var data = ScrollViewControllerLog.sc.NewsDataList[dataIndex];

            //トピック取得
            var topic = LiplisStatus.Instance.NewTopic.SearchTopic(data.DATAKEY, LiplisModels.Instance.GetModelList());

            //選択トピックセット
            CtrlLog.instance.NowTopic = topic;

            //センテンスリストセット
            sc.SetSentence(topic.TalkSentenceList);
        }

        #endregion
    }
}
