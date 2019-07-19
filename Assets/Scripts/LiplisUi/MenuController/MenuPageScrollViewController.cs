//=======================================================================
//  ClassName : PageControl
//  概要      : スクロールページコントローラ
//              
//使い方 スクロールのルートにこのスクリプトを貼り付ける。
//  　　 このスクリプトは縦(y)方向のコントロールとして作られている。横(x)方向にシたい場合は、xとyを入れ替えればOK
//       DISPLAY_ICON_NUMに、画面に表示するアイコンの工数を設定する。
//       FIX_LENGTHに最後尾のアイコンまでスクロールしたときの移動量を調整する長さ
//
///
//  LiplisMoonlight
//  Create 2018/03/11
//
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Assets.Scripts.LiplisUi.MenuController;

[RequireComponent(typeof(ScrollRect))]
public class MenuPageScrollViewController : MenuScrollViewController, IBeginDragHandler, IEndDragHandler
// ViewControllerクラスを継承して、IBeginDragHandlerインターフェイスと
// IEndDragHandlerインターフェイスを実装する
{

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float key1InTangent = 0.0f;
    [SerializeField] private float key1OutTangent = 0.1f;
    [SerializeField] private float key2InTangent = 0.0f;
    [SerializeField] private float key2OutTangent = 0.0f;
    [SerializeField] private int DISPLAY_ICON_NUM = 6; //画面上に表示するボタン数
    [SerializeField] private int FIX_LENGTH = 0;        //反対方向にオーバーしたときに修正する長さ。エディタ上で設定。ここでいじっても意味なし

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
        // 「Scroll Content」のPaddingを初期化する
        UpdateView();
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

        // GridLayoutGroupのcellSizeとspacingから1ページの高さを算出する
        float pageHeight = grid.cellSize.y + grid.spacing.y;

        // 現在のスクロール位置からフィットさせるページのインデックスを算出する
        int pageIndex =
            Mathf.RoundToInt((CachedScrollRect.content.anchoredPosition.y) / pageHeight);

        if (pageIndex == prevPageIndex && Mathf.Abs(eventData.delta.y) >= 4)
        {
            // 一定以上の速度でドラッグしていた場合、その方向に1ページ進める
            CachedScrollRect.content.anchoredPosition +=
                new Vector2(eventData.delta.y, 0.0f);
            pageIndex += (int)Mathf.Sign(eventData.delta.y);
        }

        //修正長さ
        float fixLength = 0;

        // 先頭や末尾のページの場合、それ以上先にスクロールしないようにする
        if (pageIndex < 0)
        {
            pageIndex = 0;
            fixLength = 0;
        }
        else if (pageIndex > grid.transform.childCount - DISPLAY_ICON_NUM)
        {
            pageIndex = grid.transform.childCount - DISPLAY_ICON_NUM;
            fixLength = FIX_LENGTH;
            //pageIndex = 0;
            //fixLength = 0;

        }

        prevPageIndex = pageIndex;  // 現在のページのインデックスを保持しておく

        // 最終的なスクロール位置を算出する
        float destY = pageIndex * pageHeight + fixLength;

        //横方向のブレを抑止するためxに0を設定
        //destPosition = new Vector2(CachedScrollRect.content.anchoredPosition.x, destY);
        destPosition = new Vector2( 0, destY);

        // 開始時のスクロール位置を保持しておく。開始位置もぶれないように0をセットしておく
        initialPosition = CachedScrollRect.content.anchoredPosition;
        initialPosition.x = 0;

        // アニメーションカーブを作成する
        Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);
        Keyframe keyFrame2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
        animationCurve = new AnimationCurve(keyFrame1, keyFrame2);

        // アニメーション中フラグをセットする
        isAnimating = true;
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

            if(destPosition.x!=0)
            {
                Debug.Log("");
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
