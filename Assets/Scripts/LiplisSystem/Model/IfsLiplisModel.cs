﻿//====================================================================
//  ClassName : IfsLiplisModel
//  概要      : リプリスモデルインターフェース
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Define;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Model
{
    public interface IfsLiplisModel
    {
        /// <summary>
        /// モデルオブジェクト
        /// </summary>
        GameObject ModelObject{get;set;}

        int Direction { get; set; }

        //====================================================================
        //
        //                            初期化処理
        //                         
        //====================================================================
        #region 初期化処理
        /// <summary>
        /// モデルをロードする
        /// </summary>
        void LoadModel(string modelPath, string targetSettingFileNam, Vector3 targetPosition);
        #endregion


        //====================================================================
        //
        //                            モデル操作
        //                         
        //====================================================================
        #region モデル操作
        /// <summary>
        /// 座標を設定する
        /// </summary>
        /// <param name="targetPosition"></param>
        void SetPosition(Vector3 targetPosition);

        /// <summary>
        /// スケールを設定する。
        /// </summary>
        /// <param name="targetScale"></param>
        void SetScale(Vector3 targetScale);

        /// <summary>
        /// フェードイン
        /// </summary>
        void SetFaidIn();
        void SetFaidIn(float interval);

        /// <summary>
        /// /フェドアウト
        /// </summary>
        void SetFadeOut();
        void SetFadeOut(float interval);

        /// <summary>
        /// フェードしながらビジブルを変更する
        /// </summary>
        /// <param name="flg"></param>
        void SetVisible(bool flg);

        /// <summary>
        /// VisbleONかどうか。
        /// </summary>
        bool IsVisble();

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="ModelLocation"></param>
        void SetMove(Vector3 ModelLocation);

        /// <summary>
        /// トークを開始する
        /// </summary>
        void StartTalking();

        /// <summary>
        /// トークを終了する
        /// </summary>
        void StopTalking();

        /// <summary>
        /// 再生中かどうかを返す
        /// true:再生中
        /// </summary>
        bool IsPlaying();

        /// <summary>
        /// 表情ををセットする
        /// </summary>
        /// <returns></returns>
        void SetExpression(MOTION ExpressionCode);

        /// <summary>
        /// モーションをセットする
        /// </summary>
        /// <param name="MotionCode"></param>
        void StartRandomMotion(MOTION MotionCode);
        #endregion

        //====================================================================
        //
        //                             更新制御
        //                         
        //====================================================================
        #region 更新制御
        void Update();
        #endregion
    }
}
