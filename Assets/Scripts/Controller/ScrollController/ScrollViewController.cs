//=======================================================================
//  ClassName : ScrollViewController
//  概要      : スクロールビューコントローラ
//              
//
//  LiplisLive2D
//  Create 2018/03/11
//
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;

[RequireComponent(typeof(RectTransform))]   // RectTransformコンポーネントを必須にする
public class ScrollViewController : MonoBehaviour
{
    // Rect Transformコンポーネントをキャッシュ
    private RectTransform cachedRectTransform;
    public RectTransform CachedRectTransform
    {
        get
        {
            if (cachedRectTransform == null)
            { cachedRectTransform = GetComponent<RectTransform>(); }
            return cachedRectTransform;
        }
    }

    // ビューのタイトルを取得、設定するプロパティ
    public virtual string Title { get { return ""; } set { } }

}