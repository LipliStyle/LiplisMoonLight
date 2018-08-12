//=======================================================================
//  ClassName : GlobalCoroutine
//  概要      : アクティブ状態にかかわらずコルーチンを実行する
//
//  LiplisLive2DSystem
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.LiplisSystem.Web;

namespace Assets.Scripts.Utils
{
    public class GlobalCoroutine : MonoBehaviour
    {

        public static void Go(IEnumerator coroutine)
        {
            GameObject obj = new GameObject();     // コルーチン実行用オブジェクト作成
            obj.name = "GlobalCoroutine";

            GlobalCoroutine component = obj.AddComponent<GlobalCoroutine>();
            if (component != null)
            {
                component.StartCoroutine(component.Do(coroutine));
            }
        }

        IEnumerator Do(IEnumerator src)
        {
            while (src.MoveNext())
            {               // コルーチンの終了を待つ
                yield return null;
            }

            Destroy(this.gameObject);              // コルーチン実行用オブジェクトを破棄
        }

        /// <summary>
        /// 指定したラウイメージに画像をダウンロードする。
        /// ログ画面が非表示でもダウンロード可能
        /// </summary>
        /// <param name="ImgThumbnail"></param>
        /// <param name="empty"></param>
        /// <param name="thumbnailUrl"></param>
        public static void GoWWW(RawImage ImgThumbnail, Texture empty, string thumbnailUrl)
        {
            GameObject obj = new GameObject();     // コルーチン実行用オブジェクト作成
            obj.name = "GlobalCoroutine";

            GlobalCoroutine component = obj.AddComponent<GlobalCoroutine>();
            if (component != null)
            {
                component.StartCoroutine(component.UpdateThumbnail(ImgThumbnail,empty,thumbnailUrl));
            }
        }

        /// <summary>
        /// サムネダウンロード
        /// </summary>
        /// <param name="ImgThumbnail"></param>
        /// <param name="empty"></param>
        /// <param name="thumbnailUrl"></param>
        /// <returns></returns>
        public IEnumerator UpdateThumbnail(RawImage ImgThumbnail, Texture empty, string thumbnailUrl)
        {
            //登録されていない場合は、ダウンロード

            using (WWW www = new WWW(ThumbnailUrl.CreateListThumbnailUrl(thumbnailUrl)))
            {
                // 画像ダウンロード完了を待機
                while (www.MoveNext())
                {// コルーチンの終了を待つ
                    yield return null;
                }


                if (!string.IsNullOrEmpty(www.error))
                {
                    ImgThumbnail.texture = empty;
                }

                yield return new WaitForSeconds(0.5f);

                try
                {


                    ImgThumbnail.texture = www.texture;
                }
                catch (Exception ex)
                {
                    ImgThumbnail.texture = empty;
                }
            }

            Destroy(this.gameObject);
        }
    }
}