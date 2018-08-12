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
using Assets.Scripts.Controller.UiController.TopicListController;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Web;
using HC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TabBaseTopicList : MonoBehaviour
{
    ///=============================
    ///コンテンツ
    [SerializeField] protected GameObject Content;

    //=====================================
    // 描画制御用FPS計算
    protected int frameCount = -1;

    //=====================================
    // イメージリスト
    private List<TopicPanel> panelList;

    //=====================================
    // 本コントロールのID
    protected ContentCategoly Categoly;

    //=====================================
    // spriteEmpty
    private Sprite empty;

    ///=============================
    ///プレファブ
    private GameObject PrefabNewsPanel;

    //====================================================================
    //
    //                           初期化処理
    //                         
    //====================================================================
    #region 初期化処理

    /// <summary>
    /// スタート
    /// </summary>
    protected virtual void Start()
    {
        InitClass();
        CreatePanelList();
    }

    /// <summary>
    /// クラスの初期化
    /// </summary>
    protected void InitClass()
    {
        empty = new Sprite();

        this.PrefabNewsPanel = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_NEWS_PANEL);
    }

    /// <summary>
    /// パネルリストを生成する
    /// </summary>
    public void CreatePanelList()
    {
        //パネルリスト生成
        panelList = new List<TopicPanel>();

        //オフセット
        int offsetY = 0;

        //枠作成
        for (int i = 0; i < 50; i++)
        {
            CreateRow120(offsetY);
            offsetY++;
        }
    }

    /// <summary>
    /// 行を生成する
    /// </summary>
    /// <param name="offsetY"></param>
    public void CreateRow120(float offsetY)
    {
        //パネル生成
        Image panel = CreateRowPanel_120(offsetY);

        panelList.Add(new TopicPanel(panel.gameObject, Instantiate(PrefabNewsPanel) as GameObject, -1440, this.Categoly));

        panelList.Add(new TopicPanel(panel.gameObject, Instantiate(PrefabNewsPanel) as GameObject, -480, this.Categoly));

        panelList.Add(new TopicPanel(panel.gameObject, Instantiate(PrefabNewsPanel) as GameObject, 480, this.Categoly));

        panelList.Add(new TopicPanel(panel.gameObject, Instantiate(PrefabNewsPanel) as GameObject, 1440, this.Categoly));
    }


    /// <summary>
    /// 1行パネルを作成する
    /// </summary>
    /// <returns></returns>
    public Image CreateRowPanel_120(float offsetY)
    {
        Image panel = UICreator.CreatePanel(this.Content);

        //サイズ調整
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.offsetMax = new Vector2(0, offsetY * -120);
        rectTransform.offsetMin = new Vector2(0, -120f);
        rectTransform.sizeDelta = new Vector2(0, 120f);

        return panel;
    }

    /// <summary>
    /// パネルリストを初期化する
    /// </summary>
    public void InitPanelList()
    {
        foreach (TopicPanel panel in panelList)
        {
            panel.Hide();
        }
    }



    #endregion

    //====================================================================
    //
    //                           更新インターフェース
    //                         
    //====================================================================
    #region 更新インターフェース
    /// <summary>
    /// 画面有効化時
    /// </summary>
    private void OnEnable()
    {
        //サムネ更新開始
        StartCoroutine(UpdateThumbnail());
    }

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

        //展開
        Create4SNewsList(NewsList);

        //サムネイル更新処理
        StartCoroutine(UpdateThumbnail());
    }

    /// <summary>
    /// 行単位に分割する
    /// </summary>
    /// <param name="NewsList"></param>
    /// <returns></returns>
    public List<List<MsgBaseNewsData>> SplitRowLine(List<MsgBaseNewsData> NewsList, int colNum)
    {
        //4個つづのリストに分ける
        List<List<MsgBaseNewsData>> splitNewsList = new List<List<MsgBaseNewsData>>();

        int idx = 0;
        splitNewsList.Add(new List<MsgBaseNewsData>());

        foreach (MsgBaseNewsData newsData in NewsList)
        {
            //4個になったら〆
            if (splitNewsList[idx].Count == colNum)
            {
                splitNewsList.Add(new List<MsgBaseNewsData>());
                idx++;
            }

            //追加
            splitNewsList[idx].Add(newsData);
        }

        //不足分
        for (int i = 0; i < colNum - splitNewsList[idx].Count; i++)
        {
            splitNewsList[idx].Add(new MsgBaseNewsData());
        }

        return splitNewsList;
    }

    /// <summary>
    /// サムネイルを更新する
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateThumbnail()
    {
        if(panelList == null)
        {
            yield break;
        }

        foreach (TopicPanel panel in panelList)
        {
            //パネルロード要求フラグがONなら更新
            if(panel.FlgThumbnailLoadRequest)
            {

                //登録されていない場合は、ダウンロード

                using (WWW www = new WWW(ThumbnailUrl.CreateListThumbnailUrl(panel.news.THUMBNAIL_URL)))
                {
                    // 画像ダウンロード完了を待機
                    yield return www;

                    if (!string.IsNullOrEmpty(www.error))
                    {
                        //次回走査対象外とするため、空のスプライトを設定する。
                        panel.SetThumbnail(empty);

                        Debug.Log(www.error);
                    }

                    try
                    {
                        //スプライト設定
                        panel.SetThumbnail(Sprite.Create(www.texture, new Rect(0, 0, (int)www.texture.width, (int)www.texture.height), Vector2.zero));
                    }
                    catch (Exception ex)
                    {
                        //次回走査対象外とするため、空のスプライトを設定する。
                        panel.SetThumbnail(empty);

                        Debug.Log(ex);
                    }

                    yield return new WaitForSeconds(0.1f);
                }



            }
        }
    }




    #endregion


    //====================================================================
    //
    //  コンテント操作
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


    //====================================================================
    //
    //  コンテント操作
    //  
    //  470✕480　1列 4個のコンテント作成
    //                         
    //====================================================================
    #region コンテント操作480

    /// <summary>
    /// 4分割のニュースを生成する
    /// </summary>
    /// <param name="NewsList"></param>
    /// <returns></returns>
    public IEnumerator Create4NewsList(List<MsgBaseNewsData> NewsList)
    {
        //4個つづのリストに分ける
        List<List<MsgBaseNewsData>> _4NewsList = SplitRowLine(NewsList, 4);

        //展開
        StartCoroutine(CreateRow480(_4NewsList));

        yield return 0;
    }


    public IEnumerator CreateRow480(List<List<MsgBaseNewsData>> _4NewsList)
    {

        //オフセット
        int offsetY = 0;

        //コントロールの高さ調整
        SetContentHeightRowCount480(_4NewsList.Count);


        //データ生成
        foreach (var _4news in _4NewsList)
        {
            StartCoroutine(CreateRow480(Content, offsetY, _4news[0], _4news[1], _4news[2], _4news[3]));
            offsetY++;
        }

        yield return 0;
    }

    public IEnumerator CreateRow480(GameObject Content, float offsetY, MsgBaseNewsData news1, MsgBaseNewsData news2, MsgBaseNewsData news3, MsgBaseNewsData news4)
    {
        Image panel = CreateRowPanel_480(Content, offsetY);

        yield return StartCoroutine(CreateNewsPanel_480(panel.gameObject, -1440, news1));
        yield return StartCoroutine(CreateNewsPanel_480(panel.gameObject, -480, news2));
        yield return StartCoroutine(CreateNewsPanel_480(panel.gameObject, 480, news3));
        yield return StartCoroutine(CreateNewsPanel_480(panel.gameObject, 1440, news4));

        yield return panel;
    }

    /// <summary>
    /// 1行パネルを作成する
    /// </summary>
    /// <returns></returns>
    public Image CreateRowPanel_480(GameObject Content, float offsetY)
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
    /// 記事パネルを生成する
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public IEnumerator CreateNewsPanel_480(GameObject parent, float offsetMinX, MsgBaseNewsData news)
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
        CreateNewsImage_480(panel.gameObject, news);
        StartCoroutine(CreateNewsText480(panel.gameObject, news.TITLE));

        yield return panel;
    }


    /// <summary>
    /// 記事サムネイルを生成する
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public void CreateNewsImage_480(GameObject parent, MsgBaseNewsData news)
    {
        try
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

            image.GetComponent<Image>().sprite = Sprite.Create(LpsImageLoader.CreateTextureFromBinary(news.THUMBNAIL_DATA), new Rect(0, 0, news.THUMBNAIL_WIDTH, news.THUMBNAIL_HEIGHT), Vector2.zero);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    //ニューステキストを生成する
    public IEnumerator CreateNewsText480(GameObject parent, string newsTitle)
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
    public void SetContentHeightRowCount480(int rowCount)
    {
        //高さ計算
        float height = (rowCount * 475);
        
        //サイズ調整
        RectTransform rectTransform = this.Content.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, height);
    }
    #endregion


    //====================================================================
    //
    //  コンテント操作
    //  
    //  200✕200　1列 3個のコンテント作成
    //                         
    //====================================================================
    #region コンテント操作200


    /// <summary>
    /// 4分割のニュースを生成する
    /// </summary>
    /// <param name="NewsList"></param>
    /// <returns></returns>
    public IEnumerator Create3NewsList(List<MsgBaseNewsData> NewsList)
    {
        //4個つづのリストに分ける
        List<List<MsgBaseNewsData>> _3NewsList = SplitRowLine(NewsList, 3);

        //展開
        StartCoroutine(CreateRow200(_3NewsList));

        yield return 0;
    }

    public IEnumerator CreateRow200(List<List<MsgBaseNewsData>> _3NewsList)
    {

        //オフセット
        int offsetY = 0;

        //コントロールの高さ調整
        SetContentHeightRowCount200(_3NewsList.Count);


        //データ生成
        foreach (var _3news in _3NewsList)
        {
            StartCoroutine(CreateRow200(Content, offsetY, _3news[0], _3news[1], _3news[2]));
            offsetY++;
        }

        yield return 0;
    }


    public IEnumerator CreateRow200(GameObject Content, float offsetY, MsgBaseNewsData news1, MsgBaseNewsData news2, MsgBaseNewsData news3)
    {
        Image panel = CreateRowPanel_200(Content, offsetY);

        yield return StartCoroutine(CreateNewsPanel_200(panel.gameObject, -1280, news1));
        yield return StartCoroutine(CreateNewsPanel_200(panel.gameObject, -2, news2));
        yield return StartCoroutine(CreateNewsPanel_200(panel.gameObject, 1276, news3));

        yield return panel;
    }

    /// <summary>
    /// 1行パネルを作成する
    /// </summary>
    /// <returns></returns>
    public Image CreateRowPanel_200(GameObject Content, float offsetY)
    {
        Image panel = UICreator.CreatePanel(Content);

        //サイズ調整
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.offsetMax = new Vector2(0, offsetY * -180);
        rectTransform.offsetMin = new Vector2(0, -180f);
        rectTransform.sizeDelta = new Vector2(0, 180f);

        return panel;
    }

    /// <summary>
    /// 記事パネルを生成する
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public IEnumerator CreateNewsPanel_200(GameObject parent, float offsetMinX, MsgBaseNewsData news)
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
        rectTransform.sizeDelta = new Vector2(638, 0);

        //コンテンツを生成
        CreateNewsImage_200(panel.gameObject, news);
        StartCoroutine(CreateNewsText200(panel.gameObject, news.TITLE));

        yield return panel;
    }


    /// <summary>
    /// 記事サムネイルを生成する
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public void CreateNewsImage_200(GameObject parent, MsgBaseNewsData news)
    {
        try
        {
            //イメージを生成する
            Image image = UICreator.CreateImage(parent);

            //サイズ調整
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1f);
            rectTransform.anchorMax = new Vector2(0, 1f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = new Vector2(245, -45f);
            rectTransform.offsetMin = new Vector2(22.0f, -135f);
            rectTransform.sizeDelta = new Vector2(245, 135);

            image.GetComponent<Image>().sprite = Sprite.Create(LpsImageLoader.CreateTextureFromBinary(news.THUMBNAIL_DATA), new Rect(0, 0, news.THUMBNAIL_WIDTH, news.THUMBNAIL_HEIGHT), Vector2.zero);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    //ニューステキストを生成する
    public IEnumerator CreateNewsText200(GameObject parent, string newsTitle)
    {
        //イメージを生成する
        Text text = UICreator.CreateText(parent);

        //サイズ調整
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.offsetMax = new Vector2(10f, -00f);
        rectTransform.offsetMin = new Vector2(260f, 00f);
        rectTransform.sizeDelta = new Vector2(370f, 140f);

        //テキストセット
        text.text = newsTitle;

        text.fontSize = 24;

        yield return text;
    }


    /// <summary>
    /// コンテントの高さを設定する
    /// </summary>
    public void SetContentHeightRowCount200(int rowCount)
    {
        //高さ計算
        float height = (rowCount * 180);

        //サイズ調整
        RectTransform rectTransform = this.Content.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, height);
    }
    #endregion


    //====================================================================
    //
    //  コンテント操作
    //  
    //  17✕100　1列 4個のコンテント作成
    //                         
    //====================================================================
    #region コンテント操作100

    /// <summary>
    /// 4分割のニュースを生成する
    /// </summary>
    /// <param name="NewsList"></param>
    /// <returns></returns>
    public void Create4SNewsList(List<MsgBaseNewsData> NewsList)
    {
        //パネルインデックス
        int idx = 0;

        //パネル初期化
        InitPanelList();

        //ニュースセット
        foreach (MsgBaseNewsData newsData in NewsList)
        {
            //ニュースセット
            panelList[idx].SetNews(newsData);

            //インクリメント
            idx++;

            //パネルリスト数まで行ったら抜ける。
            if(panelList.Count == idx)
            {
                break;
            }
        }

        //高さ調整
        SetContentHeightRowCount120(NewsList.Count);
    }

    /// <summary>
    /// コンテントの高さを設定する
    /// </summary>
    public void SetContentHeightRowCount120(int newsCount)
    {
        double rowCount = 1;

        rowCount = Math.Ceiling((double)newsCount / 4.0);

        //高さ計算
        double height = (rowCount * 120);

        //サイズ調整
        RectTransform rectTransform = this.Content.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, (float)height);
    }
    #endregion

    #endregion





}