﻿//=======================================================================
//  ClassName : TabNews
//  概要      : ニュースタブ
//              
//
//  LiplisLive2D
//  Create 2018/04/09
//
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Data;
using System.Collections;

public class TabNews : TabBaseTopicList
{
    ///=============================
    ///プロパティ
    public uint LastCheckTime;


    //====================================================================
    //
    //                           初期化処理
    //                         
    //====================================================================
    #region 初期化処理

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        this.Clear();
    }

    #endregion


    //====================================================================
    //
    //                           コンテント操作
    //                         
    //====================================================================
    #region コンテント操作
    void Update()
    {
        if (frameCount % 600 == 0) { UpdateUI_600F(); }

        UpdateFps();
    }

    /// <summary>
    /// 更新処理の実装
    /// </summary>
    public override void UpdateUI_600F()
    {
        StartCoroutine(UpdateNewsList());
    }

    /// <summary>
    /// ニュースリストの更新
    /// </summary>
    public IEnumerator UpdateNewsList()
    {
        //データが更新されていたら、更新する
        if (LastCheckTime == LiplisStatus.Instance.NewsList.LastUpdateTime)
        {
            yield break;
        }

        if (LiplisStatus.Instance.NewsList.LastNewsList.NewsList == null)
        {
            yield break;
        }

        StartCoroutine(UpdateContent(this.Content, LiplisStatus.Instance.NewsList.LastNewsList.NewsList));

        //最終チェック時刻更新
        LastCheckTime = LiplisStatus.Instance.NewsList.LastUpdateTime;
    }


    #endregion
}