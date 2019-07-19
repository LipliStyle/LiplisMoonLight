
//====================================================================
//  ClassName : DynamicHScrollView
//  概要      : 動的水平スクロールビュー
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
    /// Dynamic Horizontal Scroll View
    /// </summary>
    [AddComponentMenu("UI/Dynamic H Scroll View")]
    public class DynamicHScrollView : DynamicScrollView {

        protected override float contentAnchoredPosition    { get { return this.contentRect.anchoredPosition.x; } set { this.contentRect.anchoredPosition = new Vector2( value, this.contentRect.anchoredPosition.y ); } }
	    protected override float contentSize                { get { return this.contentRect.rect.width; } }
	    protected override float viewportSize               { get { return this.viewportRect.rect.width; } }
	    protected override float itemSize                   { get { return this.itemPrototype.rect.width;} }

        public override void init () {

            this.direction = Direction.Horizontal;
            base.init();
        }
        protected override void Awake() {

            base.Awake();
            this.direction = Direction.Horizontal;
        }
        protected override void Start () {

            base.Start();
        }
    }
}
