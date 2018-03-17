//=======================================================================
//  ClassName : SpriteLinkWether
//  概要      : 天気アイコンスプライトリンク
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Define;
using UnityEngine;

public class SpriteLinkWether : MonoBehaviour {
    public Sprite Wether01;
    public Sprite Wether02;
    public Sprite Wether03;
    public Sprite Wether04;
    public Sprite Wether05;
    public Sprite Wether06;
    public Sprite Wether07;
    public Sprite Wether08;
    public Sprite Wether09;
    public Sprite Wether10;
    public Sprite Wether11;
    public Sprite Wether12;
    public Sprite Wether13;
    public Sprite Wether14;
    public Sprite Wether15;
    public Sprite Wether16;
    public Sprite Wether17;
    public Sprite Wether18;
    public Sprite Wether19;
    public Sprite Wether20;
    public Sprite Wether21;
    public Sprite Wether22;
    public Sprite Wether23;
    public Sprite Wether24;
    public Sprite Wether25;
    public Sprite Wether26;
    public Sprite Wether27;
    public Sprite Wether28;
    public Sprite Wether29;

    //====================================================================
    //
    //                          シングルトン管理
    //                         
    //====================================================================
    #region シングルトン管理

    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    private static SpriteLinkWether instance;
    public static SpriteLinkWether Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SpriteLinkWether();
            }

            return instance;
        }
    }   
    #endregion

    // Use this for initialization
    void Start () {
        instance = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// ウェザーコードに対応するアイコンを返す
    /// </summary>
    /// <param name="wetherCode"></param>
    /// <returns></returns>
    public Sprite GetWetherIcon(string wetherCode)
    {
        switch(wetherCode)
        {
            case WETHER_CODE.SUNNY:                         return Wether01;//晴れ
            case WETHER_CODE.SUNNY_THEN_CLOUDY:             return Wether02;//晴れのち曇り
            case WETHER_CODE.SUNNY_THEN_RAIN:               return Wether03;//晴れのち雨
            case WETHER_CODE.SUNNY_THEN_SNOW:               return Wether04;//晴れのち雪
            case WETHER_CODE.SUNNY_WITH_OCCASIONAL_CLOUDY:  return Wether05;//晴れ時々曇り
            case WETHER_CODE.SUNNY_WITH_OCCASIONAL_RAIN:    return Wether06;//晴れ時々雨
            case WETHER_CODE.SUNNY_WITH_OCCASIONAL_SNOW:    return Wether07;//晴れ時々雪
            case WETHER_CODE.CLOUDY:                        return Wether08;//曇り
            case WETHER_CODE.CLOUDY_THEN_SUNNY:             return Wether09;//曇りのち晴れ
            case WETHER_CODE.CLOUDY_THEN_RAIN:              return Wether10;//曇りのち雨
            case WETHER_CODE.CLOUDY_THEN_SNOW:              return Wether11;//曇りのち雪
            case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_SUNNY:  return Wether12;//曇り時々晴れ
            case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_RAIN:   return Wether13;//曇り時々雨
            case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_SNOW:   return Wether14;//曇り時々雪
            case WETHER_CODE.RAIN:                          return Wether15;//雨
            case WETHER_CODE.RAIN_THEN_SUNNY:               return Wether16;//雨のち晴れ
            case WETHER_CODE.RAIN_THEN_CLOUDY:              return Wether17;//雨のち曇
            case WETHER_CODE.RAIN_THEN_SNOW:                return Wether18;//雨のち雪
            case WETHER_CODE.RAIN_WITH_OCCASIONAL_SUNNY:    return Wether19;//雨時々晴れ
            case WETHER_CODE.RAIN_WITH_OCCASIONAL_CLOUDY:   return Wether20;//雨時々曇り
            case WETHER_CODE.RAIN_WITH_OCCASIONAL_SNOW:     return Wether21;//雨時々雪
            case WETHER_CODE.SNOW:                          return Wether22;//雪
            case WETHER_CODE.SNOW_THEN_SUNNY:               return Wether23;//雪のち晴れ
            case WETHER_CODE.SNOW_THEN_CLOUDY:              return Wether24;//雪のち曇り
            case WETHER_CODE.SNOW_THEN_RAIN:                return Wether25;//雪のち雨
            case WETHER_CODE.SNOW_WITH_OCCASIONAL_SUNNY:    return Wether26;//雪時々晴れ
            case WETHER_CODE.SNOW_WITH_OCCASIONAL_CLOUDY:   return Wether27;//雪時々曇り
            case WETHER_CODE.SNOW_WITH_OCCASIONAL_RAIN:     return Wether28;//雪時々雨
            case WETHER_CODE.WIND_STOM:                     return Wether29;//暴風雪


            default: return null;
        }
    }





}
