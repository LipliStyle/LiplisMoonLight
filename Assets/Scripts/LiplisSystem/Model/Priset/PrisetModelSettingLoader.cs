//====================================================================
//  ClassName : PrisetModelSettingLoader
//  概要      : プリセットモデルの設定を読み込む
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================

using Assets.Scripts.LiplisSystem.Model.Json;
using LiplisMoonlight;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Model.Priset
{
    public class PrisetModelSettingLoader
    {
        /// <summary>
        /// プリセットモデルの設定を読み出す。
        /// リソースからの相対パスを指定
        /// </summary>
        /// <param name="pathResources"></param>
        public static LiplisMoonlightModel LoadMoonlightSetting(string pathResources)
        {
            //モデルの設定データをロードする。
            TextAsset textasset = Resources.Load(pathResources) as TextAsset;

            //デシリアライズ
            return JsonConvert.DeserializeObject<LiplisMoonlightModel>(textasset.text); ;
        }

        /// <summary>
        /// プリセットモデルの口調設定を読み出す
        /// リソースからの相対パスを指定
        /// </summary>
        /// <param name="pathResources"></param>
        /// <returns></returns>
        public static LiplisToneSetting LoadLiplisToneSetting(string pathResources)
        {
            //モデルの設定データをロードする。
            TextAsset textasset = Resources.Load(pathResources) as TextAsset;

            //デシリアライズ
            return JsonConvert.DeserializeObject<LiplisToneSetting>(textasset.text); ;
        }

        /// <summary>
        /// プリセットモデルの固定おしゃべり文章を読み出す
        /// 理s－スカラの相対パスを指定
        /// </summary>
        /// <param name="pathResources"></param>
        /// <returns></returns>
        public static LiplisChatSetting LoadLiplisChatSetting(string pathResources)
        {
            //モデルの設定データをロードする。
            TextAsset textasset = Resources.Load(pathResources) as TextAsset;

            //デシリアライズ
            return JsonConvert.DeserializeObject<LiplisChatSetting>(textasset.text); ;
        }



    }
}
