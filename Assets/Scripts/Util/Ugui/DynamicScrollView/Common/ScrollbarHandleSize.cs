﻿//====================================================================
//  ClassName : ScrollbarHandleSize
//  概要      : スクロールバーハンドルサイズ
//
// 本ソースはMITライセンスの元利用させて頂いております。
//
// 作者 mosframe様
//
// ライセンス表記
// Mosframe/Unity-DynamicScrollView
// https://github.com/Mosframe/Unity-DynamicScrollView/blob/master/LICENSE
//====================================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Util.Ugui.DynamicScrollView.Common
{
    /// <summary>
    /// Scrollbar Handle Size Controller
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollbarHandleSize : UIBehaviour
    {
        public float        maxSize = 1.0f;
        public float        minSize = 0.1f;

        private ScrollRect  scrollRect;

        // Awake

        protected override void Awake() {
            base.Awake();
            this.scrollRect = this.GetComponent<ScrollRect>();
        }

        // OnEnable

        protected override void OnEnable() {
            this.scrollRect.onValueChanged.AddListener( this.onValueChanged );
        }

        // OnDisable

        protected override void OnDisable() {
            this.scrollRect.onValueChanged.RemoveListener( this.onValueChanged );
        }

        // onValueChanged event

        public void onValueChanged( Vector2 value ) {

            var hScrollbar = this.scrollRect.horizontalScrollbar;
            if( hScrollbar!=null ) {
                if( hScrollbar.size > this.maxSize ) {
                    hScrollbar.size = this.maxSize;
                }
                else
                if( hScrollbar.size < this.minSize ) {
                    hScrollbar.size = this.minSize;
                }
            }

            var vScrollbar = this.scrollRect.verticalScrollbar;
            if( vScrollbar!=null ) {
                if( vScrollbar.size > this.maxSize ) {
                    vScrollbar.size = this.maxSize;
                }
                else
                if( vScrollbar.size < this.minSize ) {
                    vScrollbar.size = this.minSize;
                }
            }
        }
    }
}