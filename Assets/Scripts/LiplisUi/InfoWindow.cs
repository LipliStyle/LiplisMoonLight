//=======================================================================
//  ClassName : InfoWindow
//  概要      : 情報ウインドウ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using SpicyPixel.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : ConcurrentBehaviour
{

    ///=============================
    ///透明度制御
    public float alfa = 0;
    float speedFaidIn = 0.05f;
    float speedFaidOut = 0.02f;

    ///=============================
    ///オブジェクト
    Text TitleTalkText = null;
    Image image { get; set; }

    ///=============================
    ///親ウインドウ
    public GameObject ParentWindow { get; set; }

    ///=============================
    ///登録オブジェクト
    public DateTime CreateTime { get; set; }

    ///=============================
    ///制御フラグ
    public bool flgOn;
    public bool flgFadeTrans;
    public bool flgEnd;

    /// <summary>
    /// アウェーク
    /// </summary>
    protected override void Awake()
    {
        //ベースアウェーク(初期化)
        base.Awake();
    }

    // Use this for initialization
    void Start () {
        //イメージセット
        image = GetComponent<Image>();

        //テキスト取得
        TitleTalkText = this.transform.Find("TitleTalkText").GetComponent<Text>();

        //アルファ値を0セット
        SetAlfa();

        this.flgOn = true;
    }

    /// <summary>
    /// 親ウインドウのセット
    /// </summary>
    /// <param name="ParentWindow"></param>
    public void SetParentWindow(GameObject ParentWindow)
    {
        //親ウインドウ設定
        this.ParentWindow = ParentWindow;
    }

    /// <summary>
    /// ウインドウ生成時刻をセットする
    /// </summary>
    /// <param name="CreateTime"></param>
    public void SetCreateTime(DateTime CreateTime)
    {
        this.CreateTime = CreateTime;
    }

    // Update is called once per frame
    void Update () {
        //フェードイン処理
        faidIn();

        //フェードアウト処理
        fadeOut();

        //フェード透過
        fadeTrans();

    }

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    private void faidIn()
    {
        //フラグチェック
        if (!flgOn)
        {
            return;
        }

        //アルファ値加算
        alfa += speedFaidIn;

        //アルファ値チェック
        if (alfa >= 1.0f)
        {
            flgOn = false;
            alfa = 1.0f;
        }

        //ARGB取得
        SetAlfa();
    }

    /// <summary>
    /// フェードトランス
    /// </summary>
    private void fadeTrans()
    {
        //フラグチェック
        if (!flgFadeTrans)
        {
            return;
        }

        //アルファ値加算
        alfa -= speedFaidOut;

        //アルファ値チェック
        if (alfa <= 0.5)
        {
            flgFadeTrans = false;
            alfa = 0.5f;
        }

        SetAlfa();
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    private void fadeOut()
    {
        //フラグチェック
        if (!flgEnd)
        {
            return;
        }

        //アルファ値加算
        alfa -= speedFaidOut;

        //ARGB取得
        SetAlfa();

        //アルファ値チェック
        if (alfa <= 0)
        {
            flgEnd = false;
            alfa = 0f;

            Destroy(ParentWindow);
        }
    }

    /// <summary>
    /// 透明度設定
    /// </summary>
    void SetAlfa()
    {
        SetAlfaImageColor();
        SetAlfaText();
    }
    void SetAlfaText()
    {
        try
        {
            TitleTalkText.color = new Color(0, 0, 0, alfa);
        }
        catch
        {

        }
    }
    void SetAlfaImageColor()
    {
        try
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alfa);
        }
        catch
        {

        }
    }

    /// <summary>
    /// テキストセット
    /// </summary>
    /// <param name="message"></param>
    public void SetText(string title)
    {
        if(TitleTalkText == null)
        {
            this.TitleTalkText = this.transform.Find("TitleTalkText").GetComponent<Text>();
        }

        this.TitleTalkText.text = title.Trim();
    }


    /// <summary>
    /// 移動対象を設定する
    /// </summary>
    /// <param name="TargetPosition"></param>
    public void SetMoveTarget(Vector3 TargetPosition)
    {
        this.ParentWindow.transform.localPosition = TargetPosition;
    }

    /// <summary>
    /// ウインドウを閉じる
    /// </summary>
    public void CloseWindow()
    {
        this.flgEnd = true;
    }
}
