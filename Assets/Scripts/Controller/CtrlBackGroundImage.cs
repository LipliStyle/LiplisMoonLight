//=======================================================================
//  ClassName : CtrlBackGroundImage
//  概要      : 背景コントローラー
//              背景の制御 時刻、天気によって変化
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using SpicyPixel.Threading;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CtrlBackGroundImage : ConcurrentBehaviour
{

	///=============================
	///背景インスタンス
	public Image ImgBackGround;
	public Sprite SpriteEarlyMorning;
	public Sprite SpriteDayTime;
	public Sprite SpriteEvening;
	public Sprite SpriteNight;

	public Sprite SpriteCloudyEarlyMorning;
	public Sprite SpriteCloudyDayTime;
	public Sprite SpriteCloudyEvening;
	public Sprite SpriteCloudyNight;

	public Sprite SpriteRainyEarlyMorning;
	public Sprite SpriteRainyDayTime;
	public Sprite SpriteRainyEvening;
	public Sprite SpriteRainyNight;

	///=============================
	///時刻制御
	private int prvTimeCode;        //前回タイムコード
	private string prvWetherCode;      //前回天気コード

	///=============================
	///タイムアウト時間定義
	private const float TIME_OUT = 60f;


	/// <summary>
	/// 初期化
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
	}

    // Use this for initialization
    void Start()
    {
        //1回実行しておく
        //StartCoroutine(CheckChangeTimeCode());

        //定周期ループスタート
        StartCoroutine(UpdateTimerTick());
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// データ収集ループ　
    /// TIME_OUTに設定した周期で回る
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateTimerTick()
    {
        while (true)
        {
            //データ収集処理
            yield return StartCoroutine(CheckChangeTimeCode());

            //非同期待機
            yield return new WaitForSeconds(TIME_OUT);
        }
    }

	/// <summary>
	/// タイムコード更新チェック
	/// </summary>
	IEnumerator CheckChangeTimeCode()
	{
		//タイムコードを取得する
		int nowTimeCode = GetTimeCode(DateTime.Now);
		string nowWetherCode = GetWetherCode();


		//タイムコードが違っていたら、バックグラウンドを変更
		if (nowTimeCode != this.prvTimeCode || nowWetherCode != this.prvWetherCode)
		{
			ChangeBackGround(nowTimeCode, nowWetherCode);
		}

		//前回値更新
		this.prvWetherCode = nowWetherCode;
		yield return this.prvTimeCode = nowTimeCode;
	}

	/// <summary>
	/// バックグラウンドを変更する
	/// </summary>
	public IEnumerator ChangeBackground()
	{
		//タイムコードを取得する
		int nowTimeCode = GetTimeCode(DateTime.Now);
		string nowWetherCode = GetWetherCode();

		ChangeBackGround(nowTimeCode, nowWetherCode);

		//前回値更新
		this.prvWetherCode = nowWetherCode;
		yield return this.prvTimeCode = nowTimeCode;
	}

	/// <summary>
	/// タイムコードを取得する
	/// </summary>
	/// <param name="dt"></param>
	/// <returns></returns>
	int GetTimeCode(DateTime dt)
	{
		if((dt.Hour >= 18 && dt.Hour <= 23) || (dt.Hour >= 0 && dt.Hour <= 3))
		{
			return TIME_CODE.NIGHT;
		}
		else if (dt.Hour >= 4 && dt.Hour <= 5)
		{
			return TIME_CODE.EARY_MORNING;
		}
		else if (dt.Hour >= 6 && dt.Hour <= 15)
		{
			return TIME_CODE.DAY_TIME;
		}
		else if (dt.Hour >= 16 && dt.Hour <= 17)
		{
			return TIME_CODE.EVENING;
		}
		else
		{
			return 0;
		}
	}

	/// <summary>
	/// 現在のウェザーコードを取得する
	/// </summary>
	/// <returns></returns>
	string GetWetherCode()
	{
		return LiplisStatus.Instance.InfoWether.GetNowWetherCode(DateTime.Now);
	}
	

	/// <summary>
	/// 背景を更新する
	/// </summary>
	void ChangeBackGround(int nowTimeCode, string nowWetherCode)
	{
		if (nowWetherCode == WETHER_CODE.CLOUDY)
		{

			if (nowTimeCode == TIME_CODE.DAY_TIME)
			{
				ImgBackGround.sprite = SpriteCloudyDayTime;
			}
			else if (nowTimeCode == TIME_CODE.EVENING)
			{
				ImgBackGround.sprite = SpriteCloudyEvening;
			}
			else if (nowTimeCode == TIME_CODE.EARY_MORNING)
			{
				ImgBackGround.sprite = SpriteCloudyEarlyMorning;
			}
			else if (nowTimeCode == TIME_CODE.NIGHT)
			{
				ImgBackGround.sprite = SpriteCloudyNight;
			}
		}
		else if (nowWetherCode == WETHER_CODE.RAIN || nowWetherCode == WETHER_CODE.SNOW)
		{

			if (nowTimeCode == TIME_CODE.DAY_TIME)
			{
				ImgBackGround.sprite = SpriteRainyDayTime;
			}
			else if (nowTimeCode == TIME_CODE.EVENING)
			{
				ImgBackGround.sprite = SpriteRainyEvening;
			}
			else if (nowTimeCode == TIME_CODE.EARY_MORNING)
			{
				ImgBackGround.sprite = SpriteRainyEarlyMorning;
			}
			else if (nowTimeCode == TIME_CODE.NIGHT)
			{
				ImgBackGround.sprite = SpriteRainyNight;
			}
		}
		else
		{
			if (nowTimeCode == TIME_CODE.DAY_TIME)
			{
				ImgBackGround.sprite = SpriteDayTime;
			}
			else if (nowTimeCode == TIME_CODE.EVENING)
			{
				ImgBackGround.sprite = SpriteEvening;
			}
			else if (nowTimeCode == TIME_CODE.EARY_MORNING)
			{
				ImgBackGround.sprite = SpriteEarlyMorning;
			}
			else if (nowTimeCode == TIME_CODE.NIGHT)
			{
				ImgBackGround.sprite = SpriteNight;
			}
		}
		

	}


}
