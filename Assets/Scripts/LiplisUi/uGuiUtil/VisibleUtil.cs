//====================================================================
//  ClassName : VisibleUtil
//  概要      : ユニティオブジェクトの表示/非表示を設定する
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Com;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LiplisUi
{
    public class VisibleUtil
    {
        /// <summary>
        /// イメージのビジブルをセットする
        /// </summary>
        /// <param name="image"></param>
        /// <param name="visible"></param>
        public static void SetVisible(Image image, bool visible)
        {
            //NULLチェックチェック
            if(UnityNullCheck.IsNull(image))
            {
                return;
            }

            //ビジブル設定
            if(visible)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 255);
            }
            else
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            }
        }

        /// <summary>
        /// テキストのビジブルをセットする
        /// </summary>
        /// <param name="text"></param>
        /// <param name="visible"></param>
        public static void SetVisible(Text text, bool visible)
        {
            //NULLチェックチェック
            if (UnityNullCheck.IsNull(text))
            {
                return;
            }

            if (visible)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 255);
            }
            else
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }

        }

    }
}
