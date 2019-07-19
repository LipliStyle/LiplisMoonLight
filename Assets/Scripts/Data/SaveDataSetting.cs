//====================================================================
//  ClassName : SaveData
//  概要      : データを保存する機能
//              
//  Load()                                  指定のパスからjsonデータを読み込みます。
//  Save()                                  変更された値を指定ファイルに保存します。
//  SetInt(string key, int value)           int型の値を設定します。
//  SetFloat(string key, float value)       float型の値を設定します。
//  SetString(string key, string value)     string型の値を設定します。
//  SetClass<T>(string key, T value)        T型の値を設定します。
//  SetList<T>(string key, List<T> value)   List<T>型の値を設定します。
//  GetInt(string key, int default)         keyで指定されたint型の値を取得します。
//  GetFloat(string key, float default)     keyで指定されたint型の値を取得します。
//  GetString(string key, string default)   keyで指定されたstring型の値を取得します。
//  GetClass<T>(string key, T default) 	    keyで指定されたT型の値を取得します。
//  GetList<T>(string key, List<T> default) keyで指定されたList<T> 型の値を取得します。
//  Remove(string key)                      keyで指定された値を削除します。
//  Clear()                                 保存データをすべて削除します。
//
//
//※注意点！
//　　対象オブジェクトのpublicフィールドがシリアライズされる　コレ重要！
//
//　　プロパティは対象外　コレ重要！
//
//　　privateフィールドをシリアライズに含みたい場合は[SerializeField] をつける
//
//　　publicフィールドをシリアライズから除外したい場合は[NonSerialized] をつける
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class SaveDataSetting
    {
        /// <summary>
        /// SingletonなSaveDatabaseクラス
        /// </summary>
        protected static SaveDataSystem data = null;

        protected static SaveDataSystem Savedatabase
        {
            get
            {
                if (data == null)
                {
                    string path = Application.persistentDataPath + "/";
                    string fileName = Application.companyName + "." + Application.productName + ".savedata_setting.json";
                    data = new SaveDataSystem(path, fileName);
                }
                return data;
            }
        }

        #region Public Static Methods

        /// <summary>
        /// 指定したキーとT型のクラスコレクションをセーブデータに追加します。
        /// </summary>
        /// <typeparam name="T">ジェネリッククラス</typeparam>
        /// <param name="key">キー</param>
        /// <param name="list">T型のList</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>指定したキーとT型のクラスコレクションをセーブデータに追加します。</remarks>
        public static void SetList<T>(string key, List<T> list)
        {
            Savedatabase.SetList<T>(key, list);
        }

        /// <summary>
        ///  指定したキーとT型のクラスコレクションをセーブデータから取得します。
        /// </summary>
        /// <typeparam name="T">ジェネリッククラス</typeparam>
        /// <param name="key">キー</param>
        /// <param name="_default">デフォルトの値</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns></returns>
        public static List<T> GetList<T>(string key, List<T> _default)
        {
            return Savedatabase.GetList<T>(key, _default);
        }

        /// <summary>
        ///  指定したキーとT型のクラスをセーブデータに追加します。
        /// </summary>
        /// <typeparam name="T">ジェネリッククラス</typeparam>
        /// <param name="key">キー</param>
        /// <param name="_default">デフォルトの値</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns></returns>
        public static T GetClass<T>(string key, T _default) where T : class, new()
        {
            return Savedatabase.GetClass(key, _default);

        }

        /// <summary>
        ///  指定したキーとT型のクラスコレクションをセーブデータから取得します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void SetClass<T>(string key, T obj) where T : class, new()
        {
            Savedatabase.SetClass<T>(key, obj);
        }

        /// <summary>
        /// 指定されたキーに関連付けられている値を取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void SetString(string key, string value)
        {
            Savedatabase.SetString(key, value);
        }

        /// <summary>
        /// 指定されたキーに関連付けられているString型の値を取得します。
        /// 値がない場合、_defaultの値を返します。省略した場合、空の文字列を返します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="_default">デフォルトの値</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns></returns>
        public static string GetString(string key, string _default = "")
        {
            return Savedatabase.GetString(key, _default);
        }

        /// <summary>
        /// 指定されたキーに関連付けられているInt型の値を取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">デフォルトの値</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void SetInt(string key, int value)
        {
            Savedatabase.SetInt(key, value);
        }

        /// <summary>
        /// 指定されたキーに関連付けられているInt型の値を取得します。
        /// 値がない場合、_defaultの値を返します。省略した場合、0を返します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="_default">デフォルトの値</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns></returns>
        public static int GetInt(string key, int _default = 0)
        {
            return Savedatabase.GetInt(key, _default);
        }

        /// <summary>
        /// 指定されたキーに関連付けられているfloat型の値を取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">デフォルトの値</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void SetFloat(string key, float value)
        {
            Savedatabase.SetFloat(key, value);
        }

        /// <summary>
        /// 指定されたキーに関連付けられているfloat型の値を取得します。
        /// 値がない場合、_defaultの値を返します。省略した場合、0.0fを返します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="_default">デフォルトの値</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns></returns>
        public static float GetFloat(string key, float _default = 0.0f)
        {
            return Savedatabase.GetFloat(key, _default);
        }

        /// <summary>
        /// セーブデータからすべてのキーと値を削除します。
        /// </summary>
        public static void Clear()
        {
            Savedatabase.Clear();
        }

        /// <summary>
        /// 指定したキーを持つ値を セーブデータから削除します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void Remove(string key)
        {
            Savedatabase.Remove(key);
        }

        /// <summary>
        /// セーブデータ内にキーが存在するかを取得します。
        /// </summary>
        /// <param name="_key">キー</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns></returns>
        public static bool ContainsKey(string _key)
        {
            return Savedatabase.ContainsKey(_key);
        }

        /// <summary>
        /// セーブデータに格納されたキーの一覧を取得します。
        /// </summary>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns></returns>
        public static List<string> Keys()
        {
            return Savedatabase.Keys();
        }

        /// <summary>
        /// 明示的にファイルに書き込みます。
        /// </summary>
        public static void Save()
        {
            Savedatabase.Save();
        }

        #endregion

    }

}
