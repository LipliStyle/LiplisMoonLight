//====================================================================
//  ClassName : DynamicScrollViewEditor
//  概要      : 動的スクロールビューエディタ
//
// 本ソースはMITライセンスの元利用させて頂いております。
//
// 作者 mosframe様
//
// ライセンス表記
// Mosframe/Unity-DynamicScrollView
// https://github.com/Mosframe/Unity-DynamicScrollView/blob/master/LICENSE
//====================================================================
using Assets.Scripts.Util.Ugui.DynamicScrollView.ScrollView;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Util.Ugui.DynamicScrollView.Editor
{
    /// <summary>
    /// <see cref="DynamicScrollView"/> Editor
    /// </summary>
    public class DynamicScrollViewEditor {


        [MenuItem( "GameObject/UI/Dynamic H Scroll View" )]
        public static void CreateHorizontal () {

            var go = new GameObject( "Horizontal Scroll View", typeof(RectTransform) );
            go.transform.SetParent( Selection.activeTransform, false );
            go.AddComponent<DynamicHScrollView>().init();
        }

        [MenuItem( "GameObject/UI/Dynamic V Scroll View" )]
        public static void CreateVertical () {

            var go = new GameObject( "Vertical Scroll View", typeof(RectTransform) );
            go.transform.SetParent( Selection.activeTransform, false );
            go.AddComponent<DynamicVScrollView>().init();
        }
    }
}
