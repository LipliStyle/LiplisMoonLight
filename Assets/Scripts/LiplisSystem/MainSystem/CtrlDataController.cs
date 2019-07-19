//====================================================================
//  ClassName : CtrlDataController
//  概要      : データコントローラー
//              定期的なデータ取得は本クラスで行う。
//              
//  LiplisMoonlight
//  Copyright(c) 2017-2019 sachin.
//====================================================================
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Web.Clalis;
using Assets.Scripts.LiplisUi.TopicController;
using System.Collections;
using UnityEngine;

public class CtrlDataController : MonoBehaviour
{
    //=============================
    //クラリスコントローラー
    ClalisController cc;

    //=====================================
    // シングルトンインスタンス
    public static CtrlDataController instance;

    ///=============================
    ///背景ゲームオブジェクト
    GameObject BackGround;

    ///=============================
    ///タイムアウト時間定義
    private float setTimeOut = 60f;

    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
    {
        //クラリスコントローラーの初期化
        cc = new ClalisController();

        //自インスタンス取得
        instance = this;

        //定周期ループスタート
        StartCoroutine(DacaCollectTimerTick());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// データ収集ループ　
    /// TIME_OUTに設定した周期で回る
    /// </summary>
    /// <returns></returns>
    IEnumerator DacaCollectTimerTick()
    {
        while (true)
        {
            //データ収集処理
            StartCoroutine(DataCollect());

            //トピック収集処理
            StartCoroutine(DataCollectTopic());

            //非同期待機
            yield return new WaitForSeconds(setTimeOut);
        }
    }

    /// <summary>
    /// データ収集処理
    /// </summary>
    //private void DataCollect()
    private IEnumerator DataCollect()
    {
        //地域データ取得
        yield return StartCoroutine(cc.DataCollectLocation());

        //本日情報データ取得
        yield return StartCoroutine(cc.DataCollectAnniversaryDays());

        //天気情報収集
        yield return StartCoroutine(cc.DataCollectWether());

        //ニュースリスト取得
        yield return this.StartCoroutine(cc.SetLastNewsList());
    }


    /// <summary>
    /// トピックを収集する
    /// </summary>
    /// <returns></returns>
    private IEnumerator DataCollectTopic()
    {
        //ニュースデータ収集
        yield return StartCoroutine(cc.DataCollectNewTopic());

        //ニュースタブ更新
        CtrlGameController.instance.CreateTbt();
    }
    
}
