//=======================================================================
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
using Assets.Scripts.Define;
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
        this.Categoly = ContentCategoly.news;
        base.Start();
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
        if (frameCount % 30000 == 0) { UpdateUI_30000F(); }

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
    /// サムネイル更新実行
    /// </summary>
    public void UpdateUI_30000F()
    {

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

    /// <summary>
    /// ニュースリスト即更新
    /// </summary>
    public void UpdateNewsListInstant()
    {
        //データが更新されていたら、更新する
        if (LastCheckTime == LiplisStatus.Instance.NewsList.LastUpdateTime)
        {
            return;
        }

        StartCoroutine(UpdateContent(this.Content, LiplisStatus.Instance.NewsList.LastNewsList.NewsList));
    }


    #endregion
}