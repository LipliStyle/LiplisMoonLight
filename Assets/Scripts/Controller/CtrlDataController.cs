//====================================================================
//  ClassName : CtrlDataController
//  概要      : データコントローラー
//              
//              2018/07/09 定期リソース解放処理追加
//                         テクスチャなどのリソース解放は「Resources.UnloadUnusedAssets();」によって行われる。
//　　　　　　　　　　　　　→開放処理をCtrlGameControllerに移動
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Data;
using Assets.Scripts.Data.SubData;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Web.Clalis.v60;
using SpicyPixel.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlDataController : ConcurrentBehaviour
{
    ///=============================
    ///タイムアウト時間定義
    [SerializeField] Text TxtQNum;
    [SerializeField] Text TxtTopicNum;
    [SerializeField] Text TxtChatedNum;
    [SerializeField] Text TxtNewsListNum;
    [SerializeField] Slider TopicQSlider;
    [SerializeField] Text TxtTopicNumR;

    ///=============================
    ///背景ゲームオブジェクト
    GameObject BackGround;
    CtrlBackGroundImage ctrlBackground;

    ///=============================
    ///タイムアウト時間定義
    private float setTimeOut = 60f;

    ///=============================
    ///WateFrame
    private float WAIT_FRAME_NORMAL = 2f;
    private float WAIT_FRAME_LONG = 10f;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }


    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start () {
        //データロード
        Load();

        if (LiplisStatus.Instance.NewTopic.TalkTopicList == null) { LiplisStatus.Instance.NewTopic.TalkTopicList = new LpsQueue<MsgTopic>(); }
        if (LiplisStatus.Instance.NewTopic.InterruptTopicList == null) { LiplisStatus.Instance.NewTopic.InterruptTopicList = new LpsQueue<MsgTopic>(); }

        //背景ゲームオブジェクト取得
        this.BackGround = GameObject.Find("CanvasBackGround");
        this.ctrlBackground = this.BackGround.GetComponent<CtrlBackGroundImage>();

        //天気情報は常に最新を取る
        LiplisStatus.Instance.InfoWether.LastUpdateTime = LpsDatetimeUtil.enc(DateTime.Now.AddMinutes(-70));

        //ニュースリストは常に最新を取る
        LiplisStatus.Instance.NewsList.LastUpdateTime = LpsDatetimeUtil.enc(DateTime.Now.AddMinutes(-70));

        //地域データは常に最新を取る
        LiplisStatus.Instance.InfoLocation.LastUpdateTime = LpsDatetimeUtil.enc(DateTime.Now.AddMinutes(-70));

        //最終解放日時
        LiplisStatus.Instance.LastRunReleaseProcessing = DateTime.Now;

        //保持件数が少ない場合は更新時刻を初期化する
        LiplisStatus.Instance.NewTopic.InitLastUpdateTime();

        //ペンディングフォルス設定
        LiplisStatus.Instance.EnvironmentInfo.SetPendingOff();

        //定周期ループスタート
        StartCoroutine(DacaCollectTimerTick());
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update () {
        UpdateText();
    }

    /// <summary>
    /// テキストの表示
    /// </summary>
    void UpdateText()
    {
        try
        {
            //トークインスタンス取得
            DatNewTopic NewTopic = LiplisStatus.Instance.NewTopic;

            //テキスト表示
            TxtQNum.text = NewTopic.TalkTopicList.Count.ToString();
            TxtTopicNum.text = NewTopic.LastData.topicList.Count.ToString();
            TxtChatedNum.text = NewTopic.ChattedKeyList.Count.ToString();
            TxtNewsListNum.text = LiplisStatus.Instance.NewsList.LastNewsList.NewsList.Count.ToString();
            TxtTopicNumR.text = NewTopic.TalkTopicList.Count.ToString();

            //カウント
            TopicQSlider.value = NewTopic.TalkTopicList.Count;
        }
        catch
        {

        }
    }


    /// <summary>
    /// 終了時処理
    /// </summary>
    void OnDestroy()
    {
        Console.WriteLine("");
    }

    /// <summary>
    /// データ収集ループ　
    /// TIME_OUTに設定した周期で回る
    /// </summary>
    /// <returns></returns>
    IEnumerator DacaCollectTimerTick()
    {
        while (true)
        {
            //データ収集処理
            StartCoroutine(DataCollect());

            //トピック収集処理
            StartCoroutine(DataCollectTopic());

            //非同期待機
            yield return new WaitForSeconds(setTimeOut);
        }
    }

    /// <summary>
    /// データ収集処理
    /// </summary>
    //private void DataCollect()
    private IEnumerator DataCollect()
    {
        //地域データ取得
        yield return StartCoroutine(DataCollectLocation());

        //本日情報データ取得
        yield return StartCoroutine(DataCollectAnniversaryDays());

        //天気情報収集
        yield return StartCoroutine(DataCollectWether());

        //背景を変更しておく
        yield return this.ctrlBackground.ChangeBackground();

        //ニュースリスト取得
        StartCoroutine(SetLastNewsList());
    }

    /// <summary>
    /// トピックを収集する
    /// </summary>
    /// <returns></returns>
    private IEnumerator DataCollectTopic()
    {
        //ニュースデータ収集
        yield return StartCoroutine(DataCollectNewTopic());
    }



    /// <summary>
    /// データセーブ
    /// </summary>
    public IEnumerator Save()
    {
        //指定キー「LiplisStatus」でリプリスステータスのインスタンスを保存する
        SaveData.SetClass(LpsDefine.SETKEY_LIPLIS_STATUS, LiplisStatus.Instance);

        //セーブ発動
        SaveData.Save();

        yield return null;
    }

    /// <summary>
    /// データロード
    /// </summary>
    public void Load()
    {
        //必ず設定ロードを先に呼ぶ！
        //指定キー「LiplisStatus」でリプリスセッティングのインスタンスをロードする
        LiplisSetting.SetInstance(SaveDataSetting.GetClass<LiplisSetting>(LpsDefine.SETKEY_LIPLIS_SETTING, LiplisSetting.Instance));

        //指定キー「LiplisStatus」でリプリスステータスのインスタンスをロードする
        LiplisStatus.SetInstance(SaveData.GetClass<LiplisStatus>(LpsDefine.SETKEY_LIPLIS_STATUS, LiplisStatus.Instance));

        //キャッシュインスタンス化
        LiplisCache c = LiplisCache.Instance;
    }


    //====================================================================
    //
    //                        位置情報収集
    //                         
    //====================================================================
    #region 位置情報収集

    /// <summary>
    /// 位置情報収集
    /// </summary>
    private IEnumerator DataCollectLocation()
    {
        //トークインスタンス取得
        DatLocation Location = LiplisStatus.Instance.InfoLocation;

        //指定時間経過していなければ抜ける
        if (LpsDatetimeUtil.dec(Location.LastUpdateTime).AddMinutes(10) > DateTime.Now)
        {
            goto End;
        }

        //最新データをダウンロードする
        yield return StartCoroutine(SetLocation(Location));

        //終了ラベル
        End:;
    }

    /// <summary>
    /// 最新データをダウンロードする
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetLocation(DatLocation Location)
    {
        //最新ニュースデータ取得
        var Async = ClalisLocationInfomation.GetLocation();

        //非同期実行
        yield return Async;

        //データ取得
        ResLpsLocationInfo location = (ResLpsLocationInfo)Async.Current;

        //データセット
        Location.SetData(location);

        //最終更新時刻設定
        yield return Location.LastUpdateTime = LpsDatetimeUtil.Now;

    }


    #endregion


    //====================================================================
    //
    //                        天気情報収集
    //                         
    //====================================================================
    #region 天気情報収集

    /// <summary>
    /// 天気情報収集
    /// </summary>
    private IEnumerator DataCollectWether()
    {
        //トークインスタンス取得
        DatWether Wether = LiplisStatus.Instance.InfoWether;

        //指定時間経過していなければ抜ける
        if (LpsDatetimeUtil.dec(Wether.LastUpdateTime).AddMinutes(60) > DateTime.Now)
        {
            goto End;
        }

        //最新データをダウンロードする
        yield return StartCoroutine(SetWether(Wether));

        //終了ラベル
        End:;
    }


    /// <summary>
    /// 最新データをダウンロードする
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetWether(DatWether Wether)
    {
        //最新ニュースデータ取得
        var Async = ClalisLocationWetherList.GetWetherList(7);

        //非同期実行
        yield return Async;

        //データ取得
        ResLpsWeatherInfo60List DataList = (ResLpsWeatherInfo60List)Async.Current;

        //データセット
        Wether.SetData(DataList);

        //最終更新時刻設定
        yield return Wether.LastUpdateTime = LpsDatetimeUtil.Now;
    }



    #endregion

    //====================================================================
    //
    //                        本日情報データ取得
    //                         
    //====================================================================
    #region 本日情報データ取得

    /// <summary>
    /// 本日情報データ取得
    /// </summary>
    private IEnumerator DataCollectAnniversaryDays()
    {
        //トークインスタンス取得
        DatAnniversaryDays InfoAnniversary = LiplisStatus.Instance.InfoAnniversary;

        //指定時間経過していなければ抜ける
        if (LpsDatetimeUtil.dec(InfoAnniversary.LastUpdateTime).AddMinutes(10) > DateTime.Now)
        {
            goto End;
        }

        //本日データがすでに入っていれば抜ける
        if (InfoAnniversary.CheckTodayDataExists())
        {
            goto End;
        }

        //最新データをダウンロードする
        yield return StartCoroutine(SetAnniversaryDays(InfoAnniversary));

        //終了ラベル
        End:;
    }

    /// <summary>
    /// 最新データをダウンロードする
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetAnniversaryDays(DatAnniversaryDays InfoAnniversary)
    {
        //最新ニュースデータ取得
        var Async = ClalisWhatDayIsToday.GetAnniversaryDaysList();

        //非同期実行
        yield return Async;

        //データ取得
        ResWhatDayIsToday DataList = (ResWhatDayIsToday)Async.Current;

        //データリスト生成
        InfoAnniversary.SetData(DataList);

        //最終更新時刻設定
        yield return InfoAnniversary.LastUpdateTime = LpsDatetimeUtil.Now;
    }

    #endregion


    //====================================================================
    //
    //                        ニュースデータ収集関連
    //                         
    //====================================================================
    #region ニュースデータ収集関連
    /// <summary>
    /// データをセットする
    /// </summary>
    /// <param name="dataList"></param>
    public IEnumerator DataCollectNewTopic()
    {
        //指定時間経過していなければ抜ける
        if (LpsDatetimeUtil.dec(LiplisStatus.Instance.NewTopic.LastUpdateTime).AddMinutes(10) > DateTime.Now)
        {
            goto End;
        }

        //最終更新時刻設定
        yield return LiplisStatus.Instance.NewTopic.LastUpdateTime = LpsDatetimeUtil.Now;

        //最新データをダウンロードする
        yield return StartCoroutine(SetLastTopicMltData());

        //古いデータを削除する
        StartCoroutine(DeleteOldData());

        //終了ラベル
        End:;
    }

    private bool FlgRunSetLastTopicMltData = false;

    /// <summary>
    /// 最新データをダウンロードする
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetLastTopicMltData()
    {
        if(FlgRunSetLastTopicMltData)
        {
            goto End;
        }

        //実行中
        FlgRunSetLastTopicMltData = true;

        //トークインスタンス取得
        DatNewTopic NewTopic = LiplisStatus.Instance.NewTopic;

        //最新ニュースデータ取得
        IEnumerator Async;

        //件数が少ない場合はライト版で取得する
        if (NewTopic == null)
        {
            NewTopic = new DatNewTopic();
            NewTopic.LastData = new ResLpsTopicList();
            Async = ClalisForLiplisGetNewTopicMlLight.GetNewTopic();
        }
        else if (NewTopic.LastData == null)
        {
            NewTopic.LastData = new ResLpsTopicList();
            Async = ClalisForLiplisGetNewTopicMlLight.GetNewTopic();
        }
        else if (NewTopic.TalkTopicList.Count <= 25)
        {
            Async = ClalisForLiplisGetNewTopicMlLight.GetNewTopic();
        }
        else
        {
            Async = ClalisForLiplisGetNewTopicMl.GetNewTopic();
        }

        //非同期実行
        yield return Async;

        //データ取得
        ResLpsTopicList DownloadLastData = (ResLpsTopicList)Async.Current;

        //NULLチェック
        if (DownloadLastData == null) { goto End; }
        if (DownloadLastData.topicList == null) { goto End; }

        //データ取得
        NewTopic.LastData = DownloadLastData;

        //キーリスト取得
        List<string> keyList = NewTopic.GetKeyList();

        //スライス
        yield return new WaitForSeconds(1f);

        //キーが無ければ入れる。
        foreach (MsgTopic topic in NewTopic.LastData.topicList)
        {
            //話題を積む条件は精査必要
            if (!keyList.Contains(topic.DataKey))
            {
                if (LiplisSetting.Instance.Setting.CatCheck(topic.TopicClassification))
                {
                    //対象カテゴリならトピックリストに追加する
                    NewTopic.TalkTopicList.InsertToNotDuplicate(topic.Clone(), 0);
                }
                else
                {
                    //おしゃべり済みに追加
                    NewTopic.ChattedKeyList.AddToNotDuplicate(topic.Clone());
                }
            }

            //1フレームスキップ
            yield return null;
        }

        //取得した結果、120件以下ならおかわり
        if (NewTopic.TalkTopicList.Count <= 120)
        {
            //終了にする
            FlgRunSetLastTopicMltData = false;

            StartCoroutine(SetLastTopicMltData());
        }

        //重複排除
        NewTopic.RemoveDuplicateTopicList();

        //終了ラベル
        End:;

        //終了にする
        FlgRunSetLastTopicMltData = false;
    }

    /// <summary>
    /// 古いデータを削除する
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteOldData()
    {
        //指定条件に合致するデータを削除する
        LiplisStatus.Instance.NewTopic.DeleteOldData();

        //削除まで終わったらセーブする
        StartCoroutine(Save());

        //繰越
        yield return new WaitForSeconds(1f);
    }



    /// <summary>
    /// 最新のニュースリストを取得する
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetLastNewsList()
    {

        //トークインスタンス取得
        DatNewsList newsList = LiplisStatus.Instance.NewsList;

        //指定時間経過していなければ抜ける
        if (LpsDatetimeUtil.dec(newsList.LastUpdateTime).AddMinutes(60) > DateTime.Now)
        {
            goto End;
        }

        //最新データをダウンロードする
        yield return StartCoroutine(SetLastNews());

        //終了ラベル
        End:;    
    }

    /// <summary>
    /// 最新データをダウンロードする
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetLastNews()
    {
        //最新ニュースデータ取得
        var Async = ClalisForLiplisGetNewsList.GetNewsList();

        //非同期実行
        yield return Async;

        //データ取得
        ResLpsBaseNewsList DataList = (ResLpsBaseNewsList)Async.Current;

        //データセット
        LiplisStatus.Instance.NewsList.SetData(DataList);

        //取得ニュースデータ保存
        StartCoroutine(Save());

        //最終更新時刻設定
        yield return LiplisStatus.Instance.NewsList.LastUpdateTime = LpsDatetimeUtil.Now;
    }

    #endregion





}
