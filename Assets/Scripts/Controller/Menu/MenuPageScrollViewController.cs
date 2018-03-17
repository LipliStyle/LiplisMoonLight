//=======================================================================
//  ClassName : PageControl
//  概要      : スクロールページコントローラ
//              
//
//  LiplisLive2D
//  Create 2018/03/11
//
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class MenuPageScrollViewController : ScrollViewController, IBeginDragHandler, IEndDragHandler,IPointerClickHandler
// ViewControllerクラスを継承して、IBeginDragHandlerインターフェイスと
// IEndDragHandlerインターフェイスを実装する
{

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float key1InTangent = 0.0f;
    [SerializeField] private float key1OutTangent = 0.1f;
    [SerializeField] private float key2InTangent = 0.0f;
    [SerializeField] private float key2OutTangent = 0.0f;
    [SerializeField] private int MAX_ICON_NUM = 17;
    [SerializeField] private int FIX_LENGTH = 31;

    [SerializeField] private Image BTN_HOME1;
    [SerializeField] private Image BTN_HOME2;
    [SerializeField] private Image BTN_HOME3;
    [SerializeField] private Image BTN_HOME4;
    [SerializeField] private Image BTN_HOME5;
    [SerializeField] private Image BTN_HOME6;
    [SerializeField] private Image BTN_HOME7;
    [SerializeField] private Image BTN_HOME8;
    [SerializeField] private Image BTN_HOME9;
    [SerializeField] private Image BTN_HOME10;
    [SerializeField] private Image BTN_HOME11;
    [SerializeField] private Image BTN_HOME12;
    [SerializeField] private Image BTN_HOME13;
    [SerializeField] private Image BTN_HOME14;
    [SerializeField] private Image BTN_HOME15;
    [SerializeField] private Image BTN_HOME16;

    ///=============================
    /// 制御用プロパティ
    private bool isAnimating = false;       // アニメーション中フラグ
    private Vector2 destPosition;           // 最終的なスクロール位置
    private Vector2 initialPosition;        // 自動スクロール開始時のスクロール位置
    private AnimationCurve animationCurve;  // 自動スクロールのアニメーションカーブ
    private int prevPageIndex = 0;          // 前のページのインデックスを保持





    ///=============================
    ///  スクロールビューの矩形を保持
    private Rect currentViewRect;


    ///=============================
    ///ボタンリスト
    private List<Image> BtnList;

    //====================================================================
    //
    //                           初期化処理
    //                         
    //====================================================================
    #region 初期化処理
    void Start()
    {
        //ボタンの初期化
        initButtonList();

        // 「Scroll Content」のPaddingを初期化する
        UpdateView();
    }


    void initButtonList()
    {
        // 初回だけの処理
        if (BtnList == null)
        {
            BtnList = new List<Image>();
            BtnList.Add(BTN_HOME1);
            BtnList.Add(BTN_HOME2);
            BtnList.Add(BTN_HOME3);
            BtnList.Add(BTN_HOME4);
            BtnList.Add(BTN_HOME5);
            BtnList.Add(BTN_HOME6);
            BtnList.Add(BTN_HOME7);
            BtnList.Add(BTN_HOME8);
            BtnList.Add(BTN_HOME9);
            BtnList.Add(BTN_HOME10);
            BtnList.Add(BTN_HOME11);
            BtnList.Add(BTN_HOME12);
            BtnList.Add(BTN_HOME13);
            BtnList.Add(BTN_HOME14);
            BtnList.Add(BTN_HOME15);
            BtnList.Add(BTN_HOME16);

        }
    }










    #endregion

    //====================================================================
    //
    //                        イベントハンドラ
    //                         
    //====================================================================
    #region イベントハンドラ

    #endregion


    //====================================================================
    //
    //                    スクロールイベントハンドラ
    //                         
    //====================================================================
    #region スクロールイベントハンドラ

    /// <summary>
    /// ScrollRectコンポーネントをキャッシュ
    /// </summary>
    private ScrollRect cachedScrollRect;
    public ScrollRect CachedScrollRect
    {
        get
        {
            if (cachedScrollRect == null)
            { cachedScrollRect = GetComponent<ScrollRect>(); }
            return cachedScrollRect;
        }
    }

    /// <summary>
    /// ドラッグ開始時
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // アニメーション中フラグをリセットする
        isAnimating = false;
    }

    /// <summary>
    /// ドラッグ終了時
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();

        // スクロールビューの現在の動きを止める
        CachedScrollRect.StopMovement();

        // GridLayoutGroupのcellSizeとspacingから1ページの幅を算出する
        float pageWidth = -(grid.cellSize.x + grid.spacing.x);

        // 現在のスクロール位置からフィットさせるページのインデックスを算出する
        int pageIndex =
            Mathf.RoundToInt((CachedScrollRect.content.anchoredPosition.x) / pageWidth);

        if (pageIndex == prevPageIndex && Mathf.Abs(eventData.delta.x) >= 4)
        {
            // 一定以上の速度でドラッグしていた場合、その方向に1ページ進める
            CachedScrollRect.content.anchoredPosition +=
                new Vector2(eventData.delta.x, 0.0f);
            pageIndex += (int)Mathf.Sign(-eventData.delta.x);
        }

        //修正長さ
        float fixLength = 0;

        // 先頭や末尾のページの場合、それ以上先にスクロールしないようにする
        if (pageIndex < 0)
        {
            pageIndex = 0;
            fixLength = 0;
        }
        else if (pageIndex > grid.transform.childCount - MAX_ICON_NUM)
        {
            pageIndex = grid.transform.childCount - MAX_ICON_NUM;
            fixLength = FIX_LENGTH;
        }

        prevPageIndex = pageIndex;  // 現在のページのインデックスを保持しておく

        // 最終的なスクロール位置を算出する
        float destX = pageIndex * pageWidth + fixLength;
        destPosition = new Vector2(destX, CachedScrollRect.content.anchoredPosition.y);

        // 開始時のスクロール位置を保持しておく
        initialPosition = CachedScrollRect.content.anchoredPosition;

        // アニメーションカーブを作成する
        Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);
        Keyframe keyFrame2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
        animationCurve = new AnimationCurve(keyFrame1, keyFrame2);

        // アニメーション中フラグをセットする
        isAnimating = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach(var btn in BtnList)
        {
            RectTransform rt = btn.rectTransform;



            bool isOn = RectTransformUtility.RectangleContainsScreenPoint(btn.rectTransform, eventData.position);

            if (isOn)
            {
                //ホームボタンクリック
                Debug.Log("BtnHome_Click : " + btn.name);


                //if ((data.pressPosition - data.position).magnitude < CLICK_ACCURACY)
                //{
                //    uiManager.currentMember = m_Cards[i].data;
                //    uiManager.SwitchContentToCharacter();
                //}
            }
        }
    }
    #endregion


    //====================================================================
    //
    //                   自動スクロールアニメーション
    //                         
    //====================================================================
    #region 自動スクロールアニメーション
    /// <summary>
    /// LateUpdate
    ///  毎フレームUpdateメソッドの後に呼ばれる
    /// </summary>
    void LateUpdate()
    {
        if (isAnimating)
        {
            if (Time.time >= animationCurve.keys[animationCurve.length - 1].time)
            {
                // アニメーションカーブの最後のキーフレームを過ぎたら、アニメーションを終了する
                CachedScrollRect.content.anchoredPosition = destPosition;
                isAnimating = false;
                return;
            }

            // アニメーションカーブから現在のスクロール位置を算出してスクロールビューを移動させる
            Vector2 newPosition = initialPosition +
                (destPosition - initialPosition) * animationCurve.Evaluate(Time.time);
            CachedScrollRect.content.anchoredPosition = newPosition;
        }
    }
    /// <summary>
    /// 毎フレーム呼ばれる
    /// </summary>
    void Update()
    {
        if (CachedRectTransform.rect.width != currentViewRect.width ||
           CachedRectTransform.rect.height != currentViewRect.height)
        {
            // スクロールビューの幅や高さが変化したら「Scroll Content」のPaddingを更新する
            UpdateView();
        }
    }

    /// <summary>
    /// 「Scroll Content」のPaddingを更新するメソッド
    /// </summary>
    private void UpdateView()
    {
        // スクロールビューの矩形を保持しておく
        currentViewRect = CachedRectTransform.rect;

        // GridLayoutGroupのcellSizeから「Scroll Content」のPaddingを算出して設定する
        GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
        int paddingH = 0;
        int paddingV = 0;
        grid.padding = new RectOffset(paddingH, paddingH, paddingV, paddingV);
    }


    #endregion
}
