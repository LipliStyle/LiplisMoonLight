//====================================================================
//  ClassName : HtmlColorExtensions
//  概要      : HTMLカラー拡張
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

namespace Assets.Scripts.Util.Ugui.DynamicScrollView.Common
{
	/// <summary>
	/// <see cref="HtmlColor"/> Extensions
	/// </summary>
	public static class HtmlColorExtensions {


		/// <summary> Color32 to HtmlColor </summary>
		public static string toHtmlColor( this Color32 self ) {

			return string.Format("#{0:x02}{1:x02}{2:x02}{3:x02}", self.r, self.g, self.b, self.a );
		}
		/// <summary> Color to HtmlColor </summary>
		public static string toHtmlColor( this Color self ) {

			var color32 = (Color32)self;
			return string.Format("#{0:x02}{1:x02}{2:x02}{3:x02}", color32.r, color32.g, color32.b, color32.a );
		}

		/// <summary> int(RGB) to HtmlColor </summary>
		public static string toHtmlColor( this int self ) {

			return string.Format("#{0:x02}{1:x02}{2:x02}ff", (self & 0x00FF0000) >> 16, (self & 0x0000FF00) >> 8, (self & 0x000000FF) >> 0);
		}

		/// <summary> uint(ARGB) to HtmlColor </summary>
		public static string toHtmlColor( this uint self ) {

			return string.Format("#{0:x02}{1:x02}{2:x02}{3:x02}", (self & 0x00FF0000) >> 16, (self & 0x0000FF00) >> 8, (self & 0x000000FF) >> 0, (self & 0xFF000000) >> 24);
		}
	}
}

