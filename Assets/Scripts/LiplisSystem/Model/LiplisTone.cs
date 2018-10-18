//====================================================================
//  ClassName : Tone
//  概要      : キャラクター口調データ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Model.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Model
{
    public class LiplisTone
    {
        ///=============================
        ///正規表現定義
        public const string REGEX_END                             = @"(、|。|｡|．|！|!|？|\?|\)|」|』|…|―|$|・.|\s{2})";
        public const string REGEX_END_CHINESE_CHARACTER           = @"([\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}\p{IsCJKUnifiedIdeographsExtensionA}]|[\uD840-\uD869][\uDC00-\uDFFF]|\uD869[\uDC00-\uDEDF])";
        public const string REGEX_END_CHINESE_CHARACTER_ADJECTIVE = @"([\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}\p{IsCJKUnifiedIdeographsExtensionA}]|[\uD840-\uD869][\uDC00-\uDFFF]|\uD869[\uDC00-\uDEDF])い";
        public const string REGEX_ALPHABET                        = @"([a-zA-Zａ-ｚＡ-Ｚ])";
        public const string REGEX_NUMBER                          = @"(\d)";
        public const string REGEX_KATAKANA                        = @"([\p{IsKatakana}\u31F0-\u31FF\u3099-\u309C\uFF65-\uFF9F])";
        public const string REGEX_HIRAGANA                        = @"([\p{IsHiragana}\u30FC\u30A0])";
        public const string REGEX_SYMBOL                            = @"([！”＃＄％＆’（）＝～｜‘｛＋＊｝＜＞？＿－＾￥＠「；：」、。・ -/:-@\[-\`\{-\~])";
        public const string DOLL2                                 = @"$2";

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
                AddToneData(tone.befor, tone.after, tone.type);
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
        public void AddToneData(string befor, string after, string type)
        {
            //トーンディフィニションの生成
            List<ToneDefinition> toneDefinition = CreateToneDefinition(befor, after, type);
            
            //リスト数が0ならリターン
            if(toneDefinition.Count == 0)
            {
                return;
            }

            //キーチェックし、追加
            if (ToneDictionary.ContainsKey(befor))
            {
                //トーン追加
                this.ToneDictionary[befor].AddRange(toneDefinition);
            }
            else
            {
                //新規トーンデータ作成
                List<ToneDefinition> toneList = new List<ToneDefinition>();

                //トーン追加
                toneList.AddRange(toneDefinition);

                //トーンディクショナリに新規登録
                this.ToneDictionary.Add(befor, toneList);
            }
        }


        /// <summary>
        /// タイプ別にビフォーを生成する
        /// </summary>
        /// <param name="befor"></param>
        /// <param name="type"></param>
        private List<ToneDefinition> CreateToneDefinition(string befor,string after, string type)
        {
            //トーンタイプを変換する
            TONE_TYPE tType = Type2ToneType(type);

            List<ToneDefinition> resList = new List<ToneDefinition>();

            if (tType == TONE_TYPE.REPLACE)
            {
                //単純置き換え
                resList.Add(new ToneDefinition(tType, befor, after, befor, after, null));
            }
            else if (tType == TONE_TYPE.REGEX)
            {
                //正規表現置換
                resList.Add(new ToneDefinition(tType, befor, after, befor, after, new Regex(befor,RegexOptions.Compiled)));
            }
            else if (tType == TONE_TYPE.END)
            {
                //終端

                //終端の正規表現追加
                string fixBefor = AddKakko(befor) + REGEX_END;

                //終端文字の置き換え追加
                string fixAfter = after + DOLL2;

                resList.Add(new ToneDefinition(tType, befor, after, fixBefor, fixAfter, new Regex(fixBefor, RegexOptions.Compiled)));
            }
            else if (tType == TONE_TYPE.END_CHINESE_CHARACTER)
            {
                //終端 漢字

                //終端漢字の正規表現追加
                string fixBefor = REGEX_END_CHINESE_CHARACTER + REGEX_END;

                //終端文字の置き換え追加
                string fixAfter = after + DOLL2;

                resList.Add(new ToneDefinition(tType, befor, after, fixBefor, fixAfter, new Regex(fixBefor, RegexOptions.Compiled)));


                //終端漢字形容詞の正規表現追加
                string fixBefor2 = REGEX_END_CHINESE_CHARACTER_ADJECTIVE + REGEX_END;

                //終端文字の置き換え追加
                string fixAfter2 = after + DOLL2;

                resList.Add(new ToneDefinition(tType, befor, after, fixBefor2, fixAfter2, new Regex(fixBefor2, RegexOptions.Compiled)));
            }
            else if (tType == TONE_TYPE.END_NUMBER)
            {
                //終端 数字

                //終端数字の正規表現追加
                string fixBefor = REGEX_NUMBER + REGEX_END;

                //終端文字の置き換え追加
                string fixAfter = after + DOLL2;

                resList.Add(new ToneDefinition(tType, befor, after, fixBefor, fixAfter, new Regex(fixBefor, RegexOptions.Compiled)));
            }
            else if (tType == TONE_TYPE.END_ALPHABET)
            {
                //終端 アルファベット

                //終端アルファベットの正規表現追加
                string fixBefor = REGEX_ALPHABET + REGEX_END;

                //終端文字の置き換え追加
                string fixAfter = after + DOLL2;

                resList.Add(new ToneDefinition(tType, befor, after, fixBefor, fixAfter, new Regex(fixBefor, RegexOptions.Compiled)));

            }
            else if (tType == TONE_TYPE.END_SYMBOL)
            {
                //終端 記号

                //終端記号の正規表現追加
                string fixBefor = REGEX_SYMBOL + REGEX_END;

                //終端文字の置き換え追加
                string fixAfter = after + DOLL2;

                resList.Add(new ToneDefinition(tType, befor, after, fixBefor, fixAfter, new Regex(fixBefor, RegexOptions.Compiled)));

            }
            else if (tType == TONE_TYPE.END_KATAKANA)
            {
                //終端 カタカナ

                //終端カタカナの正規表現追加
                string fixBefor = REGEX_KATAKANA + REGEX_END;

                //終端文字の置き換え追加
                string fixAfter = after + DOLL2;

                resList.Add(new ToneDefinition(tType, befor, after, fixBefor, fixAfter, new Regex(fixBefor, RegexOptions.Compiled)));

            }
            else if (tType == TONE_TYPE.END_HIRAGANA)
            {
                //終端 ひらがな

                //終端ひらがなの正規表現追加
                string fixBefor = REGEX_HIRAGANA + REGEX_END;

                //終端文字の置き換え追加
                string fixAfter = after + DOLL2;

                resList.Add(new ToneDefinition(tType, befor, after, fixBefor, fixAfter, new Regex(fixBefor, RegexOptions.Compiled)));

            }

            return resList;
        }

        /// <summary>
        /// タイプをTONETYPEに変換する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private TONE_TYPE Type2ToneType(string type)
        {
            try
            {
                return (TONE_TYPE)Enum.ToObject(typeof(TONE_TYPE), int.Parse(type));
            }
            catch
            {
                return TONE_TYPE.NONE;
            }
        }

        /// <summary>
        /// カッコをつける
        /// </summary>
        /// <param name="targetSentence"></param>
        /// <returns></returns>
        private string AddKakko(string targetSentence)
        {
            return "(" + targetSentence + ")";
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

                    //トーンタイプ別に処理。
                    //照合が可能な場合は、Containsでチェックしてから処理することで、低負荷化
                    if (toneDef.Type == TONE_TYPE.REPLACE ||
                        toneDef.Type == TONE_TYPE.END
                        )
                    {
                        //ビフォーが設定されていれば、照合チェック。
                        if (toneDef.Befor != "")
                        {
                            //照合チェックし、対象外ならスキップ
                            if (!target.Contains(toneDef.Befor))
                            {
                                continue;
                            }
                        }
                        else
                        {
                            //ビフォー条件がなければ無視
                            continue;
                        }

                        if (toneDef.Type == TONE_TYPE.END)
                        {
                            //ENDの場合
                            //取得したアフターに正規表現で置換
                            result = toneDef.Rx.Replace(result, toneDef.FixAfter);
                        }
                        else
                        {
                            //REPLACEの場合
                            //リプレイスで単純置換
                            result = result.Replace(toneDef.Befor, toneDef.After);
                        }
                    }
                    else if (toneDef.Type == TONE_TYPE.REGEX ||
                               toneDef.Type == TONE_TYPE.END_CHINESE_CHARACTER ||
                               toneDef.Type == TONE_TYPE.END_NUMBER ||
                               toneDef.Type == TONE_TYPE.END_ALPHABET ||
                               toneDef.Type == TONE_TYPE.END_SYMBOL ||
                               toneDef.Type == TONE_TYPE.END_KATAKANA ||
                               toneDef.Type == TONE_TYPE.END_HIRAGANA)
                    {
                        //無条件で置換
                        result = toneDef.Rx.Replace(result, toneDef.FixAfter);
                    }

                    //それ以外の場合は何も操作しない。
                }

                return result;
            }
            catch(Exception ex)
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
        public string FixAfter { get; set; }
        public string FixBefor { get; set; }
        public TONE_TYPE Type { get; set; }

        public ToneDefinition(TONE_TYPE Type, string Befor, string After,string FixBefor,string FixAfter, Regex Rx)
        {
            this.Type = Type;
            this.Rx = Rx;
            this.After = After;
            this.Befor = Befor;
            this.FixAfter = FixAfter;
            this.FixBefor = FixBefor;
        }


    }

}