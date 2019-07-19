//=======================================================================
//  ClassName : MenuScrollViewController
//  概要      : メニュースクロールビューコントローラ
//              
//
//  LiplisMoonlight
//  Create 2018/03/11
//
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿
using UnityEngine;

namespace Assets.Scripts.LiplisUi.MenuController
{
    [RequireComponent(typeof(RectTransform))]   // RectTransformコンポーネントを必須にする
    public class MenuScrollViewController : MonoBehaviour
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

}
