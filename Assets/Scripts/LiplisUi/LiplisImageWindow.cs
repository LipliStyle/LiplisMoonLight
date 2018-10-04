//=======================================================================
//  ClassName : LiplisTitleWindow
//  概要      : タイトルウインドウ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using System;
using System.Collections.Generic;
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
        public LiplisImageWindow(GameObject Window)
        {
            //ウインドウインスタンス取得
            imgWindow = Window.GetComponent<ImageWindow>();

            //ウインドウセット
            imgWindow.SetParentWindow(Window);

            //URL設定
            //imgWindow.SetImage(url);

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
        public void SetImage(List<string> urlList)
        {
            imgWindow.SetImage(urlList);
        }

        /// <summary>
        /// 最小化する
        /// </summary>
        public void FixPicture()
        {
            imgWindow.FixPicture();
        }

        /// <summary>
        /// 移動目標を再設定する。
        /// </summary>
        /// <param name="TargetPosition"></param>
        public void SetMoveTarget(Vector3 TargetPosition)
        {
            this.imgWindow.SetMoveTarget(TargetPosition);
        }

        /// <summary>
        /// 表示位置を初期化する
        /// </summary>
        public void InitLocation()
        {
            this.imgWindow.InitLocation();
        }

    }
}
