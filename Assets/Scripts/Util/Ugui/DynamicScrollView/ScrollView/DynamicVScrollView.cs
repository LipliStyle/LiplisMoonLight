//====================================================================
//  ClassName : DynamicVScrollView
//  概要      : 動的垂直スクロールビュー
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

namespace Assets.Scripts.Util.Ugui.DynamicScrollView.ScrollView
{
	/// <summary>
	/// Dynamic Vertical Scroll View
	/// </summary>
	[AddComponentMenu("UI/Dynamic V Scroll View")]
	public class DynamicVScrollView : DynamicScrollView {

		protected override float contentAnchoredPosition    { get { return -this.contentRect.anchoredPosition.y; } set { this.contentRect.anchoredPosition = new Vector2( this.contentRect.anchoredPosition.x, -value ); } }
		protected override float contentSize                { get { return this.contentRect.rect.height; } }
		protected override float viewportSize               { get { return this.viewportRect.rect.height;} }
		protected override float itemSize                   { get { return this.itemPrototype.rect.height;} }

		public override void init () {

			this.direction = Direction.Vertical;
			base.init();
		}
		protected override void Awake() {

			base.Awake();
			this.direction = Direction.Vertical;
		}
		protected override void Start () {

			base.Start();
		}
	}
}
