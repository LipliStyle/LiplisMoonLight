//====================================================================
//  ClassName : LiplisLoading
//  概要      : ナウローディング画面の制御
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2019 sachin.
//====================================================================
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.LiplisSystem.MainSystem.StartUp
{
    public class LiplisLoading : MonoBehaviour
    {
        //　非同期動作で使用するAsyncOperation
        private AsyncOperation async;

        //　シーンロード中に表示するUI画面
        [SerializeField]
        private GameObject loadUI;


        //　前のUpdateの時の秒数
        private float oldSeconds;


        /// <summary>
        /// スタート時
        /// </summary>
        IEnumerator Start()
        {
            //バックグラウンドをアクティブにする
            loadUI.SetActive(true);

            //データロード
            yield return LoadData();

            //次のシーンに遷移
            StartCoroutine(LoadScene());
        }

        /// <summary>
        /// データをロードする
        /// </summary>
        IEnumerator LoadData()
        {
            //ローディングクラス インスタンス化
            LiplisDataLoader loader = new LiplisDataLoader();

            //ロード処理実行
            yield return loader.Load();
        }

        /// <summary>
        /// メインシーンを呼び出す
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadScene()
        {
            // シーンの読み込みをする
            async = SceneManager.LoadSceneAsync("Main");

            while (!async.isDone)
            {
                yield return null;
            }
        }

    }

}
