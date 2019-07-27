//=======================================================================
//  ClassName : GameController
//  概要      : ゲームコントローラー
//              基本情報の更新、タッチの監視、定期リソース開放
//
//              2018/07/09 定期リソース解放処理追加
//                         テクスチャなどのリソース解放は「Resources.UnloadUnusedAssets();」によって行われる。
//                         解放条件 : 5分経過 or トータル使用メモリ 600MB以上
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.Data.PcSetting;
using Assets.Scripts.Data.SubData;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.MainSystem;
using Assets.Scripts.LiplisUi.TopicController;
using Assets.Scripts.Util.Ugui;
using SpicyPixel.Threading;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class CtrlGameController : ConcurrentBehaviour
{
    //=====================================
    // カメラオブジェクト
    [SerializeField] GameObject MainCamera;

    //=====================================
    // シングルトンインスタンス
    public static CtrlGameController instance;

    //=====================================
    // UI
    [SerializeField] Text TxtTime;
    [SerializeField] Text TxtDate;
    [SerializeField] Text TxtLocation;
    [SerializeField] Text TxtMaxTemp;
    [SerializeField] Text TxtMinTemp;
    [SerializeField] Text TxtChanceOfRain;
    [SerializeField] Image ImgWether;
    [SerializeField] Text TxtTopicNumR;
    [SerializeField] Slider SliderTopicNum;
    [SerializeField] Text TxtTopicMode;

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

    ///=============================
    ///背景ゲームオブジェクト
    [SerializeField] GameObject ScrollNewsListViewport;

    //=============================
    //レンダリング階層
    public GameObject CanvasRendering;

    //=============================
    //会話コントローラー
    private CtrlTalk ctrlTalk;

    //=============================
    //タブコントローラー
    TabBaseTopicList tbt;

    //=====================================
    // 座標
    private float lastX = -1;
    private float lastY = -1;

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

        //自インスタンス取得
        instance = this;

        //PC版の場合、UIのサイズを設定する
        PcUiSizeSet();

        //環境設定
        InitEnvironmentSetting();

        //クラス初期化
        InitClass();

        //クラス初期化
        InitWindow();

        //モデルの初期化
        InitModels();

        //ホームに設定
        ChangeMode(ContentCategoly.home);

        //ログ出力
        Debug.Log(Environment.CurrentDirectory);
    }


    /// <summary>
    /// 環境設定
    /// </summary>
    private void InitEnvironmentSetting()
    {
        QualitySettings.vSyncCount = 0;

        //ベースFPS計算設定
        Application.targetFrameRate = LiplisSetting.Instance.Setting.GetFps();
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
        //プロトコルタイプ設定
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

        //タブコントローラ初期化
        tbt = new TabBaseTopicList(ScrollNewsListViewport);

        //トークインスタンス取得
        ctrlTalk = MainCamera.GetComponent<CtrlTalk>();
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

    /// <summary>
    /// モデルの初期化
    /// </summary>
    /// <param name="CanvasRendering"></param>
    /// <param name="CallbackOnNextTalkOrSkip"></param>
    private void InitModels()
    {
        LiplisModels.Instance.ActivateModels(CanvasRendering, ctrlTalk.NextTalkOrSkip);
    }



    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
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
        if (frameCount % 30 == 0) { UpdateUI_30F(); }
        if (frameCount % 60 == 0) { UpdateUI_60F(); }
        if (frameCount % 300 == 0) { UpdateUI_300F(); }
        if (frameCount % 600 == 0) { UpdateUI_600F(); }
        if (frameCount % 3600 == 0) { UpdateUI_3600F(); }

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

        //Live2dの更新
        UpdateLive2dModels();

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

        //メモリー使用量計算
        CulcUsedMemory();
    }

    /// <summary>
    /// 60フレーム(1秒以内)ごとに実行
    /// </summary>
    void UpdateUI_60F()
    {
        //デバッグ表示
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
    /// 300フレーム(約5s)置きに実施
    /// </summary>
    void UpdateUI_300F()
    {

    }


    /// <summary>
    /// 600フレーム(約10s)置きに実施
    /// </summary>
    private void UpdateUI_600F()
    {
        //場所のセット
        SetLocation();

        //天気のセット
        SetWether();

        //データ更新
        SetText();
    }

    /// <summary>
    /// 3600フレーム(約1分)置きに実施
    /// </summary>
    private void UpdateUI_3600F()
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
        if (frameCount > 60000)
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
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //ボタンアップ

            //座標をリセット
            this.lastX = -1;
            this.lastY = -1;
        }
    }

    /// <summary>
    /// Live2dモデルの更新
    /// </summary>
    private void UpdateLive2dModels()
    {
        //各モデルの更新処理を実行する
        LiplisModels.Instance.Update();
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

        if (LiplisStatus.Instance.LastRunReleaseProcessing.AddMinutes(1) > DateTime.Now)
        {
            return;
        }

        LiplisStatus.Instance.LastRunReleaseProcessing = DateTime.Now;

        //リソース開放処理
        Resources.UnloadUnusedAssets();
    }

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

    /// <summary>
    /// デバッグテキストを設定する
    /// </summary>
    private void SetText()
    {
        try
        {
            //トークインスタンス取得
            DatNewTopic NewTopic = LiplisStatus.Instance.NewTopic;

            if (LiplisSetting.Instance.Setting.FlgDebug)
            {
                    //テキスト表示
                    TxtQNum.text = NewTopic.TalkTopicList.Count.ToString();
                    TxtTopicNum.text = NewTopic.LastData.topicList.Count.ToString();
                    TxtChatedNum.text = NewTopic.ChattedKeyList.Count.ToString();
                    TxtNewsListNum.text = LiplisStatus.Instance.NewsList.LastNewsList.NewsList.Count.ToString();
            }

            //トピック件数を表示する
            TxtTopicNumR.text = NewTopic.TalkTopicList.Count.ToString();

            //カウント
            SliderTopicNum.value = NewTopic.TalkTopicList.Count;
        }
        catch
        {

        }
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
    //                         イベントハンドラ
    //                         
    //====================================================================
    #region イベントハンドラ
    /// <summary>
    /// 設定クリック
    /// </summary>
    public void Btn_Next_Click()
    {
        CtrlTalk.Instance.Btn_Next_Click();
    }

    /// <summary>
    /// 停止ボタンクリック
    /// </summary>
    public void Btn_Stop_Click()
    {

        CtrlTalk.Instance.Btn_Stop_Click();
    }

    /// <summary>
    /// ログボタンクリック
    /// </summary>
    public void Btn_Alignment_Click()
    {

        CtrlTalk.Instance.Btn_Alignment_Click();
    }


    public void BtnALL_Click()
    {
        ChangeMode(ContentCategoly.home);
    }
    public void BtnNews_Click()
    {
        ChangeMode(ContentCategoly.news);
    }
    public void BtnMatome_Click()
    {
        ChangeMode(ContentCategoly.matome);
    }
    public void BtnRetweet_Click()
    {
        ChangeMode(ContentCategoly.retweet);
    }
    public void BtnPicture_Click()
    {
        ChangeMode(ContentCategoly.hotPicture);
    }
    public void BtnHash_Click()
    {
        ChangeMode(ContentCategoly.hotHash);
    }
    public void BtnHotWoed_Click()
    {
        ChangeMode(ContentCategoly.home);
    }
    #endregion

    //====================================================================
    // 
    //                         ゲーミング処理
    //                         
    //====================================================================
    #region ゲーミング処理
    /// <summary>
    /// モード変更時処理
    /// </summary>
    /// <param name="mode"></param>
    private void ChangeMode(ContentCategoly mode)
    {
        try
        {
            //モードを設定
            LiplisStatus.Instance.EnvironmentInfo.SelectMode = mode;

            //テキスト修正
            TxtTopicMode.text = ContentCategolyText.GetContentText(mode);

            //モード別処理
            CreateTbt();
        }
        catch
        {

        }
      
    }

    /// <summary>
    /// ニュースリストテーブルを更新する
    /// </summary>
    public void CreateTbt()
    {
        try
        {
            //モード別処理
            if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.home)
            {
                //全話題から取得
                StartCoroutine(tbt.CreateNewsList(LiplisStatus.Instance.NewTopic.GetTopic2BaseNewsDataAllList()));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.news)
            {
                //ニュースリストから取得
                StartCoroutine(tbt.CreateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.NewsList));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.matome)
            {
                //まとめニュースリストから取得
                StartCoroutine(tbt.CreateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.MatomeList));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.retweet)
            {
                //リツイートリストから取得
                StartCoroutine(tbt.CreateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.ReTweetList));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.hotPicture)
            {
                //画像ニュースリストから取得
                StartCoroutine(tbt.CreateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.PictureList));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.hotHash)
            {
                //ハッシュニュースリストから取得
                StartCoroutine(tbt.CreateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.HashList));
            }
            else
            {
                //全話題から取得
                StartCoroutine(tbt.CreateNewsList(LiplisStatus.Instance.NewTopic.GetTopic2BaseNewsDataAllList()));
            }
        }
        catch
        {

        }

    }

    /// <summary>
    /// アップデートする
    /// </summary>
    public void UpdateTbt()
    {
        try
        {
            //モード別処理
            if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.home)
            {
                //全話題から取得
                StartCoroutine(tbt.UpdateNewsList(LiplisStatus.Instance.NewTopic.GetTopic2BaseNewsDataAllList()));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.news)
            {
                //ニュースリストから取得
                StartCoroutine(tbt.UpdateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.NewsList));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.matome)
            {
                //まとめニュースリストから取得
                StartCoroutine(tbt.UpdateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.MatomeList));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.retweet)
            {
                //リツイートリストから取得
                StartCoroutine(tbt.UpdateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.ReTweetList));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.hotPicture)
            {
                //画像ニュースリストから取得
                StartCoroutine(tbt.UpdateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.PictureList));
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.hotHash)
            {
                //ハッシュニュースリストから取得
                StartCoroutine(tbt.UpdateNewsList(LiplisStatus.Instance.NewsList.LastNewsQ.HashList));
            }
            else
            {
                //全話題から取得
                StartCoroutine(tbt.UpdateNewsList(LiplisStatus.Instance.NewTopic.GetTopic2BaseNewsDataAllList()));
            }
        }
        catch
        {

        }

    }

    #endregion

}
