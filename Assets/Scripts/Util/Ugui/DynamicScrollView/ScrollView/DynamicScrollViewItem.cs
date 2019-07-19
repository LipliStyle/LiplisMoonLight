//====================================================================
//  ClassName : IDynamicScrollViewItem
//  概要      : スクロールビューアイテムインターフェース
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

    /// <summary>
    /// DynamicScrollView Item interface
    /// </summary>
    public interface IDynamicScrollViewItem {
        void OnUpdateItem( int index );
    }
}
