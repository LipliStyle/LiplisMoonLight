//=======================================================================
//  ClassName : CtrlTalk
//  概要      : トークコントローラー
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.Com;
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.LiplisSystem.Sentece;
using Assets.Scripts.LiplisSystem.Web.Clalis.v60;
using Assets.Scripts.LiplisUi;
using Assets.Scripts.LiplisUi.TopicController;
using Assets.Scripts.Msg;
using Assets.Scripts.Utils;
using SpicyPixel.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LiplisSystem.MainSystem
{
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
        private Queue<ImageWindow> WindowImageListQ;

        ///=============================
        /// 現在処理中ウインドウインスタンス
        private TalkWindow2 NowTalkWindow;
        private ImageWindow NowImageWindow;

        ///=============================
        /// 現在ロードトピック
        private MsgTopic NowLoadTopic;
        private MsgTopic LogLoadTopic;
        private int NowSentenceCount;

        ///=============================
        /// おしゃべりウェイトカウンター
        private int TalkWaitCount = 0;

        ///=============================
        // ウインドウインスタンス
        private GameObject ImageWindowInstanse;

        ///=============================
        // レンダリングUIキャッシュ
        public GameObject CanvasRendering;

        ///=============================
        // メッセージウインドウテキスト
        public Text TextTitle;    // Titleテキスト
        public Text TextMainTalk; //メイントークウインドウテキスト
        public Text TextName;       //キャラクター名テキスト
        public Image ImgChar;       //キャラクターイメージ

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


            ScrollViewController.sc.SetNews(LiplisStatus.Instance.NewTopic.GetTopic2BaseNewsDataAllList());

            //あいさつ実行
            StartCoroutine(DelayMethod(1.5f, () =>
            {


                Greet();
            }));
        }

        /// <summary>
        /// 渡された処理を指定時間後に実行する
        /// </summary>
        /// <param name="waitTime">遅延時間[ミリ秒]</param>
        /// <param name="action">実行したい処理</param>
        /// <returns></returns>
        private IEnumerator DelayMethod(float waitTime, Action action)
        {
            yield return new WaitForSeconds(waitTime);
            action();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        void init()
        {
            if (this.WindowImageListQ == null) { WindowImageListQ = new Queue<ImageWindow>(); }
            instance = this;

            //なうセンテンスカウント 初期化
            initNowSentenceCount();

            //キャラクターイメージを非表示にしておく
            if(this.ImgChar != null)
            {
                this.ImgChar.gameObject.SetActive(false);
            }
 
            //初期化
            if(TextTitle != null)
            {
                TextTitle.text = "";
                TextMainTalk.text = "";
                TextName.text = "";
                ImgChar.sprite = null;
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
            LiplisModels.Instance.WindowMaintenance(WINDOW_LIFESPAN_TIME);

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
                if (LiplisModels.Instance.IsPlaying())
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
            List<MsgTopic> lst = LiplisModels.Instance.GetGreet();

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
                    LiplisModel cahrData = LiplisModels.Instance.TableModelId[AllocationId];

                    if (sentenceIdx == 0)
                    {
                        sentence.BaseSentence = "今日は" + sentence.BaseSentence + "みたいです～♪";
                        sentence.ToneConvert();
                        //sentence.TalkSentence = sentence.BaseSentence;
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
                    if (AllocationId > LiplisModels.Instance.GetMaxAllocationId())
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

            if (topic.TalkSentenceList.Count < 1)
            {
                return;
            }

            //最終センテンス取得
            MsgSentence lastSentence = topic.TalkSentenceList[topic.TalkSentenceList.Count - 1];

            //現在時刻取得
            DateTime dt = DateTime.Now;

            //天気コード取得
            MsgDayWether todayWether = LiplisStatus.Instance.InfoWether.GetWetherSentenceToday(dt);

            //0～12 今日 午前、午後、夜の天気
            if (dt.Hour >= 0 && dt.Hour <= 18)
            {
                //次モデル取得
                LiplisModel model = LiplisModels.Instance.GetCharacterModelNext(lastSentence.AllocationId);

                //センテンス生成
                LiplisWeather.CreateWetherMessage("今日の天気は", todayWether, topic.TalkSentenceList, model.Tone, model.AllocationId);
            }

            //19～23 明日の天気
            else if (dt.Hour >= 19 && dt.Hour <= 23)
            {
                //明日の天気も取得
                MsgDayWether tomorrowWether = LiplisStatus.Instance.InfoWether.GetWetherSentenceTommorow(dt);

                //次モデル取得
                LiplisModel model1 = LiplisModels.Instance.GetCharacterModelNext(lastSentence.AllocationId);

                //1文目追加
                LiplisWeather.CreateWetherMessage("", todayWether, topic.TalkSentenceList, model1.Tone, model1.AllocationId);

                //次モデル取得
                LiplisModel model2 = LiplisModels.Instance.GetCharacterModelNext(model1.AllocationId);

                //2文目追加
                LiplisWeather.CreateWetherMessage("明日の天気は", tomorrowWether, topic.TalkSentenceList, model2.Tone, model2.AllocationId);

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
                //音声ストップ
                LiplisModels.Instance.StopVoiceAll();


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

        /// <summary>
        /// ネクストセンテンスをセットする
        /// </summary>
        /// <param name="sentence"></param>
        private void SetNextSentence(MsgSentence sentence)
        {
            //トーンコンバート
            //sentence.ToneConvert();

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

            //メイントークウインドウに設定する
            SetMainTalkWindow(sentence);

            //音声再生
            VoiceTalk(sentence);

            //表情設定
            StartCoroutine(LiplisModels.Instance.SetExpression(sentence));

            //おしゃべりの開始
            LiplisModels.Instance.StartTalking(sentence);

            //ログに追記
            if (this.LogLoadTopic != null)
            {
                this.LogLoadTopic.TalkSentenceList.Add(sentence);
            }


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
                    LiplisModels.Instance.StartVoice(sentence.AllocationId, sentence.VoiceData);
                }
            }
        }

        /// <summary>
        /// メインウインドウを設定する
        /// </summary>
        private void SetMainTalkWindow(MsgSentence sentence)
        {
            //名前設定
            this.TextName.text = LiplisModels.Instance.GetModelName(sentence);

            //顔アイコン設定
            this.ImgChar.gameObject.SetActive(true);
            this.ImgChar.sprite = LiplisModels.Instance.GetModelCharImage(sentence);
        }

        /// <summary>
        /// 次の話題をセットする
        /// </summary>
        public void SetNextTopic()
        {
            //ウインドウを一旦クリア
            DestroyAllWindow();

            //前回の話題を繰り越す
            ScrollViewController.sc.Refresh();

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

            //ニュースリストを更新する
            CarryForwardTopic();
        }

        /// <summary>
        /// 話題設定の終了処理
        /// </summary>
        private IEnumerator SetToipcEnd()
        {
            //なうセンテンスカウントの初期化
            initNowSentenceCount();

            //キャラクター位置移動
            LiplisModels.Instance.ShuffleCharPosition(this.NowLoadTopic);

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
        private void SetToipcEndInterrupt()
        {
            //なうセンテンスカウントの初期化
            initNowSentenceCount();

            //キャラクター位置移動
            LiplisModels.Instance.ShuffleCharPosition(this.NowLoadTopic);

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
            this.NowLoadTopic = LiplisStatus.Instance.NewTopic.TopicListDequeue(LiplisModels.Instance.GetModelList());

            //ログを記録した後、あらたらなトピックを生成する
            RegisterLogAndCreateLogLoadTopic();

            //話題のセット、移動
            StartCoroutine(SetToipcEnd());
        }

        /// <summary>
        /// ログトピックを生成する
        /// </summary>
        /// <returns></returns>
        private void RegisterLogAndCreateLogLoadTopic()
        {
            //ログを登録する
            if (this.LogLoadTopic != null)
            {
                LiplisTalkLog.Instance.AddLog(this.LogLoadTopic);
            }

            //ログトピックを生成する
            this.LogLoadTopic = this.NowLoadTopic.Clone();

            //センテンスリストを初期化する
            this.LogLoadTopic.TalkSentenceList.Clear();
        }

        /// <summary>
        /// 割り込み話題リストから取得する
        /// </summary>
        private void SetTopicInterruptTopicList()
        {
            //割り込み話題があれば、そちらから取得する
            this.NowLoadTopic = LiplisStatus.Instance.NewTopic.InterruptTopicList.Dequeue();

            //割り込み次は、即ログに記録するので次回記録されないようにNULLを設定しておく
            this.LogLoadTopic = null;

            //ログ追加
            LiplisTalkLog.Instance.AddLog(this.LogLoadTopic);

            //話題のセット、移動
            SetToipcEndInterrupt();
        }


        /// <summary>
        /// ニュースリストを取得する
        /// </summary>
        private void SetTopicTalkFromNewsList()
        {
            //ニュースリストからデータキーを取得する
            SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.DequeueKeyNewsList(), ContentCategoly.news);
        }

        /// <summary>
        /// まとめリストを取得する
        /// </summary>
        private void SetTopicTalkFromSummaryList()
        {
            //ニュースリストからデータキーを取得する
            SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.DequeueKeyMatomeList(), ContentCategoly.matome);
        }

        /// <summary>
        /// リツイートリストを取得する
        /// </summary>
        private void SetTopicTalkFromReTweetList()
        {
            //ニュースリストからデータキーを取得する
            SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.DequeueKeyReTweetList(), ContentCategoly.retweet);
        }


        /// <summary>
        /// ピクチャーリストを取得する
        /// </summary>
        private void SetTopicTalkFromPictureList()
        {
            //ニュースリストからデータキーを取得する
            SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.DequeueKeyPictureList(), ContentCategoly.hotPicture);
        }

        /// <summary>
        /// ハッシュリストを取得する
        /// </summary>
        private void SetTopicTalkFromHashList()
        {
            //ニュースリストからデータキーを取得する
            SetTopicTalkFromLastNewsList(LiplisStatus.Instance.NewsList.DequeueKeyHashList(), ContentCategoly.matome);
        }

        /// <summary>
        /// 特定の話題をおしゃべり
        /// </summary>
        /// <param name="DataKey"></param>
        public void SetTopicTalkFromLastNewsList(string DataKey, ContentCategoly NewsSource)
        {
            //次の話題をロードする
            MsgTopic topic = LiplisStatus.Instance.NewTopic.SearchTopic(DataKey, LiplisModels.Instance.GetModelList());

            //Topicが取得できたら、なうろーどに入れて終了
            if (topic != null)
            {
                //対象リストを洗い替えする
                LiplisStatus.Instance.NewsList.DequeueKeyNewsList(NewsSource, DataKey);

                //カテゴリ修正
                topic.TopicClassification = ((int)NewsSource).ToString();

                //ナウロードセット
                this.NowLoadTopic = topic;

                //ログ追加
                LiplisTalkLog.Instance.AddLog(this.NowLoadTopic.Clone());

                //話題のセット、移動
                StartCoroutine(SetToipcEnd());

                return;
            }

            //Clalisサーバーから対象データ取得
            StartCoroutine(SetTopicTalkFromClalis(DataKey, NewsSource));
        }

        /// <summary>
        /// ログから指定のログの話題をおしゃべり
        /// </summary>
        /// <param name="DataKey"></param>
        /// <param name="NewsSource"></param>
        public void SetTopicTalkFromLastNewsListFromLog(string DataKey, ContentCategoly NewsSource)
        {
            //ウインドウを一旦クリア
            DestroyAllWindow();

            //おしゃべり
            SetTopicTalkFromLastNewsList(DataKey, NewsSource);
        }

        /// <summary>
        /// トピックリストから取得する
        /// </summary>
        private void SetTopicFromResLpsTopicList()
        {
            //次の話題をロードする
            this.NowLoadTopic = LiplisStatus.Instance.NewTopic.GetTopicFromResLpsTopicList(LiplisModels.Instance.GetModelList());


            if (NowLoadTopic.TalkSentenceList.Count < 2)
            {
                Debug.Log("2件以下！");
            }

            //ログ追加
            LiplisTalkLog.Instance.AddLog(this.LogLoadTopic.Clone());

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
                //対象リストを洗い替えする
                LiplisStatus.Instance.NewsList.DequeueKeyNewsList(NewsSource, DataKey);

                //話題取得
                this.NowLoadTopic = resTopic.topicList[0];

                //アロケーションID設定
                TopicUtil.SetAllocationIdAndTone(this.NowLoadTopic, LiplisModels.Instance.GetModelList());

                //カテゴリセット
                this.NowLoadTopic.TopicClassification = ((int)NewsSource).ToString();

                //おしゃべり済みに追加
                LiplisStatus.Instance.NewTopic.ChattedKeyList.AddToNotDuplicate(this.NowLoadTopic.Clone());

                //話題のセット、移動
                StartCoroutine(SetToipcEnd());

                //ログ追加
                LiplisTalkLog.Instance.AddLog(this.LogLoadTopic.Clone());
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
                this.NowLoadTopic = LiplisModels.Instance.CreateTopicFromShortNews();
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
                this.NowLoadTopic = LiplisModels.Instance.CreateTopicFromShortNews();
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
                TopicUtil.SetAllocationIdAndTone(topic, LiplisModels.Instance.GetModelList());
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
            if (LiplisModels.Instance.GetWindowQCount() > 0)
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


        /// <summary>
        /// 話題を繰り越す
        /// </summary>
        private void CarryForwardTopic()
        {
            //テーブルの更新
            CtrlGameController.instance.UpdateTbt();
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
                if (NowLoadTopic.TalkSentenceList.Count < 0)
                {
                    //0以下なら何もしない
                }
                else
                {
                    //初期データをセット
                    yield return StartCoroutine(ClalisForLiplisGetVoiceMp3Ondemand.SetVoiceDataStart(NowLoadTopic, LiplisModels.Instance.GetModelCount()));

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
                else if (sentence.AllocationId < 0 || sentence.AllocationId >= LiplisModels.Instance.GetModelCount())
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
            //sentence.ToneConvert();

            var Async = ClalisForLiplisGetVoiceMp3Ondemand.GetAudioClip(sentence, LiplisModels.Instance.GetModelCount());

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
            LiplisModels.Instance.DestroyAllWindow();
        }
        
        /// <summary>
        /// ウインドウ作成
        /// </summary>
        /// <param name="message"></param>
        /// <param name="AllocationId"></param>
        public void CreateWindow(string message, int AllocationId)
        {
            //キャラクターデータ取得
            LiplisModel charData = LiplisModels.Instance.GetCharacterModel(AllocationId);

            //おしゃべりウインドウ生成し、現在ウインドウ設置
            this.NowTalkWindow = charData.CreateWindowTalk(message, TextMainTalk);
        }



        #endregion



        //====================================================================
        //
        //                          タイトル設定関連
        //                         
        //====================================================================
        #region タイトル設定関連

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
            TextTitle.text = this.NowLoadTopic.Title;

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
            window.transform.position = new Vector3(-999, -999, 0);

            //スケール設定
            window.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            //親キャンバスに登録
            window.transform.SetParent(CanvasRendering.transform, false);

            //ウインドウ生成
            ImageWindow lpsWindow = window.GetComponent<ImageWindow>();

            //デフォルトロケーションを設定
            lpsWindow.SetLocationX(LiplisModels.Instance.ModelList.Count);

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
            //ペンディング設定
            LiplisStatus.Instance.EnvironmentInfo.SetPendingOn();
        }

        /// <summary>
        /// 整列クリック
        /// </summary>
        public void Btn_Alignment_Click()
        {
            //Live2Dの整列
            LiplisModels.Instance.InitCharPosition();

            //イメージウインドウのロケーションリセット
            this.NowImageWindow.InitLocation();
        }

        #endregion




    }
}
