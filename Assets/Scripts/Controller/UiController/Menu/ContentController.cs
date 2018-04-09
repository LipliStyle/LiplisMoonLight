//=======================================================================
//  ClassName : ContentController
//  概要      : コンテントコントローラー
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;

public class ContentController : MonoBehaviour {

    public void SetLock()
    {
        GetComponent<CanvasGroup>().interactable = false;
    }

    public void SetUnlock()
    {
        GetComponent<CanvasGroup>().interactable = true;
    }
}
