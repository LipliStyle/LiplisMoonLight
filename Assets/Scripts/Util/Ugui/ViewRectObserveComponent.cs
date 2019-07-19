//=======================================================================
//  ClassName : ViewRectObserveComponent
//  概要      : Rect(矩形)内に入っているかどうかを監視するコンポーネント
//
//  LiplisMoonlight
//  Copyright(c) 2017-2019 sachin.
//=======================================================================﻿
using System;
using UnityEngine;

namespace Assets.Scripts.Util.Ugui
{
    /// <summary>
    /// Rect(矩形)内に入っているかどうかを監視するコンポーネント
    /// </summary>
    public class ViewRectObserveComponent : MonoBehaviour
    {

        /// <summary>
        /// 描画されているかどうかをチェックしたいRectTransform
        /// </summary>
        [SerializeField]
        private RectTransform _visibleZoneRectTransform;

        /// <summary>
        /// 初期化済みか
        /// </summary>
        private bool _initialized;
        /// <summary>
        /// 描画範囲にはいっているかどうかの対象のRect(矩形)
        /// </summary>
        private Rect _targetViewRect;

        /// <summary>
        /// 範囲内に出入りしたときに呼ばれるコールバック
        /// </summary>
        private Action<bool> _onEnteredViewRect;
        /// <summary>
        /// 範囲内にいるかどうか
        /// </summary>
        private bool _isInViewRect;

        void LateUpdate()
        {
            if (!_initialized || !_visibleZoneRectTransform || _onEnteredViewRect == null) return;
            var preFrameState = _isInViewRect;
            _isInViewRect = _visibleZoneRectTransform.IsVisibleFrom(_targetViewRect);
            if (_isInViewRect != preFrameState)
            {
                _onEnteredViewRect.Invoke(_isInViewRect);
            }
        }

        public void Initialize(RectTransform targetRect, Action<bool> onEnteredViewRect)
        {
            _targetViewRect = targetRect.GetWorldRect(Vector2.one);
            _onEnteredViewRect = onEnteredViewRect;
            _initialized = true;

            _isInViewRect = _visibleZoneRectTransform.IsVisibleFrom(_targetViewRect);
            if (_onEnteredViewRect != null)
            {
                _onEnteredViewRect.Invoke(_isInViewRect);
            }
        }
    }
}
