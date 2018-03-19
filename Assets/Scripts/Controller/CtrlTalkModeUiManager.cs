//=======================================================================
//  ClassName : PageControl
//  概要      : スクロールページコントローラ
//              
//
//  LiplisLive2D
//  Create 2018/03/11
//
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;
using System;
using System.Collections;

public class CtrlTalkModeUiManager : MonoBehaviour
{ 
	[SerializeField] private CoverUI coverUI;
    [SerializeField] private ContentCategoly m_CurrentContentCategoly = ContentCategoly.home;
    [SerializeField] private ContentController[] m_Contents;


    private float m_FadeInDuration = 0.5f;
    private float m_FadeOutDuration = 0.2f;

    //====================================================================
    //
    //                          初期化処理
    //                         
    //====================================================================
    #region 初期化処理

    /// <summary>
    /// アウェイク
    /// </summary>
    void Awake()
    {
        foreach (var content in m_Contents)
        {
            content.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// スタート処理
    /// </summary>
    void Start () {
        StartCoroutine(SwitchContent(m_CurrentContentCategoly));
    }

    #endregion

    public enum ContentCategoly
    {
        home = 0,
        omakase = 1,
        news = 2
    }


    IEnumerator SwitchContent(ContentCategoly nextContent)
    {
        if (m_CurrentContentCategoly != nextContent)
        {
            coverUI.FadeOut(m_FadeOutDuration);
            yield return new WaitForSeconds(m_FadeOutDuration);

            m_Contents[(int)m_CurrentContentCategoly].gameObject.SetActive(false);
            m_CurrentContentCategoly = nextContent;
            m_Contents[(int)m_CurrentContentCategoly].gameObject.SetActive(true);

            yield return new WaitForSeconds(m_FadeInDuration);
            coverUI.FadeIn(m_FadeInDuration);

        }
        else
        {

            m_Contents[(int)m_CurrentContentCategoly].gameObject.SetActive(true);
            yield break;
        }
    }
    //====================================================================
    //
    //                          メニュー操作
    //                         
    //====================================================================
    #region メニュー操作

    /// <summary>
    /// ホームボタンクリック
    /// </summary>
    public void Btn_Home_Click()
    {
        Debug.Log("GameCtlr BtnHome_Click");
        StartCoroutine(SwitchContent(ContentCategoly.home));
    }

    /// <summary>
    /// おまかせボタンクリック
    /// </summary>
    public void Btn_Omakase_Click()
    {
        Debug.Log("GameCtlr BtnOmakase_Click");
        StartCoroutine(SwitchContent(ContentCategoly.omakase));
    }

    /// <summary>
    /// ニュースボタンクリック
    /// </summary>
    public void Btn_News_Click()
    {
        Debug.Log("GameCtlr Btn_News_Click");
        StartCoroutine(SwitchContent(ContentCategoly.news));
    }

    #endregion  
}
