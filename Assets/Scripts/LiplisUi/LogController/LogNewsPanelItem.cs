//=======================================================================
//  ClassName : LogNewsPanelItem
//  概要      : ログニュースパネルアイテム
//              
//
//  LiplisMoonlight
//  Create 2019/06/27
//
//  Copyright(c) 2017-2019 sachin.
//=======================================================================﻿
using Assets.Scripts.Com;
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Web;
using Assets.Scripts.Msg;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Ugui.DynamicScrollView.ScrollView;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.LiplisUi.LogController
{
    public class LogNewsPanelItem : UIBehaviour, IDynamicScrollViewItem
    {
        public Image icon;
        public Text txtNews;
        public Text txtTime;
        public Text txtType;

        //=============================
        // データインデックス
        private int dataIndex = -1;

        //=============================
        //前回表示データ
        private MsgBaseNewsData prvData;

        //====================================================================
        //
        //                           イベントハンドラ
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
            catch (Exception ex)
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
            ScrollViewControllerTalk.sc.SetSentence(this.dataIndex);
        }




        #endregion

        //====================================================================
        //
        //                           更新処理
        //                         
        //====================================================================
        #region 更新処理

        /// <summary>
        /// アイテム更新
        /// </summary>
        /// <returns></returns>
        private IEnumerator updateItem()
        {

            if (this.dataIndex == -1)
            {
                yield break;
            }

            //ニュースデータNULLチェック
            if (ScrollViewControllerLog.sc.NewsDataList == null)
            {
                yield break;
            }

            //ニュースデータカウントチェック
            if (ScrollViewControllerLog.sc.NewsDataList.Count - 1 < this.dataIndex)
            {
                yield break;
            }

            //ニュースデータ取得
            MsgBaseNewsData data = ScrollViewControllerLog.sc.NewsDataList[this.dataIndex];

            //コンペアチェック
            if (CompareData(data))
            {
                yield break;
            }

            //一個前のURL設定
            prvData = data;

            //タイトル表示
            this.txtNews.text = data.TITLE;

            //サムネイルURL取得
            string thumbUrl = ThumbnailUrl.CreateListThumbnailUrl(data.THUMBNAIL_URL);

            //時刻表示
            this.txtTime.text = LpsDatetimeUtil.dec(data.CREATE_TIME).ToString("yyyy/MM/dd HH:mm:ss");

            //タイプ表示
            this.txtType.text = ContentCategolyText.GetContentText(data.DATA_TYPE);

            //ファイルからサムネイル取得を試みる
            Texture2D texture = LiplisCache.Instance.ImagePath.GetWebTexutreFromFile(thumbUrl);

            //NULLならノーイメージ適用
            if (texture == null)
            {
                texture = LiplisCache.Instance.ImagePath.GetNoImageTex();
            }

            //NULLならWebからダウンロードする
            if (texture == null)
            {
                //からテクスチャ取得
                texture = LiplisCache.Instance.ImagePath.GetNoImageTex();

                //設定
                icon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                //最新ニュースデータ取得
                var Async = LiplisCache.Instance.ImagePath.GetWebTexutre(thumbUrl);

                //非同期実行
                yield return CoroutineHandler.StartStaticCoroutine(Async);

                //再度データを取り直す
                data = ScrollViewControllerLog.sc.NewsDataList[this.dataIndex];

                //データ取得
                texture = (Texture2D)Async.Current;
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
                if(this.prvData == null)
                {
                    return false;
                }

                //URLを比較する
                return newData.URL == this.prvData.URL;
            }
            catch
            {
                return false;
            }
        }



        #endregion
    }
}
