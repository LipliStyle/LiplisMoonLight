//=======================================================================
//  ClassName : CoverUI
//  概要      : フェード制御用画面
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoverUI : MonoBehaviour {

    private float m_BuffRate = 0.95f;
    private Image m_Image;
    public Image image
    {
        get
        {
            if (m_Image == null)
                m_Image = GetComponent<Image>();
            return m_Image;
        }
    }

    void Awake()
    {
        image.enabled = false;
    }

    public void FadeIn(float duration)
    {
        image.enabled = true;
        image.color = new Color(0, 0, 0, 1);
        image.DOFade(0, duration * m_BuffRate).SetEase(Ease.OutQuad);
    }

    public void FadeOut(float duration)
    {
        image.enabled = true;
        image.color = new Color(0, 0, 0, 0);
        image.DOFade(1, duration * m_BuffRate).SetEase(Ease.InQuad);
    }
}
