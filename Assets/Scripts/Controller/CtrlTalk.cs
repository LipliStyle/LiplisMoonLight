//=======================================================================
//  ClassName : CtrlTalk
//  概要      : トークコントローラー
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.DataChar;
using Assets.Scripts.DataChar.CharacterTalk;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.UI;
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


	///=============================
	/// ウインドウ保持時間
	private const int WINDOW_LIFESPAN_TIME = 15;
	private const int TALK_WAIT_DEFAULT = 7;
	private const int TALK_WAIT_NEUTRAL_DEFAULT = 5;
	private const int TALK_WAIT_NEUTRAL_IDLE_DEFAULT = 2;

	[SerializeField] Image TxtTitle;
	[SerializeField] Image TopicImage;

	///=============================
	/// ウインドウインスタンス
	Queue<LiplisTitleWindow> WindowTitleListQ;
	Queue<LiplisImageWindow> WindowImageListQ;

	///=============================
	/// 現在処理中ウインドウインスタンス
	private LiplisWindow NowTalkWindow;
	private LiplisTitleWindow NowTitleWindow;
	private LiplisImageWindow NowImageWindow;

	///=============================
	/// 現在ロードトピック
	public MsgTopic NowLoadTopic;

	///=============================
	/// おしゃべりウェイトカウンター
	public int TalkWaitCount = 0;

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
	void Start () {
		//ベースアウェーク(初期化)
		base.Awake();

		//初期化処理
		init();

		//タイマースタート
		startTimer();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	void init()
	{
		if (this.WindowTitleListQ == null) { WindowTitleListQ = new Queue<LiplisTitleWindow>(); }
		if (this.WindowImageListQ == null) { WindowImageListQ = new Queue<LiplisImageWindow>(); }
		instance                   = this;
	}

	/// <summary>
	/// タイマースタート
	/// </summary>
	void startTimer()
	{
		StartCoroutine(WindowMaintenanceTimerTick());
	}


	// Update is called once per frame
	void Update () {

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
	IEnumerator WindowMaintenanceTimerTick()
	{
		while (true)
		{
			//データ収集処理
			WindowMaintenance();

			//おしゃべり待ち
			TalkWating();

			//件数が5件以下ならプッシュする
			if(LiplisStatus.Instance.NewTopic.TalkTopicList.Count < 5)
			{
				StartCoroutine(SetTopicDirectTopicAsync());
			}

			//非同期待機
			yield return new WaitForSeconds(1.0f);
		}
	}


	/// <summary>
	/// ウインドウメンテナンス
	/// </summary>
	private void WindowMaintenance()
	{
		LiplisStatus.Instance.CharDataList.WindowMaintenance(WINDOW_LIFESPAN_TIME);
	}

	/// <summary>
	/// おしゃべり待ち
	/// </summary>
	private void TalkWating()
	{
		try
		{

			if (!UnityNullCheck.IsNull(NowTalkWindow))
			{
				if (!NowTalkWindow.imgWindow.FlgTalking)
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

			//ニュートラル戻しタイムアウト
			if (TalkWaitCount == TALK_WAIT_NEUTRAL_DEFAULT)
			{
				OnNeutralAll();
			}
			if (TalkWaitCount == TALK_WAIT_NEUTRAL_IDLE_DEFAULT)
			{
				OnIdleAll();
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
		//挨拶トピックリストを生成する
		//AddInterruptTopic(LiplisStatus.Instance.CharDataList.GetGreet());
		this.NowLoadTopic = CreateGreet();

		//センテンスをセットする
		SetNextSentence();
	}
	public MsgTopic CreateGreet()
	{
		//トピックを生成する
		MsgTopic topic = new MsgTopic();

		//各キャラクターの挨拶を取得する
		List<MsgTopic> lst = LiplisStatus.Instance.CharDataList.GetGreet();

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
				CharacterData cahrData = LiplisStatus.Instance.CharDataList.CharIdList[AllocationId];

				if (sentenceIdx == 0)
				{
					sentence.BaseSentence = "今日は" + sentence.BaseSentence + "みたいです～♪";
					sentence.TalkSentence = sentence.BaseSentence;
				}
				else
				{
					sentence.ToneConvert(cahrData.Tone);
				}

				//アロケーションID設定
				sentence.AllocationId = AllocationId;

				//インデックスインクリメント
				sentenceIdx++;
				AllocationId++;

				//アロケーションIDコントロール
				if(AllocationId > 3)
				{
					AllocationId = 0;
				}

				//センテンスを追加
				topic.TalkSentenceList.Add(sentence);
			}
		}
	}

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
		CharacterData cahrData1;
		CharacterData cahrData2;
		CharacterData cahrData3;
		int AllocationId1;
		int AllocationId2;
		int AllocationId3;

		//アロケーションID取得
		AllocationId1 = lastSentence.AllocationId + 1;
		if (AllocationId1 > 3){ AllocationId1 = 0;}
		AllocationId2 = AllocationId1 + 1;
		if (AllocationId2 > 3) { AllocationId2 = 0; }
		AllocationId3 = AllocationId2 + 1;
		if (AllocationId3 > 3) { AllocationId3 = 0; }

		//キャラクターデータ取得
		cahrData1 = LiplisStatus.Instance.CharDataList.CharIdList[AllocationId1];
		cahrData2 = LiplisStatus.Instance.CharDataList.CharIdList[AllocationId2];
		cahrData3 = LiplisStatus.Instance.CharDataList.CharIdList[AllocationId3];


		//現在時刻取得
		DateTime dt = DateTime.Now;

		//天気コード取得
		MsgDayWether todayWether = LiplisStatus.Instance.InfoWether.GetWetherSentenceToday(dt);

		//0～12 今日 午前、午後、夜の天気
		if (dt.Hour >= 0 && dt.Hour <= 18)
		{
			SentenceCreatorWether.CreateWetherMessage("今日の天気は", todayWether, topic.TalkSentenceList, cahrData1.Tone, cahrData1.AllocationId);
		}

		//19～23 明日の天気
		else if (dt.Hour >= 19 && dt.Hour <= 23)
		{
			//明日の天気も取得
			MsgDayWether tomorrowWether = LiplisStatus.Instance.InfoWether.GetWetherSentenceTommorow(dt);

			SentenceCreatorWether.CreateWetherMessage("", todayWether, topic.TalkSentenceList, cahrData2.Tone, cahrData2.AllocationId);
			SentenceCreatorWether.CreateWetherMessage("明日の天気は", tomorrowWether, topic.TalkSentenceList, cahrData3.Tone, cahrData3.AllocationId);

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
	/// ニュートラル戻し
	/// </summary>
	private void OnNeutralAll()
	{
		//全登録モデルをニュートラルに戻す
		LiplisStatus.Instance.CharDataList.NeutralAll();
	}
	private void OnIdleAll()
	{
		//全登録モデルをニュートラルに戻す
		LiplisStatus.Instance.CharDataList.NeutralAll();
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

		//表情設定
		LiplisStatus.Instance.CharDataList.SetExpression(sentence);

		Debug.Log(sentence.Emotion + " : " + sentence.Point);
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
		else
		{
			//蓄積された話題が無い場合はショートニュース取得
			SetTopicDirect();
		}

		//キャラクター位置移動
		LiplisStatus.Instance.CharDataList.ShuffleCharPosition(this.NowLoadTopic);

		//タイトルウインドウをセットする
		SetTitleWindow(this.NowLoadTopic.Title, this.NowLoadTopic.ThumbnailUrl, this.NowLoadTopic.Url);

		//センテンスをセットする
		SetNextSentence();
	}



	/// <summary>
	/// 話題をセットする
	/// </summary>
	private void SetToipc()
	{
		if (LiplisStatus.Instance.NewTopic.InterruptTopicList.Count < 1)
		{
			//次の話題をロードする
			this.NowLoadTopic = LiplisStatus.Instance.NewTopic.TalkTopicList.Dequeue();

			//現在ロード中の話題をおしゃべり済みに入れる
			if(!this.NowLoadTopic.FlgNotAddChatted)
			{
				LiplisStatus.Instance.NewTopic.ChattedKeyList.Add(this.NowLoadTopic);
			}
		}
		else
		{
			//割り込み話題があれば、そちらから取得する
			this.NowLoadTopic = LiplisStatus.Instance.NewTopic.InterruptTopicList.Dequeue();
		}
	}
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
		//トピックをセット
		this.NowLoadTopic = SetTopicDirectTopic();
	}

	/// <summary>
	/// ショートニュースからトピックを生成する
	/// </summary>
	/// <returns></returns>
	private MsgTopic SetTopicDirectTopic()
	{
		try
		{
			//トピックをランダムで取得する
			ResLpsTopicList resTopic = ClalisForLiplisGetNewTopicOne.GetNewTopic(LiplisStatus.Instance.NewTopic.ToneUrlList);

			//NULLチェック
			if (resTopic == null || resTopic.topicList.Count < 0)
			{
				return LiplisStatus.Instance.CharDataList.CreateTopicFromShortNews();
			}

			//取得したトピックを返す
			return resTopic.topicList[0];
		}
		catch
		{
			return LiplisStatus.Instance.CharDataList.CreateTopicFromShortNews();
		}
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
		if (data           == null) { yield break; }
		if (data.topicList == null) { yield break; }

		//おしゃべり済みに追加しないでおしゃべり
		foreach (var topic in data.topicList)
		{
			topic.FlgNotAddChatted = true;

			//アロケーションIDを設定する
			TopicUtil.SetAllocationId(topic);
		}

		//データ追加
		LiplisStatus.Instance.NewTopic.TalkTopicList.AddRange(data.topicList);
	}


	/// <summary>
	/// 次のおしゃべり、おしゃべり中であればスキップする
	/// </summary>
	public void NextTalkOrSkip()
	{
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
		if (LiplisStatus.Instance.CharDataList.GetWindowQCount() > 0)
		{
			//ウインドウがあれば、先頭ウインドウのおしゃべり中フラグを調べる
			return this.NowTalkWindow.imgWindow.FlgTalking;
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
		this.NowTalkWindow.imgWindow.Skip();
	}
	#endregion

	//====================================================================
	//
	//                      トークウインドウ関連処理
	//                         
	//====================================================================
	#region ウインドウ関連処理

	/// <summary>
	/// ウインドウインフォを作成する
	/// </summary>
	/// <param name="targetModel"></param>
	/// <returns></returns>
	private MsgWindowInfo GetWindowInfo(string targetModel)
	{
		MsgWindowInfo result = new MsgWindowInfo();

		//ウインドウのプレハブからインスタンス生成
		result.windowInstances = (GameObject)Resources.Load(LiplisStatus.Instance.CharDataList.GetWindowName(targetModel));

		//インスタンティエイト
		result.window = Instantiate(result.windowInstances) as GameObject;

		//親キャンバス取得
		result. canvasParent = transform.Find("CanvasFront");

		//テキストオブジェクト取得
		result.windowText = result.window.transform.Find("TxtTalkText").gameObject;

		return result;
	}

	/// <summary>
	/// すべてのウインドウを除去する
	/// </summary>
	public void DestroyAllWindow()
	{
		LiplisStatus.Instance.CharDataList.DestroyAllWindow();
	}

	/// <summary>
	/// ウインドウ作成
	/// </summary>
	/// <param name="message"></param>
	/// <param name="AllocationId"></param>
	public void CreateWindow(string message, int AllocationId)
	{
		//キャラクターデータ取得
		CharacterData charData = LiplisStatus.Instance.CharDataList.GetCharacter(AllocationId);

		//向き変更
		charData.ChengeDirectionRandam();

		//ウインドウ
		LiplisWindow window  = charData.CreateWindowTalk(message, GetWindowInfo(charData.ModelName));

		if(window == null)
		{
			return;
		}

		//現在おしゃべりウインドウ設置
		this.NowTalkWindow = window;

		//ウインドウセット
		charData.SetWindow(window, AllocationId);
	}



	#endregion


	//====================================================================
	//
	//                      タイトルウインドウ関連処理
	//                         
	//====================================================================
	#region タイトルウインドウ関連処理
	private const float TITLE_HEIGHT_IMG_3 = 60;
	private const float TITLE_HEIGHT_TXT_3 = 50;
	private const float TITLE_POS_Y_TXT_3 = 7;
						
	private const float TITLE_HEIGHT_IMG_2 = 45;
	private const float TITLE_HEIGHT_TXT_2 = 35;
	private const float TITLE_POS_Y_TXT_2 = 7;
						
	private const float TITLE_HEIGHT_IMG_1 = 30;
	private const float TITLE_HEIGHT_TXT_1 = 20;
	private const float TITLE_POS_Y_TXT_1 = 7;


	/// <summary>
	/// ウインドウを作成する
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	private LiplisTitleWindow CreateWindowTitle(float x, float y, float z, string message, string thumbnailUrl, string url)
	{
		message = message.Trim().Replace("\n", "");

		//サイズ計算
		float width = CulcWindowTitleWidth(message);

		double div = Math.Ceiling(message.Length / 21.0);

		float heightImg = TITLE_HEIGHT_IMG_1;
		float heightText = TITLE_HEIGHT_TXT_1;
		float posTextY = TITLE_POS_Y_TXT_1;

		if (div >= 3)
		{
			heightImg = TITLE_HEIGHT_IMG_3;
			heightText = TITLE_HEIGHT_TXT_3;
			posTextY = TITLE_POS_Y_TXT_3;
		}
		else if (div == 2)
		{
			heightImg = TITLE_HEIGHT_IMG_2;
			heightText = TITLE_HEIGHT_TXT_2;
			posTextY = TITLE_POS_Y_TXT_2;
		}
		else
		{
			heightImg = TITLE_HEIGHT_IMG_1;
			heightText = TITLE_HEIGHT_TXT_1;
			posTextY = TITLE_POS_Y_TXT_1;
		}

		//ウインドウのプレハブからインスタンス生成
		GameObject windowInstances = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_INFO);

		//インスタンティエイト
		GameObject window = Instantiate(windowInstances) as GameObject;

		//ウインドウ名設定
		window.name = "TitleWindow" + WindowTitleListQ.Count;

		//位置設定
		window.transform.position = new Vector3(x, y, z);

		//サイズ変更
		RectTransform windowRect = window.GetComponent<RectTransform>();
		windowRect.sizeDelta = new Vector2(width, heightImg);

		//テキスト　サイズ、位置調整
		GameObject windowText = window.transform.Find("TitleTalkText").gameObject;
		Vector3 txtPos = windowText.transform.position;
		windowText.transform.position = new Vector3(txtPos.x, txtPos.y + posTextY, txtPos.z);
		RectTransform textRect = windowText.GetComponent<RectTransform>();
		textRect.sizeDelta = new Vector2(width, (float)(heightText));

		//スケール設定
		window.transform.localScale = new Vector3(1, 1, 1);

		//親キャンバス取得
		Transform canvasParent = transform.Find("CanvasFront");

		//ウインドウインスタンス取得
		InfoWindow imgWindow = window.GetComponent<InfoWindow>();

		//テキスト設定
		imgWindow.SetText(message);

		//親キャンバスに登録
		window.transform.SetParent(canvasParent, false);

		//クリックイベント
		try
		{
			window.GetComponent<Button>().onClick.AddListener(() => Title_Click(url));
		}
		catch(Exception ex)
		{
			Debug.Log(ex.Message);
		}
		

		//結果を返す
		return new LiplisTitleWindow(window, heightImg, heightText, posTextY);
	}
	
	/// <summary>
	/// タイトルクリック
	/// </summary>
	private void Title_Click(string url)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
			options.displayURLAsPageTitle = false;
			options.pageTitle = LpsDefine.APPLICATION_TITLE;

			InAppBrowser.OpenURL(url, options);
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer )
		{
			Application.OpenURL(url);
		}


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


	/// <summary>
	/// タイトルウインドウを追加する
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <param name="message"></param>
	private void AddTitleWindow(float x, float y, float z, string message, string thumbnailUrl, string url)
	{
		//ウインドウ生成
		LiplisTitleWindow window = CreateWindowTitle(x, y, z, message,thumbnailUrl, url);
		 
		//1個以上ならスライドする
		if (WindowTitleListQ.Count >= 1)
		{
			while(WindowTitleListQ.Count > 0)
			{
				WindowTitleListQ.Dequeue().CloseWindow();
			}
		}

		//キューに追加
		this.WindowTitleListQ.Enqueue(window);

		//現在おしゃべりウインドウ設置
		this.NowTitleWindow = window;
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

    //ウインドウ位置定義
    private const float TITLE_POS_X = 250; //130;
    private const float TITLE_POS_Y = 150; //-185;
	private const float TITLE_POS_Z = 0;

	/// <summary>
	/// ウインドウセット
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <param name="message"></param>
	public void SetTitleWindow(string title, string thumbnailUrl, string url)
	{
		if (title == null || title == "")
		{
			DelTitleWindow();
			DelImageWindow();

			return;
		}

		//タイトルウインドウ表示
		AddTitleWindow(TITLE_POS_X, TITLE_POS_Y, TITLE_POS_Z, title,thumbnailUrl,url);

		//ウインドウイメージ
		SetImage(thumbnailUrl);
	}

    #endregion

    //====================================================================
    //
    //                      イメージウインドウ関連処理
    //                         
    //====================================================================


    //ウインドウ位置定義
    private const float IMAGE_POS_X = 0;
    private const float IMAGE_POS_Y = 0;
    private const float IMAGE_POS_Z = 50;

    #region イメージウインドウ関連処理
    /// <summary>
    /// ウインドウを作成する
    /// </summary>
    private LiplisImageWindow CreateWindowImage(string url)
	{
		//ウインドウのプレハブからインスタンス生成
		GameObject windowInstances = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_IMAGE);

		//インスタンティエイト
		GameObject window = Instantiate(windowInstances) as GameObject;

		//ウインドウ名設定
		window.name = "ImageWindow" + WindowImageListQ.Count;

		//ウインドウ生成
		LiplisImageWindow lpsWindow = new LiplisImageWindow(window, url);

		//位置設定
		window.transform.position = new Vector3(Screen.width/6, IMAGE_POS_Y, IMAGE_POS_Z);

		//スケール設定
		window.transform.localScale = new Vector3(1, 1, 1);

		//親キャンバス取得
		Transform canvasParent = transform.Find("CanvasBackGround");

		//親キャンバスに登録
		window.transform.SetParent(canvasParent, false);

		//結果を返す
		return lpsWindow;

	}

	/// <summary>
	/// ウインドウを追加する
	/// </summary>
	private void SetImage(string url)
	{
		if(UnityNullCheck.IsNull(this.NowImageWindow))
		{
			this.NowImageWindow = CreateWindowImage(url);
		}
		else
		{
			this.NowImageWindow.FaidIn();
			this.NowImageWindow.SetImage(url);
		}
	}
	
	/// <summary>
	/// ウインドウ非表示
	/// </summary>
	private void DelImageWindow()
	{
		if(NowImageWindow != null)
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
		//AddInterruptTopic(LiplisStatus.Instance.CharDataList.GetTemperature());

		////トピック送り
		//SetNextTopic();
	}

	/// <summary>
	/// 降水確率クリック
	/// </summary>
	public void TxtChanceOfRain_Click()
	{
		////割り込みリストに追加する
		//AddInterruptTopic(LiplisStatus.Instance.CharDataList.GetChanceOfRain());


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
		//
		SetNextTopic();

	}

	#endregion




}
