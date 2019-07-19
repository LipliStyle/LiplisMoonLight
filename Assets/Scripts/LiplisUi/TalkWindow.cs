//=======================================================================
//  ClassName : ImgWindow
//  概要      : 吹き出しイメージウインドウ
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
#pragma warning disable 649,414
using Assets.Scripts.Data;
using Assets.Scripts.LiplisSystem.Model;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TalkWindow : MonoBehaviour {


    //================================
    //  ウインドウ制御関連プロパティ                 
    //================================

    ///=============================
    ///透明度制御
    public float alfa;
    float speedFaidIn = 0.05f;
    float speedFaidOut = 0.1f;

    ///=============================
    ///制御フラグ
    public bool flgOn;
    public bool flgFadeTrans;
    public bool flgEnd;

    ///=============================
    ///対象モデル
    public IfsLiplisModel TargetModel;

    //================================
    //テキスト
    [SerializeField]
    Text TextFukidashi;

    //================================
    //メイントークウインドウテキスト
    Text TextMainTalk;

    ///=============================
    ///背景イメージ
    Image image { get; set; }

    ///=============================
    ///ウインドウ生成時刻
    public DateTime CreateTime { get; set; }

    ///=============================
    ///親ウインドウ
    public GameObject ParentWindow { get; set; }

    ///=============================
    ///移動先位置
    public Vector3 TargetPosition { get; set; }

    //おしゃべり中フラグ
    public bool FlgTalking = false;

    //スキップフラグ
    private bool FlgSkip = false;
    
    ///=============================
    ///サイズ、表示位置
    public float heightImg { get; set; }

    //================================
    // 文字制御関連プロパティ                 
    private string currentText = string.Empty;
    private int lastUpdateCharacter = -1;

    //================================
    // レイアウトエレメント    
    LayoutElement _layoutElement;
    public LayoutElement layoutElement { get { return _layoutElement; } }

    //================================
    // コンテントサイズフィルター
    ContentSizeFitter _contentSizeFitter;
    public ContentSizeFitter contentSizeFitter { get { return _contentSizeFitter; } }

    //================================
    // テキストバッファ
    StringBuilder TextBuffer = new StringBuilder();

    //================================
    //レクトトランスフォーム
    RectTransform _RectMine;
    RectTransform _RectText;


    //====================================================================
    //
    //                         ウインドウ制御
    //                         
    //====================================================================
    #region ウインドウ制御
    /// <summary>
    /// 破棄
    /// </summary>
    private void OnDestroy()
    {
        image = null;
        ParentWindow = null;
    }

    /// <summary>
    /// 親ウインドウのセット
    /// </summary>
    /// <param name="ParentWindow"></param>
    public void SetParentWindow(GameObject ParentWindow)
    {
        //親ウインドウ設定
        this.ParentWindow = ParentWindow;

        //移動目標の初期化
        if (ParentWindow != null)
        {
            this.TargetPosition = ParentWindow.transform.localPosition;
        }
    }

    /// <summary>
    /// メインテキストを設定する
    /// </summary>
    /// <param name="text"></param>
    public void SetMainText(Text text)
    {
        this.TextMainTalk = text;
    }


    /// <summary>
    /// 移動対象を設定する
    /// </summary>
    /// <param name="TargetPosition"></param>
    public void SetMoveTarget(Vector3 TargetPosition)
    {
        this.TargetPosition = TargetPosition;
    }


    /// <summary>
    /// 高さ設定
    /// </summary>
    /// <param name="heightImg"></param>
    public void SetHeightImg(float heightImg)
    {
        this.heightImg = heightImg;
    }

    /// <summary>
    /// 生成時刻を設定する
    /// </summary>
    /// <param name="targetDate"></param>
    public void SetCreateTime(DateTime targetDate)
    {
        this.CreateTime = targetDate;
    }

    /// <summary>
    /// テクスチャをセットする
    /// </summary>
    /// <param name="texture"></param>
    public void SetWindowImage(Sprite WindowSprite)
    {
        
    }

    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
    {
        //イメージ取得
        image = this.GetComponent<Image>();

        image.color = new Color(0.0f / 255.0f, 0.0f / 0.0f, 93.0f / 0.0f, 100.0f / 255.0f);

        //アルファ値を0セット
        SetAlfa();

        this.flgOn = true;

        //テキスト関連
        _layoutElement = GetComponent<LayoutElement>();
        _contentSizeFitter = GetComponent<ContentSizeFitter>();

        //レクト取得
        _RectMine = GetComponent<RectTransform>();
        _RectText = TextFukidashi.GetComponent<RectTransform>();

        //タイマースタート
        startTimer();
    }


    /// <summary>
    /// タイマースタート
    /// </summary>
    void startTimer()
    {
        StartCoroutine(UpdateTick());
    }

    /// <summary>
    /// ウインドウを閉じる
    /// </summary>
    public void CloseWindow()
    {
        this.flgEnd = true;
    }

    private const float UPDATE_INTERVAL = 0.1f;

    /// <summary>
    /// ウインドウメンテループ　
    /// 1秒周期で実行
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateTick()
    {
        while (true)
        {
            //トーク処理
            if (!FlgSkip)
            {
                Talk();
            }

            //非同期待機
            yield return new WaitForSeconds(LiplisSetting.Instance.Setting.GetTalkSpeed());
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //フェードイン処理
        faidIn();

        //フェードアウト処理
        fadeOut();
        
        //フェード透過
        fadeTrans();

        //位置の更新
        UpdatePosition();

        if (FlgSkip)
        {
            Skip();
        }
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
        if(!flgFadeTrans)
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

        //ARGB取得
        SetAlfa();
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    private void fadeOut()
    {
        //フラグチェック
        if(!flgEnd)
        {
            return;
        }

        //アルファ値加算
        alfa -= speedFaidOut;

        //アルファ値チェック
        if (alfa <= 0)
        {
            flgEnd = false;
            alfa = 0f;

            DestroyWindow();
        }

        SetAlfa();
    }

    /// <summary>
    /// ウインドウを破棄する
    /// </summary>
    public void DestroyWindow()
    {
        Destroy(image);
        Destroy(ParentWindow);
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
            //TxtTalkText.color = new Color(0, 0, 0, alfa);
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
    /// 位置の更新
    /// </summary>
    public void UpdatePosition()
    {
        //NULL回避
        if(ParentWindow == null)
        {
            return;
        }

        //判定
        if (this.TargetPosition.x != this.ParentWindow.transform.localPosition.x ||
            this.TargetPosition.y != this.ParentWindow.transform.localPosition.y ||
            this.TargetPosition.z != this.ParentWindow.transform.localPosition.z)
        {
            //移動量算出
            float diffX = this.TargetPosition.x - this.ParentWindow.transform.localPosition.x;
            float diffY = this.TargetPosition.y - this.ParentWindow.transform.localPosition.y;
            float diffZ = this.TargetPosition.z - this.ParentWindow.transform.localPosition.z;

            float x = 0;
            float y = 0;
            float z = 0;
            float moveVal = 1.0f * (Screen.width / 360.0f);

            if (diffX > 0)
            {
                x = this.ParentWindow.transform.localPosition.x + moveVal;
            }
            else if (diffX == 0 || Math.Abs(diffX) < 2)
            {
                x = this.ParentWindow.transform.localPosition.x;
            }
            else if (diffX < 0)
            {
                x = this.ParentWindow.transform.localPosition.x;
            }
            else
            {
                x = this.ParentWindow.transform.localPosition.x - moveVal;
            }

            if (diffY > 0)
            {
                y = this.ParentWindow.transform.localPosition.y + moveVal;
            }
            else if (diffY == 0 || Math.Abs(diffY) < 2)
            {
                y = this.ParentWindow.transform.localPosition.y;
            }
            else if (diffY < 0)
            {
                y = this.ParentWindow.transform.localPosition.y;
            }
            else
            {
                y = this.ParentWindow.transform.localPosition.y - moveVal;
            }

            if (diffZ > 0)
            {
                z = this.ParentWindow.transform.localPosition.z + moveVal;
            }
            else if (diffZ == 0 || Math.Abs(diffZ) < 2)
            {
                z = this.ParentWindow.transform.localPosition.z;
            }
            else if (diffZ < 0)
            {
                z = this.ParentWindow.transform.localPosition.z;
            }
            else
            {
                z = this.ParentWindow.transform.localPosition.z - moveVal;
            }

            //移動
            ParentWindow.transform.localPosition = new Vector3(x, y, z);
        }

        //高さ設定
        _layoutElement.preferredHeight = _RectText.rect.height + 33;
    }

    private void moveEnd()
    {

    }

    #endregion

    //====================================================================
    //
    //                         会話関連処理
    //                         
    //====================================================================
    #region 会話関連処理

    /// <summary>
    /// トーク処理
    /// </summary>
    private void Talk()
    {
        //終了チェック
        if (!this.FlgTalking || (currentText.Length <= lastUpdateCharacter))
        {
            return;
        }

        try
        {
            //テキスト送り

            if (currentText == "")
            {
                return;
            }

            if (lastUpdateCharacter == -1)
            {
                //最初はなにもしない
            }
            else
            {
                setText(currentText.Substring(lastUpdateCharacter, 1));
            }

            //送りカウント
            lastUpdateCharacter++;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }


        //終了チェック
        if (currentText.Length <= lastUpdateCharacter && this.FlgTalking)
        {
            //口を閉じる
            TargetModel.StopTalking();

            //おしゃべり終了
            this.FlgTalking = false;
        }
    }

    /// <summary>
    /// スキップ
    /// </summary>
    private void Skip()
    {
        if(!this.FlgTalking)
        {
            return;
        }

        if (currentText == "")
        {
            return;
        }

        //トークを高速で進める
        Talk();
    }

    /// <summary>
    /// 次のメッセージをセットする
    /// </summary>
    /// <param name="message"></param>
    public void SetNextLine(string message)
    {
        //メッセージ設定
        currentText = message;

        //最終更新文字初期化
        lastUpdateCharacter = -1;

        //おしゃべり中に変更
        this.FlgTalking = true;
    }

    /// <summary>
    /// テキストを追加する
    /// </summary>
    /// <param name="message"></param>
    public void AddText(string message)
    {
        //テキスト追加
        currentText = currentText + message;

        //おしゃべり中に変更
        this.FlgTalking = true;
    }

    /// <summary>
    /// スキップ処理
    /// </summary>
    public void SetSkip()
    {
        FlgSkip = true;
    }
    
    /// <summary>
    /// 非同期送り
    /// </summary>
    /// <param name="text"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public void setText(string text)
    {
        //テキストセット
        TextFukidashi.text = TextBuffer.Append(text).ToString();
        TextMainTalk.text = TextFukidashi.text;
    }

    /// <summary>
    /// バブルテキストを初期化する
    /// </summary>
    public void cleanBubbleText()
    {
        TextFukidashi.text = string.Empty;
    }

    /// <summary>
    /// 現在入力中のテキストを取得する
    /// </summary>
    /// <returns></returns>
    public string GetCurrentText()
    {
        return currentText;
    }

    #endregion
}
