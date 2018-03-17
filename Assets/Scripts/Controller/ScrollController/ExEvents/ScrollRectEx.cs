//=======================================================================
//  ClassName : ScrollRectEx
//  概要      : スクロールクリックイベント拡張
//              
//
//  LiplisLive2D
//  Create 2018/03/11
//
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectEx : ScrollRect
{
    public ScrollRectExEvent onClicked;
    const float MAX_DISTANCE = 20.0F;

    private Vector2 startPos;

    new void Start()
    {
        base.Start();
        if (onClicked == null)
        {
            onClicked = new ScrollRectExEvent();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        float distance = Vector2.Distance(startPos, eventData.position);
        if (distance < MAX_DISTANCE)
        {
            onClicked.Invoke(eventData);
        }
    }

}