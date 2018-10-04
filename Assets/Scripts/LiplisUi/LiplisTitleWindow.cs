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
    public class LiplisTitleWindow
    {
        ///=============================
        ///登録オブジェクト
        //public GameObject Window { get; set; }
        public InfoWindow imgWindow { get; set; }
        public DateTime CreateTime { get; set; }

        ///=============================
        ///サイズ、表示位置
        public float heightImg { get; set; }
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="Window"></param>
        public LiplisTitleWindow(GameObject Window, float heightImg)
        {
            //ウインドウ取得
            //this.Window = Window;

            //初期表示位置
            this.heightImg = heightImg;

            //ウインドウインスタンス取得
            imgWindow = Window.GetComponent<InfoWindow>();

            //ウインドウセット
            imgWindow.SetParentWindow(Window);

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
        /// ウインドウを除去する
        /// </summary>
        //public void Destroy()
        //{
        //    //ウインドウ破棄
        //    Destroy(Window);
        //}

        /// <summary>
        /// 移動目標を再設定する。
        /// </summary>
        /// <param name="TargetPosition"></param>
        public void SetMoveTarget(Vector3 TargetPosition)
        {
            this.imgWindow.SetMoveTarget(TargetPosition);
        }

        /// <summary>
        /// テキストを追加する
        /// </summary>
        /// <param name="message"></param>
        public void AddText(string message)
        {
            imgWindow.SetText(message);
        }





    }
}
