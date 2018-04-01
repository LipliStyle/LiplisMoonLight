//====================================================================
//  ClassName : CtrlDataController
//  概要      : データコントローラー
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================

using Assets.Scripts.Data;
using Assets.Scripts.Data.SubData;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Web.Clalis.v60;
using Assets.Scripts.LiplisUi;
using Assets.Scripts.Utils;
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

    ///=============================
    ///背景ゲームオブジェクト
    GameObject BackGround;
    CtrlBackGroundImage ctrlBackground;

    ///=============================
    ///タイムアウト時間定義
    private float setTimeOut = 60f;

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

        //ビジブルセット
        SetHidden();

        //定周期ループスタート
        StartCoroutine(DacaCollectTimerTick());
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
        }
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
        StartCoroutine(DataCollectLocation());

        //本日情報データ取得
        StartCoroutine(DataCollectAnniversaryDays());

        //天気情報収集
        StartCoroutine(DataCollectWether());

        //背景を変更しておく
        yield return this.ctrlBackground.ChangeBackground();

        //取得ニュースデータ保存
        Save();
    }

    /// <summary>
    /// トピックを収集する
    /// </summary>
    /// <returns></returns>
    private IEnumerator DataCollectTopic()
    {
        //ニュースデータ収集
        yield return StartCoroutine(DataCollectNewTopic());

        //取得ニュースデータ保存
        Save();
    }

    /// <summary>
    /// データセーブ
    /// </summary>
    public void Save()
    {
        //指定キー「LiplisStatus」でリプリスステータスのインスタンスを保存する
        SaveData.SetClass(LpsDefine.SETKEY_LIPLIS_STATUS, LiplisStatus.Instance);

        //セーブ発動
        SaveData.Save();
    }

    /// <summary>
    /// データロード
    /// </summary>
    public void Load()
    {
        //指定キー「LiplisStatus」でリプリスステータスのインスタンスをロードする
        LiplisStatus.SetInstance(SaveData.GetClass<LiplisStatus>(LpsDefine.SETKEY_LIPLIS_STATUS, LiplisStatus.Instance));
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

        //最新データをダウンロードする
        //StartCoroutine(SetLastTopicData());
        StartCoroutine(SetLastTopicMltData());

        //古いデータを削除する
        StartCoroutine(DeleteOldData());

        //最終更新時刻設定
        yield return LiplisStatus.Instance.NewTopic.LastUpdateTime = LpsDatetimeUtil.Now;

        //終了ラベル
        End:;
    }

    /// <summary>
    /// 最新データをダウンロードする
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetLastTopicMltData()
    {
        //最新ニュースデータ取得
        var Async = ClalisForLiplisGetNewTopic2Mlt.GetNewTopic(LiplisStatus.Instance.NewTopic.ToneUrlList);

        //非同期実行
        yield return Async;

        //トークインスタンス取得
        DatNewTopic NewTopic = LiplisStatus.Instance.NewTopic;

        //データ取得
        NewTopic.LastData = (ResLpsTopicList)Async.Current;

        //キーリスト取得
        List<string> keyList = NewTopic.GetKeyList();

        //NULLチェック
        if (NewTopic.LastData == null) { goto End; }
        if (NewTopic.LastData.topicList == null) { goto End; }

        //スライス
        yield return new WaitForSeconds(1f);

        //キーが無ければ入れる。
        foreach (MsgTopic topic in NewTopic.LastData.topicList)
        {
            //話題を積む条件は精査必要
            if (!keyList.Contains(topic.DataKey))
            {
                NewTopic.TalkTopicList.Add(topic);
            }


            //yield return new WaitForSeconds(1f);
        }

        //シャッフル
        NewTopic.TalkTopicList.Shuffle();

        //終了ラベル
        End:;
    }


    /// <summary>
    /// 最新データをダウンロードする
    /// 
    /// 廃止予定
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetLastTopicData()
    {
        //最新ニュースデータ取得
        var Async = ClalisForLiplisGetNewTopic2.GetNewTopic(LpsDefine.LIPLIS_TONE_URL_HAZUKI);

        //非同期実行
        yield return Async;

        //トークインスタンス取得
        DatNewTopic NewTopic = LiplisStatus.Instance.NewTopic;

        //データ取得
        NewTopic.LastData = (ResLpsTopicList)Async.Current;

        //アロケーションIDチェック
        FixAllocationId();

        //キーリスト取得
        List<string> keyList = NewTopic.GetKeyList();

        //NULLチェック
        if (NewTopic.LastData == null) { goto End; }
        if (NewTopic.LastData.topicList == null) { goto End; }

        //スライス
        yield return new WaitForSeconds(1f);

        //キーが無ければ入れる。
        foreach (MsgTopic topic in NewTopic.LastData.topicList)
        {
            //口調変換
            //foreach (MsgSentence sentence in topic.TalkSentenceList)
            //{
            //    sentence.ToneConvert();
            //}

            //アロケーションIDを設定する
            TopicUtil.SetAllocationId(topic);

            //話題を積む条件は精査必要
            if (!keyList.Contains(topic.DataKey))
            {
                NewTopic.TalkTopicList.Add(topic);
            }


            yield return new WaitForSeconds(1f);
        }

        //終了ラベル
        End:;
    }

    /// <summary>
    /// 古いデータを削除する
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteOldData()
    {
        //トークインスタンス取得
        DatNewTopic NewTopic = LiplisStatus.Instance.NewTopic;

        //指定条件に合致するデータを削除する
        NewTopic.TalkTopicList.RemoveAll(TermOldTopicDelete);
        NewTopic.ChattedKeyList.RemoveAll(TermOldTopicDelete);

        //繰越
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 古データ 削除条件
    /// </summary>
    /// <param name="topic"></param>
    /// <returns></returns>
    private bool TermOldTopicDelete(MsgTopic topic)
    {
        return LpsDatetimeUtil.dec(topic.CreateTime) <= DateTime.Now.AddHours(-6);
    }

    /// <summary>
    /// アロケーションIDの修正
    /// </summary>
    private void FixAllocationId()
    {

    }


    #endregion





}
