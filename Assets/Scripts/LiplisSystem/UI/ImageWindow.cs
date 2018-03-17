//=======================================================================
//  ClassName : ImageWindow
//  概要      : イメージウインドウ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.UI;
using Assets.Scripts.Utils;
using SpicyPixel.Threading;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageWindow : ConcurrentBehaviour
{
    ///=============================
    ///画面制御
    Texture2D tex;      //最大表示テクスチャ
    Texture2D texMin;   //サムネイルテクスチャ
    Sprite sprite;      //最大表示スプライト
    Sprite spriteMin;   //サムネイルスプライト
    float MaxWidth = 0;    //幅
    float MaxHeight = 0;   //高さ
    float MaxLocationX = 0;   //マックス時X座標
    public float TexWidth = 0;    //幅
    public float TexHeight = 0;   //高さ

    ///=============================
    ///子イメージ名
    private const string TOPIC_IMAGE = "TopicImage";

    ///=============================
    ///計算用定数
    private const float ASPECT_RATIO_BOUNDARY = 0.5625f; // アスペクト比境界値 9/16の値 
    private const float 
        _FRAME = 30f;

    ///=============================
    ///子要素インスタンス
    RectTransform rect;
    Image image;
    Image baseImage;

    ///=============================
    ///親ウインドウ
    public GameObject ParentWindow { get; set; }
    public RectTransform ParentWindowRect { get; set; }

    ///=============================
    ///移動先位置
    public Vector3 TargetPosition { get; set; }

    ///=============================
    ///移動先
    float moveTargetWidth     = 0;   //移動先幅
    float moveTargetHeight    = 0;   //移動先高さ
    float moveTargetLocationX = 0;   //移動先座標 X
    float moveTargetLocationY = 0;   //移動先座標 Y

    ///=============================
    ///移動量
    float moveTargetIncrimentalValWidth     = 0;   //移動先幅
    float moveTargetIncrimentalValHeight    = 0;   //移動先高さ
    float moveTargetIncrimentalValLocationX = 0;   //移動先座標 X
    float moveTargetIncrimentalValLocationY = 0;   //移動先座標 Y

    ///=============================
    ///移動中制御
    float movingWidth     = 0;   //移動中幅
    float movingHeight    = 0;   //移動中高さ
    float movingLocationX = 0;   //移動中座標 X
    float movingLocationY = 0;   //移動中座標 Y
    bool flgMoving        = false;


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
    ///デフォルト画像サイズ
    public const float IMAGE_SIZE_DEFAULT_WIDTH = 400f;
    public const float IMAGE_SIZE_DEFAULT_HEIGTH = 400f;

    /// <summary>
    /// アウェーク
    /// </summary>
    protected override void Awake()
    {
        //ベースアウェーク(初期化)
        base.Awake();
    }

    /// <summary>
    /// スタート
    /// </summary>
    void Start () {
        init();
    }

    /// <summary>
    /// クラス初期化
    /// </summary>
    void init()
    {
        //レクト取得
        this.rect = GameObject.Find(TOPIC_IMAGE).GetComponent<RectTransform>();

        //イメージインスタンス取得
        this.image = GameObject.Find(TOPIC_IMAGE).GetComponent<Image>();
       // this.baseImage = GetComponent<Image>();

        this.moveTargetWidth = IMAGE_SIZE_DEFAULT_WIDTH;
        this.moveTargetHeight = IMAGE_SIZE_DEFAULT_HEIGTH;

        this.moveTargetLocationX = -5;
        this.moveTargetLocationY = -10;
    }

    /// <summary>
    /// 親ウインドウのセット
    /// </summary>
    /// <param name="ParentWindow"></param>
    public void SetParentWindow(GameObject ParentWindow)
    {
        //親ウインドウ設定
        this.ParentWindow = ParentWindow;
        this.ParentWindowRect = this.ParentWindow.GetComponent<RectTransform>();

        //移動目標の初期化
        if (ParentWindow != null)
        {
            this.TargetPosition = ParentWindow.transform.position;
        }
    }

    /// <summary>
    /// 画面更新処理
    /// </summary>
    void Update () {
        UpdateMove();

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

        //ARGB取得
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

        //アルファ値チェック
        if (alfa <= 0)
        {
            flgEnd = false;
            alfa = 0f; 
        }

        SetAlfa();
    }

    /// <summary>
    /// 透明度設定
    /// </summary>
    void SetAlfa()
    {
        SetAlfaBaseImage();
        SetAlfaImageColor();
    }
    void SetAlfaBaseImage()
    {
        try
        {
            baseImage.color = new Color(baseImage.color.r, baseImage.color.g, baseImage.color.b, alfa);
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
    /// 移動処理
    /// </summary>
    void UpdateMove()
    {
        //移動中でなければ抜ける
        if (!flgMoving)
        {
            return;
        }

        //座標移動
        this.movingWidth += this.moveTargetIncrimentalValWidth;
        this.movingHeight += this.moveTargetIncrimentalValHeight;
        this.movingLocationX += this.moveTargetIncrimentalValLocationX;
        this.movingLocationY += this.moveTargetIncrimentalValLocationY;

        //ターゲットによる場合分け
        if (this.moveTargetWidth != TexWidth)
        {
            //移動完了チェック
            if(this.movingWidth <= this.moveTargetWidth)
            {
                //移動完了
                this.flgMoving = false;

                //移動位置調整
                this.movingWidth = this.moveTargetWidth;
                this.movingHeight = this.moveTargetHeight;
                this.movingLocationX = this.moveTargetLocationX;
                this.movingLocationY = this.moveTargetLocationY;

                this.image.color = new Color(this.image.color.r, this.image.color.g, this.image.color.b, 1.0f);
            }
        }
        else
        {
            //移動完了チェック
            if (this.movingWidth >= this.moveTargetWidth)
            {
                //移動完了
                this.flgMoving = false;

                //移動位置調整
                this.movingWidth = this.moveTargetWidth;
                this.movingHeight = this.moveTargetHeight;
                this.movingLocationX = this.moveTargetLocationX;
                this.movingLocationY = this.moveTargetLocationY;

                this.image.color = new Color(this.image.color.r, this.image.color.g, this.image.color.b, 1.0f);

            }
        }

        this.rect.sizeDelta = new Vector2(this.movingWidth, this.movingHeight);
        this.rect.localPosition = new Vector3(this.movingLocationX, this.movingLocationY, 0);

        //スプライト設定
        this.image.GetComponent<Image>().sprite = sprite;
    }



    /// <summary>
    /// クリックイベント
    /// </summary>
    public void OnPointerClick()
    {
        //Vector3 mousePosition = Input.mousePosition;
        //Vector3 loc =ParentWindowRect.localPosition;

        SetMoveTarget();
    }


    /// <summary>
    /// 移動先を設定する
    /// </summary>
    void SetMoveTarget()
    {
        flgMoving = true;

        if (this.moveTargetWidth != TexWidth)
        {
            SetMoveTargetMin();
        }
        else
        {
            SetMoveTargetMax();
        }
    }

    /// <summary>
    /// 最小表示
    /// </summary>
    void SetMoveTargetMin()
    {
        this.moveTargetWidth = TexWidth;
        this.moveTargetHeight = TexHeight;
        this.moveTargetLocationX = 0;
        this.moveTargetLocationY = 0;
    }

    /// <summary>
    /// 最大表示
    /// </summary>
    void SetMoveTargetMax()
    {
        this.moveTargetWidth = MaxWidth;
        this.moveTargetHeight = MaxHeight;
        this.moveTargetLocationX = MaxLocationX;
        this.moveTargetLocationY = 0;
    }

    /// <summary>
    /// 最大表示になっている場合、サイズ是正する
    /// </summary>
    void FixPicture()
    {
        //一旦元に戻す
        this.moveTargetWidth = TexWidth;
        this.moveTargetHeight = TexHeight;
        this.moveTargetLocationX = 0;
        this.moveTargetLocationY = -0;

        this.movingWidth = this.moveTargetWidth;
        this.movingHeight = this.moveTargetHeight;
        this.movingLocationX = this.moveTargetLocationX;
        this.movingLocationY = this.moveTargetLocationY;

        this.rect.sizeDelta = new Vector2(this.movingWidth, this.movingHeight);
        this.rect.localPosition = new Vector3(this.movingLocationX, this.movingLocationY, 0);

        //ウインドウレクトセット
        ParentWindowRect.sizeDelta = new Vector2(TexWidth + 30, TexHeight + 40);

        //ウインドウ位置設定
        //ParentWindowRect.transform.position = new Vector3(0, (Screen.width / 4 - TexWidth / 2), -100);
        //ParentWindow.transform.position = new Vector3((TexWidth/ 100), 0, 100);

        if (this.moveTargetWidth != TexWidth)
        {
            //最大化
            SetMoveTargetMax();
            flgMoving = true;
        }
    }

    /// <summary>
    /// 画像セット開始
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public void SetImage(string url)
    {
        if (url != null && url != "")
        {
            StartCoroutine(SetImageIE(url));
        }
        else
        {
            this.image.sprite = null;
        }       
    }

    /// <summary>
    /// 非同期ダウンロードメソッド
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator SetImageIE(string url)
    {
        //データダウンロード
        WWW www = new WWW(url);

        // 画像ダウンロード完了を待機
        yield return www;

        //画像ダウンロードエラーチェック
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }

        try
        {
            int width = www.texture.width;
            int height = www.texture.height;

            //テクスチャ生成
            CreateTex(www.texture);

            //縮小用テクスチャ作成
            CreateTexMin();

            //スプライト設定
            this.image.GetComponent<Image>().sprite = spriteMin;

            //サイズ是正
            FixPicture();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// テクスチャ生成
    /// </summary>
    /// <param name="tex"></param>
    private void CreateTex(Texture2D tex)
    {
        try
        {
            if(!UnityNullCheck.IsNull(this.tex))
            {
                Destroy(this.tex);
            }

            if(!UnityNullCheck.IsNull(this.sprite))
            {
                Destroy(this.sprite);
            }

            //テクスチャのロード
            this.tex = tex;
            this.TexWidth = tex.width;
            this.TexHeight = tex.height;

            //比率
            float rate = 0;

            //画像のアスペクト比取得
            float texAspectRate = ((float)tex.width / (float)tex.height);

            //アスペクト比境界から縦、横どちらに合わせるか決定

            //マックス値の計算
            //if (texAspectRate <= ASPECT_RATIO_BOUNDARY)
            //{
            //    //スクリーン高さ
            //    float screanHeight = Screen.height;

            //    //縦に合わせる
            //    rate = screanHeight / (float)tex.height;

            //    //求めた比率から横算出
            //    this.MaxWidth = (float)tex.width * rate;

            //    //高さ固定
            //    this.MaxHeight = screanHeight;
            //}
            //else
            //{
            //    //スクリーン幅の半分取得
            //    float screanWidthHerf = Screen.width / 2;

            //    //横に合わせる
            //    rate = screanWidthHerf / (float)tex.width;

            //    //横固定 スクリーン幅の半分に固定
            //    this.MaxWidth = screanWidthHerf;

            //    //求めた比率から高さ算出
            //    this.MaxHeight = (float)tex.height * rate;
            //}


            //マックス値の計算
            //-----------------------------------------
            //スクリーン高さ
            float maxScreanHeight = Screen.height;

            //縦に合わせる
            rate = maxScreanHeight / (float)tex.height;

            //求めた比率から横算出
            this.MaxWidth = (float)tex.width * rate;

            //高さ固定
            this.MaxHeight = maxScreanHeight;
            //-----------------------------------------

            
            if (texAspectRate < 1)
            {
                //縦長なら そのままの座標にする
                MaxLocationX = 0;
            }
            else
            {
                //横長なら 0に調整
                MaxLocationX = -Screen.width / 6;
            }

            //表示サイズの計算
            if (texAspectRate >= ASPECT_RATIO_BOUNDARY)
            {
                //スクリーン幅の33%
                float screanWidt = Screen.width / 3;

                //横に合わせる
                rate = screanWidt / (float)tex.width;

                //横固定 スクリーン幅の半分に固定
                this.TexWidth = screanWidt;

                //求めた比率から高さ算出
                this.TexHeight = (float)tex.height * rate;
            }
            else
            {
                //スクリーン高さの半分取得
                float screanHeight = Screen.height;

                //縦に合わせる
                rate = screanHeight / (float)tex.height;

                //求めた比率から横算出
                this.TexWidth = (float)tex.width * rate;

                //高さ固定
                this.TexHeight = screanHeight;
            }

            //サイズ調整
            TextureScale.Bilinear(this.tex, (int)MaxWidth, (int)MaxHeight);

            //スプライト生成
            sprite = Sprite.Create(this.tex, new Rect(0, 0, (int)MaxWidth, (int)MaxHeight), Vector2.zero);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// ミニテクスチャ生成
    /// </summary>
    private void CreateTexMin()
    {
        try
        {
            if (!UnityNullCheck.IsNull(this.texMin))
            {
                Destroy(this.texMin);
            }

            if (!UnityNullCheck.IsNull(this.spriteMin))
            {
                Destroy(this.spriteMin);
            }

            //縮小用テクスチャ作成
            this.texMin = GameObjectUtils.Clone(tex);

            //サイズ調整
            TextureScale.Bilinear(texMin, (int)tex.width, (int)tex.height);

            //スプライト生成
            this.spriteMin = Sprite.Create(texMin, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
