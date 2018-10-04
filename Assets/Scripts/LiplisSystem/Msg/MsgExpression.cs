//====================================================================
//  ClassName : MsgExpressionDefault
//  概要      : デフォルト表情メッセージ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Model.Priset;
using Assets.Scripts.LiplisSystem.Model.Setting;
using Assets.Scripts.Utils;
using LiplisMoonlight.LiplisModel;
using Live2D.Cubism.Framework.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Msg
{
    public class MsgExpression
    {
        //=============================
        //ライブ2d 表情テーブル
        public Dictionary<int, List<string>> TableExpression;
        public Dictionary<string, AnimationClip> TabelAnimationClip;

        //=============================
        //エモーションファイル定義
        const string EMOTION_00_NORMAL_01       = "EMOTION_00_NORMAL_01.motion3.json";
        const string EMOTION_01_JOY_M_01        = "EMOTION_01_JOY_M_01.motion3.json";
        const string EMOTION_01_JOY_P_01        = "EMOTION_01_JOY_P_01.motion3.json";
        const string EMOTION_01_JOY_P_02        = "EMOTION_01_JOY_P_02.motion3.json";
        const string EMOTION_02_ADMIRATION_M_01 = "EMOTION_02_ADMIRATION_M_01.motion3.json";
        const string EMOTION_02_ADMIRATION_P_01 = "EMOTION_02_ADMIRATION_P_01.motion3.json";
        const string EMOTION_02_ADMIRATION_P_02 = "EMOTION_02_ADMIRATION_P_02.motion3.json";
        const string EMOTION_03_PEACE_M_01      = "EMOTION_03_PEACE_M_01.motion3.json";
        const string EMOTION_03_PEACE_P_01      = "EMOTION_03_PEACE_P_01.motion3.json";
        const string EMOTION_04_ECSTASY_M_01    = "EMOTION_04_ECSTASY_M_01.motion3.json";
        const string EMOTION_04_ECSTASY_P_01    = "EMOTION_04_ECSTASY_P_01.motion3.json";
        const string EMOTION_05_AMAZEMENT_M_01  = "EMOTION_05_AMAZEMENT_M_01.motion3.json";
        const string EMOTION_05_AMAZEMENT_P_01  = "EMOTION_05_AMAZEMENT_P_01.motion3.json";
        const string EMOTION_06_RAGE_M_01       = "EMOTION_06_RAGE_M_01.motion3.json";
        const string EMOTION_06_RAGE_P_01       = "EMOTION_06_RAGE_P_01.motion3.json";
        const string EMOTION_07_INTETEST_M_01   = "EMOTION_07_INTETEST_M_01.motion3.json";
        const string EMOTION_07_INTETEST_P_01   = "EMOTION_07_INTETEST_P_01.motion3.json";
        const string EMOTION_08_RESPECT_M_01    = "EMOTION_08_RESPECT_M_01.motion3.json";
        const string EMOTION_08_RESPECT_P_01    = "EMOTION_08_RESPECT_P_01.motion3.json";
        const string EMOTION_09_CLAMLY_M_01     = "EMOTION_09_CLAMLY_M_01.motion3.json";
        const string EMOTION_09_CLAMLY_P_01     = "EMOTION_09_CLAMLY_P_01.motion3.json";
        const string EMOTION_10_PROUD_M_01      = "EMOTION_10_PROUD_M_01.motion3.json";
        const string EMOTION_10_PROUD_P_01      = "EMOTION_10_PROUD_P_01.motion3.json";


        /// <summary>
        /// コンストラクター
        /// </summary>
        public MsgExpression()
        {
            //テーブルの初期化
            initTableExpression();

            //アニメーションクリックテーブルの初期化
            TabelAnimationClip = new Dictionary<string, AnimationClip>();

        }

        /// <summary>
        /// 表情テーブル初期化
        /// </summary>
        public void initTableExpression()
        {
            TableExpression = new Dictionary<int, List<string>>();
            TableExpression.Add((int)MOTION.MOTION_NORMAL, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_JOY_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_JOY_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_ADMIRATION_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_ADMIRATION_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_PEACE_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_PEACE_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_ECSTASY_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_ECSTASY_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_AMAZEMENT_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_AMAZEMENT_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_RAGE_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_RAGE_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_INTEREST_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_INTEREST_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_RESPECT_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_RESPECT_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_CLAMLY_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_CLAMLY_P, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_PROUD_M, new List<string>());
            TableExpression.Add((int)MOTION.MOTION_PROUD_P, new List<string>());
        }


        /// <summary>
        /// テーブルに表情データを追加する。
        /// </summary>
        /// <param name="ModelPath"></param>
        /// <param name="expression"></param>
        public void Add(string ModelPath, LiplisMotion expression)
        {
            //モーションパス取得
            var path = ModelPath + ModelPathDefine.EXPRESSIONS + "/" + expression.FileName;

            //対象エモーションにファイルを追加する。
            TableExpression[(int)MotionMap.GetMotion(expression.Emotion, expression.Emotion)].Add(expression.FileName);

            //ModelJsonオブジェクト取得
            var model3Json = CubismMotion3Json.LoadFrom(AssetLoader.LoadAsset<string>(path));

            //アニメーションクリップテーブルに追加
            TabelAnimationClip.Add(expression.FileName,model3Json.ToAnimationClip());
        }

        /// <summary>
        /// 登録されなかった感情データを検索し、デフォルト表情ファイルで補完し、リビルドする。
        /// </summary>
        public void FixAndRebuildTable()
        {
            foreach (KeyValuePair<int, List<string>> kv in TableExpression)
            {
                //デフォルトを追加する
                if(kv.Value.Count == 0)
                {
                    AddDeafultExpression(kv.Key);
                }
            }
        }

        /// <summary>
        /// デフォルトの表情を追加する
        /// </summary>
        /// <param name="emotion"></param>
        private void AddDeafultExpression(int emotion)
        {
            string Live2D_PrisetExpressionsPath = "Live2D_PrisetExpressions/";


            if(emotion == (int)MOTION.MOTION_NORMAL)
            {
                TabelAnimationClip.Add(EMOTION_00_NORMAL_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_00_NORMAL_01));
                TableExpression[(int)MOTION.MOTION_NORMAL].Add(EMOTION_00_NORMAL_01);
            }
            else if (emotion == (int)MOTION.MOTION_JOY_P)
            {
                TabelAnimationClip.Add(EMOTION_01_JOY_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_01_JOY_P_01));
                TableExpression[(int)MOTION.MOTION_JOY_P].Add(EMOTION_01_JOY_P_01);
                TableExpression[(int)MOTION.MOTION_JOY_P].Add(EMOTION_01_JOY_P_02);
            }
            else if (emotion == (int)MOTION.MOTION_JOY_M)
            {
                TabelAnimationClip.Add(EMOTION_01_JOY_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_01_JOY_M_01));
                TableExpression[(int)MOTION.MOTION_JOY_M].Add(EMOTION_01_JOY_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_ADMIRATION_P)
            {
                TabelAnimationClip.Add(EMOTION_02_ADMIRATION_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_02_ADMIRATION_P_01));
                TableExpression[(int)MOTION.MOTION_ADMIRATION_P].Add(EMOTION_02_ADMIRATION_P_01);
                TableExpression[(int)MOTION.MOTION_ADMIRATION_P].Add(EMOTION_02_ADMIRATION_P_02);
            }
            else if (emotion == (int)MOTION.MOTION_ADMIRATION_M)
            {
                TabelAnimationClip.Add(EMOTION_02_ADMIRATION_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_02_ADMIRATION_M_01));
                TableExpression[(int)MOTION.MOTION_ADMIRATION_M].Add(EMOTION_02_ADMIRATION_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_PEACE_P)
            {
                TabelAnimationClip.Add(EMOTION_03_PEACE_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_03_PEACE_P_01));
                TableExpression[(int)MOTION.MOTION_PEACE_P].Add(EMOTION_03_PEACE_P_01);
            }
            else if (emotion == (int)MOTION.MOTION_PEACE_M)
            {
                TabelAnimationClip.Add(EMOTION_03_PEACE_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_03_PEACE_M_01));
                TableExpression[(int)MOTION.MOTION_PEACE_M].Add(EMOTION_03_PEACE_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_ECSTASY_P)
            {
                TabelAnimationClip.Add(EMOTION_04_ECSTASY_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_04_ECSTASY_P_01));
                TableExpression[(int)MOTION.MOTION_ECSTASY_P].Add(EMOTION_04_ECSTASY_P_01);
            }
            else if (emotion == (int)MOTION.MOTION_ECSTASY_M)
            {
                TabelAnimationClip.Add(EMOTION_04_ECSTASY_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_04_ECSTASY_M_01));
                TableExpression[(int)MOTION.MOTION_ECSTASY_M].Add(EMOTION_04_ECSTASY_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_AMAZEMENT_P)
            {
                TabelAnimationClip.Add(EMOTION_05_AMAZEMENT_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_05_AMAZEMENT_P_01));
                TableExpression[(int)MOTION.MOTION_AMAZEMENT_P].Add(EMOTION_05_AMAZEMENT_P_01);
            }
            else if (emotion == (int)MOTION.MOTION_AMAZEMENT_M)
            {
                TabelAnimationClip.Add(EMOTION_05_AMAZEMENT_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_05_AMAZEMENT_M_01));
                TableExpression[(int)MOTION.MOTION_AMAZEMENT_M].Add(EMOTION_05_AMAZEMENT_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_RAGE_P)
            {
                TabelAnimationClip.Add(EMOTION_06_RAGE_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_06_RAGE_P_01));
                TableExpression[(int)MOTION.MOTION_RAGE_P].Add(EMOTION_06_RAGE_P_01);
            }
            else if (emotion == (int)MOTION.MOTION_RAGE_M)
            {
                TabelAnimationClip.Add(EMOTION_06_RAGE_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_06_RAGE_M_01));
                TableExpression[(int)MOTION.MOTION_RAGE_M].Add(EMOTION_06_RAGE_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_INTEREST_P)
            {
                TabelAnimationClip.Add(EMOTION_07_INTETEST_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_07_INTETEST_P_01));
                TableExpression[(int)MOTION.MOTION_INTEREST_P].Add(EMOTION_07_INTETEST_P_01);
            }
            else if (emotion == (int)MOTION.MOTION_INTEREST_M)
            {
                TabelAnimationClip.Add(EMOTION_07_INTETEST_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_07_INTETEST_M_01));
                TableExpression[(int)MOTION.MOTION_INTEREST_M].Add(EMOTION_07_INTETEST_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_RESPECT_P)
            {
                TabelAnimationClip.Add(EMOTION_08_RESPECT_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_08_RESPECT_P_01));
                TableExpression[(int)MOTION.MOTION_RESPECT_P].Add(EMOTION_08_RESPECT_P_01);
            }
            else if (emotion == (int)MOTION.MOTION_RESPECT_M)
            {
                TabelAnimationClip.Add(EMOTION_08_RESPECT_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_08_RESPECT_M_01));
                TableExpression[(int)MOTION.MOTION_RESPECT_M].Add(EMOTION_08_RESPECT_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_CLAMLY_P)
            {
                TabelAnimationClip.Add(EMOTION_09_CLAMLY_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_09_CLAMLY_P_01));
                TableExpression[(int)MOTION.MOTION_CLAMLY_P].Add(EMOTION_09_CLAMLY_P_01);
            }
            else if (emotion == (int)MOTION.MOTION_CLAMLY_M)
            {
                TabelAnimationClip.Add(EMOTION_09_CLAMLY_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_09_CLAMLY_M_01));
                TableExpression[(int)MOTION.MOTION_CLAMLY_M].Add(EMOTION_09_CLAMLY_M_01);
            }
            else if (emotion == (int)MOTION.MOTION_PROUD_P)
            {
                TabelAnimationClip.Add(EMOTION_10_PROUD_P_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_10_PROUD_P_01));
                TableExpression[(int)MOTION.MOTION_PROUD_P].Add(EMOTION_10_PROUD_P_01);
            }
            else if (emotion == (int)MOTION.MOTION_PROUD_M)
            {
                TabelAnimationClip.Add(EMOTION_10_PROUD_M_01, PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + EMOTION_10_PROUD_M_01));
                TableExpression[(int)MOTION.MOTION_PROUD_M].Add(EMOTION_10_PROUD_M_01);
            }
        }

        public void Create()
        {
            string Live2D_PrisetExpressionsPath = "Live2D_PrisetExpressions/";

            
            TabelAnimationClip.Add("EMOTION_01_JOY_M_01.motion3.json"       ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_01_JOY_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_01_JOY_P_01.motion3.json"       ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_01_JOY_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_01_JOY_P_02.motion3.json"       ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_01_JOY_P_02.motion3.json"));
            TabelAnimationClip.Add("EMOTION_02_ADMIRATION_M_01.motion3.json",PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_02_ADMIRATION_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_02_ADMIRATION_P_01.motion3.json",PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_02_ADMIRATION_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_02_ADMIRATION_P_02.motion3.json",PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_02_ADMIRATION_P_02.motion3.json"));
            TabelAnimationClip.Add("EMOTION_03_PEACE_M_01.motion3.json"     ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_03_PEACE_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_03_PEACE_P_01.motion3.json"     ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_03_PEACE_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_04_ECSTASY_M_01.motion3.json"   ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_04_ECSTASY_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_04_ECSTASY_P_01.motion3.json"   ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_04_ECSTASY_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_05_AMAZEMENT_M_01.motion3.json" ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_05_AMAZEMENT_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_05_AMAZEMENT_P_01.motion3.json" ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_05_AMAZEMENT_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_06_RAGE_M_01.motion3.json"      ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_06_RAGE_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_06_RAGE_P_01.motion3.json"      ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_06_RAGE_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_07_INTETEST_M_01.motion3.json"  ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_07_INTETEST_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_07_INTETEST_P_01.motion3.json"  ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_07_INTETEST_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_08_RESPECT_M_01.motion3.json"   ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_08_RESPECT_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_08_RESPECT_P_01.motion3.json"   ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_08_RESPECT_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_09_CLAMLY_M_01.motion3.json"    ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_09_CLAMLY_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_09_CLAMLY_P_01.motion3.json"    ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_09_CLAMLY_P_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_10_PROUD_M_01.motion3.json"     ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_10_PROUD_M_01.motion3.json"));
            TabelAnimationClip.Add("EMOTION_10_PROUD_P_01.motion3.json"     ,PrisetMotionLoader.Load(Live2D_PrisetExpressionsPath + "EMOTION_10_PROUD_P_01.motion3.json"));



            TableExpression[(int)MOTION.MOTION_JOY_M].Add("EMOTION_01_JOY_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_JOY_P].Add("EMOTION_01_JOY_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_JOY_P].Add("EMOTION_01_JOY_P_02.motion3.json");
            TableExpression[(int)MOTION.MOTION_ADMIRATION_M].Add("EMOTION_02_ADMIRATION_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_ADMIRATION_P].Add("EMOTION_02_ADMIRATION_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_ADMIRATION_P].Add("EMOTION_02_ADMIRATION_P_02.motion3.json");
            TableExpression[(int)MOTION.MOTION_PEACE_M].Add("EMOTION_03_PEACE_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_PEACE_P].Add("EMOTION_03_PEACE_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_ECSTASY_M].Add("EMOTION_04_ECSTASY_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_ECSTASY_P].Add("EMOTION_04_ECSTASY_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_AMAZEMENT_M].Add("EMOTION_05_AMAZEMENT_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_AMAZEMENT_P].Add("EMOTION_05_AMAZEMENT_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_RAGE_M].Add("EMOTION_06_RAGE_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_RAGE_P].Add("EMOTION_06_RAGE_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_INTEREST_M].Add("EMOTION_07_INTETEST_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_INTEREST_P].Add("EMOTION_07_INTETEST_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_RESPECT_M].Add("EMOTION_08_RESPECT_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_RESPECT_P].Add("EMOTION_08_RESPECT_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_CLAMLY_M].Add("EMOTION_09_CLAMLY_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_CLAMLY_P].Add("EMOTION_09_CLAMLY_P_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_PROUD_M].Add("EMOTION_10_PROUD_M_01.motion3.json");
            TableExpression[(int)MOTION.MOTION_PROUD_P].Add("EMOTION_10_PROUD_P_01.motion3.json");
        }


    }
}
