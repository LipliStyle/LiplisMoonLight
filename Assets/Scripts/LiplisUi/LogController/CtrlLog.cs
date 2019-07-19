//=======================================================================
//  ClassName : CtrlSetting
//  概要      : 設定画面コントローラー
//
//　新規設定追加手順
//　　1. 画面に設定UIを追加する。
//　　2. CtrlSetting(本クラス)に設定値のプロパティを追加する
//　　3. UnityのIDE上で、
//　　　 CanvasSettingにアタッチされている、CtrlSettingの追加したパラメータに、
//       対象UIをドラッグドロップする。
//    4. DatSettingにUIに対応するプロパティを追加する。
//　　　 必要であれば、ゲッターも作成(値を読み替える必要がある場合など)
//　　5. DatSettingのコンストラクターに初期化処理を追加する。
//　　6. CtrlSetting.InitWindowに設定処理を追加する。　 
//　　7. CtrlSetting.SaveSettingに設定処理を追加する。
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//=======================================================================﻿

using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.MainSystem;
using Assets.Scripts.Msg;
using UnityEngine;

namespace Assets.Scripts.LiplisUi.LogController
{
    public class CtrlLog : MonoBehaviour
    {
        //=============================
        // レンダリングUIキャッシュ
        public GameObject UiSetting;

        //=============================
        // シングルトンインスタンス
        public static CtrlLog instance;

        //=============================
        // 選択中のトピック
        public MsgTopic NowTopic;

        /// <summary>
        /// 開始時
        /// </summary>
        void Start()
        {
            //シングルトン
            instance = this;

            initWindow();
        }

        /// <summary>
        /// アセイク
        /// </summary>
        void Awake()
        {
            
        }

        /// <summary>
        /// ウインドウの初期化
        /// </summary>
        private void initWindow()
        {
            NowTopic = null;
        }

        /// <summary>
        /// 設定クリック
        /// </summary>
        public void Btn_Log_Click()
        {
            //画面表示
            UiSetting.gameObject.SetActive(true);

            //スクロールビューを更新
            ScrollViewControllerLog.sc.SetNews(LiplisTalkLog.Instance.GetLogList());

            //一番上の話題を選択
            ScrollViewControllerTalk.sc.SetSentence(0);
        }

        /// <summary>
        /// 設定を閉じる
        /// </summary>
        public void Btn_Log_Close_Click()
        {
            UiSetting.gameObject.SetActive(false);
        }

        /// <summary>
        /// 選択中の話題を再度しゃべるボタンを押したときの処理
        /// </summary>
        public void Btn_ReTalk_Click()
        {
            //NULLチェック
            if(NowTopic == null)
            {
                return;
            }

            //次の話題に差し込む
            CtrlTalk.Instance.SetTopicTalkFromLastNewsList(NowTopic.DataKey, ContentCategolyText.GetContentCategoly(NowTopic.TopicClassification));
            
            //画面を閉じる
            UiSetting.gameObject.SetActive(false);
        }

    }
}
