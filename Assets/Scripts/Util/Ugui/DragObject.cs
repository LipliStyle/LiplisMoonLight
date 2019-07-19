//====================================================================
//  ClassName : DragObject
//  概要      : ドラッグ機能を付与する
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Util.Ugui
{
    public class DragObject : MonoBehaviour, IDragHandler
    {      
        public RectTransform m_rectTransform = null;

        private void Reset()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData e)
        {
            m_rectTransform.localPosition += new Vector3(e.delta.x, e.delta.y, 0f);
        }

    }
}
