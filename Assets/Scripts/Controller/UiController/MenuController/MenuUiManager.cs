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
#pragma warning disable 649,414
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Data;
using Assets.Scripts.Define;

public class MenuUiManager : MonoBehaviour
{
    //=============================
    // 制御用プロパティ
    //フェード時に暗転させるUI
    [SerializeField] private CoverUI coverUI;

    //=============================
    //現在選択中のページ
    [SerializeField] private ContentCategoly CurrentContentCategoly = ContentCategoly.home;

    //=============================
    //コンテントリスト
    [SerializeField] private ContentController[] ContentsPage;

    //=============================
    //モード表示テキスト
    [SerializeField] Text TxtTalkMode;
    [SerializeField] Image ImgMode;

    //=============================
    // レンダリングUIキャッシュ
    public GameObject UiRenderingFront;

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
            content.gameObject.SetActive(true);
            content.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// スタート処理
    /// </summary>
    void Start () {
        StartCoroutine(SwitchContent(CurrentContentCategoly));

        initWindow();
    }

    /// <summary>
    /// ウインドウの初期化
    /// </summary>
    private void initWindow()
    {
        TxtTalkMode.text = "おまかせ";
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
            //フェードアウト
            coverUI.FadeOut(DURATION_FAID_OUT);
            yield return new WaitForSeconds(DURATION_FAID_OUT);

            //現在のオブジェクトを無効にする
            ContentsPage[(int)CurrentContentCategoly].gameObject.SetActive(false);

            //モード変更
            CurrentContentCategoly = nextContent;
            ChangeMode(nextContent);

            //新しくセットしたオブジェクトを有効にする
            ContentsPage[(int)CurrentContentCategoly].gameObject.SetActive(true);

            //フェードイン
            yield return new WaitForSeconds(DURATION_FAID_IN);
            coverUI.FadeIn(DURATION_FAID_IN);

        }
        else
        {
            //変更がなければ現在のオブジェクトを有効化
            ContentsPage[(int)CurrentContentCategoly].gameObject.SetActive(true);
            yield break;
        }
    }

    /// <summary>
    /// モード変更
    /// </summary>
    private void ChangeMode(ContentCategoly nextContent)
    {
        TxtTalkMode.text = ContentCategolyText.GetContentText(nextContent);
        LiplisStatus.Instance.EnvironmentInfo.SelectMode = nextContent;
    }


    /// <summary>
    /// ホームボタンクリック
    /// </summary>
    public void Btn_Home_Click()
    {
        //フロント画面が非アクティブのときは何もしない
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        StartCoroutine(SwitchContent(ContentCategoly.home));
    }

    /// <summary>
    /// ニュースボタンクリック
    /// </summary>
    public void Btn_News_Click()
    {
        //フロント画面が非アクティブのときは何もしない
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        StartCoroutine(SwitchContent(ContentCategoly.news));
    }

    /// <summary>
    /// まとめボタンクリック
    /// </summary>
    public void Btn_Matome_Click()
    {
        //フロント画面が非アクティブのときは何もしない
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        StartCoroutine(SwitchContent(ContentCategoly.matome));
    }

    /// <summary>
    /// リツイートボタンクリック
    /// </summary>
    public void Btn_Retweet_Click()
    {
        //フロント画面が非アクティブのときは何もしない
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        StartCoroutine(SwitchContent(ContentCategoly.retweet));
    }

    /// <summary>
    /// 話題の画像ボタンクリック
    /// </summary>
    public void Btn_HotPicture_Click()
    {
        //フロント画面が非アクティブのときは何もしない
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        StartCoroutine(SwitchContent(ContentCategoly.hotPicture));
    }

    /// <summary>
    /// ハッシュボタンクリック
    /// </summary>
    public void Btn_TweetHash_Click()
    {
        //フロント画面が非アクティブのときは何もしない
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        StartCoroutine(SwitchContent(ContentCategoly.hotHash));
    }
    #endregion  
}
