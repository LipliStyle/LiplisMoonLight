﻿//=======================================================================
//  ClassName : NewsPanelItem
//  概要      : ニュースパネルアイテム
//              
//
//  LiplisMoonlight
//  Create 2019/06/27
//
//  Copyright(c) 2017-2019 sachin.
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.Data.SubData;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.MainSystem;
using Assets.Scripts.LiplisSystem.Web;
using Assets.Scripts.Msg;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Ugui.DynamicScrollView.ScrollView;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.LiplisUi.TopicController
{
    public class NewsPanelItem : UIBehaviour, IDynamicScrollViewItem
    {
        public Image icon;
        public Text title;

        //=============================
        // データインデックス
        private int dataIndex = -1;

        //=============================
        //前回表示データ
        private MsgBaseNewsData prvData;

        //====================================================================
        //
        //                             初期化処理
        //                         
        //====================================================================
        #region 初期化処理

        /// <summary>
        /// スタート
        /// </summary>
        protected override void Start()
        {
            base.Start();

            SetEvent();
        }


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {

            base.OnDisable();
        }

        public void OnUpdateItem(int index)
        {
            try
            {
                this.dataIndex = index;
                StartCoroutine(this.updateItem());
            }
            catch(Exception ex)
            {
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// イベントの設定
        /// </summary>
        private void SetEvent()
        {
            EventTrigger trigger = this.icon.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { OnClick(); });
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// クリック時処理
        /// </summary>
        public void OnClick()
        {
            //自データをロードする
            var data = ScrollViewController.sc.NewsDataList[this.dataIndex];

            //データキーから話題セット
            CtrlTalk.Instance.SetNextTopic(data.DATAKEY, ContentCategolyText.GetContentCategoly(data.DATA_TYPE));
        }

        #endregion

        //====================================================================
        //
        //                           イベントハンドラ
        //                         
        //====================================================================
        #region イベントハンドラ

        /// <summary>
        /// アイテム更新
        /// </summary>
        /// <returns></returns>
        private IEnumerator updateItem()
        {
            //インデックスチェック
            if (this.dataIndex == -1)
            {
                yield break;
            }

            //ニュースデータNULLチェック
            if(ScrollViewController.sc.NewsDataList == null)
            {
                yield break;
            }

            //ニュースデータカウントチェック
            if(ScrollViewController.sc.NewsDataList.Count - 1 < this.dataIndex)
            {
                yield break;
            }

            //ニュースデータ取得
            MsgBaseNewsData data = ScrollViewController.sc.NewsDataList[this.dataIndex];

            //コンペアチェック
            if (CompareData(data))
            {
                yield break;
            }

            //一個前のURL設定
            prvData = data;

            //タイトル表示
            this.title.text = data.TITLE;

            //サムネイルURL取得
            string thumbUrl = ThumbnailUrl.CreateListThumbnailUrl(data.THUMBNAIL_URL);

            //ファイルからサムネイル取得を試みる
            Texture2D texture = LiplisCache.Instance.ImagePath.GetWebTexutreFromFile(thumbUrl);

            //NULLならノーイメージ適用
            if (texture == null)
            {
                //優先要求リストに入れる
                LiplisCache.Instance.ImagePath.SetRequestUrlQPrioritize(thumbUrl);

                //ノーイメージテクスチャを返す
                texture = LiplisCache.Instance.ImagePath.GetNoImageTex();
            }
            
            //ボタンのテキスト変更
            if (texture != null)
            {
                try
                {
                    icon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
                catch
                {

                }
            }
            else
            {
                Debug.Log("テクスチャ未更新");
            }
        }

        /// <summary>
        /// データを比較する
        /// 同じならtrue
        /// </summary>
        /// <param name="newData"></param>
        /// <returns></returns>
        private bool CompareData(MsgBaseNewsData newData)
        {
            try
            {
                //前回値が空ならfalse
                if(prvData == null)
                {
                    return false;
                }

                //URLを比較する
                return newData.URL == prvData.URL;
            }
            catch
            {
                return false;
            }
        }


        #endregion
    }
}
