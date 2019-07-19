//=======================================================================
//  ClassName : LogCharTalkItem
//  概要      : ログニュースパネルアイテム
//              
//
//  LiplisMoonlight
//  Create 2019/06/27
//
//  Copyright(c) 2017-2019 sachin.
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.Msg;
using Assets.Scripts.Util.Ugui.DynamicScrollView.ScrollView;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.LiplisUi.LogController
{
    public class LogCharTalkItem : UIBehaviour, IDynamicScrollViewItem
    {
        public Image iconLeft;
        public Image iconRight;
        public Image imageBalloonLeft;
        public Image imageBalloonRight;
        public Text nameLeft;
        public Text nameRight;
        public Text messageLeft;
        public Text messageRight;

        private int dataIndex = -1;

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

            if (this.dataIndex == -1)
            {
                yield break;
            }

            //センテンス取得
            MsgSentence data = ScrollViewControllerTalk.sc.TalkSentenceList[this.dataIndex];

            //モデル取得
            LiplisModel charData = LiplisModels.Instance.TableModelId[data.AllocationId];

            if (this.dataIndex % 2 == 0)
            {
                iconLeft.gameObject.SetActive(true);
                iconRight.gameObject.SetActive(false);
                nameLeft.gameObject.SetActive(true);
                nameRight.gameObject.SetActive(false);
                imageBalloonLeft.gameObject.SetActive(true);
                imageBalloonRight.gameObject.SetActive(false);
                nameLeft.text = charData.ModelName;
                iconLeft.sprite = charData.SpriteCharIcon;

                imageBalloonLeft.color = charData.GetColorFromSetting();

                messageLeft.text = data.TalkSentence;
            }
            else
            {
                iconLeft.gameObject.SetActive(false);
                iconRight.gameObject.SetActive(true);
                nameLeft.gameObject.SetActive(false);
                nameRight.gameObject.SetActive(true);
                imageBalloonLeft.gameObject.SetActive(false);
                imageBalloonRight.gameObject.SetActive(true);
                nameRight.text = charData.ModelName;
                iconRight.sprite = charData.SpriteCharIcon;

                imageBalloonRight.color = charData.GetColorFromSetting();

                messageRight.text = data.TalkSentence;
            }


        }
        #endregion
    }
}
