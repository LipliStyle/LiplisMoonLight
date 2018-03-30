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
using System;
using System.Collections;

public class MenuUiManager : MonoBehaviour
{
    ///=============================
    /// 制御用プロパティ
    //フェード時に暗転させるUI
    [SerializeField] private CoverUI coverUI;

    //現在選択中のページ
    [SerializeField] private ContentCategoly CurrentContentCategoly = ContentCategoly.home;

    //コンテントリスト
    [SerializeField] private ContentController[] ContentsPage;

    ///=============================
    /// フェード制御プロパティ
    /// ここでメニューのフェードアニメーションの速さを設定する
     
    // フェードインインターバル
    private float DURATION_FAID_IN = 0.1f;

    //フェードアウトインターバル
    private float DURATION_FAID_OUT = 0.2f;


    //====================================================================
    //
    //                          コンテンツ定義
    //                         
    //====================================================================
    #region コンテンツ定義

    /// <summary>
    /// コンテンツ定義
    /// 
    /// ここで設定するインデックスは、Unity上に設定したページの順番と揃える必要あり！
    /// </summary>
    public enum ContentCategoly
    {
        home = 0,
        omakase = 1,
        news = 2
    }

    #endregion

    //====================================================================
    //
    //                          初期化処理
    //                         
    //====================================================================
    #region 初期化処理

    /// <summary>
    /// アウェイク
    /// </summary>
    void Awake()
    {
        foreach (var content in ContentsPage)
        {
            content.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// スタート処理
    /// </summary>
    void Start () {
        StartCoroutine(SwitchContent(CurrentContentCategoly));
    }

    #endregion

    //====================================================================
    //
    //                          メニュー操作
    //                         
    //====================================================================
    #region メニュー操作

    /// <summary>
    /// コンテンツ切り替え
    /// </summary>
    /// <param name="nextContent"></param>
    /// <returns></returns>
    IEnumerator SwitchContent(ContentCategoly nextContent)
    {
        if (CurrentContentCategoly != nextContent)
        {
            coverUI.FadeOut(DURATION_FAID_OUT);
            yield return new WaitForSeconds(DURATION_FAID_OUT);

            ContentsPage[(int)CurrentContentCategoly].gameObject.SetActive(false);
            CurrentContentCategoly = nextContent;
            ContentsPage[(int)CurrentContentCategoly].gameObject.SetActive(true);

            yield return new WaitForSeconds(DURATION_FAID_IN);
            coverUI.FadeIn(DURATION_FAID_IN);

        }
        else
        {

            ContentsPage[(int)CurrentContentCategoly].gameObject.SetActive(true);
            yield break;
        }
    }


    /// <summary>
    /// ホームボタンクリック
    /// </summary>
    public void Btn_Home_Click()
    {
        Debug.Log("GameCtlr BtnHome_Click");
        StartCoroutine(SwitchContent(ContentCategoly.home));
    }

    /// <summary>
    /// おまかせボタンクリック
    /// </summary>
    public void Btn_Omakase_Click()
    {
        Debug.Log("GameCtlr BtnOmakase_Click");
        StartCoroutine(SwitchContent(ContentCategoly.omakase));
    }

    /// <summary>
    /// ニュースボタンクリック
    /// </summary>
    public void Btn_News_Click()
    {
        Debug.Log("GameCtlr Btn_News_Click");
        StartCoroutine(SwitchContent(ContentCategoly.news));
    }

    #endregion  
}
