//====================================================================
//  ClassName : DatWether
//  概要      : 天気データ
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.Com;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.Msg;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatWether
    {
        ///=============================
        ///プロパティ
        public uint LastUpdateTime;

        ///=============================
        ///天気リスト
        public List<ResLpsWeatherInfo60> WetherList;
        public List<ResLpsWeatherInfoDtl60> WetherDtlList;

        ///=============================
        ///天気データ
        public MsgWeatherInfoDtl60 WetherToday;
        public MsgWeatherInfoDtl60 WetherTodayLast;
        public MsgWeatherInfoDtl60 WetherYesterday;

        //====================================================================
        //
        //                             初期化処理
        //                         
        //====================================================================
        #region 初期化処理
        /// <summary>
        /// コンストラクター
        /// </summary>
        public DatWether()
        {
            //天気データ初期化
            this.WetherList = new List<ResLpsWeatherInfo60>();

            //1時間前の時刻をセット(最初に強制取得するため)
            this.LastUpdateTime = LpsDatetimeUtil.enc(DateTime.Now.AddHours(-1));
        }

        #endregion

        //====================================================================
        //
        //                         本日天気関連処理
        //                         
        //====================================================================
        #region 本日天気関連処理
        /// <summary>
        /// データをセットする
        /// </summary>
        /// <param name="dataList"></param>
        public void SetData(ResLpsWeatherInfo60List DataList)
        {
            //NULLチェック
            if(DataList == null)
            {
                return;
            }

            //天気情報セット
            this.WetherList = DataList.WetherList;
            this.WetherDtlList = DataList.WetherDescriptionList;

            //前回取得時 天気情報
            SetTodayWetherLast();

            //本日天気情報
            SetTodayWether();
        }

        /// <summary>
        /// 本日天気セット
        /// </summary>
        private void SetTodayWether()
        {
            //ウェザーリストが取得できていない場合は更新しない
            if(WetherDtlList == null)
            {
                return;
            }

            //時間帯を取得
            string nowTimeId = getTimeId();

            foreach (var wehter in this.WetherDtlList)
            {
                //NULL回避
                if(wehter.date == null)
                {
                    continue;
                }

                //本日の天気を検索
                if(wehter.date.Value.Date == DateTime.Now.Date && nowTimeId == wehter.timeId)
                {
                    //本日天気セット
                    this.WetherToday = new MsgWeatherInfoDtl60(wehter);

                    //抜ける
                    return;
                }
            }

            //抜けてきてしまった場合は、取得できていない。nullを設定
            this.WetherToday = null;
        }

        /// <summary>
        /// 時間コードを取得する
        /// </summary>
        /// <returns></returns>
        private string getTimeId()
        {
            int time = DateTime.Now.Hour;

            if(time >= 0 && time <= 6)
            {
                return "0";
            }
            else if (time >= 7 && time <= 12)
            {
                return "1";
            }
            else if (time >= 13 && time <= 18)
            {
                return "2";
            }
            else if (time >= 19 && time <= 23)
            {
                return "3";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 前回天気をセットする
        /// </summary>
        private void SetTodayWetherLast()
        {
            WetherTodayLast = new MsgWeatherInfoDtl60(WetherToday);
        }

        /// <summary>
        /// 今日の天気状況を取得する
        /// </summary>
        /// <returns></returns>
        public MsgDayWether GetWetherSentenceToday(DateTime dt)
        {
            MsgDayWether result = new MsgDayWether();

            //foreach (ResLpsWeatherInfoDtl60 data in WetherDtlList)
            //{
            //    if (data.timeId == "1" && data.date.Value.Day == targetDdate)
            //    {
            //        result.wether1 = data.weather;
            //        result.wether1Roughly = GetWegherRoughly(data.weather);
            //    }

            //    if (data.timeId == "2" && data.date.Value.Day == targetDdate)
            //    {
            //        result.wether2 = data.weather;
            //        result.wether2Roughly = GetWegherRoughly(data.weather);
            //    }

            //    if (data.timeId == "3" && data.date.Value.Day == targetDdate)
            //    {
            //        result.wether3 = data.weather;
            //        result.wether3Roughly = GetWegherRoughly(data.weather);
            //    }
            //}

            if(WetherDtlList.Count >=4 )
            {
                if (dt.Hour >= 0 && dt.Hour <= 11)
                {
                    result.wether1 = WetherDtlList[1].weather;
                    result.wether1Roughly = GetWegherRoughly(WetherDtlList[1].weather);
                    result.wether2 = WetherDtlList[2].weather;
                    result.wether2Roughly = GetWegherRoughly(WetherDtlList[2].weather);
                    result.wether3 = WetherDtlList[3].weather;
                    result.wether3Roughly = GetWegherRoughly(WetherDtlList[3].weather);
                }
                else if (dt.Hour >= 12 && dt.Hour <= 18)
                {
                    result.wether1 = "";
                    result.wether1Roughly = "";
                    result.wether2 = WetherDtlList[2].weather;
                    result.wether2Roughly = GetWegherRoughly(WetherDtlList[2].weather);
                    result.wether3 = WetherDtlList[3].weather;
                    result.wether3Roughly = GetWegherRoughly(WetherDtlList[3].weather);
                }
                else if (dt.Hour >= 19 && dt.Hour <= 23)
                {
                    result.wether1 = "";
                    result.wether1Roughly = "";
                    result.wether2 = "";
                    result.wether2Roughly = "";
                    result.wether3 = WetherDtlList[3].weather;
                    result.wether3Roughly = GetWegherRoughly(WetherDtlList[3].weather);
                }
            }

            return result;
        }

        public MsgDayWether GetWetherSentenceTommorow(DateTime dt)
        {
            MsgDayWether result = new MsgDayWether();


            if (WetherDtlList.Count >= 8)
            {
                result.wether1 = WetherDtlList[5].weather;
                result.wether1Roughly = GetWegherRoughly(WetherDtlList[5].weather);
                result.wether2 = WetherDtlList[6].weather;
                result.wether2Roughly = GetWegherRoughly(WetherDtlList[6].weather);
                result.wether3 = WetherDtlList[7].weather;
                result.wether3Roughly = GetWegherRoughly(WetherDtlList[7].weather);
            }

            return result;
        }

        /// <summary>
        /// おおざっぱな天気を取得する
        /// </summary>
        /// <returns></returns>
        public string GetWegherRoughly(string wetherCode)
        {
            switch (wetherCode)
            {
                case WETHER_CODE.SUNNY:                             return WETHER_CODE.SUNNY;
                case WETHER_CODE.SUNNY_THEN_CLOUDY:                 return WETHER_CODE.SUNNY;
                case WETHER_CODE.SUNNY_THEN_RAIN:                   return WETHER_CODE.RAIN;
                case WETHER_CODE.SUNNY_THEN_SNOW:                   return WETHER_CODE.SNOW;
                case WETHER_CODE.SUNNY_WITH_OCCASIONAL_CLOUDY:      return WETHER_CODE.SUNNY;
                case WETHER_CODE.SUNNY_WITH_OCCASIONAL_RAIN:        return WETHER_CODE.RAIN;
                case WETHER_CODE.SUNNY_WITH_OCCASIONAL_SNOW:        return WETHER_CODE.SNOW;
                case WETHER_CODE.CLOUDY:                            return WETHER_CODE.CLOUDY;
                case WETHER_CODE.CLOUDY_THEN_SUNNY:                 return WETHER_CODE.CLOUDY;
                case WETHER_CODE.CLOUDY_THEN_RAIN:                  return WETHER_CODE.RAIN;
                case WETHER_CODE.CLOUDY_THEN_SNOW:                  return WETHER_CODE.SNOW;
                case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_SUNNY:      return WETHER_CODE.CLOUDY;
                case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_RAIN:       return WETHER_CODE.RAIN;
                case WETHER_CODE.CLOUDY_WITH_OCCASIONAL_SNOW:       return WETHER_CODE.SNOW;
                case WETHER_CODE.RAIN:                              return WETHER_CODE.RAIN;
                case WETHER_CODE.RAIN_THEN_SUNNY:                   return WETHER_CODE.SUNNY;
                case WETHER_CODE.RAIN_THEN_CLOUDY:                  return WETHER_CODE.CLOUDY;
                case WETHER_CODE.RAIN_THEN_SNOW:                    return WETHER_CODE.SNOW;
                case WETHER_CODE.RAIN_WITH_OCCASIONAL_SUNNY:        return WETHER_CODE.RAIN;
                case WETHER_CODE.RAIN_WITH_OCCASIONAL_CLOUDY:       return WETHER_CODE.RAIN;
                case WETHER_CODE.RAIN_WITH_OCCASIONAL_SNOW:         return WETHER_CODE.RAIN;
                case WETHER_CODE.SNOW:                              return WETHER_CODE.SNOW;
                case WETHER_CODE.SNOW_THEN_SUNNY:                   return WETHER_CODE.SUNNY;
                case WETHER_CODE.SNOW_THEN_CLOUDY:                  return WETHER_CODE.CLOUDY;
                case WETHER_CODE.SNOW_THEN_RAIN:                    return WETHER_CODE.RAIN;
                case WETHER_CODE.SNOW_WITH_OCCASIONAL_SUNNY:        return WETHER_CODE.SNOW;
                case WETHER_CODE.SNOW_WITH_OCCASIONAL_CLOUDY:       return WETHER_CODE.SNOW;
                case WETHER_CODE.SNOW_WITH_OCCASIONAL_RAIN:         return WETHER_CODE.SNOW;
                case WETHER_CODE.WIND_STOM:                         

                default: return "";
            }
        }




        /// <summary>
        /// 現在のウェザーコードを取得する
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetNowWetherCode(DateTime dt)
        {
            if (WetherDtlList == null)
            {
                return WETHER_CODE.SUNNY;
            }


            if (WetherDtlList.Count >= 4)
            {
                if (dt.Hour >= 0 && dt.Hour <= 11)
                {
                    return GetWegherRoughly(WetherDtlList[1].weather);
                }
                else if (dt.Hour >= 12 && dt.Hour <= 18)
                {
                    return GetWegherRoughly(WetherDtlList[2].weather);
                }
                else if (dt.Hour >= 19 && dt.Hour <= 23)
                {
                    return GetWegherRoughly(WetherDtlList[3].weather);
                }
            }

            return WETHER_CODE.SUNNY;
        }
        #endregion


        //====================================================================
        //
        //                          現在温度関連
        //                          
        //====================================================================
        #region 現在温度関連

        /// <summary>
        /// 現在の最高気温を取得する
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int? GetMaxTemperature(DateTime dt)
        {
            if (WetherDtlList.Count >= 4)
            {
                if (dt.Hour >= 0 && dt.Hour <= 6)
                {
                    return WetherDtlList[0].tempMax;
                }
                else if (dt.Hour >= 7 && dt.Hour <= 12)
                {
                    return WetherDtlList[1].tempMax;
                }
                else if (dt.Hour >= 13 && dt.Hour <= 18)
                {
                    return WetherDtlList[2].tempMax;
                }
                else if (dt.Hour >= 19 && dt.Hour <= 23)
                {
                    return WetherDtlList[3].tempMax;
                }
            }

            return null;
        }

        #endregion


        //====================================================================
        //
        //                         現在降水確率関連
        //                          
        //====================================================================
        #region 現在降水確率関連
        /// <summary>

        /// <summary>
        /// 現在の最高気温を取得する
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int? GetChanceOfRain(DateTime dt)
        {
            if (WetherDtlList.Count >= 4)
            {
                if (dt.Hour >= 0 && dt.Hour <= 6)
                {
                    return WetherDtlList[0].chanceOfRain;
                }
                else if (dt.Hour >= 7 && dt.Hour <= 12)
                {
                    return WetherDtlList[1].chanceOfRain;
                }
                else if (dt.Hour >= 13 && dt.Hour <= 18)
                {
                    return WetherDtlList[2].chanceOfRain;
                }
                else if (dt.Hour >= 19 && dt.Hour <= 23)
                {
                    return WetherDtlList[3].chanceOfRain;
                }
            }

            return null;
        }


        #endregion

    }
}
