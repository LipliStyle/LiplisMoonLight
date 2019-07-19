//====================================================================
//  ClassName : DynamicScrollViewItemExample
//  概要      : スクロールビューアイテムイグザンプル
//
// 本ソースはMITライセンスの元利用させて頂いております。
//
// 作者 mosframe様
//
// ライセンス表記
// Mosframe/Unity-DynamicScrollView
// https://github.com/Mosframe/Unity-DynamicScrollView/blob/master/LICENSE
//====================================================================
namespace Assets.Scripts.Util.Ugui.DynamicScrollView.ScrollView
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class DynamicScrollViewItemExample : UIBehaviour, IDynamicScrollViewItem
    {
        private readonly Color[] colors = new Color[] {
            Color.cyan,
            Color.green,
        };

        public Text title;
        public Image background;

        public void OnUpdateItem(int index)
        {

            this.title.text = string.Format("Name{0:d3}", (index + 1));
            this.background.color = this.colors[Mathf.Abs(index) % this.colors.Length];

           
        }
    }
}