//=======================================================================
//  ClassName : EXPRESSION
//  概要      : 感情定義
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//=======================================================================﻿
using Assets.Scripts.Com;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Define
{
    public class EXPRESSION
    {
        ///=============================
        /// エクスプレッション定義
        public const string NORMAL_01       = "EMOTION_00_NORMAL_01";
        public const string JOY_M_01        = "EMOTION_01_JOY_M_01";
        public const string JOY_P_01        = "EMOTION_01_JOY_P_01";
        public const string JOY_P_02        = "EMOTION_01_JOY_P_02";
        public const string ADMIRATION_M_01 = "EMOTION_02_ADMIRATION_M_01";
        public const string ADMIRATION_P_01 = "EMOTION_02_ADMIRATION_P_01";
        public const string ADMIRATION_P_02 = "EMOTION_02_ADMIRATION_P_02";
        public const string PEACE_M_01      = "EMOTION_03_PEACE_M_01";
        public const string PEACE_P_01      = "EMOTION_03_PEACE_P_01";
        public const string ECSTASY_M_01    = "EMOTION_04_ECSTASY_M_01";
        public const string ECSTASY_P_01    = "EMOTION_04_ECSTASY_P_01";
        public const string AMAZEMENT_M_01  = "EMOTION_05_AMAZEMENT_M_01";
        public const string AMAZEMENT_P_01  = "EMOTION_05_AMAZEMENT_P_01";
        public const string RAGE_M_01       = "EMOTION_06_RAGE_M_01";
        public const string RAGE_P_01       = "EMOTION_06_RAGE_P_01";
        public const string INTETEST_M_01   = "EMOTION_07_INTETEST_M_01";
        public const string INTETEST_P_01   = "EMOTION_07_INTETEST_P_01";
        public const string RESPECT_M_01    = "EMOTION_08_RESPECT_M_01";
        public const string RESPECT_P_01    = "EMOTION_08_RESPECT_P_01";
        public const string CLAMLY_M_01     = "EMOTION_09_CLAMLY_M_01";
        public const string CLAMLY_P_01     = "EMOTION_09_CLAMLY_P_01";
        public const string PROUD_M_01      = "EMOTION_10_PROUD_M_01";
        public const string PROUD_P_01      = "EMOTION_10_PROUD_P_01";

        ///=============================
        /// エクスプレッションリスト
        public List<string> LstNORMAL        ;
        public List<string> LstJOY_M         ;
        public List<string> LstJOY_P         ;
        public List<string> LstADMIRATION_M  ;
        public List<string> LstADMIRATION_P  ;
        public List<string> LstPEACE_M       ;
        public List<string> LstPEACE_P       ;
        public List<string> LstECSTASY_M     ;
        public List<string> LstECSTASY_P     ;
        public List<string> LstAMAZEMENT_M   ;
        public List<string> LstAMAZEMENT_P   ;
        public List<string> LstRAGE_M        ;
        public List<string> LstRAGE_P        ;
        public List<string> LstINTETEST_M    ;
        public List<string> LstINTETEST_P    ;
        public List<string> LstRESPECT_M     ;
        public List<string> LstRESPECT_P     ;
        public List<string> LstCLAMLY_M      ;
        public List<string> LstCLAMLY_P      ;
        public List<string> LstPROUD_M       ;
        public List<string> LstPROUD_P       ;

        //====================================================================
        //
        //                          シングルトン管理
        //                         
        //====================================================================
        #region シングルトン管理

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static EXPRESSION instance;
        public static EXPRESSION Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EXPRESSION();
                }

                return instance;
            }
        }
#endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        private EXPRESSION()
        {
            LstNORMAL         = new List<string>();
            LstJOY_M          = new List<string>();
            LstJOY_P          = new List<string>();
            LstADMIRATION_M   = new List<string>();
            LstADMIRATION_P   = new List<string>();
            LstPEACE_M        = new List<string>();
            LstPEACE_P        = new List<string>();
            LstECSTASY_M      = new List<string>();
            LstECSTASY_P      = new List<string>();
            LstAMAZEMENT_M    = new List<string>();
            LstAMAZEMENT_P    = new List<string>();
            LstRAGE_M         = new List<string>();
            LstRAGE_P         = new List<string>();
            LstINTETEST_M     = new List<string>();
            LstINTETEST_P     = new List<string>();
            LstRESPECT_M      = new List<string>();
            LstRESPECT_P      = new List<string>();
            LstCLAMLY_M       = new List<string>();
            LstCLAMLY_P       = new List<string>();
            LstPROUD_M        = new List<string>();
            LstPROUD_P        = new List<string>();

            LstNORMAL.Add(NORMAL_01);
            LstJOY_M.Add(JOY_M_01);
            LstJOY_P.Add(JOY_P_01);
            LstJOY_P.Add(JOY_P_02);
            LstADMIRATION_M.Add(ADMIRATION_M_01);
            LstADMIRATION_P.Add(ADMIRATION_P_01);
            LstADMIRATION_P.Add(ADMIRATION_P_02);
            LstPEACE_M.Add(PEACE_M_01);
            LstPEACE_P.Add(PEACE_P_01);
            LstECSTASY_M.Add(ECSTASY_M_01);
            LstECSTASY_P.Add(ECSTASY_P_01);
            LstAMAZEMENT_M.Add(AMAZEMENT_M_01);
            LstAMAZEMENT_P.Add(AMAZEMENT_P_01);
            LstRAGE_M.Add(RAGE_M_01);
            LstRAGE_P.Add(RAGE_P_01);
            LstINTETEST_M.Add(INTETEST_M_01);
            LstINTETEST_P.Add(INTETEST_P_01);
            LstRESPECT_M.Add(RESPECT_M_01);
            LstRESPECT_P.Add(RESPECT_P_01);
            LstCLAMLY_M.Add(CLAMLY_M_01);
            LstCLAMLY_P.Add(CLAMLY_P_01);
            LstPROUD_M.Add(PROUD_M_01);
            LstPROUD_P.Add(PROUD_P_01);
        }                    

        /// <summary>
        /// ランダムに感情コードを取得する
        /// 
        /// 正式にモデルが完成したら調整
        /// </summary>
        /// <returns></returns>
        public string GetExpressionRandam()
        {
            Random r = new Random();

            int idx = r.Next(0, 21);

            switch (idx)
            {
                case 0: return LstNORMAL.GetAtRandom();
                case 1: return LstJOY_M.GetAtRandom();
                case 2: return LstJOY_P.GetAtRandom();
                case 3: return LstADMIRATION_M.GetAtRandom();
                case 4: return LstADMIRATION_P.GetAtRandom();
                case 5: return LstPEACE_M.GetAtRandom();
                case 6: return LstPEACE_P.GetAtRandom();
                case 7: return LstECSTASY_M.GetAtRandom();
                case 8: return LstECSTASY_P.GetAtRandom();
                case 9: return LstAMAZEMENT_M.GetAtRandom();
                case 10: return LstAMAZEMENT_P.GetAtRandom();
                case 11: return LstRAGE_M.GetAtRandom();
                case 12: return LstRAGE_P.GetAtRandom();
                case 13: return LstINTETEST_M.GetAtRandom();
                case 14: return LstINTETEST_P.GetAtRandom();
                case 15: return LstRESPECT_M.GetAtRandom();
                case 16: return LstRESPECT_P.GetAtRandom();
                case 17: return LstCLAMLY_M.GetAtRandom();
                case 18: return LstCLAMLY_P.GetAtRandom();
                case 19: return LstPROUD_M.GetAtRandom();
                case 20: return LstPROUD_P.GetAtRandom();
                default: return LstNORMAL.GetAtRandom();
            }
        }

        /// <summary>
        /// 感情を取得する
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public string GetExpression(int emotion, int point)
        {
            if (emotion == EMOTION.NORMAL)                          { return LstNORMAL.GetAtRandom(); }
            else if (emotion == EMOTION.JOY && point >= 0)          { return LstJOY_P.GetAtRandom(); }
            else if (emotion == EMOTION.JOY && point < 0)           { return LstJOY_M.GetAtRandom(); }
            else if (emotion == EMOTION.ADMIRATION && point >= 0)   { return LstADMIRATION_P.GetAtRandom(); }
            else if (emotion == EMOTION.ADMIRATION && point < 0)    { return LstADMIRATION_M.GetAtRandom(); }
            else if (emotion == EMOTION.PEACE && point >= 0)        { return LstPEACE_P.GetAtRandom(); }
            else if (emotion == EMOTION.PEACE && point < 0)         { return LstPEACE_M.GetAtRandom(); }
            else if (emotion == EMOTION.ECSTASY && point >= 0)      { return LstECSTASY_P.GetAtRandom(); }
            else if (emotion == EMOTION.ECSTASY && point < 0)       { return LstECSTASY_M.GetAtRandom(); }
            else if (emotion == EMOTION.AMAZEMENT && point >= 0)    { return LstAMAZEMENT_P.GetAtRandom(); }
            else if (emotion == EMOTION.AMAZEMENT && point < 0)     { return LstAMAZEMENT_M.GetAtRandom(); }
            else if (emotion == EMOTION.RAGE && point >= 0)         { return LstRAGE_P.GetAtRandom(); }
            else if (emotion == EMOTION.RAGE && point < 0)          { return LstRAGE_M.GetAtRandom(); }
            else if (emotion == EMOTION.INTEREST && point >= 0)     { return LstINTETEST_P.GetAtRandom(); }
            else if (emotion == EMOTION.INTEREST && point < 0)      { return LstINTETEST_M.GetAtRandom(); }
            else if (emotion == EMOTION.RESPECT && point >= 0)      { return LstRESPECT_P.GetAtRandom(); }
            else if (emotion == EMOTION.RESPECT && point < 0)       { return LstRESPECT_M.GetAtRandom(); }
            else if (emotion == EMOTION.CLAMLY && point >= 0)       { return LstCLAMLY_P.GetAtRandom(); }
            else if (emotion == EMOTION.CLAMLY && point < 0)        { return LstCLAMLY_M.GetAtRandom(); }
            else if (emotion == EMOTION.PROUD && point >= 0)        { return LstPROUD_P.GetAtRandom(); }
            else if (emotion == EMOTION.PROUD && point < 0)         { return LstPROUD_M.GetAtRandom(); }
            else                                                    { return LstNORMAL.GetAtRandom(); }
        }

        /// <summary>
        /// ノーマルエクスプレッションを返す
        /// </summary>
        /// <returns></returns>
        public string GetDefaultExpresssion()
        {
            return LstNORMAL.GetAtRandom();
        }
    }
}