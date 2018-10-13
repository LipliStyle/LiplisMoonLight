//====================================================================
//  ClassName : Tone
//  概要      : キャラクター口調データ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Model.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Assets.Scripts.LiplisSystem.Model
{
    public class LiplisTone
    { 
        ///=============================
        ///トーンディクショナリ
        Dictionary<string, List<ToneDefinition>> ToneDictionary;

        //====================================================================
        //
        //                          初期化処理
        //                         
        //====================================================================
        #region 初期化処理

        /// <summary>
        /// コンストラクター
        /// </summary>
        public LiplisTone()
        {
            this.ToneDictionary = new Dictionary<string, List<ToneDefinition>>();
        }
        public LiplisTone(LiplisToneSetting toneSetting)
        {
            //トーンディクショナリー登録
            InitToneDictionary(toneSetting);
        }

        /// <summary>
        /// トーン設定の初期化
        /// トーン設定ファイルのインスタンスからロードする
        /// </summary>
        /// <param name="toneSetting"></param>
        public void InitToneDictionary(LiplisToneSetting toneSetting)
        {
            this.ToneDictionary = new Dictionary<string, List<ToneDefinition>>();

            //トーン設定を回して1件づつ登録する。
            foreach (ToneSetting tone in toneSetting.ToneList)
            {
                //読み出した変換設定を登録する。
                AddToneData(tone.befor, tone.after);
            }

        }

        #endregion

        //====================================================================
        //
        //                             データロード
        //                         
        //====================================================================
        #region データロード

        /// <summary>
        /// 口調データを追加する
        /// </summary>
        public void AddToneData(string befor, string after)
        {
            if (ToneDictionary.ContainsKey(befor))
            {
                //トーン追加
                this.ToneDictionary[befor].Add(new ToneDefinition(befor, after, new Regex(befor)));
            }
            else
            {
                //新規トーンデータ作成
                List<ToneDefinition> toneList = new List<ToneDefinition>();

                //トーン追加
                toneList.Add(new ToneDefinition(befor, after, new Regex(befor, RegexOptions.Compiled)));

                //トーンディクショナリに新規登録
                this.ToneDictionary.Add(befor, toneList);
            }
        }

        #endregion

        //====================================================================
        //
        //                          パブリックメソッド
        //                         
        //====================================================================
        #region パブリックメソッド

        /// <summary>
        /// convert
        /// 語尾変換
        /// </summary>
        /// <param name="target"></param>
        public string Convert(string target)
        {
            string result = target;

            try
            {
                //語尾変換
                foreach (KeyValuePair<string, List<ToneDefinition>> kv in ToneDictionary)
                {
                    //変換対象文字列が含まれているかチェック
                    //含まれていたら置換
                    ToneDefinition toneDef = kv.Value.GetAtRandom();

                    //取得したアフター文字列がNULLなら次
                    if (toneDef == null) { continue; }

                    //取得したアフターに正規表現で置換
                    result = toneDef.Rx.Replace(result, toneDef.After);
                }
                return result;
            }
            catch
            {
                return target;
            }
        }




        #endregion
    }

    /// <summary>
    /// トーン定義
    /// </summary>
    public class ToneDefinition
    {
        public Regex Rx { get; set; }
        public string After { get; set; }
        public string Befor { get; set; }

        public ToneDefinition(string Befor, string After, Regex Rx)
        {
            this.Rx = Rx;
            this.After = After;
            this.Befor = Befor;
        }


    }

}