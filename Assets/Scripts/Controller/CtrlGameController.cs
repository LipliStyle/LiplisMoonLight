//=======================================================================
//  ClassName : GameController
//  概要      : ゲームコントローラー
//              タッチの監視
//
//              2018/07/09 定期リソース解放処理追加
//                         テクスチャなどのリソース解放は「Resources.UnloadUnusedAssets();」によって行われる。
//                         解放条件 : 5分経過 or トータル使用メモリ 600MB以上
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.Data.PcSetting;
using Assets.Scripts.LiplisUi;
using SpicyPixel.Threading;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class CtrlGameController : ConcurrentBehaviour{

    //=====================================
    // UI
    [SerializeField] Text TxtTime;
    [SerializeField] Text TxtDate;
    [SerializeField] Text TxtLocation;
    [SerializeField] Text TxtMaxTemp;
    [SerializeField] Text TxtMinTemp;
    [SerializeField] Text TxtChanceOfRain;
    [SerializeField] Image ImgWether;


    [SerializeField] Image BtnSetting;

    ///=============================
    ///デバッグ表示
    [SerializeField] Text TxtQNum;
    [SerializeField] Text TxtTopicNum;
    [SerializeField] Text TxtChatedNum;
    [SerializeField] Text TxtNewsListNum;
    [SerializeField] Text TxtSize;
    [SerializeField] Text TxtFps;
    [SerializeField] Text TxtMemory1;
    [SerializeField] Text TxtMemory2;

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
    private int fpsFrameCount;
    private float prevTime;

    //=====================================
    // 描画制御用FPS計算
    private int frameCount;


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

        //クラス初期化
        InitClass();

        //クラス初期化
        InitWindow();

        //ログ出力
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
    /// クラスの初期化
    /// </summary>
    private void InitClass()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
    }

    /// <summary>
    /// ウインドウの初期化
    /// </summary>
    private void InitWindow()
    {
        //ビジブルセット
        SetHidden();
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
        fpsFrameCount = 0;
        prevTime = 0.0f;
    }

    /// <summary>
    /// ボタンを設定する
    /// </summary>
    private void SetHidden()
    {
        //エディタ以外の場合は非表示とする
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            //件数表示の非表示
            VisibleUtil.SetVisible(TxtQNum, false);
            VisibleUtil.SetVisible(TxtTopicNum, false);
            VisibleUtil.SetVisible(TxtChatedNum, false);
            VisibleUtil.SetVisible(TxtNewsListNum, false);
            VisibleUtil.SetVisible(TxtSize, false);
            VisibleUtil.SetVisible(TxtFps, false);
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
    /// アップデート
    /// 1フレームごとに1回呼ばれる
    /// </summary>
    void Update()
    {
        //リアルタイム更新
        UpdateRealTime();

        //UI更新
        if (frameCount %  30 == 0) { UpdateUI_30F(); }
        if (frameCount %  60 == 0) { UpdateUI_60F(); }
        if (frameCount % 300 == 0) { UpdateUI_300F(); }
        if (frameCount % 600 == 0) { UpdateUI_600F(); }

        //FPS更新
        UpdateFps();
    }

    /// <summary>
    /// リアルタイム更新
    /// </summary>
    void UpdateRealTime()
    {
        //UI処理
        UiOperation();

        //OS別処理
        UpdateOsProcessing();

        //FPS計算
        CulcFps();
    }

    /// <summary>
    /// 30フレーム(1秒以内)ごとに実行
    /// </summary>
    void UpdateUI_30F()
    {
        //時刻表示
        SetTime();

        CulcUsedMemory();
    }

    /// <summary>
    /// 60フレーム(1秒以内)ごとに実行
    /// </summary>
    void UpdateUI_60F()
    {
        if (LiplisSetting.Instance.Setting.FlgDebug)
        {
            //件数表示の非表示
            VisibleUtil.SetVisible(TxtQNum, true);
            VisibleUtil.SetVisible(TxtTopicNum, true);
            VisibleUtil.SetVisible(TxtChatedNum, true);
            VisibleUtil.SetVisible(TxtNewsListNum, true);
            VisibleUtil.SetVisible(TxtSize, true);
            VisibleUtil.SetVisible(TxtFps, true);
            VisibleUtil.SetVisible(TxtMemory1, true);
            VisibleUtil.SetVisible(TxtMemory2, true);
        }
        else
        {
            //件数表示の非表示
            VisibleUtil.SetVisible(TxtQNum, false);
            VisibleUtil.SetVisible(TxtTopicNum, false);
            VisibleUtil.SetVisible(TxtChatedNum, false);
            VisibleUtil.SetVisible(TxtNewsListNum, false);
            VisibleUtil.SetVisible(TxtSize, false);
            VisibleUtil.SetVisible(TxtFps, false);
            VisibleUtil.SetVisible(TxtMemory1, false);
            VisibleUtil.SetVisible(TxtMemory2, false);
        }
    }

    /// <summary>
    /// 300フレーム(約30s)置きに実施
    /// </summary>
    void UpdateUI_300F()
        {
        //場所のセット
        SetLocation();

        //天気のセット
        SetWether();
    }


    /// <summary>
    /// 600フレーム(約30s)置きに実施
    /// </summary>
    private void UpdateUI_600F()
    {
        //定期リソース解放
        UnloadUnusedAssets();
    }

    /// <summary>
    /// FPSの更新
    /// </summary>
    private void UpdateFps()
    {
        //インクリメント
        frameCount++;

        //60000回でリセット
        if(frameCount > 60000)
        {
            frameCount = 0;
        }
    }


    /// <summary>
    /// UIオペレーション
    /// </summary>
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
        ++fpsFrameCount;

        //前回からの経過時間計算
        float time = Time.realtimeSinceStartup - prevTime;

        //0.5秒経っているか
        if (time >= 0.5f)
        {
            //FPS算出、表示
            TxtFps.text = Math.Round(fpsFrameCount / time, 1, MidpointRounding.AwayFromZero) + "fps";

            //フレームカウントリセット
            fpsFrameCount = 0;
            
            //前回時刻更新
            prevTime = Time.realtimeSinceStartup;
        }
    }

    /// <summary>
    /// メモリ使用量の計算
    /// </summary>
    private void CulcUsedMemory()
    {
        if (LiplisSetting.Instance.Setting.FlgDebug)
        {
            long monoUsed = Profiler.GetMonoUsedSizeLong();
            long monoSize = Profiler.GetMonoHeapSizeLong();
            long totalUsed = Profiler.GetTotalAllocatedMemoryLong(); // == Profiler.usedHeapSize
            long totalSize = Profiler.GetTotalReservedMemoryLong();
            TxtMemory1.text = string.Format("mono:{0}/{1} kb({2:f1}%)", monoUsed / 1024, monoSize / 1024, 100.0 * monoUsed / monoSize);
            TxtMemory2.text = string.Format("total:{0}/{1} kb({2:f1}%)", totalUsed / 1024, totalSize / 1024, 100.0 * totalUsed / totalSize);
        }
    }

    /// <summary>
    /// リソースを開放する
    /// </summary>
    private void UnloadUnusedAssets()
    {
        long totalUsed = Profiler.GetTotalAllocatedMemoryLong();

        //指定時間経過していなければ抜ける
        if (LiplisStatus.Instance.LastRunReleaseProcessing.AddMinutes(5) > DateTime.Now &&
            totalUsed < 600000000)
        {
            return;
        }

        LiplisStatus.Instance.LastRunReleaseProcessing = DateTime.Now;

        //リソース開放処理
        Resources.UnloadUnusedAssets();
    }

    #endregion


    //====================================================================
    //
    //                          メニュー操作
    //                         
    //====================================================================
    #region メニュー操作





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
            TxtChanceOfRain.text = LiplisStatus.Instance.InfoWether.WetherToday.chanceOfRain.ToString() + "%";
        }

    }
    #endregion

}