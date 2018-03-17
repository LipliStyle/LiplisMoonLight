//=======================================================================
//  ClassName : ImgWindow
//  概要      : 吹き出しイメージウインドウ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using System;
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
    float speedFaidOut = 0.02f;

    ///=============================
    ///制御フラグ
    public bool flgOn;
    public bool flgFadeTrans;
    public bool flgEnd;

    ///=============================
    ///制御フラグ
    public string TargetModelName = "";

    ///=============================
    ///毎テキスト
    Text TxtTalkText = null;
    Image image { get; set; }

    ///=============================
    ///親ウインドウ
    public GameObject ParentWindow { get; set; }

    ///=============================
    ///移動先位置
    public Vector3 TargetPosition { get; set; }


    //================================
    //  文字制御関連プロパティ                 
    //================================

    float intervalForCharacterDisplay = 0.05f;

    public bool FlgTalking = false;
    private string currentText = string.Empty;
    private float timeUntilDisplay = 0;
    private float timeElapsed = 1;
    private int currentLine = 0;
    private int lastUpdateCharacter = -1;

    private float LipValue = 0;                 //口パク量

    /// <summary>
    /// 破棄
    /// </summary>
    private void OnDestroy()
    {
        TxtTalkText = null;
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
    /// 開始時処理
    /// </summary>
    void Start()
    {
        //イメージ取得
        image = GetComponent<Image>();

        //テキスト取得
        TxtTalkText = this.transform.Find("TxtTalkText").GetComponent<Text>();

        //アルファ値を0セット
        SetAlfa();

        this.flgOn = true;
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

        //トーク処理
        talk();
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
    private void DestroyWindow()
    {
        Destroy(TxtTalkText);
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
            TxtTalkText.color = new Color(0, 0, 0, alfa);
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

    /// <summary>
    /// トーク処理
    /// </summary>
    private void talk()
    {
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
        if (displayCharacterCount != lastUpdateCharacter)
        {
            //テキスト送り
            TxtTalkText.text = currentText.Substring(0, displayCharacterCount);

            //送りカウント
            lastUpdateCharacter = displayCharacterCount;

            //リップシンク
            LAppLive2DManager.Instance.SetLip(TargetModelName, (float)Math.Sin(LipValue * (Math.PI / 180)));

            //リップシンク送り
            LipValue = LipValue + 45f;
        }

        //終了チェック
        if (currentText.Length == lastUpdateCharacter && this.FlgTalking)
        {
            //口を閉じる
            LAppLive2DManager.Instance.StopTalking(TargetModelName);

            //おしゃべり終了
            this.FlgTalking = false;
        }
    }

    /// <summary>
    /// 次のメッセージをセットする
    /// </summary>
    /// <param name="message"></param>
    public void SetNextLine(string message)
    {
        //メッセージ設定
        currentText = message;

        //時刻単位設定
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;

        //タイムタイム初期化
        timeElapsed = Time.time;

        //ラインインクリメント
        currentLine++;

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
    /// 文字の表示が終了しているかどうか
    /// </summary>
    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUntilDisplay; }
    }

    /// <summary>
    /// スキップ処理
    /// </summary>
    public void Skip()
    {
        // 完了してないなら文字をすべて表示する
        if (!IsCompleteDisplayText)
        {
            timeUntilDisplay = 0;
        }
    }

}
