//=======================================================================
//  ClassName : TabBaseTopicList
//  概要      : ニュースベース
//              一覧UIの操作を行う
//              
//
//  LiplisLive2D
//  Create 2018/04/09
//
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.LiplisSystem.Msg;
using HC.UI;
using SpicyPixel.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class TabBaseTopicList : MonoBehaviour
{
    ///=============================
    ///コンテンツ
    [SerializeField] protected GameObject Content;

    //=====================================
    // 描画制御用FPS計算
    protected int frameCount = -1;

    //====================================================================
    //
    //                           初期化処理
    //                         
    //====================================================================
    #region 初期化処理


    protected virtual void Start()
    {

    }


    #endregion

    //====================================================================
    //
    //                           更新インターフェース
    //                         
    //====================================================================
    #region 更新インターフェース


    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {

    }

    public virtual void UpdateUI_600F()
    {

    }

    /// <summary>
    /// FPSの更新
    /// </summary>
    protected void UpdateFps()
    {
        //インクリメント
        frameCount++;

        //60000回でリセット
        if (frameCount > 60000)
        {
            frameCount = 0;
        }
    }


    /// <summary>
    /// 汎用アップデートメソッド
    /// </summary>
    public IEnumerator UpdateContent(GameObject Content, List<MsgBaseNewsData> NewsList)
    {
        //ニュースリストの件数が0件なら何もしない
        if (NewsList == null) { yield break; }
        if (NewsList.Count < 1) { yield break; }


        //コンテント初期化
        this.Clear();

        //オフセット
        int offsetY = 0;

        //4個つづのリストに分ける
        List<List<MsgBaseNewsData>> _4NewsList = new List<List<MsgBaseNewsData>>();

        int idx = 0;
        _4NewsList.Add(new List<MsgBaseNewsData>());

        foreach (MsgBaseNewsData newsData in NewsList)
        {
            //4個になったら〆
            if (_4NewsList[idx].Count == 4)
            {
                _4NewsList.Add(new List<MsgBaseNewsData>());
                idx++;
            }

            //追加
            _4NewsList[idx].Add(newsData);
        }

        //不足分
        for (int i = 0; i < 4 - _4NewsList[idx].Count; i++)
        {
            _4NewsList[idx].Add(new MsgBaseNewsData());
        }


        //コントロールの高さ調整
        SetContentHeightRowCount(_4NewsList.Count);


        //データ生成
        foreach (var _4news in _4NewsList)
        {
            StartCoroutine(CreateRow(Content, offsetY, _4news[0], _4news[1], _4news[2], _4news[3]));
            offsetY++;
        }

    }








    #endregion


    //====================================================================
    //
    //                           コンテント操作
    //                         
    //====================================================================
    #region コンテント操作

    /// <summary>
    /// 子要素のクリア
    /// </summary>
    public void Clear()
    {
        foreach (Transform n in Content.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }


    /// <summary>
    /// 1行データを生成する
    /// </summary>
    /// <returns></returns>
    //public Image CreateRow(GameObject Content, float offsetY)
    //{
    //    Image panel = CreateRowPanel(Content,offsetY);

    //    CreateNewsPanel1(panel.gameObject,"1111111");
    //    CreateNewsPanel2(panel.gameObject,"222222222");
    //    CreateNewsPanel3(panel.gameObject,"33333333333333");
    //    CreateNewsPanel4(panel.gameObject,"4444444444444444444");

    //    return panel;
    //}

    public IEnumerator CreateRow(GameObject Content, float offsetY, MsgBaseNewsData news1, MsgBaseNewsData news2, MsgBaseNewsData news3, MsgBaseNewsData news4)
    {
        Image panel = CreateRowPanel(Content, offsetY);

        StartCoroutine(CreateNewsPanel1(panel.gameObject, news1.TITLE, news1.THUMBNAIL_URL));
        StartCoroutine(CreateNewsPanel2(panel.gameObject, news2.TITLE, news2.THUMBNAIL_URL));
        StartCoroutine(CreateNewsPanel3(panel.gameObject, news3.TITLE, news3.THUMBNAIL_URL));
        StartCoroutine(CreateNewsPanel4(panel.gameObject, news4.TITLE, news4.THUMBNAIL_URL));

        yield return panel;
    }




    /// <summary>
    /// 1行パネルを作成する
    /// </summary>
    /// <returns></returns>
    public Image CreateRowPanel(GameObject Content, float offsetY)
    {
        Image panel = UICreator.CreatePanel(Content);

        //サイズ調整
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.offsetMax = new Vector2(0, offsetY * -475);
        rectTransform.offsetMin = new Vector2(0, -470f);
        rectTransform.sizeDelta = new Vector2(0, 470f);

        return panel;
    }


    /// <summary>
    /// 記事パネル作成
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public IEnumerator CreateNewsPanel1(GameObject parent, string newsTitle, string thumbnail)
    {
        yield return StartCoroutine(CreateNewsPanel(parent, -1440, newsTitle, thumbnail));
    }


    public IEnumerator CreateNewsPanel2(GameObject parent, string newsTitle, string thumbnail)
    {
        yield return StartCoroutine(CreateNewsPanel(parent, -480, newsTitle, thumbnail));
    }


    public IEnumerator CreateNewsPanel3(GameObject parent, string newsTitle, string thumbnail)
    {
        yield return StartCoroutine(CreateNewsPanel(parent, 480, newsTitle, thumbnail));
    }

    public IEnumerator CreateNewsPanel4(GameObject parent, string newsTitle, string thumbnail)
    {
        yield return StartCoroutine(CreateNewsPanel(parent, 1440, newsTitle, thumbnail));
    }

    /// <summary>
    /// 記事パネルを生成する
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public IEnumerator CreateNewsPanel(GameObject parent, float offsetMinX, string newsTitle, string thumbnail)
    {
        //パネルを生成する
        Image panel = UICreator.CreatePanel(parent);

        //サイズ調整
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0);
        rectTransform.anchorMax = new Vector2(0.5f, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = new Vector2(offsetMinX, 0);
        rectTransform.sizeDelta = new Vector2(480, 0);

        //コンテンツを生成
        StartCoroutine(CreateNewsImage(panel.gameObject, thumbnail));
        StartCoroutine(CreateNewsText(panel.gameObject, newsTitle));

        yield return panel;
    }


    /// <summary>
    /// 記事サムネイルを生成する
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public IEnumerator CreateNewsImage(GameObject parent, string url)
    {
        //イメージを生成する
        Image image = UICreator.CreateImage(parent);

        //サイズ調整
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1f);
        rectTransform.anchorMax = new Vector2(0, 1f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.offsetMax = new Vector2(454, -23.5f);
        rectTransform.offsetMin = new Vector2(22.0f, -266.5f);
        rectTransform.sizeDelta = new Vector2(432, 243);

        WWW www = new WWW(url);

        yield return www;

        image.GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, 320, 240), Vector2.zero);

        yield return image;
    }

    //ニューステキストを生成する
    public IEnumerator CreateNewsText(GameObject parent, string newsTitle)
    {
        //イメージを生成する
        Text text = UICreator.CreateText(parent);

        //サイズ調整
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.offsetMax = new Vector2(218.1f, -40.1f);
        rectTransform.offsetMin = new Vector2(-225.1f, -209.4f);
        rectTransform.sizeDelta = new Vector2(443.1f, 161.3f);

        //テキストセット
        text.text = newsTitle;

        text.fontSize = 32;

        yield return text;
    }


    /// <summary>
    /// コンテントの高さを設定する
    /// </summary>
    public void SetContentHeightRowCount(int rowCount)
    {
        SetContentHeight(rowCount * 475);
    }
    public void SetContentHeight(float height)
    {
        //サイズ調整
        RectTransform rectTransform = this.Content.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, height);
    }

    #endregion





}