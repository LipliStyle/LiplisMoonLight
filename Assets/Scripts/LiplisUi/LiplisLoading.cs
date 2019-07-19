//====================================================================
//  ClassName : LiplisLoading
//  概要      : ナウローディング画面の制御
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LiplisLoading : MonoBehaviour {

    //　非同期動作で使用するAsyncOperation
    private AsyncOperation async;
    //　シーンロード中に表示するUI画面
    [SerializeField]
    private GameObject loadUI;
    //　読み込み率を表示するスライダー
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private float seconds;

    [SerializeField]
    private Text text;

    //　前のUpdateの時の秒数
    private float oldSeconds;


    // Use this for initialization
    void Start () {
        loadUI.SetActive(true);
        StartCoroutine(LoadScene());
    }
	
	// Update is called once per frame
	void Update () {
        seconds += Time.deltaTime;

        //　値が変わった時だけテキストUIを更新
        if (seconds >= 15)
        {
            slider.value = 1;
        }
        else
        {
            slider.value = seconds / 15f;
        }

        text.text = seconds.ToString();
    }

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
