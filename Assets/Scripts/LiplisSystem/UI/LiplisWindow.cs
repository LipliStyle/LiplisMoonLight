//=======================================================================
//  ClassName : LiplisWindow
//  概要      : イメージウインドウ
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using System;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.UI
{
    public class LiplisWindow
    {
        ///=============================
        ///登録オブジェクト
        //public GameObject Window { get; set; }
        public TalkWindow imgWindow { get; set; }
        public DateTime CreateTime { get; set; }

        ///=============================
        ///サイズ、表示位置
        public float heightImg  { get; set; }
        public float heightText { get; set; }
        public float posTextY   { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="Window"></param>
        public LiplisWindow(GameObject Window, float heightImg, float heightText, float posTextY)
        {
            //ウインドウ取得
            //this.Window = Window;

            //初期表示位置
            this.heightImg  = heightImg;
            this.heightText = heightText;
            this.posTextY   = posTextY;

            //ウインドウインスタンス取得
            imgWindow = Window.GetComponent<TalkWindow>();

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

        ///// <summary>
        ///// ウインドウを除去する
        ///// </summary>
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
            imgWindow.AddText(message);
        }
    }
}
