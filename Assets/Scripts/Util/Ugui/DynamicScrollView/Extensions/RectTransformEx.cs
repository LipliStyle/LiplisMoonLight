//====================================================================
//  ClassName : RectTransformEx
//  概要      : レクトトランスフォーム拡張
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
    /// RectTransform Extention
    /// </summary>
    public static class RectTransformEx
    {
        /// <summary>
        /// フルサイズに設定
        /// </summary>
        public static RectTransform setFullSize( this RectTransform self ) {

            self.sizeDelta  = new Vector2(0.0f,0.0f);
            self.anchorMin  = new Vector2(0.0f,0.0f);
            self.anchorMax  = new Vector2(1.0f,1.0f);
            self.pivot      = new Vector2(0.5f,0.5f);
            return self;
        }
        
        /// <summary>
        /// サイズ取得
        /// </summary>
        public static Vector2 getSize( this RectTransform self ) {
            return self.rect.size;
        }

        /// <summary>
        /// サイズ設定
        /// </summary>
        public static void setSize( this RectTransform self, Vector2 newSize ) {

            var pivot   = self.pivot;
            var dist    = newSize - self.rect.size;
            self.offsetMin = self.offsetMin - new Vector2( dist.x * pivot.x, dist.y * pivot.y );
            self.offsetMax = self.offsetMax + new Vector2( dist.x * (1f - pivot.x), dist.y * (1f - pivot.y) );
        }
       
        /// <summary>
        /// 左側の基準サイズに設定
        /// </summary>
        public static RectTransform setSizeFromLeft( this RectTransform self, float rate ) {

            self.setFullSize();

            var width = self.rect.width;

            self.anchorMin  = new Vector2(0.0f,0.0f);
            self.anchorMax  = new Vector2(0.0f,1.0f);
            self.pivot      = new Vector2(0.0f,1.0f);
            self.sizeDelta  = new Vector2(width*rate,0.0f);

            return self;
        }

        /// <summary>
        /// 右側の基準にサイズ設定
        /// </summary>
        public static RectTransform setSizeFromRight( this RectTransform self, float rate ) {

            self.setFullSize();

            var width = self.rect.width;

            self.anchorMin  = new Vector2(1.0f,0.0f);
            self.anchorMax  = new Vector2(1.0f,1.0f);
            self.pivot      = new Vector2(1.0f,1.0f);
            self.sizeDelta  = new Vector2(width*rate,0.0f);

            return self;
        }

        /// <summary>
        /// 上側基準にサイズ設定
        /// </summary>
        public static RectTransform setSizeFromTop( this RectTransform self, float rate ) {

            self.setFullSize();

            var height = self.rect.height;

            self.anchorMin  = new Vector2(0.0f,1.0f);
            self.anchorMax  = new Vector2(1.0f,1.0f);
            self.pivot      = new Vector2(0.0f,1.0f);
            self.sizeDelta  = new Vector2(0.0f,height*rate);

            return self;
        }

        /// <summary>
        /// 下側基準にサイズ設定
        /// </summary>
        public static RectTransform setSizeFromBottom( this RectTransform self, float rate ) {

            self.setFullSize();

            var height = self.rect.height;

            self.anchorMin  = new Vector2(0.0f,0.0f);
            self.anchorMax  = new Vector2(1.0f,0.0f);
            self.pivot      = new Vector2(0.0f,0.0f);
            self.sizeDelta  = new Vector2(0.0f,height*rate);

            return self;
        }
        
        /// <summary>
        /// オフセット設定
        /// </summary>
        public static void setOffset( this RectTransform self, float left, float top, float right, float bottom ) {

            self.offsetMin = new Vector2( left, top );
            self.offsetMax = new Vector2( right, bottom );
        }

        /// <summary>
        /// スクリーンが対象矩形の中に含まれているか
        /// </summary>
        public static bool inScreenRect( this RectTransform self, Vector2 screenPos ) {

            var canvas = self.GetComponentInParent<Canvas>();
            switch( canvas.renderMode )
            {
            case RenderMode.ScreenSpaceCamera:
                {
                    var camera = canvas.worldCamera;
                    if( camera != null )
                    {
                        return RectTransformUtility.RectangleContainsScreenPoint( self, screenPos, camera );
                    }
                }
                break;
            case RenderMode.ScreenSpaceOverlay:
                return RectTransformUtility.RectangleContainsScreenPoint( self, screenPos );
            case RenderMode.WorldSpace:
                return RectTransformUtility.RectangleContainsScreenPoint( self, screenPos );
            }
            return false;
        }
        
        /// <summary>
        /// 他の矩形が対象矩形の中に含まれているか
        /// </summary>
        public static bool inScreenRect( this RectTransform self, RectTransform rectTransform ) {

            var rect1 = getScreenRect( self );
            var rect2 = getScreenRect( rectTransform );
            return rect1.Overlaps( rect2 );
        }
        
        /// <summary>
        /// スクリーンの矩形を得る
        /// </summary>
        public static Rect getScreenRect( this RectTransform self ) {

            var rect = new Rect();
            var canvas = self.GetComponentInParent<Canvas>();
            var camera = canvas.worldCamera;
            if( camera != null )
            {
                var corners = new Vector3[4];
                self.GetWorldCorners( corners );
                rect.min = camera.WorldToScreenPoint( corners[0] );
                rect.max = camera.WorldToScreenPoint( corners[2] );
            }
            return rect;
        }
    }
}