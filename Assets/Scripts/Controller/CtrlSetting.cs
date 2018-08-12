//=======================================================================
//  ClassName : CtrlSetting
//  概要      : 設定画面コントローラー
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.LiplisSystem.Com;
using SpicyPixel.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CtrlSetting : MonoBehaviour {
    ///=============================
    // レンダリングUIキャッシュ
    public GameObject UiRenderingBack;
    public GameObject UiRenderingFront;
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
    public Dropdown GraphicLevelDrop;

    /// <summary>
    /// 開始時
    /// </summary>
    void Start () {
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
    void Update () {
		
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
        //フロント画面が非アクティブのときは何もしない
        if (!UiRenderingFront.gameObject.activeSelf)
        {
            return;
        }

        //ペンディング設定
        LiplisStatus.Instance.EnvironmentInfo.SetPendingOn();

        UiRenderingBack.gameObject.SetActive(false);
        UiRenderingFront.gameObject.SetActive(false);
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

        UiRenderingBack.gameObject.SetActive(true);
        UiRenderingFront.gameObject.SetActive(true);
        UiSetting.gameObject.SetActive(false);
    }

    /// <summary>
    /// ウインドウを初期化する
    /// </summary>
    private void InitWindow()
    {
        ToggleTopicNews.isOn       = LiplisSetting.Instance.Setting.FlgTopicNews;
        ToggleTopicSummary.isOn    = LiplisSetting.Instance.Setting.FlgTopicSummary;
        ToggleTopicRetweet.isOn    = LiplisSetting.Instance.Setting.FlgTopicRetweet;
        ToggleTopicHash.isOn       = LiplisSetting.Instance.Setting.FlgTopicHash;
        ToggleTalkVoice.isOn       = LiplisSetting.Instance.Setting.FlgVoice;
        ToggleDebug.isOn           = LiplisSetting.Instance.Setting.FlgDebug;

        TopicSpeed.value = LiplisSetting.Instance.Setting.TalkSpeed;
        TopicNumDrop.value = LiplisSetting.Instance.Setting.TalkNum;

        GraphicLevelDrop.value = LiplisSetting.Instance.Setting.GraphicLevel;
    }

    /// <summary>
    /// 設定保存
    /// </summary>
    private IEnumerator SaveSetting()
    {
        LiplisSetting.Instance.Setting.FlgTopicNews       = ToggleTopicNews.isOn;
        LiplisSetting.Instance.Setting.FlgTopicSummary    = ToggleTopicSummary.isOn;
        LiplisSetting.Instance.Setting.FlgTopicRetweet    = ToggleTopicRetweet.isOn;
        LiplisSetting.Instance.Setting.FlgTopicHash       = ToggleTopicHash.isOn;
        LiplisSetting.Instance.Setting.FlgVoice           = ToggleTalkVoice.isOn;
        LiplisSetting.Instance.Setting.FlgDebug           = ToggleDebug.isOn;

        //おしゃべりスピード
        LiplisSetting.Instance.Setting.TalkSpeed = TopicSpeed.value;
        LiplisSetting.Instance.Setting.TalkNum = TopicNumDrop.value;

        //グラフィックレベル
        LiplisSetting.Instance.Setting.GraphicLevel = GraphicLevelDrop.value;

        //指定キー「LiplisStatus」でリプリスステータスのインスタンスを保存する
        SaveDataSetting.SetClass(LpsDefine.SETKEY_LIPLIS_SETTING, LiplisSetting.Instance);

        //セーブ発動
        SaveDataSetting.Save();

        yield return null;
    }
}
