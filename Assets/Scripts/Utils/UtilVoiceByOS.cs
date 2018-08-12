//=======================================================================
//  ClassName : UtilVoiceByOS
//  概要      : OS別音声ユーティリティ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿

using Assets.Scripts.LiplisSystem.Com;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class UtilVoiceByOS
    {
        /// <summary>
        /// OS別のボイスAPIURLを返す
        /// </summary>
        /// <returns></returns>
        public static string GetVoiceApiUrl()
        {
            // PC向けならWavファイルのURLを返す
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer)
            {
                return LpsDefine.LIPLIS_API_VOICE_WAV;
            }
            else
            {
                return LpsDefine.LIPLIS_API_VOICE_MP3;
            }
        }
        public static string GetOndemandVoiceApiUrl()
        {
            // PC向けならWavファイルのURLを返す
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer)
            {
                return LpsDefine.LIPLIS_API_VOICE_WAV_ONDEMAND;
            }
            else
            {
                return LpsDefine.LIPLIS_API_VOICE_MP3_ONDEMAND;
            }
        }

        /// <summary>
        /// OS別のファイルタイプを取得する
        /// </summary>
        /// <returns></returns>
        public static AudioType GetVoiceFileTypeByOs()
        {
            // PC向けならWavファイルのURLを返す
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer)
            {
                return AudioType.WAV;
            }
            else
            {
                return AudioType.MPEG;
            }
        }
    }
}
