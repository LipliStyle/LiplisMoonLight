//=======================================================================
//  ClassName : CtrlLog
//  概要      : ログ画面コントローラー
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Web;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CtrlLog : MonoBehaviour {
	///=============================
	// レンダリングUIキャッシュ
	public GameObject UiRenderingBack;
	public GameObject UiRenderingFront;
	public GameObject UiLog;

    ///=============================
    /// UI
    public GameObject Content;
    public GameObject ContentChild;
    public Button BtnReTalk;
    RectTransform ContentRect;
    RectTransform ContentChildRect;


    ///=============================
    ///プレファブ
    private GameObject PrefabLogNewsContent;

    private GameObject PrefabLogWindowL1;
    private GameObject PrefabLogWindowL2;
    private GameObject PrefabLogWindowL3;
    private GameObject PrefabLogWindowL4;
    private GameObject PrefabLogWindowR1;
    private GameObject PrefabLogWindowR2;
    private GameObject PrefabLogWindowR3;
    private GameObject PrefabLogWindowR4;

    ///=============================
    ///パネルリスト
    private List<GameObject> PanelList;
    private List<GameObject> PanelChildList;

    ///=============================
    ///選択キー
    private MsgTalkLog selectedLog;

    ///=============================
    ///コンテンツ
    private Texture empty;


    //====================================================================
    //
    //                           初期化処理
    //                         
    //====================================================================
    #region 初期化処理

    /// <summary>
    /// スタート処理
    /// </summary>
    void Start()
    {
        ClearChild();
    }

    /// <summary>
    /// 画面有効化時
    /// </summary>
    private void OnEnable()
    {

    }

    /// <summary>
    /// クラスの初期化
    /// </summary>
    private void InitClass()
    {
        InitPrefab();
        InitRect();
        InitPanelList();
    }

    /// <summary>
    /// プレファブを初期化する
    /// </summary>
    private void InitPrefab()
    {
        if (this.PrefabLogNewsContent == null)
        {
            this.PrefabLogNewsContent = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_NEWS);


            this.PrefabLogWindowL1 = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_L1);
            this.PrefabLogWindowL2 = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_L2);
            this.PrefabLogWindowL3 = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_L3);
            this.PrefabLogWindowL4 = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_L4);
            this.PrefabLogWindowR1 = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_R1);
            this.PrefabLogWindowR2 = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_R2);
            this.PrefabLogWindowR3 = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_R3);
            this.PrefabLogWindowR4 = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_R4);
        }
    }

    /// <summary>
    /// レクトを初期化する
    /// </summary>
    private void InitRect()
    {
        if (this.ContentRect == null)
        {
            this.ContentRect = Content.GetComponent<RectTransform>();
            this.ContentChildRect = ContentChild.GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// パネルリストを初期化する
    /// </summary>
    private void InitPanelList()
    {
        if (this.PanelList == null)
        {
            this.PanelList = new List<GameObject>();
            this.PanelChildList = new List<GameObject>();
        }
    }

    /// <summary>
    /// 子要素のクリア
    /// </summary>
    public void Clear()
    {
        foreach (Transform n in Content.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }

    public void ClearChild()
    {
        //この話題おしゃべり非表示
        BtnReTalk.gameObject.SetActive(false);

        foreach (Transform n in ContentChild.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }

    /// <summary>
    /// エンプティを生成する
    /// </summary>
    public void CreateEmpty()
    {
        if (empty == null)
        {
            empty = new Texture();
        }
    }

    #endregion

    //====================================================================
    //
    //                           イベントハンドラ
    //                         
    //====================================================================
    #region イベントハンドラ

    /// <summary>
    /// クリックイベント
    /// </summary>
    public void OpneDescription(MsgTalkLog log, GameObject panel)
    {
        //色替え
        panel.GetComponent<Image>().color = Color.blue;

        //現在選択中ログ取得
        this.selectedLog = log;

        //会話ログ表示
        StartCoroutine(OpenDescription(log.TalkSentenceList));
    }
    /// <summary>
    /// ブラウザを開くボタン押下
    /// </summary>
    /// <param name="url"></param>
    public void OpneWeb(string url)
    {
        Browser.Open(url);
    }


    /// <summary>
    /// この話題をもう一度おしゃべり
    /// </summary>
    public void Btn_BtnReTalk_Click()
    {
        //おしゃべり
        if (this.selectedLog != null)
        {
            //閉じるボタン実行
            Btn_Log_Close_Click();

            //おしゃべり実行
            CtrlTalk.Instance.SetTopicTalkFromLastNewsList(selectedLog.DATAKEY, ContentCategolyText.GetContentCategoly(selectedLog.DATA_TYPE));
        }
    }


    /// <summary>
    /// 設定クリック
    /// </summary>
    public void Btn_Log_Click()
    {
        //フロント画面が非アクティブのときは何もしない
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        //ペンディング設定
        LiplisStatus.Instance.EnvironmentInfo.SetPendingOn();

        UiRenderingBack.gameObject.SetActive(false);
        UiRenderingFront.gameObject.SetActive(false);
        UiLog.gameObject.SetActive(true);
    }

    /// <summary>
    /// 設定を閉じる
    /// </summary>
    public void Btn_Log_Close_Click()
    {
        //ペンディング設定ON
        LiplisStatus.Instance.EnvironmentInfo.SetPendingOff();

        UiRenderingBack.gameObject.SetActive(true);
        UiRenderingFront.gameObject.SetActive(true);
        UiLog.gameObject.SetActive(false);

    }

    #endregion
    //====================================================================
    //
    //                           コンテント操作
    //                         
    //====================================================================
    #region コンテント操作
    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// ログを追加する
    /// </summary>
    /// <param name="topic"></param>
    /// <returns></returns>
    public IEnumerator AddLog(MsgTopic topic)
    {
        //クラスの初期化チェック
        InitClass();

        //ログメッセージ生成
        MsgTalkLog log = new MsgTalkLog();

        //データ生成
        log.CREATE_TIME = topic.CreateTime;
        log.DATA_TYPE = topic.TopicClassification;
        log.DATAKEY = topic.DataKey;
        log.TITLE = topic.Title;
        log.URL = topic.Url;
        log.THUMBNAIL_URL = topic.ThumbnailUrl;
        log.PANEL_KEY = log.DATAKEY + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        log.TalkSentenceList = topic.TalkSentenceList;

        if (log.TalkSentenceList.Count < 1)
        {
            Debug.Log("明細0件！");
        }

        //スクロール
        MoveAndClean();

        yield return null;

        //UI生成
        IEnumerator Async = CreateLogUi(log);

        //待ち
        yield return Async;

        //パネル生成
        GameObject panel = (GameObject)Async.Current;

        //親パネルにセット
        panel.transform.SetParent(Content.transform, false);

        //パネルリストに追加
        this.PanelList.Add(panel);

    }

    /// <summary>
    /// UIを生成する
    /// </summary>
    /// <param name="log"></param>
    /// <returns></returns>
    public IEnumerator CreateLogUi(MsgTalkLog log)
    {
        //ウインドウのプレハブからインスタンス生成
        GameObject panel = Instantiate(this.PrefabLogNewsContent) as GameObject;

        //ウインドウ名設定
        panel.name = "LiplisLog" + log.DATAKEY;

        //要素取得
        Text TxtTitle = panel.transform.Find("TxtTitle").GetComponent<Text>();
        Text TxtCat = panel.transform.Find("TxtCat").GetComponent<Text>();
        Text TxtTime = panel.transform.Find("TxtTime").GetComponent<Text>();
        Text TxtDataKey = panel.transform.Find("TxtDataKey").GetComponent<Text>();
        Text TxtPanelKey = panel.transform.Find("TxtPanelKey").GetComponent<Text>();
        Button BtnWeb = panel.transform.Find("BtnWeb").GetComponent<Button>();
        RawImage ImgThumbnail = panel.transform.Find("ImgThumbnail").GetComponent<RawImage>();

        //内容設定
        TxtTitle.text = log.TITLE;
        TxtCat.text = ContentCategolyText.GetContentText(log.DATA_TYPE);
        TxtTime.text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        TxtDataKey.text = log.DATAKEY;
        TxtPanelKey.text = log.PANEL_KEY + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


        //ポインターダウンイベントトリガー生成
        RegisterEventOpenDescription(log, ImgThumbnail.gameObject, panel);
        BtnWeb.GetComponent<Button>().onClick.AddListener(() => OpneWeb(log.URL));

        //イメージのロード
        CreateEmpty();
        GlobalCoroutine.GoWWW(ImgThumbnail, empty, log.THUMBNAIL_URL);

        yield return panel;
    }

    /// <summary>
    /// イベント登録
    /// </summary>
    /// <param name="log"></param>
    /// <param name="imageObject"></param>
    public void RegisterEventOpenDescription(MsgTalkLog log, GameObject imageObject, GameObject panel)
    {
        EventTrigger trigger = imageObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { OpneDescription(log, panel); });
        trigger.triggers.Add(entry);
    }


    /// <summary>
    /// 移動とクリーン
    /// </summary>
    private void MoveAndClean()
    {
        //100個以下になるまでデストロイする
        while (PanelList.Count > 9)
        {
            //パネル取得
            GameObject panel = PanelList.Dequeue();

            //パネルキー取得
            Text TxtPanelKey = panel.transform.Find("TxtPanelKey").GetComponent<Text>();

            //パネルキーが一致したら、子を初期化する
            if (selectedLog != null)
            {
                if (selectedLog.PANEL_KEY == TxtPanelKey.text)
                {
                    ClearChild();
                }
            }

            //子オブジェクトをすべて削除
            foreach (Transform child in panel.transform)
            {
                Destroy(child.gameObject);
            }

            //パネルを削除
            Destroy(panel);
        }

        int idx = 1;



        //移動
        foreach (var panel in PanelList)
        {
            Vector3 txtPos = panel.transform.position;
            panel.transform.position = new Vector3(txtPos.x, txtPos.y - 90, txtPos.z);

            idx++;
        }

        //サイズ変更
        ContentRect.sizeDelta = new Vector2(ContentRect.sizeDelta.x, 600);
    }

    /// <summary>
    /// サムネイルを更新する
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateThumbnail(RawImage ImgThumbnail, string thumbnailUrl)
    {
        //登録されていない場合は、ダウンロード

        using (WWW www = new WWW(ThumbnailUrl.CreateListThumbnailUrl(thumbnailUrl)))
        {
            // 画像ダウンロード完了を待機
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                CreateEmpty();

                ImgThumbnail.texture = empty;
            }

            yield return new WaitForSeconds(0.5f);

            try
            {


                ImgThumbnail.texture = www.texture;
            }
            catch (Exception ex)
            {
                CreateEmpty();

                ImgThumbnail.texture = empty;
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    /// <summary>
    /// ディスクリプションを開く
    /// </summary>
    /// <param name="topic"></param>
    /// <returns></returns>
    public IEnumerator OpenDescription(List<MsgSentence> SentenceList)
    {
        //一旦クリアする
        CleanPanelChildList();
        ClearChild();

        //管理ID
        int prvAllocationId = -1;
        bool rlBit = true;
        int idx = 0;

        //センテンスリストを回し、チャットを生成する
        foreach (MsgSentence sentence in SentenceList)
        {
            CreateLogTalkUi(sentence, rlBit, idx);

            //反転
            if (prvAllocationId != sentence.AllocationId)
            {
                rlBit = !rlBit;
            }

            //アロケーションID取得
            prvAllocationId = sentence.AllocationId;

            //インデックスインクリメント
            idx++;
        }

        //高さ設定
        ContentChildRect.sizeDelta = new Vector2(ContentRect.sizeDelta.x, (idx) * 40);

        //この話題おしゃべり表示
        BtnReTalk.gameObject.SetActive(true);

        //終了
        yield return null;
    }

    /// <summary>
    /// クリーン
    /// </summary>
    private void CleanPanelChildList()
    {
        //100個以下になるまでデストロイする
        while (PanelChildList.Count > 0)
        {
            //パネル取得
            GameObject panel = PanelChildList.Dequeue();

            //子オブジェクトをすべて削除
            foreach (Transform child in panel.transform)
            {
                Destroy(child.gameObject);
            }

            //パネルを削除
            Destroy(panel);
        }

        //クリア
        PanelChildList.Clear();
    }


    /// <summary>
    /// トークUIを生成する
    /// </summary>
    /// <param name="sentence"></param>
    /// <param name="rlBit"></param>
    /// <param name="idx"></param>
    public void CreateLogTalkUi(MsgSentence sentence, bool rlBit, int idx)
    {
        //ウインドウのプレハブからインスタンス生成
        GameObject panel = CreateCharPanel(sentence.AllocationId, rlBit);

        //ウインドウ名設定
        panel.name = "LiplisTalkLog" + idx;

        //要素取得
        Text TxtMessage = panel.transform.Find("ImgText").GetComponent<Image>().transform.Find("Text").GetComponent<Text>();

        //内容設定
        TxtMessage.text = sentence.TalkSentence;

        Vector3 txtPos = panel.transform.position;
        panel.transform.position = new Vector3(txtPos.x, -idx * 40, txtPos.z);

        //親パネルにセット
        PanelChildList.Add(panel);
        panel.transform.SetParent(ContentChild.transform, false);
    }

    /// <summary>
    /// キャラクターのパネルを生成する
    /// </summary>
    /// <param name="AllocationId"></param>
    /// <param name="rlBit"></param>
    /// <returns></returns>
    private GameObject CreateCharPanel(int AllocationId, bool rlBit)
    {
        if (AllocationId == 0)//葉月
        {
            if (rlBit)
            {
                return Instantiate(this.PrefabLogWindowL1) as GameObject;
            }
            else
            {
                return Instantiate(this.PrefabLogWindowR1) as GameObject;
            }
        }
        else if (AllocationId == 1)//白葉
        {
            if (rlBit)
            {
                return Instantiate(this.PrefabLogWindowL2) as GameObject;
            }
            else
            {
                return Instantiate(this.PrefabLogWindowR2) as GameObject;
            }
        }
        else if (AllocationId == 2)//黒葉
        {
            if (rlBit)
            {
                return Instantiate(this.PrefabLogWindowL3) as GameObject;
            }
            else
            {
                return Instantiate(this.PrefabLogWindowR3) as GameObject;
            }
        }
        else if (AllocationId == 3)//桃葉
        {
            if (rlBit)
            {
                return Instantiate(this.PrefabLogWindowL4) as GameObject;
            }
            else
            {
                return Instantiate(this.PrefabLogWindowR4) as GameObject;
            }
        }
        else
        {
            return null;
        }




    }
    #endregion

}
