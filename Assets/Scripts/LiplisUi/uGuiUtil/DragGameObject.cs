//====================================================================
//  ClassName : DragGameObject
//  概要      : ゲームオブジェクトにドラッグ機能を付与する
//              別途コライダーがアタッチされていることが必要
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using UnityEngine;
namespace Assets.Scripts.LiplisUi.uGuiUtil
{
    public class DragGameObject : MonoBehaviour
    {
        //スタート位置を取得
        private Vector3 StartMousePosition;
        private Vector3 StartPosition;


        /// <summary>
        /// マウスクリック時、現在座標を記録
        /// </summary>
        void OnMouseDown()
        {
            //現在座標記録
            this.StartMousePosition = GetMousePointInWorld();
            this.StartPosition = this.transform.position;
        }

        void OnMouseUp()
        {

        }

        void OnMouseDrag()
        {
            //現在マウス位置取得
            Vector3 mousePointInWorld = GetMousePointInWorld();

            //z軸座標は保持
            mousePointInWorld.z = this.transform.position.z;

            //移動結果
            Vector3 result = new Vector3(StartPosition.x + mousePointInWorld.x - StartMousePosition.x,
                StartPosition.y + mousePointInWorld.y - StartMousePosition.y,
                this.transform.position.z);

            this.transform.position = result;
        }

        /// <summary>
        /// マウスポインター座標を取得
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMousePointInWorld()
        {
            Vector3 objectPointInScreen = Camera.main.WorldToScreenPoint(this.transform.position);

            //画面上のポイント位置を取得
            Vector3 mousePointInScreen
                = new Vector3(Input.mousePosition.x,
                              Input.mousePosition.y,
                              objectPointInScreen.z);

            //マウスポイント座標を変換
            Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);

            return mousePointInWorld;
        }
    }
}
