﻿//=======================================================================
//  ClassName : TopicPanel
//  概要      : トピックパネル
//              
//
//  LiplisLive2D
//  Create 2018/04/09
//
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Msg;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.UiController.TopicListController
{
    public class TopicPanel : IDisposable
    {
        //=====================================
        //UI要素
        GameObject Panel;
        protected Text Text;
        protected Image Thumbnail;

        //=====================================
        //ニュースデータ
        public MsgBaseNewsData news;

        //=====================================
        // 本コントロールのID
        protected ContentCategoly Categoly;

        //=====================================
        // サムネイルロードフラグ
        public bool FlgThumbnailLoadRequest =false;

        //====================================================================
        //
        //                           初期化処理
        //                         
        //====================================================================
        #region 初期化処理
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="parent"></param>
        public TopicPanel(GameObject parent, GameObject PrefabNewsPanel,float offsetY, ContentCategoly Categoly)
        {
            this.Categoly = Categoly;
            this.Panel = PrefabNewsPanel;
            Init(offsetY,parent);
            SetEvent();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="parent"></param>
        public void Init(float offsetY,GameObject parent)
        {
            //サイズ調整
            RectTransform rectTransform = Panel.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = new Vector2(offsetY, 0);
            rectTransform.sizeDelta = new Vector2(480, 0);

            //サムネインスタンス取得
            this.Thumbnail = Panel.transform.Find("Image").GetComponent<Image>();
            this.Text = Panel.transform.Find("Text").GetComponent<Text>();

            //非表示に設定
            Panel.gameObject.SetActive(false);

            //親パネルにセット
            Panel.transform.SetParent(parent.transform, false);

            //ニュース初期化
            this.news = null;
        }

        /// <summary>
        /// イベントのセット
        /// </summary>
        private void SetEvent()
        {
            //ポインターダウンイベントトリガー生成
            EventTrigger trigger = this.Thumbnail.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { OnClickImage(); });
            trigger.triggers.Add(entry);
        }
     
        /// <summary>
        /// クリックイベント
        /// </summary>
        public void OnClickImage()
        {
            if(news != null)
            {
                CtrlTalk.Instance.SetNextTopic(news.DATAKEY, this.Categoly);
                Debug.Log(news.DATAKEY);
            }
        }

        /// <summary>
        /// ディスポーズ
        /// </summary>
        public void Dispose()
        {
            Text = null;
            Thumbnail = null;
            Panel = null;
        }

        #endregion

        //====================================================================
        //
        //                           操作処理
        //                         
        //====================================================================
        #region 操作処理

        /// <summary>
        /// ニュースをセットする
        /// </summary>
        public void SetNews(MsgBaseNewsData news)
        {
            //ニュースセット
            this.news = news;

            //活性化
            this.Panel.gameObject.SetActive(true);

            //サムネクリア 更新対象にする
            ClearThumbnail(true);

            //タイトルセット
            this.Text.text = news.TITLE;
        }

        /// <summary>
        /// パネルを非表示にする
        /// </summary>
        public void Hide()
        {
            //ニュースセット
            this.news = null;

            //活性化
            this.Panel.gameObject.SetActive(false);

            //サムネクリア 更新済みマーク
            ClearThumbnail(false);

            //タイトルセット
            this.Text.text = "";
        }

        /// <summary>
        /// サムネイルをクリアする
        /// </summary>
        public void ClearThumbnail(bool flgLoad)
        {
            this.FlgThumbnailLoadRequest = flgLoad;

            this.Thumbnail.sprite = null;
        }

        /// <summary>
        /// サムネイルをセットする
        /// </summary>
        /// <param name="sprite"></param>
        public void SetThumbnail(Sprite sprite)
        {
            this.FlgThumbnailLoadRequest = false;
            this.Thumbnail.sprite = null;
            this.Thumbnail.sprite = sprite;
        }


        #endregion    
    }
}