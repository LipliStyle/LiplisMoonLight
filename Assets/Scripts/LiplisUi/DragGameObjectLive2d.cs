//====================================================================
//  ClassName : DragGameObject
//  概要      : ゲームオブジェクトにドラッグ機能を付与する
//              別途コライダーがアタッチされていることが必要
//
//              Live2d 3.0のキャラクターのドラッグ移動は本クラスで行っている。
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using UnityEngine;

namespace Assets.Scripts.LiplisUi.uGuiUtil
{
    public class DragGameObjectLive2d : MonoBehaviour
    {
        //=============================
        //スタート位置を取得
        private Vector3 StartMousePosition;
        private Vector3 StartPosition;

        //=============================
        //最低移動量
        private const float MOVE_VALUE_MIN = 0.2f;

        //=============================
        //移動可否
        private bool flgMove;

        /// <summary>
        /// マウスクリック時、現在座標を記録
        /// </summary>
        void OnMouseDown()
        {
            //現在座標記録
            this.StartMousePosition = GetMousePointInWorld();
            this.StartPosition = this.transform.position;


            if (Mathf.Abs(this.transform.position.x - StartMousePosition.x) > 1.0)
            {
                flgMove = false;
            }
            else
            {
                flgMove = true;
            }
        }

        /// <summary>
        /// ドラッグ移動
        /// </summary>
        void OnMouseDrag()
        {
            //移動可否チェック
            if (!flgMove)
            {
                return;
            }

            //現在マウス位置取得
            Vector3 mousePointInWorld = GetMousePointInWorld();

            //z軸座標は保持
            mousePointInWorld.z = this.transform.position.z;

            //移動量を算出
            float moveValueX = mousePointInWorld.x - StartMousePosition.x;
            float moveValueY = mousePointInWorld.y - StartMousePosition.y;

            //閾値でフィルターする理由は、クリックでも動いてしまうこと、
            //キャラクターの場所入れ替え時、この処理によって座標が上書きされて移動できない場合があるため。
            //一定以上移動していたら、アタッチされているオブジェクトを移動させる
            if (Mathf.Abs(moveValueX) > MOVE_VALUE_MIN || Mathf.Abs(moveValueY) > MOVE_VALUE_MIN)
            {
                //移動結果
                Vector3 result = new Vector3(StartPosition.x + moveValueX,
                                            StartPosition.y + moveValueY,
                                            this.transform.position.z);

                this.transform.position = result;
            }
        }

        /// <summary>
        /// マウスポインター座標を取得
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMousePointInWorld()
        {
            //スクリーン上のマウスポインター座標を取得　
            Vector3 objectPointInScreen = Camera.main.WorldToScreenPoint(this.transform.position);

            //画面上のポイント位置を取得
            Vector3 mousePointInScreen
                = new Vector3(Input.mousePosition.x,
                              Input.mousePosition.y,
                              objectPointInScreen.z);

            //マウスポイント座標を変換
            return Camera.main.ScreenToWorldPoint(mousePointInScreen);
        }
    }
}
