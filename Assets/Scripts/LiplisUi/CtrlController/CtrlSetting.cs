//=======================================================================
//  ClassName : CtrlSetting
//  概要      : 設定画面コントローラー
//
//　新規設定追加手順
//　　1. 画面に設定UIを追加する。
//　　2. CtrlSetting(本クラス)に設定値のプロパティを追加する
//　　3. UnityのIDE上で、
//　　　 CanvasSettingにアタッチされている、CtrlSettingの追加したパラメータに、
//       対象UIをドラッグドロップする。
//    4. DatSettingにUIに対応するプロパティを追加する。
//　　　 必要であれば、ゲッターも作成(値を読み替える必要がある場合など)
//　　5. DatSettingのコンストラクターに初期化処理を追加する。
//　　6. CtrlSetting.InitWindowに設定処理を追加する。　 
//　　7. CtrlSetting.SaveSettingに設定処理を追加する。
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.LiplisSystem.Com;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LiplisUi
{
    class CtrlSetting : MonoBehaviour
    {
        ///=============================
        // レンダリングUIキャッシュ
        public GameObject UiSetting;

        ///=============================
        // 設定値
        public Toggle ToggleTopicNews;
        public Toggle ToggleTopicSummary;
        public Toggle ToggleTopicRetweet;
        public Toggle ToggleTopicHash;
        public Toggle ToggleTalkVoice;
        public Toggle ToggleDebug;
        public Slider TopicSpeed;
        public Dropdown TopicNumDrop;
        public Text TopicTextNews;
        public Dropdown CboGraphicLevel;
        public Dropdown CboDrawingFps;
        public Dropdown CboSpeechBallonNum;
        public Dropdown CboArrangement;

        /// <summary>
        /// 開始時
        /// </summary>
        void Start()
        {
            initWindow();
        }

        /// <summary>
        /// ウインドウの初期化
        /// </summary>
        private void initWindow()
        {
            //デバッグモードボタン
#if UNITY_EDITOR
            this.TopicTextNews.gameObject.SetActive(true);
#endif
        }

        /// <summary>
        /// 更新時
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// 設定画面オープン時
        /// </summary>
        private void OnEnable()
        {
            //設定反映
            InitWindow();
        }

        /// <summary>
        /// 設定クリック
        /// </summary>
        public void Btn_Setting_Click()
        {
            //設定反映
            InitWindow();

            //ペンディング設定
            LiplisStatus.Instance.EnvironmentInfo.SetPendingOn();

            UiSetting.gameObject.SetActive(true);
        }

        /// <summary>
        /// 設定を閉じる
        /// </summary>
        public void Btn_Setting_Close_Click()
        {
            //設定保存
            StartCoroutine(SaveSetting());

            //ペンディング設定ON
            LiplisStatus.Instance.EnvironmentInfo.SetPendingOff();

            UiSetting.gameObject.SetActive(false);
        }

        /// <summary>
        /// ウインドウを初期化する
        /// </summary>
        private void InitWindow()
        {
            ToggleTopicNews.isOn = LiplisSetting.Instance.Setting.FlgTopicNews;
            ToggleTopicSummary.isOn = LiplisSetting.Instance.Setting.FlgTopicSummary;
            ToggleTopicRetweet.isOn = LiplisSetting.Instance.Setting.FlgTopicRetweet;
            ToggleTopicHash.isOn = LiplisSetting.Instance.Setting.FlgTopicHash;
            ToggleTalkVoice.isOn = LiplisSetting.Instance.Setting.FlgVoice;
            ToggleDebug.isOn = LiplisSetting.Instance.Setting.FlgDebug;

            TopicSpeed.value = LiplisSetting.Instance.Setting.TalkSpeed;
            TopicNumDrop.value = LiplisSetting.Instance.Setting.TalkNum;

            CboGraphicLevel.value = LiplisSetting.Instance.Setting.GraphicLevel;

            CboDrawingFps.value = LiplisSetting.Instance.Setting.DrawingFps;
            CboSpeechBallonNum.value = LiplisSetting.Instance.Setting.SpeechBallonNum;

            CboArrangement.value = LiplisSetting.Instance.Setting.CharArrangement;
        }

        /// <summary>
        /// 設定保存
        /// </summary>
        private IEnumerator SaveSetting()
        {
            LiplisSetting.Instance.Setting.FlgTopicNews = ToggleTopicNews.isOn;
            LiplisSetting.Instance.Setting.FlgTopicSummary = ToggleTopicSummary.isOn;
            LiplisSetting.Instance.Setting.FlgTopicRetweet = ToggleTopicRetweet.isOn;
            LiplisSetting.Instance.Setting.FlgTopicHash = ToggleTopicHash.isOn;
            LiplisSetting.Instance.Setting.FlgVoice = ToggleTalkVoice.isOn;
            LiplisSetting.Instance.Setting.FlgDebug = ToggleDebug.isOn;

            //おしゃべりスピード
            LiplisSetting.Instance.Setting.TalkSpeed = TopicSpeed.value;
            LiplisSetting.Instance.Setting.TalkNum = TopicNumDrop.value;

            //グラフィックレベル
            LiplisSetting.Instance.Setting.GraphicLevel = CboGraphicLevel.value;
            LiplisSetting.Instance.Setting.DrawingFps = CboDrawingFps.value;
            LiplisSetting.Instance.Setting.SpeechBallonNum = CboSpeechBallonNum.value;

            //キャラクター配置
            LiplisSetting.Instance.Setting.CharArrangement = CboArrangement.value;

            //FPS変更
            Application.targetFrameRate = LiplisSetting.Instance.Setting.GetFps();

            //指定キー「LiplisStatus」でリプリスステータスのインスタンスを保存する
            SaveDataSetting.SetClass(LpsDefine.SETKEY_LIPLIS_SETTING, LiplisSetting.Instance);

            //セーブ発動
            SaveDataSetting.Save();

            yield return null;
        }
    }
}
