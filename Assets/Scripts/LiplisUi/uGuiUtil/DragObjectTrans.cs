//====================================================================
//  ClassName : DragObjectTrans
//  概要      : ドラッグ機能を付与する
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.LiplisSystem.UI
{
    public class DragObjectTrans : MonoBehaviour, IDragHandler
    {
        public Transform m_rectTransform = null;

        private void Reset()
        {
            m_rectTransform = GetComponent<Transform>();
        }

        public void OnDrag(PointerEventData e)
        {
            m_rectTransform.position += new Vector3(e.delta.x, e.delta.y, 0f);
            Debug.Log("x:" + e.delta.x + "y:" + e.delta.y + " mouX" + Input.mousePosition.x);
        }

    }
}
