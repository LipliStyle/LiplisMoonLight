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
    [SerializeField] private ContentCategoly m_CurrentContentCategoly = ContentCategoly.WorldMap;
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
        WorldMap = 0,
        PartiyEdit = 1,
        Character = 2,
        Battle = 3
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

    public void SwitchContentToWorldMap()
    {
        StartCoroutine(SwitchContent(ContentCategoly.WorldMap));
    }

    public void SwitchContentToPartiyEdit()
    {
        StartCoroutine(SwitchContent(ContentCategoly.PartiyEdit));
    }

    public void SwitchContentToCharacter()
    {
        StartCoroutine(SwitchContent(ContentCategoly.Character));
    }

    public void SwitchContentToBattle()
    {
        StartCoroutine(SwitchContent(ContentCategoly.Battle));
    }
}
