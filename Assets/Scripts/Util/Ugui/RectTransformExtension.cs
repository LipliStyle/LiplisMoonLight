//=======================================================================
//  ClassName : RectTransformExtension
//  概要      : RectTransformの頂点が対象の矩形範囲内にいくつ入ってるか取得する
//
//  LiplisMoonlight
//  Copyright(c) 2017-2019 sachin.
//=======================================================================﻿
using UnityEngine;

namespace Assets.Scripts.Util.Ugui
{
    public static class RectTransformExtension
    {
        /// <summary>
        /// RectTransformの頂点が対象の矩形範囲内にいくつ入ってるか取得する
        /// </summary>
        private static int CountCornersVisibleFrom(RectTransform rectTransform, Rect windowRect)
        {
            Vector3[] objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            int visibleCorners = 0;
            Vector3 tempScreenSpaceCorner;

            for (var i = 0; i < objectCorners.Length; i++)
            {
                tempScreenSpaceCorner = objectCorners[i];
                if (windowRect.Contains(tempScreenSpaceCorner))
                {
                    visibleCorners++;
                }
            }
            return visibleCorners;
        }

        /// <summary>
        /// RectTransformのRectをワールド座標に変換して取得
        /// </summary>
        /// <returns>The world rect.</returns>
        /// <param name="rt">Rt.</param>
        /// <param name="scale">CanvasのScaleサイズ</param>
        public static Rect GetWorldRect(this RectTransform rt, Vector2 scale)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            Vector3 topLeft = corners[0];
            Vector3 bottomRight = corners[2];

            float width = Mathf.Abs(bottomRight.x - topLeft.x);
            float height = Mathf.Abs(topLeft.y - bottomRight.y);
            return new Rect(topLeft.x, topLeft.y, width, height);
        }

        /// <summary>
        /// RectTransformのもつ上下左右の矩形が対象の矩形範囲内に全て映っているかどうか
        /// </summary>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Rect windowRect)
        {
            return CountCornersVisibleFrom(rectTransform, windowRect) == 4;
        }

        /// <summary>
        /// RectTransformのもつ上下左右の矩形が対象の矩形範囲内に一つでも映っているかどうか
        /// </summary>
        public static bool IsVisibleFrom(this RectTransform rectTransform, Rect windowRect)
        {
            var visibleCount = CountCornersVisibleFrom(rectTransform, windowRect);
            return visibleCount > 0;
        }

        /// <summary>
        /// 幅を返します
        /// </summary>
        public static float GetWidth(this RectTransform self)
        {
            return self.sizeDelta.x;
        }

        /// <summary>
        /// 高さを返します
        /// </summary>
        public static float GetHeight(this RectTransform self)
        {
            return self.sizeDelta.y;
        }

        /// <summary>
        /// 幅を設定します
        /// </summary>
        public static void SetWidth(this RectTransform self, float width)
        {
            var size = self.sizeDelta;
            size.x = width;
            self.sizeDelta = size;
        }

        /// <summary>
        /// 高さを設定します
        /// </summary>
        public static void SetHeight(this RectTransform self, float height)
        {
            var size = self.sizeDelta;
            size.y = height;
            self.sizeDelta = size;
        }

        /// <summary>
        /// サイズを設定します
        /// </summary>
        public static void SetSize(this RectTransform self, float width, float height)
        {
            self.sizeDelta = new Vector2(width, height);
        }
    }
}
