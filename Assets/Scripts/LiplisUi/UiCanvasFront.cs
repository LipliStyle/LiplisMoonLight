using System;
using System.Collections;
using UnityEngine;

public class UiCanvasFront : MonoBehaviour {

    private bool FlgStart = false;


	// Use this for initialization
	void Start () {
        //3.5秒後に実行する
        StartCoroutine(DelayMethod(1.0f, () =>
        {
            //あいさつ実行
            CtrlTalk.Instance.Greet();                       
        }));
    }

    /// <summary>
    /// 渡された処理を指定時間後に実行する
    /// </summary>
    /// <param name="waitTime">遅延時間[ミリ秒]</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    // Update is called once per frame
    void Update () {

	}
}
