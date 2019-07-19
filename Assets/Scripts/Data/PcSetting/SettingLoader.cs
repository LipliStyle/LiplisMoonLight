//====================================================================
//  ClassName : SettingLoader
//  概要      : 環境設定ローダー
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Data.PcSetting
{
    public class SettingLoader
    {
        //--------------------
        //設定ファイル定義
        public const string SETTING_FILE_NAME = "Setting.json";
        public const string DATA_DIR = "LiplisMonnlight_Data";


        /// <summary>
        /// 設定をロードする
        /// </summary>
        public static LiplisMoonlightSetting LoadSetting()
        {
            try
            {
                //ディレクトリ存在チェック
                if (File.Exists(GetJsonFilePath()))
                {
                    //データロード
                    using (StreamReader r = new StreamReader(GetJsonFilePath(), Encoding.UTF8))
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<LiplisMoonlightSetting>(r.ReadToEnd());
                    }
                }
                else
                {
                    return new LiplisMoonlightSetting();
                }
            }
            catch
            {
                return new LiplisMoonlightSetting();
            }
        }

        /// <summary>
        /// 設定をセーブする
        /// </summary>
        public static void SaveSetting(LiplisMoonlightSetting setting)
        {
            try
            {
                //データ書き込み
                using (StreamWriter w = new StreamWriter(GetJsonFilePath()))
                {
                    w.Write(Newtonsoft.Json.JsonConvert.SerializeObject(setting));
                }
            }
            catch
            {

            }
        }


        //======================================================================
        //
        //                             パス関連操作
        //
        //======================================================================
        #region パス関連操作
        /// <summary>
        /// 設定ファイルパスを返す
        /// </summary>
        /// <returns></returns>
        public static string GetJsonFilePath()
        {
            return GetDataPath() + "\\" + SETTING_FILE_NAME;
        }

        /// <summary>
        /// データパスを生成する
        /// </summary>
        /// <returns></returns>
        public static string GetDataPath()
        {
            return Application.dataPath;
        }

        #endregion
    }
}
