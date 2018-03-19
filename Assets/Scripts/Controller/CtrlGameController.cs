//=======================================================================
//  ClassName : GameController
//  概要      : ゲームコントローラー
//              タッチの監視
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Setting;
using Assets.Scripts.LiplisSystem.Web.Clalis.v40;
using SpicyPixel.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class CtrlGameController : ConcurrentBehaviour{

    //=====================================
    // UI
    [SerializeField] GameObject Canvas;
    [SerializeField] Text TxtTime;
    [SerializeField] Text TxtDate;
    [SerializeField] Text TxtLocation;
    [SerializeField] Text TxtMaxTemp;
    [SerializeField] Text TxtMinTemp;
    [SerializeField] Text TxtChanceOfRain;
    [SerializeField] Text TxtSize;
    [SerializeField] Text TxtFps;
    [SerializeField] Image ImgWether;

    [SerializeField] Image BtnSetting;

    public TxtGetStr txtGetStr { get; set; }




    //=====================================
    // 座標
    private float lastX = -1;
	private float lastY = -1;

    //=====================================
    //スケール変更アニメーション用プロパティ
    private float nowTarget = 0.2f;
    private bool scaleChangingSc = false;
    private bool scaleChangingEx = false;
    private float acceleration = 0.001f;
    private float targetScale = 0.2f;
    private float motoScale = 0.4f;

    //=====================================
    // FPS計算
    private int frameCount;
    private float prevTime;

    //====================================================================
    //
    //                           初期化処理
    //                         
    //====================================================================
    #region 初期化処理

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Awake()
    {
        //ベースアウェーク(初期化)
        base.Awake();

        //ベースFPS計算設定
        Application.targetFrameRate = 60;

        //カメラセット
        SetCamera();

        //PC版の場合、UIのサイズを設定する
        PcUiSizeSet();

        //環境設定
        InitEnvironmentSetting();

        Debug.Log(Environment.CurrentDirectory);
    }

    /// <summary>
    /// 環境設定
    /// </summary>
    private void InitEnvironmentSetting()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// カメラセット
    /// </summary>
    private void SetCamera()
    {

        //メインカメラ取得
        var camera = GameObject.Find(LAppDefine.GAME_OBJECT_MAIN_CAMERA);

        //カメラの取得チェック nullでなければ、セット
        if (camera != null)
        {
            //タッチ可能チェック
            if (camera.GetComponent<Camera>().orthographic)
            {
                //可能であれば、管理クラスにタッチフラグセット
                LAppLive2DManager.Instance.SetTouchMode2D(true);

            }
            else
            {
                //投影視点の場合、タッチフラグOFF
                Debug.Log("\"Main Camera\" Projection : Perspective");

                LAppLive2DManager.Instance.SetTouchMode2D(false);
            }
        }
    }

    /// <summary>
    /// UIサイズセット
    /// </summary>
    private void PcUiSizeSet()
    {
        // PC向けビルドだったらサイズ変更
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.LinuxPlayer)
        {
            //設定のロード
            LiplisMoonlightSetting setting = SettingLoader.LoadSetting();

            Vector2 size = setting.GetSize();

            Screen.SetResolution((int)size.x, (int)size.y, false);
            //Screen.SetResolution(1080, 1920, false);
        }
    }

    /// <summary>
    /// 開始時処理
    /// </summary>
    private void Start()
    {
        init();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void init()
    {
        //スクリーンサイズ取得
        int width = Screen.width;
        int height = Screen.height;

        //画面表示
        TxtSize.text = Screen.width + "×" + Screen.height;

        //FPS計算用変数 初期化
        frameCount = 0;
        prevTime = 0.0f;
    }
    #endregion

    //====================================================================
    //
    //                         更新処理  UI操作
    //                         
    //====================================================================
    #region 更新処理  UI操作

    /// <summary>
    /// アップデート
    /// 1フレームごとに1回呼ばれる
    /// </summary>
    void Update()
    {
        //UI処理
        UiOperation();

        //OS別処理
        UpdateOsProcessing();

        //スケール変更アニメーション
        AnimationLive2dScaleSc();

        //スケール変更アニメーション
        AnimationLive2DScaleEx();

        //時刻表示
        SetTime();

        //場所のセット
        SetLocation();

        //天気のセット
        SetWether();

        //FPS計算
        CulcFps();
    }

    private void UiOperation()
    {
        // タッチイベントの取得
        if (Input.GetMouseButtonDown(0))
        {
            //マウスボタンダウン

            //座標取得
            this.lastX = Input.mousePosition.x;
            this.lastY = Input.mousePosition.y;

            //Live2DManagerのタッチビギンを呼ぶ
            LAppLive2DManager.Instance.TouchesBegan(Input.mousePosition);


        }
        else if (Input.GetMouseButton(0))
        {
            //マウスボタン

            //前回値と同座標なら抜ける
            if (lastX == Input.mousePosition.x && lastY == Input.mousePosition.y)
            {
                return;
            }

            //座標取得
            this.lastX = Input.mousePosition.x;
            this.lastY = Input.mousePosition.y;

            //Live2DManagerのフリック処理を呼ぶ
            LAppLive2DManager.Instance.TouchesMoved(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //ボタンアップ

            //座標をリセット
            this.lastX = -1;
            this.lastY = -1;

            //Live2DManagerのタッチ終了処理を呼ぶ
            LAppLive2DManager.Instance.TouchesEnded(Input.mousePosition);
        }
    }

    /// <summary>
    /// FPSを計算する
    /// </summary>
    private void CulcFps()
    {
        //FPSカウントアップ
        ++frameCount;

        //前回からの経過時間計算
        float time = Time.realtimeSinceStartup - prevTime;

        //0.5秒経っているか
        if (time >= 0.5f)
        {
            //FPS算出、表示
            TxtFps.text = Math.Round(frameCount / time, 1, MidpointRounding.AwayFromZero) + "fps";

            //フレームカウントリセット
            frameCount = 0;
            
            //前回時刻更新
            prevTime = Time.realtimeSinceStartup;
        }
    }
    #endregion


    //====================================================================
    //
    //                          メニュー操作
    //                         
    //====================================================================
    #region メニュー操作

    /// <summary>
    /// ホームボタンクリック
    /// </summary>
    public void Btn_Home_Click()
    {
        Debug.Log("GameCtlr BtnHome_Click");




    }

    /// <summary>
    /// おまかせボタンクリック
    /// </summary>
    public void Btn_Omakase_Click()
    {
        Debug.Log("GameCtlr BtnOmakase_Click");
    }

    /// <summary>
    /// ニュースボタンクリック
    /// </summary>
    public void Btn_News_Click()
    {
        Debug.Log("GameCtlr Btn_News_Click");
    }

    /// <summary>
    /// まとめボタンクリック
    /// </summary>
    public void Btn_Matome_Click()
    {
        //ホームボタンクリック
        Debug.Log("GameCtlr Btn_Matome_Click");
    }

    /// <summary>
    /// 話題の画像タンクリック
    /// </summary>
    public void Btn_Photo_Click()
    {
        Debug.Log("GameCtlr Btn_Photo_Click");
    }
    
    /// <summary>
    /// リツイートボタンクリック
    /// </summary>
    public void Btn_Retweet_Click()
    {
        Debug.Log("GameCtlr Btn_Retweet_Click");
    }

    /// <summary>
    /// ホットワードボタンクリック
    /// </summary>
    public void Btn_HotWord_Click()
    {
        Debug.Log("GameCtlr Btn_HotWord_Click");
    }

    /// <summary>
    /// ハッシュワードボタンクリック
    /// </summary>
    public void Btn_HashWord_Click()
    {
        Debug.Log("GameCtlr Btn_HashWord_Click");
    }



    /// <summary>
    /// アニメボタンクリック
    /// </summary>
    public void Btn_Anime_Click()
    {
        Debug.Log("GameCtlr Btn_Anime_Click");
    }

    /// <summary>
    /// テレビボタンクリック
    /// </summary>
    public void Btn_Tv_Click()
    {
        Debug.Log("GameCtlr Btn_Tv_Click");
    }

    /// <summary>
    /// ゲームボタンクリック
    /// </summary>
    public void Btn_Game_Click()
    {
        Debug.Log("GameCtlr Btn_Game_Click");
    }


    /// <summary>
    /// 天気ボタンクリック
    /// </summary>
    public void Btn_Wether_Click()
    {
        Debug.Log("GameCtlr Btn_Wether_Click");
    }
    /// <summary>
    /// 季節ボタンクリック
    /// </summary>
    public void Btn_Season_Click()
    {
        Debug.Log("GameCtlr Btn_Season_Click");
    }

    /// <summary>
    /// 現在地ボタンクリック
    /// </summary>
    public void Btn_Region_Click()
    {
        Debug.Log("GameCtlr Btn_Region_Click");
    }

    /// <summary>
    /// グルメボタンクリック
    /// </summary>
    public void Btn_Food_Click()
    {
        Debug.Log("GameCtlr Btn_Food_Click");
    }

    /// <summary>
    /// 動画クリック
    /// </summary>
    public void Btn_Movie_Click()
    {
        Debug.Log("GameCtlr Btn_Movie_Click");
    }


    /// <summary>
    /// 動画クリック
    /// </summary>
    public void Btn_Log_Click()
    {
        Debug.Log("GameCtlr Btn_Log_Click");
    }

    #endregion

    //====================================================================
    // 
    //                        更新処理  OS別処理
    //                         
    //====================================================================
    #region 更新処理  OS別処理
    /// <summary>
    /// OS別処理
    /// </summary>
    private void UpdateOsProcessing()
    {
        //Android処理
        UpdateOsProcessingAndroid();
    }

    /// <summary>
    /// Android
    /// </summary>
    private void UpdateOsProcessingAndroid()
    {
        //アンドロイド処理
        if (Application.platform == RuntimePlatform.Android)
        {
            // Androidのバックボタンで終了
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
        }
    }
    #endregion

    //====================================================================
    //
    //                    更新処理  UIアニメーション
    //                         
    //====================================================================
    #region 更新処理  UIアニメーション
    /// <summary>
    /// スケール拡大変更アニメーション
    /// </summary>
    private void AnimationLive2DScaleEx()
    {

        //スケール変更アニメーション
        if (scaleChangingEx)
        {
            //加速
            acceleration = acceleration + 0.0001f;

            Vector3 v = Canvas.transform.localScale;

            if (v.x >= motoScale)
            {
                //完了
                scaleChangingEx = false;
                acceleration = 0.001f;
                v.x = motoScale;
                v.y = motoScale;
                v.z = motoScale;
            }
            else
            {
                //縮小
                v.x = v.x + acceleration;
                v.y = v.y + acceleration;
                v.z = v.z + acceleration;
            }

            Canvas.transform.localScale = v;
        }

    }

    /// <summary>
    /// スケール縮小変更アニメーション
    /// </summary>
    private void AnimationLive2dScaleSc()
    {
        if (scaleChangingSc)
        {
            //加速
            acceleration = acceleration + 0.0001f;

            Vector3 v = Canvas.transform.localScale;

            if (v.x <= targetScale)
            {
                //完了
                scaleChangingSc = false;
                acceleration = 0.001f;
                v.x = targetScale;
                v.y = targetScale;
                v.z = targetScale;
            }
            else
            {
                //縮小
                v.x = v.x - acceleration;
                v.y = v.y - acceleration;
                v.z = v.z - acceleration;
            }

            Canvas.transform.localScale = v;
        }
    }
    #endregion

    //====================================================================
    //
    //                              更新処理
    //                         
    //====================================================================
    #region 更新処理
    /// <summary>
    /// 時間の設定
    /// </summary>
    private void SetTime()
    {
        TxtTime.text = DateTime.Now.ToString("HH:mm:ss");
        TxtDate.text = DateTime.Now.ToString("yyyy/MM/dd ddd");
    }

    /// <summary>
    /// ロケーションの設定
    /// </summary>
    private void SetLocation()
    {
        string name = LiplisStatus.Instance.InfoLocation.MostSpecificSubdivisionNameJp;

        if (name == null)
        {
            return;
        }

        if (name.Length <= 3)
        {
            TxtLocation.text = name;
        }
        else
        {
            TxtLocation.text = name.Substring(0, 3);
        }
    }


    /// <summary>
    /// 天気の設定
    /// </summary>
    private void SetWether()
    {
        if (LiplisStatus.Instance.InfoWether.WetherToday != null)
        {
            ImgWether.sprite = SpriteLinkWether.Instance.GetWetherIcon(LiplisStatus.Instance.InfoWether.WetherToday.weather);
            TxtMaxTemp.text = LiplisStatus.Instance.InfoWether.WetherToday.tempMax.ToString();
            TxtMinTemp.text = LiplisStatus.Instance.InfoWether.WetherToday.tempMin.ToString();
            TxtChanceOfRain.text = LiplisStatus.Instance.InfoWether.WetherToday.chanceOfRain.ToString();
        }

    }
    #endregion

    //====================================================================
    //
    //                           テストメソッド
    //                         
    //====================================================================
    #region テストメソッド

    /// <summary>
    /// 表情変更
    /// </summary>
    public void ChangeExpression()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f06");
    }

    public void ChangeExpressionF01()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f01");
    }
    public void ChangeExpressionF02()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f02");
    }
    public void ChangeExpressionF03()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f03");
    }
    public void ChangeExpressionF04()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f04");
    }
    public void ChangeExpressionF05()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f05");
    }
    public void ChangeExpressionF06()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f06");
    }
    public void ChangeExpressionF07()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f07");
    }
    public void ChangeExpressionF08()
    {
        LAppLive2DManager.Instance.SetExpression("Live2D_Canvas_01", "f08");
    }


    /// <summary>
    /// 表情変更
    /// </summary>
    public void StartMotion01()
    {
        LAppLive2DManager.Instance.StartMotion("Live2D_Canvas_01", "tap_body", 0, 2);
    }

    public void StartMotion02()
    {
        LAppLive2DManager.Instance.StartMotion("Live2D_Canvas_01", "tap_body", 1, 2);
    }

    public void StartMotion03()
    {
        LAppLive2DManager.Instance.StartMotion("Live2D_Canvas_01", "tap_body", 2, 2);
    }

    public void StartMotion04()
    {
        LAppLive2DManager.Instance.StartMotion("Live2D_Canvas_01", "flick_head", 0, 2);
    }

    public void StartMotion05()
    {
        LAppLive2DManager.Instance.StartMotion("Live2D_Canvas_01", "pinch_in", 0, 2);
    }

    public void StartMotion06()
    {
        LAppLive2DManager.Instance.StartMotion("Live2D_Canvas_01", "pinch_out", 0, 2);
    }

    public void StartMotion07()
    {
        LAppLive2DManager.Instance.StartMotion("Live2D_Canvas_01", "shake", 0, 2);
    }

    public void StartMotion08()
    {
        LAppLive2DManager.Instance.StartMotion("Live2D_Canvas_01", "idle", 0, 2);
    }

    /// <summary>
    /// ウェブデータを取得する
    /// </summary>
    public void GetWebData()
    {
        MsgTalkMessage msg = ClalisShortNews.getShortNews("", "", "");

        txtGetStr.GetComponent<TxtGetStr>().message = msg.sorce;
    }

    /// <summary>
    /// スケールチェンジ
    /// </summary>
    public void ScaleChange()
    {
        Vector3 v = Canvas.transform.localScale;

        v.x = v.x * 0.9f;
        v.y = v.y * 0.9f;
        v.z = v.z * 0.9f;

        Canvas.transform.localScale = v;
    }

    public void ChangeLocation()
    {
        Vector3 pos = Canvas.transform.position;

        pos.x = pos.x * 0.9f;
        pos.y = pos.y * 0.9f;

        Canvas.transform.position = pos;
    }

    /// <summary>
    /// モデル変更
    /// </summary>
    public void ModelChange()
    {
        LAppLive2DManager.Instance.ChangeModel("Live2D_Canvas_01", "live2d/tsumikiComx/model.model.json");
        //LAppLive2DManager.Instance.ChangeModel("live2d/tsumikiComx/model.model.json");
    }

    /// <summary>
    /// 拡大
    /// </summary>
    public void Expantion()
    {
        scaleChangingEx = true;
        scaleChangingSc = false;
    }

    /// <summary>
    /// 拡少
    /// </summary>
    public void Shirinking()
    {
        scaleChangingSc = true;
        scaleChangingEx = false;
    }
    #endregion


}