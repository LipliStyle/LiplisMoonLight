//=======================================================================
//  ClassName : LiplisTitleWindow
//  概要      : タイトルウインドウ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using System;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.UI
{
    public class LiplisImageWindow
    {
        ///=============================
        ///登録オブジェクト
        //public GameObject Window { get; set; }
        //public RectTransform windowRect { get; set; }
        public ImageWindow imgWindow { get; set; }
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="Window"></param>
        public LiplisImageWindow(GameObject Window, string url)
        {
            //ウインドウ取得
            //this.Window = Window;

            //サイズ変更
            //windowRect = this.Window.GetComponent<RectTransform>();

            //ウインドウインスタンス取得
            imgWindow = Window.GetComponent<ImageWindow>();

            //ウインドウセット
            imgWindow.SetParentWindow(Window);

            //URL設定
            imgWindow.SetImage(url);

            //作成時刻設定
            this.CreateTime = DateTime.Now;
        }



        /// <summary>
        /// ウインドウを更新する
        /// </summary>
        public void Update()
        {

        }

        /// <summary>
        /// ウインドウを閉じる
        /// </summary>
        public void CloseWindow()
        {
            this.imgWindow.flgEnd = true;
        }


        /// <summary>
        /// フェードイン
        /// </summary>
        public void FaidIn()
        {
            this.imgWindow.flgOn = true;
        }

        /// <summary>
        /// ウインドウを除去する
        /// </summary>
        //public void Destroy()
        //{
        //    //ウインドウ破棄
        //    Destroy(Window);
        //}

        /// テキストを追加する
        /// </summary>
        /// <param name="message"></param>
        public void SetImage(string url)
        {
            imgWindow.SetImage(url);
        }




    }
}
