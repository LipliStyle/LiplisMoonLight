//=======================================================================
//  ClassName : CtrlTalk
//  概要      : トークコントローラー
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Controller;
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Sentece;
using Assets.Scripts.LiplisSystem.Web;
using Assets.Scripts.LiplisSystem.Web.Clalis.v60;
using Assets.Scripts.Utils;
using SpicyPixel.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlTalk : ConcurrentBehaviour
{
    //=============================
    // ウインドウ保持時間
    private const int WINDOW_LIFESPAN_TIME = 60;            //ウインドウの寿命
    private const int TALK_WAIT_DEFAULT = 7;
    private const int TALK_WAIT_NEUTRAL_DEFAULT = 5;
    private const int TALK_WAIT_NEUTRAL_IDLE_DEFAULT = 2;
    private const float TALK_MANAGEMENT_INTERVAL = 0.5f;

    ///=============================
    /// ウインドウインスタンス
    private Queue<InfoWindow> WindowTitleListQ;
    private Queue<ImageWindow> WindowImageListQ;

    ///=============================
    /// 現在処理中ウインドウインスタンス
    private TalkWindow NowTalkWindow;
    private InfoWindow NowTitleWindow;
    private ImageWindow NowImageWindow;

    ///=============================
    /// 現在ロードトピック
    private MsgTopic NowLoadTopic;
    private int NowSentenceCount;

    ///=============================
    /// おしゃべりウェイトカウンター
    private int TalkWaitCount = 0;

    ///=============================
    // ウインドウインスタンス
    private GameObject InfoWindowInstanse;
    private GameObject ImageWindowInstanse;

    ///=============================
    // レンダリングUIキャッシュ
    public GameObject UiRenderingBack;
    public GameObject UiRenderingFront;

    ///=============================
    /// ログ画面
    public GameObject CanvasLog;
    private CtrlLog NewsLog;

    ///=============================
    /// モデルコントローラー
    public CtrlModelController modelController;

    ///=============================
    ///制御フラグ
    private bool FlgPendig = false;

    ///=============================
    ///オーディオソース
    //private List<AudioSource> audioSourceList;

    //====================================================================
    //
    //                          シングルトン管理
    //                         
    //====================================================================
    #region シングルトン管理

    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    private static CtrlTalk instance;
    public static CtrlTalk Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CtrlTalk();
            }

            return instance;
        }
    }

    /// <summary>
    /// インスタンス化
    /// </summary>
    private CtrlTalk()
    {
        init();
    }

    #endregion


    //====================================================================
    //
    //                             初期化処理
    //                         
    //====================================================================
    #region 初期化処理
    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Awake()
    {
        //ベースアウェーク(初期化)
        base.Awake();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        //ベースアウェーク(初期化)
        base.Awake();

        //初期化処理
        init();

        //オーディオの初期化
        InitAudio();

        //タイマースタート
        startTimer();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void init()
    {
        if (this.WindowTitleListQ == null) { WindowTitleListQ = new Queue<InfoWindow>(); }
        if (this.WindowImageListQ == null) { WindowImageListQ = new Queue<ImageWindow>(); }
        instance = this;

        //なうセンテンスカウント 初期化
        initNowSentenceCount();

        //ニュースログ取得
        if (CanvasLog == null)
        {
            //NULLなら何もしない
        }
        else
        {
            NewsLog = CanvasLog.GetComponent<CtrlLog>();
        }
    }

    /// <summary>
    /// なうセンテンスカウントを初期化する
    /// </summary>
    private void initNowSentenceCount()
    {
        NowSentenceCount = 0;
    }

    /// <summary>
    /// ウインドウインスタンスの初期化
    /// </summary>
    private void InitInfoWindowInstanse()
    {
        if (InfoWindowInstanse == null)
        {
            InfoWindowInstanse = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_INFO);
        }
    }

    /// <summary>
    /// イメージウインドウインスタンスの初期化
    /// </summary>
    private void InitImageWindowInstanse()
    {
        if (ImageWindowInstanse == null)
        {
            ImageWindowInstanse = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_IMAGE);
        }
    }

    /// <summary>
    /// オーディオのの初期化
    /// </summary>
    private void InitAudio()
    {
        //audioSourceList = new List<AudioSource>(gameObject.GetComponents<AudioSource>());
    }


    /// <summary>
    /// タイマースタート
    /// </summary>
    void startTimer()
    {
        StartCoroutine(TalkManagementTimerTick());
    }


    // Update is called once per frame
    void Update()
    {

    }

    //削除時実行
    void OnDestroy()
    {

    }

    #endregion


    //====================================================================
    //
    //                    ウインドウメンテンナンス関連
    //                         
    //====================================================================
    #region ウインドウメンテンナンス関連
    /// <summary>
    /// ウインドウメンテループ　
    /// 1秒周期で実行
    /// </summary>
    /// <returns></returns>
    IEnumerator TalkManagementTimerTick()
    {
        while (true)
        {
            //データ収集処理
            WindowMaintenance();

            //おしゃべり待ち
            TalkWating();

            //件数が5件以下ならプッシュする
            if (LiplisStatus.Instance.NewTopic.TalkTopicList.Count < 5)
            {
                //nullチェック
                if (LiplisStatus.Instance.NewTopic.ChattedKeyList != null)
                {
                    //おしゃべり済みが存在したら、そちらから取得する
                    if (LiplisStatus.Instance.NewTopic.ChattedKeyList.Count > 0)
                    {
                        //おしゃべり済みが存在したらセットする
                        LiplisStatus.Instance.NewTopic.SetTopicListFromChattedKeyList();
                    }
                    else if (LiplisStatus.Instance.NewTopic.LastData != null)
                    {
                        //ラストデータNULLチェック
                        if (LiplisStatus.Instance.NewTopic.LastData.topicList != null)
                        {
                            //トピックデータNULLチェック
                            if (LiplisStatus.Instance.NewTopic.LastData.topicList.Count > 0)
                            {
                                //最新データからセットする
                                LiplisStatus.Instance.NewTopic.SetTopicListFromLastData();
                            }
                            else
                            {
                                StartCoroutine(SetTopicDirectTopicAsync());
                            }

                        }
                        else
                        {
                            StartCoroutine(SetTopicDirectTopicAsync());
                        }
                    }
                    else
                    {
                        StartCoroutine(SetTopicDirectTopicAsync());
                    }
                }
                else
                {
                    StartCoroutine(SetTopicDirectTopicAsync());
                }
            }

            //非同期待機
            yield return new WaitForSeconds(TALK_MANAGEMENT_INTERVAL);
        }
    }


    /// <summary>
    /// ウインドウメンテナンス
    /// </summary>
    private void WindowMaintenance()
    {
        modelController.WindowMaintenance(WINDOW_LIFESPAN_TIME);

        //一定時間経過したウインドウは自動的に除去する
        if (!UnityNullCheck.IsNull(NowTalkWindow))
        {
            if (NowTalkWindow.CreateTime.AddSeconds(WINDOW_LIFESPAN_TIME) < DateTime.Now)
            {
                NowTalkWindow.FlgTalking = false;
                NowTalkWindow.CloseWindow();
            }
        }

    }

    /// <summary>
    /// おしゃべり待ち
    /// </summary>
    private void TalkWating()
    {
        try
        {
            //ペンディング設定ならスキップ
            if (LiplisStatus.Instance.EnvironmentInfo.FlgTalkPending)
            {
                FlgPendig = true;
                return;
            }
            else if (FlgPendig && !LiplisStatus.Instance.EnvironmentInfo.FlgTalkPending)
            {
                //復帰直後の場合、スキップをかける。
                FlgPendig = false;
                SkipWindow();
            }



            //音声再生完了待ち
            if (modelController.IsPlaying())
            {
                return;
            }

            if (!UnityNullCheck.IsNull(NowTalkWindow))
            {
                if (!NowTalkWindow.FlgTalking)
                {

                    if (NowLoadTopic != null)
                    {
                        if (NowLoadTopic.TalkSentenceList.Count == 0)
                        {
                            //おしゃべり終了済みならカウントアップ
                            TalkWaitCount++;
                        }
                        else
                        {
                            //次のセンテンスをセットする
                            SetNextSentence();
                        }
                    }
                    else
                    {
                        //おしゃべり終了済みならカウントアップ
                        TalkWaitCount++;
                    }
                }
                else
                {
                    //おしゃべり中は常にカウントリセット
                    TalkWaitCount = 0;
                }
            }
            else
            {
                //おしゃべりウインドウがなければカウントアップ
                TalkWaitCount++;
            }

            //おしゃべり待ちタイムアウト
            if (TalkWaitCount >= TALK_WAIT_DEFAULT)
            {
                TalkWaitCount = 0;
                OnTalkWaitTimeout();
            }



        }
        catch
        {

        }
    }

    /// <summary>
    /// キーリストを取得する
    /// </summary>
    /// <returns></returns>
    public List<string> GetKeyList()
    {
        List<string> keyList = new List<string>();

        foreach (MsgTopic topic in LiplisStatus.Instance.NewTopic.TalkTopicList)
        {
            keyList.Add(topic.DataKey);
        }

        return keyList;
    }
    #endregion


    //====================================================================
    //
    //                       割り込み話題関連処理
    //                         
    //====================================================================
    #region 割り込み話題関連処理
    /// <summary>
    /// 割り込みメッセージを追加する
    /// </summary>
    /// <param name="topic"></param>
    public void AddInterruptTopic(MsgTopic topic)
    {
        LiplisStatus.Instance.NewTopic.InterruptTopicList.Add(topic);
    }
    public void AddInterruptTopic(List<MsgTopic> topicList)
    {
        LiplisStatus.Instance.NewTopic.InterruptTopicList.AddRange(topicList);
    }

    //====================================================================
    //
    //                  定型文関連処理(あいさつ、時報など)
    //                         
    //====================================================================

    /// <summary>
    /// 
    /// </summary>
    public void Greet()
    {
        //グリート追加
        LiplisStatus.Instance.NewTopic.InterruptTopicList.Add(CreateGreet());

        //次の話題
        SetNextTopic();
    }
    public MsgTopic CreateGreet()
    {
        //トピックを生成する
        MsgTopic topic = new MsgTopic();

        //各キャラクターの挨拶を取得する
        List<MsgTopic> lst = modelController.GetGreet();

        //センテンスを入れなおす
        foreach (var charGreet in lst)
        {
            foreach (var sentence in charGreet.TalkSentenceList)
            {
                topic.TalkSentenceList.Add(sentence);
            }
        }

        //アニバーサリーセンテンスセット
        SetAnniversarySentence(topic);

        //お天気センテンスセット
        SetWetherSentence(topic);

        return topic;
    }

    /// <summary>
    /// アニバーサリーセンテンスをセットする。
    /// </summary>
    /// <param name="topic"></param>
    public void SetAnniversarySentence(MsgTopic topic)
    {
        //データ取得
        ResWhatDayIsToday DataList = LiplisStatus.Instance.InfoAnniversary.DataList;

        if (DataList == null)
        {
            return;
        }

        int sentenceIdx = 0;
        int AllocationId = 0;

        foreach (var data in DataList.AnniversaryDaysList)
        {
            foreach (MsgSentence talkSentence in data.TalkSentenceList)
            {
                MsgSentence sentence = talkSentence.Clone();

                //キャラデータ取得
                LiplisModel cahrData = modelController.TableModelId[AllocationId];

                if (sentenceIdx == 0)
                {
                    sentence.BaseSentence = "今日は" + sentence.BaseSentence + "みたいです～♪";
                    sentence.TalkSentence = sentence.BaseSentence;
                }
                else
                {
                    sentence.ToneConvert();
                }

                //アロケーションID設定
                sentence.AllocationId = AllocationId;

                //インデックスインクリメント
                sentenceIdx++;
                AllocationId++;

                //アロケーションIDコントロール
                if (AllocationId > 3)
                {
                    AllocationId = 0;
                }

                //センテンスを追加
                topic.TalkSentenceList.Add(sentence);
            }
        }
    }

    /// <summary>
    /// 天気文章をセットする
    /// 
    /// TODO CtrlTalk:SetWetherSentence アロケーションIDの付け方再考
    /// </summary>
    /// <param name="topic"></param>
    public void SetWetherSentence(MsgTopic topic)
    {
        //NULLチェック
        if (LiplisStatus.Instance.InfoWether.WetherDtlList == null)
        {
            return;
        }

        if (LiplisStatus.Instance.InfoWether.WetherDtlList.Count < 1)
        {
            return;
        }

        //最終センテンス取得
        MsgSentence lastSentence = topic.TalkSentenceList[topic.TalkSentenceList.Count - 1];

        //モデル
        LiplisModel cahrData1;
        LiplisModel cahrData2;
        LiplisModel cahrData3;

        //アロケーションID
        int AllocationId1;
        int AllocationId2;
        int AllocationId3;

        //アロケーションID取得
        AllocationId1 = lastSentence.AllocationId + 1;
        if (AllocationId1 > 3) { AllocationId1 = 0; }
        AllocationId2 = AllocationId1 + 1;
        if (AllocationId2 > 3) { AllocationId2 = 0; }
        AllocationId3 = AllocationId2 + 1;
        if (AllocationId3 > 3) { AllocationId3 = 0; }

        //キャラクターデータ取得
        cahrData1 = modelController.TableModelId[AllocationId1];
        cahrData2 = modelController.TableModelId[AllocationId2];
        cahrData3 = modelController.TableModelId[AllocationId3];


        //現在時刻取得
        DateTime dt = DateTime.Now;

        //天気コード取得
        MsgDayWether todayWether = LiplisStatus.Instance.InfoWether.GetWetherSentenceToday(dt);

        //0～12 今日 午前、午後、夜の天気
        if (dt.Hour >= 0 && dt.Hour <= 18)
        {
            LiplisWeather.CreateWetherMessage("今日の天気は", todayWether, topic.TalkSentenceList, cahrData1.Tone, cahrData1.AllocationId);
        }

        //19～23 明日の天気
        else if (dt.Hour >= 19 && dt.Hour <= 23)
        {
            //明日の天気も取得
            MsgDayWether tomorrowWether = LiplisStatus.Instance.InfoWether.GetWetherSentenceTommorow(dt);

            LiplisWeather.CreateWetherMessage("", todayWether, topic.TalkSentenceList, cahrData2.Tone, cahrData2.AllocationId);
            LiplisWeather.CreateWetherMessage("明日の天気は", tomorrowWether, topic.TalkSentenceList, cahrData3.Tone, cahrData3.AllocationId);

        }
    }

    #endregion

    //====================================================================
    //
    //                 　        トーク関連処理
    //                         
    //====================================================================
    #region トーク関連処理
    /// <summary>
    /// おしゃべり待ち終了時処理
    /// </summary>
    private void OnTalkWaitTimeout()
    {
        NextTalkOrSkip();
    }

    /// <summary>
    /// 次の文章をセットする
    /// </summary>
    private void SetNextSentence()
    {
        try
        {
            //バカよけ
            if (NowLoadTopic.TalkSentenceList.Count < 1)
            {
                SetNextTopic();

                return;
            }

            //設定数おしゃべりしたら次の話題
            if (NowSentenceCount > LiplisSetting.Instance.Setting.GetTalkNum())
            {
                while (NowLoadTopic.TalkSentenceList.Count > 0)
                {
                    MsgSentence s = NowLoadTopic.TalkSentenceList.Dequeue();
                }

                return;
            }

            //センテンスを1個取り出す
            MsgSentence sentence = NowLoadTopic.TalkSentenceList.Dequeue();

            //センテンスをセットする
            SetNextSentence(sentence);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    private void SetNextSentence(MsgSentence sentence)
    {
        //トーンコンバート
        sentence.ToneConvert();

        //ウインドウを表示する
        if (!sentence.FlgAddMessge)
        {
            if (sentence.TalkSentence != null)
            {
                CreateWindow(sentence.TalkSentence, sentence.AllocationId);
            }
        }
        else
        {
            this.NowTalkWindow.AddText(sentence.TalkSentence);
        }

        //音声再生
        VoiceTalk(sentence);

        //表情設定
        StartCoroutine(modelController.SetExpression(sentence));

        //おしゃべりの開始
        modelController.StartTalking(sentence);

        //センテンスカウントインクリメント
        NowSentenceCount++;
    }

    /// <summary>
    /// 音声おしゃべり
    /// </summary>
    private void VoiceTalk(MsgSentence sentence)
    {
        //音声おしゃべり設定がONなら、音声おしゃべりする
        if (LiplisSetting.Instance.Setting.FlgVoice)
        {
            if (sentence.VoiceData != null)
            {
                modelController.StartVoice(sentence.AllocationId, sentence.VoiceData);
            }
        }
    }

    /// <summary>
    /// 次の話題をセットする
    /// </summary>
    public void SetNextTopic()
    {
        //ウインドウを一旦クリア
        DestroyAllWindow();

        //話題取得
        if (LiplisStatus.Instance.NewTopic.TalkTopicList.Count > 0 || LiplisStatus.Instance.NewTopic.InterruptTopicList.Count > 0)
        {
            //トピックをセットする
            SetToipc();
        }
        else if (LiplisStatus.Instance.NewTopic.LastData.topicList.Count > 0)
        {
            //話題が尽きた場合、最新データリストに話題があれば、取得する
            LiplisStatus.Instance.NewTopic.SetTopicListFromChattedKeyList();
        }
        else
        {
            //蓄積された話題が無い場合はショートニュース取得
            SetTopicDirect();
        }
    }
    /// <summary>
    /// 指定キーで次の話題をセットする
    /// </summary>
    /// <param name="DataKey"></param>
    /// <param name="Categoly"></param>
    public void SetNextTopic(string DataKey, ContentCategoly Categoly)
    {
        //ウインドウを一旦クリア
        DestroyAllWindow();

        //トピックをセットする
        SetTopicTalkFromLastNewsList(DataKey, Categoly);
    }

    /// <summary>
    /// 話題をセットする
    /// </summary>
    private void SetToipc()
    {
        if (LiplisStatus.Instance.NewTopic.InterruptTopicList.Count < 1)
        {
            if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.home)
            {
                //全話題から取得
                SetTopicTalkTopicList();
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.news)
            {
                //ニュースリストから取得
                SetTopicTalkFromNewsList();
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.matome)
            {
                //画像ニュースリストから取得
                SetTopicTalkFromSummaryList();
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.retweet)
            {
                //リツイートリストから取得
                SetTopicTalkFromReTweetList();
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.hotPicture)
            {
                //画像ニュースリストから取得
                SetTopicTalkFromPictureList();
            }
            else if (LiplisStatus.Instance.EnvironmentInfo.SelectMode == ContentCategoly.hotHash)
            {
                //画像ニュースリストから取得
                SetTopicTalkFromHashList();
            }
            else
            {
                //全話題から取得
                SetTopicTalkTopicList();
            }
        }
        else
        {
            //話題設定、移動
            SetTopicInterruptTopicList();
        }
    }

    /// <summary>
    /// 話題設定の終了処理
    /// </summary>
    private IEnumerator SetToipcEnd()
    {
        //なうセンテンスカウントの初期化
        initNowSentenceCount();

        //キャラクター位置移動
        modelController.ShuffleCharPosition(this.NowLoadTopic);

        //音声ダウンロード開始
        yield return StartCoroutine(SetVoiceData());

        //タイトルウインドウをセットする
        SetTitleWindow();

        //センテンスをセットする
        SetNextSentence();
    }

    /// <summary>
    /// 割り込み話題話題設定の終了処理
    /// </summary>
    private IEnumerator SetToipcEndInterrupt()
    {
        //なうセンテンスカウントの初期化
        initNowSentenceCount();

        //キャラクター位置移動
        modelController.ShuffleCharPosition(this.NowLoadTopic);

        //音声ダウンロード開始
        yield return StartCoroutine(SetVoiceDataInterrupt());

        //タイトルウインドウをセットする
        SetTitleWindow();

        //センテンスをセットする
        SetNextSentence();
    }

    /// <summary>
    /// トピックリストから取得する
    /// </summary>
    private void SetTopicTalkTopicList()
    {
        //次の話題をロードする
        this.NowLoadTopic = LiplisStatus.Instance.NewTopic.TopicListDequeue(modelController.GetModelList());

        //ログ追加
        StartCoroutine(NewsLog.AddLog(this.NowLoadTopic.Clone()));

        //話題のセット、移動
        StartCoroutine(SetToipcEnd());
    }

    /// <summary>
    /// 割り込み話題リストから取得する
    /// </summary>
    private void SetTopicInterruptTopicList()
    {
        //割り込み話題があれば、そちらから取得する
        this.NowLoadTopic = LiplisStatus.Instance.NewTopic.InterruptTopicList.Dequeue();

        //ログ追加
        StartCoroutine(NewsLog.AddLog(this.NowLoadTopic.Clone()));

        //話題のセット、移動
        StartCoroutine(SetToipcEndInterrupt());
    }


    /// <summary>
    /// ニュースリストを取得する
    /// </summary>
    private void SetTopicTalkFromNewsList()
    {
        //ニュースリストからデータキーを取得する
        SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.LastNewsList.GetNewsKeyFromNewsList(), ContentCategoly.news);
    }

    /// <summary>
    /// まとめリストを取得する
    /// </summary>
    private void SetTopicTalkFromSummaryList()
    {
        //ニュースリストからデータキーを取得する
        SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.LastNewsList.GetNewsKeyFromMatomeList(), ContentCategoly.matome);
    }

    /// <summary>
    /// リツイートリストを取得する
    /// </summary>
    private void SetTopicTalkFromReTweetList()
    {
        //ニュースリストからデータキーを取得する
        SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.LastNewsList.GetNewsKeyFromReTweetList(), ContentCategoly.retweet);
    }


    /// <summary>
    /// ピクチャーリストを取得する
    /// </summary>
    private void SetTopicTalkFromPictureList()
    {
        //ニュースリストからデータキーを取得する
        SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.LastNewsList.GetNewsKeyFromPictureList(), ContentCategoly.hotPicture);
    }

    /// <summary>
    /// まとめリストを取得する
    /// </summary>
    private void SetTopicTalkFromHashList()
    {
        //ニュースリストからデータキーを取得する
        SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.LastNewsList.GetNewsKeyFromHashList(), ContentCategoly.matome);
    }

    /// <summary>
    /// 特定の話題をおしゃべり
    /// </summary>
    /// <param name="DataKey"></param>
    public void SetTopicTalkFromLastNewsList(string DataKey, ContentCategoly NewsSource)
    {
        //次の話題をロードする
        MsgTopic topic = LiplisStatus.Instance.NewTopic.SearchTopic(DataKey,modelController.GetModelList());

        //Topicが取得できたら、なうろーどに入れて終了
        if (topic != null)
        {
            //カテゴリ修正
            topic.TopicClassification = ((int)NewsSource).ToString();

            //ナウロードセット
            this.NowLoadTopic = topic;

            //ログ追加
            StartCoroutine(NewsLog.AddLog(this.NowLoadTopic.Clone()));

            //話題のセット、移動
            StartCoroutine(SetToipcEnd());

            return;
        }

        //Clalisサーバーから対象データ取得
        StartCoroutine(SetTopicTalkFromClalis(DataKey, NewsSource));
    }

    /// <summary>
    /// トピックリストから取得する
    /// </summary>
    private void SetTopicFromResLpsTopicList()
    {
        //次の話題をロードする
        this.NowLoadTopic = LiplisStatus.Instance.NewTopic.GetTopicFromResLpsTopicList();


        if (NowLoadTopic.TalkSentenceList.Count < 2)
        {
            Debug.Log("2件以下！");
        }

        //ログ追加
        StartCoroutine(NewsLog.AddLog(this.NowLoadTopic.Clone()));

        //話題のセット、移動
        StartCoroutine(SetToipcEnd());
    }

    private IEnumerator SetTopicTalkFromClalis(string DataKey, ContentCategoly NewsSource)
    {
        //トピックをランダムで取得する
        var Async = ClalisForLiplisGetNewTopicOne.GetNewTopicAsync(LiplisStatus.Instance.NewTopic.ToneUrlList, DataKey, ((int)NewsSource).ToString());

        //非同期実行
        yield return Async;

        //取得結果取得
        ResLpsTopicList resTopic = (ResLpsTopicList)Async.Current;

        //現在ロード中の話題をおしゃべり済みに入れる
        if (resTopic != null)
        {
            //話題取得
            this.NowLoadTopic = resTopic.topicList[0];

            //カテゴリセット
            this.NowLoadTopic.TopicClassification = ((int)NewsSource).ToString();

            //おしゃべり済みに追加
            LiplisStatus.Instance.NewTopic.ChattedKeyList.AddToNotDuplicate(this.NowLoadTopic.Clone());

            //話題のセット、移動
            StartCoroutine(SetToipcEnd());

            //ログ追加
            StartCoroutine(NewsLog.AddLog(this.NowLoadTopic.Clone()));
        }
        else
        {
            //だめなら、デフォルト処理
            SetTopicTalkTopicList();
        }
    }
    public bool flag;
    IEnumerator Wait()
    {
        while (flag == false)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    /// <summary>
    /// トピックを先頭に挿入する
    /// </summary>
    /// <param name="topic"></param>
    private void InsertTopToipc(MsgTopic topic)
    {
        LiplisStatus.Instance.NewTopic.TalkTopicList.Insert(0, topic);

        SetNextTopic();
    }

    /// <summary>
    /// 話題を直接セットする
    /// </summary>
    private void SetTopicDirect()
    {
        try
        {
            //トピックをセット
            StartCoroutine(SetTopicDirectTopic());
        }
        catch
        {
            this.NowLoadTopic = modelController.CreateTopicFromShortNews();
        }

    }

    /// <summary>
    /// ショートニュースからトピックを生成する
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetTopicDirectTopic()
    {
        //トピックをランダムで取得する
        var Async = ClalisForLiplisGetNewTopicOne.GetNewTopicAsync(LiplisStatus.Instance.NewTopic.ToneUrlList);

        //非同期実行
        yield return Async;

        ResLpsTopicList resTopic = (ResLpsTopicList)Async.Current;

        //NULLチェック
        if (resTopic == null || resTopic.topicList.Count < 0)
        {
            this.NowLoadTopic = modelController.CreateTopicFromShortNews();
        }

        //取得したトピックを返す
        this.NowLoadTopic = resTopic.topicList[0];

        //話題のセット、移動
        StartCoroutine(SetToipcEnd());
    }

    /// <summary>
    /// 最新データをダウンロードする
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetTopicDirectTopicAsync()
    {
        //最新ニュースデータ取得
        var Async = ClalisForLiplisGetNewTopicOne.GetNewTopicAsync(LiplisStatus.Instance.NewTopic.ToneUrlList);

        //非同期実行
        yield return Async;

        //データ取得
        ResLpsTopicList data = (ResLpsTopicList)Async.Current;

        //NULL回避
        if (data == null) { yield break; }
        if (data.topicList == null) { yield break; }

        //おしゃべり済みに追加しないでおしゃべり
        foreach (var topic in data.topicList)
        {
            topic.FlgNotAddChatted = true;

            //アロケーションIDを設定する
            TopicUtil.SetAllocationIdAndTone(topic, modelController.GetModelList());
        }

        //データ追加
        LiplisStatus.Instance.NewTopic.TalkTopicList.AddRange(data.topicList);
    }

    /// <summary>
    /// 次のおしゃべり、おしゃべり中であればスキップする
    /// </summary>
    public void NextTalkOrSkip()
    {
        //ペンディング設定
        LiplisStatus.Instance.EnvironmentInfo.SetPendingOff();

        if (IsTalkingNow())
        {
            //おしゃべり中ならスキップ
            SkipWindow();
        }
        else
        {
            SetNextSentence();
        }
    }

    /// <summary>
    /// おしゃべり中かチェック
    /// </summary>
    private bool IsTalkingNow()
    {
        if (modelController.GetWindowQCount() > 0)
        {
            //ウインドウがあれば、先頭ウインドウのおしゃべり中フラグを調べる
            return this.NowTalkWindow.FlgTalking;
        }
        else
        {
            //ウインドウがなければおしゃべり中ではない
            return false;
        }
    }

    /// <summary>
    /// 1番目のウインドウに対し、スキップをかける
    /// </summary>
    private void SkipWindow()
    {
        this.NowTalkWindow.SetSkip();
    }


    #endregion

    //====================================================================
    //
    //                 　        音声おしゃべり関連
    //                         
    //====================================================================
    #region 音声おしゃべり関連

    /// <summary>
    /// ボイスデータを設定する
    /// </summary>
    public IEnumerator SetVoiceData()
    {
        //音声おしゃべり設定がONなら、音声おしゃべりする
        if (LiplisSetting.Instance.Setting.FlgVoice)
        {
            if(NowLoadTopic.TalkSentenceList.Count < 0)
            {
                //0以下なら何もしない
            }
            else
            {
                //トーンコンバート
                NowLoadTopic.TalkSentenceList[0].ToneConvert();

                //初期データをセット
                yield return StartCoroutine(ClalisForLiplisGetVoiceMp3Ondemand.SetVoiceDataStart(NowLoadTopic, modelController.GetModelCount()));

                //以降は順次セット
                SetVoiceData(this.NowLoadTopic);
            }
        }
    }

    /// <summary>
    /// ボイスデータを取得する
    /// </summary>
    /// <param name="NowLoadTopic"></param>
    /// <returns></returns>
    public void SetVoiceData(MsgTopic NowLoadTopic)
    {
        foreach (MsgSentence sentence in NowLoadTopic.TalkSentenceList)
        {
            //センテンス状態チェック
            if (sentence.VoiceData != null)
            {
                //すでにデータがあれば何もしない
            }
            else if (sentence.AllocationId < 0 || sentence.AllocationId >= modelController.GetModelCount())
            {
                //何もしない
            }
            else
            {
                StartCoroutine(SetVoiceData(NowLoadTopic, sentence));
            }
        }
    }
    public IEnumerator SetVoiceData(MsgTopic NowLoadTopic, MsgSentence sentence)
    {
        //トーンコンバート
        sentence.ToneConvert();

        //var Async = ClalisForLiplisGetVoiceMp3.GetAudioClip(NowLoadTopic, sentence.AllocationId, sentence.SubId);
        var Async = ClalisForLiplisGetVoiceMp3Ondemand.GetAudioClip(sentence,modelController.GetModelCount());
        
        //非同期実行
        yield return Async;

        //データ取得
        sentence.VoiceData = (AudioClip)Async.Current;
    }


    /// <summary>
    /// ボイスデータを設定する
    /// </summary>
    public IEnumerator SetVoiceDataInterrupt()
    {
        //音声おしゃべり設定がONなら、音声おしゃべりする
        if (LiplisSetting.Instance.Setting.FlgVoice)
        {
            //初期データをセット
            yield return StartCoroutine(ClalisForLiplisGetVoiceMp3Ondemand.SetVoiceDataStart(NowLoadTopic, modelController.GetModelCount()));

            //以降は順次セット
            SetVoiceDataInterrupt(this.NowLoadTopic);
        }
    }

    /// <summary>
    /// ボイスデータを取得する
    /// </summary>
    /// <param name="NowLoadTopic"></param>
    /// <returns></returns>
    public void SetVoiceDataInterrupt(MsgTopic NowLoadTopic)
    {
        foreach (MsgSentence sentence in NowLoadTopic.TalkSentenceList)
        {
            //センテンス状態チェック
            if (sentence.VoiceData != null)
            {
                //すでにデータがあれば何もしない
            }
            else if (sentence.AllocationId < 0 || sentence.AllocationId >= modelController.GetModelCount())
            {
                //何もしない
            }
            else
            {
                StartCoroutine(SetVoiceDataInterrupt(sentence));
            }
        }
    }
    public IEnumerator SetVoiceDataInterrupt(MsgSentence sentence)
    {
        var Async = ClalisForLiplisGetVoiceMp3Ondemand.GetAudioClip(sentence, modelController.GetModelCount());

        //非同期実行
        yield return Async;

        //データ取得
        sentence.VoiceData = (AudioClip)Async.Current;
    }
    #endregion

    //====================================================================
    //
    //                      トークウインドウ関連処理
    //                         
    //====================================================================
    #region ウインドウ関連処理
    /// <summary>
    /// すべてのウインドウを除去する
    /// </summary>
    public void DestroyAllWindow()
    {
        modelController.DestroyAllWindow();
    }

    /// <summary>
    /// ウインドウ作成
    /// </summary>
    /// <param name="message"></param>
    /// <param name="AllocationId"></param>
    public void CreateWindow(string message, int AllocationId)
    {
        //キャラクターデータ取得
        LiplisModel charData = modelController.GetCharacterModel(AllocationId);
        
        //おしゃべりウインドウ生成し、現在ウインドウ設置
        this.NowTalkWindow = charData.CreateWindowTalk(message);
    }



    #endregion


    //====================================================================
    //
    //                      タイトルウインドウ関連処理
    //                         
    //====================================================================
    #region タイトルウインドウ関連処理
    private const float TITLE_HEIGHT_IMG_3 = 62;
    private const float TITLE_HEIGHT_IMG_2 = 46;
    private const float TITLE_HEIGHT_IMG_1 = 30;


    /// <summary>
    /// ウインドウを作成する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    private InfoWindow CreateWindowTitle(float x, float y, float z, string message, string url)
    {
        message = message.Trim().Replace("\n", "");

        //サイズ計算
        float width = CulcWindowTitleWidth(message);

        double div = Math.Ceiling(message.Length / 21.0);

        float heightImg = TITLE_HEIGHT_IMG_1;

        if (div >= 3)
        {
            heightImg = TITLE_HEIGHT_IMG_3;
        }
        else if (div == 2)
        {
            heightImg = TITLE_HEIGHT_IMG_2;
        }
        else
        {
            heightImg = TITLE_HEIGHT_IMG_1;
        }

        //ウインドウインスタンスチェック
        InitInfoWindowInstanse();

        //インスタンティエイト
        GameObject window = Instantiate(this.InfoWindowInstanse) as GameObject;

        //ウインドウ名設定
        window.name = "TitleWindow" + WindowTitleListQ.Count;

        //位置設定
        window.transform.position = new Vector3(x, y, z);

        //サイズ変更
        RectTransform windowRect = window.GetComponent<RectTransform>();
        windowRect.sizeDelta = new Vector2(width, heightImg);

        //スケール設定
        window.transform.localScale = new Vector3(1, 1, 1);

        //ウインドウインスタンス取得
        InfoWindow imgWindow = window.GetComponent<InfoWindow>();

        //テキスト設定
        imgWindow.SetText(message);

        //親キャンバスに登録
        window.transform.SetParent(UiRenderingFront.transform, false);

        //親ウインドウ登録
        imgWindow.SetParentWindow(window);

        //生成時刻セット
        imgWindow.SetCreateTime(DateTime.Now);

        //クリックイベント
        try
        {
            //テキスト　サイズ、位置調整
            GameObject windowText = window.transform.Find("TitleTalkText").gameObject;

            //TODO CtrlTalk:CreateWindowTitleここでなぜかエラーが出る 要調査
            //windowText.GetComponent<Button>().onClick.AddListener(() => Title_Click(url));
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }


        //結果を返す
        return imgWindow;
    }


    /// <summary>
    /// タイトルクリック
    /// </summary>
    public void Title_Click(string url)
    {
        Browser.Open(url);
    }

    /// <summary>
    /// 横幅計算
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    const float MAX_WIDTH_TITLE = 300;
    private float CulcWindowTitleWidth(string message)
    {
        float width = (float)message.Length * 13.5f + 20.0f;

        if (width >= MAX_WIDTH_TITLE)
        {
            return MAX_WIDTH_TITLE;
        }
        else
        {
            return width;
        }
    }


    //ウインドウ位置定義
    private const float TITLE_POS_X = -180; //250 //130;
    private const float TITLE_POS_Y = 150; //-185;
    private const float TITLE_POS_Z = 0;

    /// <summary>
    /// タイトルウインドウを追加する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="message"></param>
    private void AddTitleWindow(string message, string url)
    {
        float x = TITLE_POS_X;
        float y = TITLE_POS_Y;
        float z = TITLE_POS_Z;

        //ウインドウ生成
        InfoWindow window = null;

        if (WindowTitleListQ.Count >= 1)
        {
            //1個前のウインドウ取得
            InfoWindow LastWindow = WindowTitleListQ.Dequeue();

            //現在ウインドウXYZ取得
            x = LastWindow.ParentWindow.transform.localPosition.x;
            y = LastWindow.ParentWindow.transform.localPosition.y;
            z = LastWindow.ParentWindow.transform.localPosition.z;

            //ウインドウ生成
            window = CreateWindowTitle(x, y, z, message, url);

            //削除
            LastWindow.CloseWindow();
        }
        else
        {
            //移動位置復活
            if (LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_X != 0 &&
               LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_Y != 0 &&
               LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_Z != 0)
            {
                x = LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_X;
                y = LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_Y;
                z = LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_Z;
            }

            //ウインドウ生成
            window = CreateWindowTitle(x, y, z, message, url);
        }

        //1個以上ならスライドする
        if (WindowTitleListQ.Count >= 1)
        {
            while (WindowTitleListQ.Count > 0)
            {
                WindowTitleListQ.Dequeue().CloseWindow();
            }
        }

        //キューに追加
        this.WindowTitleListQ.Enqueue(window);

        //現在おしゃべりウインドウ設置
        this.NowTitleWindow = window;

        //座標保存
        LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_X = x;
        LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_Y = y;
        LiplisStatus.Instance.EnvironmentInfo.TITLE_LOCATION_Z = z;
    }

    /// <summary>
    /// ウインドウ消去
    /// </summary>
    private void DelTitleWindow()
    {
        //1個以上ならスライドする
        if (WindowTitleListQ.Count >= 1)
        {
            while (WindowTitleListQ.Count > 0)
            {
                WindowTitleListQ.Dequeue().CloseWindow();
            }
        }

    }



    /// <summary>
    /// ウインドウセット
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="message"></param>
    public void SetTitleWindow()
    {
        if (this.NowLoadTopic.Title == null || this.NowLoadTopic.Title == "")
        {
            DelTitleWindow();
            DelImageWindow();

            return;
        }

        //URLリストの補完
        if (this.NowLoadTopic.ThumbnailUrl != "")
        {
            if (this.NowLoadTopic.THUMBNAIL_URL_LIST == null)
            {
                this.NowLoadTopic.THUMBNAIL_URL_LIST = new List<string>();
                this.NowLoadTopic.THUMBNAIL_URL_LIST.Add(this.NowLoadTopic.ThumbnailUrl);
            }
            else if (this.NowLoadTopic.THUMBNAIL_URL_LIST.Count == 0)
            {
                this.NowLoadTopic.THUMBNAIL_URL_LIST.Add(this.NowLoadTopic.ThumbnailUrl);
            }
        }

        //タイトルウインドウ表示
        AddTitleWindow(this.NowLoadTopic.Title, this.NowLoadTopic.Url);

        //ウインドウイメージ
        SetImage(this.NowLoadTopic.THUMBNAIL_URL_LIST);
    }


    #endregion


    //====================================================================
    //
    //                      イメージウインドウ関連処理
    //                         
    //====================================================================
    #region イメージウインドウ関連処理
    //ウインドウ位置定義
    private const float IMAGE_POS_X = 0;
    private const float IMAGE_POS_Y = 0;
    private const float IMAGE_POS_Z = -28;


    /// <summary>
    /// ウインドウを作成する
    /// </summary>
    private ImageWindow CreateWindowImage()
    {
        //ウインドウのプレハブからインスタンス生成
        InitImageWindowInstanse();

        //インスタンティエイト
        GameObject window = Instantiate(ImageWindowInstanse) as GameObject;

        //ウインドウ名設定
        window.name = "ImageWindow" + WindowImageListQ.Count;

        //位置設定
        //window.transform.position = new Vector3(LpsDefine.SCREAN_DEFAULT_WIDTH / 6, IMAGE_POS_Y, IMAGE_POS_Z);
        window.transform.position = new Vector3(-999, -999, 0);

        //スケール設定
        window.transform.localScale = new Vector3(1, 1, 1);

        //親キャンバスに登録
        window.transform.SetParent(UiRenderingBack.transform, false);

        //ウインドウ生成
        ImageWindow lpsWindow = window.GetComponent<ImageWindow>();

        //親ウインドウ登録
        lpsWindow.SetParentWindow(window);

        //生成時刻登録
        lpsWindow.SetCreateTime(DateTime.Now);

        //結果を返す
        return lpsWindow;

    }

    /// <summary>
    /// ウインドウを追加する
    /// </summary>
    private void SetImage(List<string> urlList)
    {
        if (UnityNullCheck.IsNull(this.NowImageWindow))
        {
            this.NowImageWindow = CreateWindowImage();
        }
        else
        {
            this.NowImageWindow.FaidIn();
        }

        //イメージセット
        //StartCoroutine(this.NowImageWindow.SetImage(urlList));
        this.NowImageWindow.SetImage(urlList);
    }

    /// <summary>
    /// ウインドウ非表示
    /// </summary>
    private void DelImageWindow()
    {
        if (NowImageWindow != null)
        {
            NowImageWindow.CloseWindow();
        }
    }

    #endregion


    //====================================================================
    //
    //                          イベントハンドラ
    //                         
    //====================================================================
    #region イベントハンドラ

    /// <summary>
    /// 天気クリックイベント
    /// </summary>
    public void ImgWeather_Click()
    {
        ////トピックを生成する
        //MsgTopic topic = new MsgTopic();

        ////お天気センテンスセット
        //SetWetherSentence(topic);

        ////トピックスセット
        //InsertTopToipc(topic);
    }

    /// <summary>
    /// 日付クリックイベント
    /// </summary>
    public void TxtDate_Click()
    {

        ////トピックを生成する
        //MsgTopic topic = new MsgTopic();

        ////お天気センテンスセット
        //SetAnniversarySentence(topic);

        ////トピックスセット
        //InsertTopToipc(topic);
    }

    /// <summary>
    /// 温度クリック
    /// </summary>
    public void TxtTemp_Click()
    {
        ////割り込みリストに追加する
        //AddInterruptTopic(modelController.GetTemperature());

        ////トピック送り
        //SetNextTopic();
    }

    /// <summary>
    /// 降水確率クリック
    /// </summary>
    public void TxtChanceOfRain_Click()
    {
        ////割り込みリストに追加する
        //AddInterruptTopic(modelController.GetChanceOfRain());


        ////トピック送り
        //SetNextTopic();
    }

    /// <summary>
    /// 時刻クリックイベント
    /// </summary>
    public void TxtTime_Click()
    {
        Debug.Log("TxtTime_Click");
    }

    /// <summary>
    /// ロケーションクリック
    /// </summary>
    public void TxtLocation_Click()
    {
        Debug.Log("TxtLocation_Click");
    }

    /// <summary>
    /// 次へ送るボタンクリック
    /// </summary>
    public void Btn_Next_Click()
    {
        //メイン画面がOFFのときは抜ける
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        //ペンディング設定
        LiplisStatus.Instance.EnvironmentInfo.SetPendingOff();

        //次の話題
        SetNextTopic();
    }

    /// <summary>
    /// ストップクリック
    /// </summary>
    public void Btn_Stop_Click()
    {
        //メイン画面がOFFのときは抜ける
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }


        //ペンディング設定
        LiplisStatus.Instance.EnvironmentInfo.SetPendingOn();
    }

    /// <summary>
    /// 整列クリック
    /// </summary>
    public void Btn_Alignment_Click()
    {
        //メイン画面がOFFのときは抜ける
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        //Live2Dの整列
        modelController.InitCharPosition();

        //イメージウインドウの整列
        this.NowTitleWindow.SetMoveTarget(new Vector3(TITLE_POS_X, TITLE_POS_Y, TITLE_POS_Z));
        //this.NowImageWindow.SetMoveTarget(new Vector3(-999, -999, 0));
        this.NowImageWindow.InitLocation();
    }

    #endregion




}
