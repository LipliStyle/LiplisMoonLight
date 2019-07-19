//====================================================================
//  ClassName : LiplisDataLoader
//  概要      : データをロードし、LiplisSetting、LiplisStatusのシングルトンインスタンスを生成する
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2019 sachin.
//====================================================================
using Assets.Scripts.Data;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Web.Clalis;
using Assets.Scripts.Util;
using System.Collections;

namespace Assets.Scripts.LiplisSystem.MainSystem.StartUp
{
    public class LiplisDataLoader
    {
        //=============================
        //クラリスコントローラー
        ClalisController cc;

        /// <summary>
        /// コンストラクター
        /// 
        /// データ初期化を行う
        /// </summary>
        public LiplisDataLoader()
        {
            //クラリスコントローラーの初期化
            cc = new ClalisController();
        }

        /// <summary>
        /// データロード
        /// </summary>
        public IEnumerator Load()
        {
            //必ず設定ロードを先に呼ぶ！
            //指定キー「LiplisStatus」でリプリスセッティングのインスタンスをロードする
            LiplisSetting.SetInstance(SaveDataSetting.GetClass<LiplisSetting>(LpsDefine.SETKEY_LIPLIS_SETTING, LiplisSetting.Instance));

            //指定キー「LiplisStatus」でリプリスステータスのインスタンスをロードする
            LiplisStatus.SetInstance(SaveDataClalis.GetClass<LiplisStatus>(LpsDefine.SETKEY_LIPLIS_STATUS, LiplisStatus.Instance));

            //指定キー「LiplisStatus」でリプリスステータスのインスタンスをロードする
            LiplisCache.SetInstance(SaveDataCache.GetClass<LiplisCache>(LpsDefine.SETKEY_LIPLIS_CACHE, LiplisCache.Instance));

            //モデルのロード 
            LiplisModels l = LiplisModels.Instance;

            //ログインスタンス
            LiplisTalkLog log = LiplisTalkLog.Instance;

            //データクリア
            LiplisCache.Instance.ImagePath.Clean();

            //データダウンロード
            yield return CoroutineHandler.StartStaticCoroutine(DataCollect());
        }

        /// <summary>
        /// データ収集処理
        /// </summary>
        //private void DataCollect()
        private IEnumerator DataCollect()
        {
            //地域データ取得
            yield return CoroutineHandler.StartStaticCoroutine(cc.DataCollectLocation());

            //本日情報データ取得
            yield return CoroutineHandler.StartStaticCoroutine(cc.DataCollectAnniversaryDays());

            //天気情報収集
            yield return CoroutineHandler.StartStaticCoroutine(cc.DataCollectWether());

            //ニュースリスト取得
            yield return CoroutineHandler.StartStaticCoroutine(cc.SetLastNewsList());

            //ニュースデータ取得
            if (LiplisStatus.Instance.NewTopic.TalkTopicList.Count <= 25)
            {
                yield return CoroutineHandler.StartStaticCoroutine(cc.DataCollectNewTopic());
            }
        }

    }
}
