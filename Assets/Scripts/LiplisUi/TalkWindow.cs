//=======================================================================
//  ClassName : ImgWindow
//  概要      : 吹き出しイメージウインドウ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
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
    ///制御フラグ
    public IfsLiplisModel TargetModel;

    ///=============================
    ///マイテキスト
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
    //  新　文字制御関連プロパティ                 
    //================================
    private string currentText = string.Empty;
    private float timeUntilDisplay = 0;
    private int lastUpdateCharacter = -1;
    private float LipValue = 0;                 //口パク量

    [SerializeField]
    public BubbleTextCtrl _bubbleText;

    [SerializeField]
    float _maxImageWidth = 0f; // zero to inf
    public float maxImageWidth { set { _maxImageWidth = value; } get { return _maxImageWidth; } }

    [SerializeField]
    float _maxImageHeight = 0f;
    public float maxImageHeight { set { _maxImageHeight = value; } get { return _maxImageHeight; } }

    [SerializeField]
    float _printSpeed = 0.1f;
    public float printSpeed { set { _printSpeed = value; } get { return _printSpeed; } }

    [SerializeField]
    bool _isSayToClear;
    public bool isSayToClear { set { _isSayToClear = value; } get { return _isSayToClear; } }

    LayoutElement _layoutElement;
    public LayoutElement layoutElement { get { return _layoutElement; } }

    ContentSizeFitter _contentSizeFitter;
    public ContentSizeFitter contentSizeFitter { get { return _contentSizeFitter; } }

    StringBuilder _builder = new StringBuilder();
    public bool isTextPrinting { private set; get; }

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
            this.TargetPosition = ParentWindow.transform.position;
        }
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
    /// 開始時処理
    /// </summary>
    void Start()
    {
        //イメージ取得
        image = GetComponent<Image>();

        //アルファ値を0セット
        SetAlfa();

        this.flgOn = true;

        //テキスト関連
        isTextPrinting = false;
        _layoutElement = GetComponent<LayoutElement>();
        _contentSizeFitter = GetComponent<ContentSizeFitter>();

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
        if (this.TargetPosition.x != this.ParentWindow.transform.position.x ||
            this.TargetPosition.y != this.ParentWindow.transform.position.y ||
            this.TargetPosition.z != this.ParentWindow.transform.position.z)
        {
            //移動量算出
            float diffX = this.TargetPosition.x - this.ParentWindow.transform.position.x;
            float diffY = this.TargetPosition.y - this.ParentWindow.transform.position.y;
            float diffZ = this.TargetPosition.z - this.ParentWindow.transform.position.z;

            float x = 0;
            float y = 0;
            float z = 0;
            float moveVal = 1.0f * (Screen.width / 360.0f);

            if (diffX > 0)
            {
                x = this.ParentWindow.transform.position.x + moveVal;
            }
            else if (diffX == 0 || Math.Abs(diffX) < 2)
            {
                x = this.ParentWindow.transform.position.x;
            }
            else if (diffX < 0)
            {
                x = this.ParentWindow.transform.position.x;
            }
            else
            {
                x = this.ParentWindow.transform.position.x - moveVal;
            }

            if (diffY > 0)
            {
                y = this.ParentWindow.transform.position.y + moveVal;
            }
            else if (diffY == 0 || Math.Abs(diffY) < 2)
            {
                y = this.ParentWindow.transform.position.y;
            }
            else if (diffY < 0)
            {
                y = this.ParentWindow.transform.position.y;
            }
            else
            {
                y = this.ParentWindow.transform.position.y - moveVal;
            }

            if (diffZ > 0)
            {
                z = this.ParentWindow.transform.position.z + moveVal;
            }
            else if (diffZ == 0 || Math.Abs(diffZ) < 2)
            {
                z = this.ParentWindow.transform.position.z;
            }
            else if (diffZ < 0)
            {
                z = this.ParentWindow.transform.position.z;
            }
            else
            {
                z = this.ParentWindow.transform.position.z - moveVal;
            }

            //移動
            ParentWindow.transform.position = new Vector3(x, y, z);
        }
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

        //リップシンク有効
        this.TargetModel.StartTalking();

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
        _bubbleText.text.text = _builder.Append(text).ToString();
        if (_bubbleText.text.preferredWidth + 100 < _maxImageWidth)
        {
            _layoutElement.preferredWidth = _bubbleText.text.preferredWidth + 100;
        }

        if (_bubbleText.text.preferredHeight + 42 < _maxImageHeight)
        {
            _layoutElement.preferredHeight = _bubbleText.text.preferredHeight + 42;
        }
        else
        {
            _layoutElement.preferredHeight = _bubbleText.text.preferredHeight;
        }
    }

    /// <summary>
    /// バブルテキストを初期化する
    /// </summary>
    public void cleanBubbleText()
    {
        _bubbleText.text.text = string.Empty;
    }

    #endregion
}
