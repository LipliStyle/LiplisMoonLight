//=======================================================================
//  ClassName : Browser
//  概要      : ブラウザを開く
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Com;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Web
{
    public class Browser
    {
        /// <summary>
        /// ブラウザを開く
        /// </summary>
        /// <param name="url"></param>
        public static void Open(string url)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                //InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
                //options.displayURLAsPageTitle = false;
                //options.pageTitle = LpsDefine.APPLICATION_TITLE;

                //InAppBrowser.OpenURL(url, options);
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                Application.OpenURL(url);
            }
        }
    }
}
