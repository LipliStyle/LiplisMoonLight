//=======================================================================
//  ClassName : ImageWindow
//  概要      : イメージウインドウ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Web;
using SpicyPixel.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageWindow : ConcurrentBehaviour
{
    ///=============================
    ///画面制御
    private int SelectedThumbnailIndex;             //選択インデックス
    private List<MsgThumbnailData> thumbnailList;   //サムネリスト

    ///=============================
    ///画面制御
    public DateTime CreateTime { get; set; }

    ///=============================
    ///子コンポーネント名
    private const string CHILD_COMPONENT_IMAGE_TOPIC = "TopicImage";
    private const string CHILD_COMPONENT_TEXT_TITLE = "TextTitle";

    ///=============================
    ///計算用定数
    private const float _FRAME = 30f;

    ///=============================
    ///子要素インスタンス
    RectTransform rect;
    RawImage image;
    RawImage baseImage;
    Text textTitle;

    ///=============================
    ///親ウインドウ
    public GameObject ParentWindow { get; set; }
    public RectTransform ParentWindowRect { get; set; }

    ///=============================
    ///移動先位置
    public Vector3 TargetPosition { get; set; }

    ///=============================
    ///親ウインドウ座標
    public Vector3 ParentLocalPosition { get; set; }

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
        this.rect = GameObject.Find(CHILD_COMPONENT_IMAGE_TOPIC).GetComponent<RectTransform>();

        //イメージインスタンス取得
        this.image = GameObject.Find(CHILD_COMPONENT_IMAGE_TOPIC).GetComponent<RawImage>();

        //タイトルテキスト取得
        this.textTitle = GameObject.Find(CHILD_COMPONENT_TEXT_TITLE).GetComponent<Text>();

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
    /// ウインドウ生成時刻をセットする
    /// </summary>
    /// <param name="CreateTime"></param>
    public void SetCreateTime(DateTime CreateTime)
    {
        this.CreateTime = CreateTime;
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    public void FaidIn()
    {
        this.flgOn = true;
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
        if (this.moveTargetWidth != GetTextureWidth())
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

        //サイズ設定
        this.rect.sizeDelta = new Vector2(this.movingWidth, this.movingHeight);

        //画像位置設定
        this.rect.localPosition = new Vector3(this.movingLocationX, this.movingLocationY, 0);

        //スプライト設定
        this.image.texture = GetTextureSprite();
    }



    /// <summary>
    /// クリックイベント
    /// </summary>
    public void OnPointerClick()
    {
        if (this.moveTargetWidth != GetTextureWidth())
        {
            flgMoving = true;
            SetMoveTargetMin();
        }
    }


    /// <summary>
    /// 移動先を設定する
    /// </summary>
    void SetMoveTarget()
    {
        flgMoving = true;

        if (this.moveTargetWidth != GetTextureWidth())
        {
            SetMoveTargetMin();
        }
        else
        {
            SetMoveTargetMax();
        }
    }

    /// <summary>
    /// ウインドウを移動する
    /// </summary>
    /// <param name="TargetPosition"></param>
    public void SetMoveTarget(Vector3 TargetPosition)
    {
        this.ParentWindow.transform.localPosition = TargetPosition;
        SetFirstPicture();
    }

    /// <summary>
    /// 表示位置の初期化
    /// </summary>
    public void InitLocation()
    {
        float windowLocationX = ParentWindowRect.sizeDelta.x / 2;
        float windowLocationY = -ParentWindowRect.sizeDelta.y / 2 + 450;

        if (540 - ParentWindowRect.sizeDelta.y / 2 < windowLocationY)
        {
            windowLocationY = (540 - ParentWindowRect.sizeDelta.y / 2);
        }

        ParentWindowRect.anchoredPosition = new Vector2(windowLocationX, windowLocationY);
    }

    /// <summary>
    /// 最小表示
    /// </summary>
    public void SetMoveTargetMin()
    {
        this.moveTargetWidth = GetTextureWidth();
        this.moveTargetHeight = GetTextureHeight() - 10;
        this.moveTargetLocationX = 0;
        this.moveTargetLocationY = -18;

        this.ParentWindow.transform.localPosition = this.ParentLocalPosition;
    }

    /// <summary>
    /// 最大表示
    /// </summary>
    public void SetMoveTargetMax()
    {
        this.moveTargetWidth = GetTextureMaxWidth();
        this.moveTargetHeight = GetTextureMaxHeight();
        this.moveTargetLocationX = 0;
        this.moveTargetLocationY = 0;

        this.ParentLocalPosition = this.ParentWindow.transform.localPosition;
        this.ParentWindow.transform.localPosition = new Vector3(this.movingLocationX, this.movingLocationY, 0);
    }

    /// <summary>
    /// 最大表示になっている場合、サイズ是正する
    /// </summary>
    public void FixPicture()
    {
        //一旦元に戻す
        this.moveTargetWidth = GetTextureWidth();
        this.moveTargetHeight = GetTextureHeight()- 10;
        this.moveTargetLocationX = 0;
        this.moveTargetLocationY = -18;

        this.movingWidth = this.moveTargetWidth;
        this.movingHeight = this.moveTargetHeight;
        this.movingLocationX = this.moveTargetLocationX;
        this.movingLocationY = this.moveTargetLocationY;

        this.rect.sizeDelta = new Vector2(this.movingWidth, this.movingHeight);
        this.rect.localPosition = new Vector3(this.movingLocationX, this.movingLocationY, 0);

        //ウインドウレクトセット
        ParentWindowRect.sizeDelta = new Vector2(GetTextureWidth() + 8, GetTextureHeight() + 36);

        //ウインドウ座標設定
        SetIMageWindowLocation();

        if (this.moveTargetWidth != GetTextureWidth())
        {
            //最大化
            SetMoveTargetMax();
            flgMoving = true;
        }
    }

    public void SetIMageWindowLocation()
    {
        //あんカードロケーション取得
        float windowLocationX = ParentWindowRect.anchoredPosition.x;
        float windowLocationY = ParentWindowRect.anchoredPosition.y;

        //初期位置なら調整
        if (windowLocationX == -999 && windowLocationY == -999)
        {
            windowLocationX = ParentWindowRect.sizeDelta.x/2;
            windowLocationY = -ParentWindowRect.sizeDelta.y / 2 + 450;
        }

        if(540 - ParentWindowRect.sizeDelta.y / 2< windowLocationY)
        {
            windowLocationY = (540 - ParentWindowRect.sizeDelta.y / 2);
        }

        //基本的に元の位置設定
        ParentWindowRect.anchoredPosition = new Vector2(windowLocationX, windowLocationY);
    }

    /// <summary>
    /// 画像セット開始
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public void SetImage(List<string> urlList)
    {
        //現在設定されているテクスチャの破棄
        DestroyTexThumbnailList();

        if (urlList != null && urlList.Count > 0 )
        {
            //サムネイルリスト初期化
            foreach (string url in urlList)
            {
                thumbnailList.Add(new MsgThumbnailData(url));
            }

            //インデックス
            int idx = 0;

            //非同期ダウンロード
            foreach (MsgThumbnailData ThumbnailData in thumbnailList)
            {
                //ダウンロード処理
                StartCoroutine(SetImageIE(ThumbnailData, idx == 0));

                //カウントアップ
                idx++;
            }
        }
        else
        {
            //仮コメント
            //Destroy(this.image.texture);
        }       
    }

    /// <summary>
    /// 非同期ダウンロードメソッド
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator SetImageIE(MsgThumbnailData ThumbnailData, bool FlgFirst )
    {
        //データダウンロード
        using (WWW www = new WWW(CreateUrl(ThumbnailData)))
        {
            // 画像ダウンロード完了を待機
            yield return www;

            //タイトル設定
            this.textTitle.text = ThumbnailData.thumbnailUrl;

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
                ThumbnailData.CreateTex(www.texture);

                //最初のデータならウインドウ表示
                if(FlgFirst)
                {
                    SetFirstPicture();
                }

                //WEBのテクスチャ解放
                Destroy(www.texture);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }

    /// <summary>
    /// URLを生成する
    /// </summary>
    /// <param name="ThumbnailData"></param>
    /// <returns></returns>
    private string CreateUrl(MsgThumbnailData ThumbnailData)
    {
        if (LiplisSetting.Instance.Setting.GraphicLevel == GRAPHIC_LEVEL.GraphicLevel_Low)
        {
            //ローレベル リストURL
            return ThumbnailUrl.CreateListThumbnailUrl(ThumbnailData.thumbnailUrl);
        }
        else if (LiplisSetting.Instance.Setting.GraphicLevel == GRAPHIC_LEVEL.GraphicLevel_Middle)
        {
            //普通　スモールURL
            return ThumbnailUrl.CreateThumbnailSmallUrl(ThumbnailData.thumbnailUrl);
        }
        else if (LiplisSetting.Instance.Setting.GraphicLevel == GRAPHIC_LEVEL.GraphicLevel_Heigh)
        {
            //高い 生データURL
            return ThumbnailUrl.CreateThumbnailUrl(ThumbnailData.thumbnailUrl);
        }
        else
        {
            //それ以外はスモールURLリストURL
            return ThumbnailUrl.CreateThumbnailSmallUrl(ThumbnailData.thumbnailUrl);
        }
    }
   
    /// <summary>
    /// ファーストピクチャーをセットする
    /// </summary>
    private void SetFirstPicture()
    {
        //未追加なら0番目セットし、表示
        if (thumbnailList.Count > 0)
        {
            //選択インデックスを0に初期化
            this.SelectedThumbnailIndex = 0;

            //スプライト設定
            this.image.texture = thumbnailList[0].tex;

            //サイズ是正
            FixPicture();
        }
    }


    /// <summary>
    /// ウインドウを閉じる
    /// </summary>
    public void CloseWindow()
    {
        this.flgEnd = true;
    }

    //====================================================================
    //
    //                       テクスチャリスト操作
    //                         
    //====================================================================
    #region テクスチャリスト操作

    /// <summary>
    /// 現在選択中のサムネイルデータを取得する
    /// </summary>
    /// <returns></returns>
    private MsgThumbnailData GetNowSelectedThumbnail()
    {
        if(thumbnailList == null)
        {
            return null;
        }

        if(thumbnailList.Count == 0)
        {
            return null;
        }

        if(SelectedThumbnailIndex < thumbnailList.Count)
        {

            return thumbnailList[SelectedThumbnailIndex];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// テクスチャサイズを返す
    /// </summary>
    /// <returns></returns>
    private float GetTextureWidth()
    {
        MsgThumbnailData thumbnail = GetNowSelectedThumbnail();

        if(thumbnail != null)
        {
            return thumbnail.TexWidth;
        }
        else
        {
            return 0;
        }
    }
    private float GetTextureHeight()
    {
        MsgThumbnailData thumbnail = GetNowSelectedThumbnail();

        if (thumbnail != null)
        {
            return thumbnail.TexHeight;
        }
        else
        {
            return 0;
        }
    }
    private float GetTextureMaxWidth()
    {
        MsgThumbnailData thumbnail = GetNowSelectedThumbnail();

        if (thumbnail != null)
        {
            return thumbnail.MaxWidth;
        }
        else
        {
            return 0;
        }
    }
    private float GetTextureMaxHeight()
    {
        MsgThumbnailData thumbnail = GetNowSelectedThumbnail();

        if (thumbnail != null)
        {
            return thumbnail.MaxHeight;
        }
        else
        {
            return 0;
        }
    }
    private Texture GetTextureSprite()
    {
        MsgThumbnailData thumbnail = GetNowSelectedThumbnail();

        if (thumbnail != null)
        {
            return thumbnail.tex;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// テクスチャをクリアする
    /// </summary>
    private void DestroyTexThumbnailList()
    {
        if(thumbnailList != null)
        {
            if(thumbnailList.Count > 0)
            {
                foreach (var thumbnail in thumbnailList)
                {
                    Destroy(thumbnail.tex);

                    thumbnail.tex =null;
                }
            }

            //リストクリア
            thumbnailList.Clear();
        }
        else
        {
            thumbnailList = new List<MsgThumbnailData>();
        }
       
    }
#endregion

    //====================================================================
    //
    //                         更新処理  UI操作
    //                         
    //====================================================================
    #region 更新処理  UI操作

    /// <summary>
    /// 右へクリック
    /// </summary>
    public void Btn_Right_Click()
    {
        if (SelectedThumbnailIndex >= thumbnailList.Count - 1)
        {
            SelectedThumbnailIndex = thumbnailList.Count - 1;
            return;
        }

        SelectedThumbnailIndex++;

        //スプライト設定
        this.image.texture= GetTextureSprite();

        //サイズ是正
        FixPicture();

    }

    /// <summary>
    /// 左へクリック
    /// </summary>
    public void Btn_Left_Click()
    {
        if (SelectedThumbnailIndex < 1)
        {
            SelectedThumbnailIndex = 0;
            return;
        }

        SelectedThumbnailIndex--;


        //スプライト設定
        this.image.texture = GetTextureSprite();

        //サイズ是正
        FixPicture();
    }

    /// <summary>
    /// Maxクリック
    /// </summary>
    public void Btn_Max_Click()
    {
        SetMoveTarget();
    }
    #endregion
}
