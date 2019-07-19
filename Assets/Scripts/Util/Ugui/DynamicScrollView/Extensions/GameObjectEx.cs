//====================================================================
//  ClassName : GameObjectEx
//  概要      : ゲームオブジェクト拡張
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

namespace Assets.Scripts.Util.Ugui.DynamicScrollView.Extensions
{
    /// <summary>
    /// GameObject Extention
    /// </summary>
    public static class GameObjectEx
    {
        /// <summary>
        /// set layer
        /// </summary>
        public static void setLayer( this GameObject self, int layer, bool includeChildren = true )
        {
            self.layer = layer;
            if( includeChildren )
            {
                var children = self.transform.GetComponentsInChildren<Transform>(true);
                for( var c=0; c<children.Length; ++c ) {
                    children[c].gameObject.layer = layer;
                }
            }
        }
    }
}