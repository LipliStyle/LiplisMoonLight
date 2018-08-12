//====================================================================
//  ClassName : SaveDataBase
//  概要      : データを保存する機能
//              
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Data
{

    [Serializable]
    public class SaveDataBase
    {
        #region Fields

        private string path;
        //保存先
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private string fileName;
        //ファイル名
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private Dictionary<string, string> saveDictionary;
        //keyとjson文字列を格納

        #endregion

        #region Constructor&Destructor

        public SaveDataBase(string _path, string _fileName)
        {
            path = _path;
            fileName = _fileName;
            saveDictionary = new Dictionary<string, string>();
            Load();

        }

        /// <summary>
        /// クラスが破棄される時点でファイルに書き込みます。
        /// </summary>
        ~SaveDataBase()
        {
            //エラーが発生するため、コメントアウト
            //Save();
        }

        #endregion

        #region Public Methods

        public void SetList<T>(string key, List<T> list)
        {
            keyCheck(key);
            var serializableList = new Serialization<T>(list);
            string json = JsonUtility.ToJson(serializableList);
            saveDictionary[key] = json;
        }

        public List<T> GetList<T>(string key, List<T> _default)
        {
            keyCheck(key);
            if (!saveDictionary.ContainsKey(key))
            {
                return _default;
            }
            string json = saveDictionary[key];
            Serialization<T> deserializeList = JsonUtility.FromJson<Serialization<T>>(json);

            return deserializeList.ToList();
        }

        public T GetClass<T>(string key, T _default) where T : class, new()
        {
            keyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                return _default;

            string json = saveDictionary[key];
            T obj = JsonUtility.FromJson<T>(json);
            return obj;

        }

        public void SetClass<T>(string key, T obj) where T : class, new()
        {
            keyCheck(key);
            string json = JsonUtility.ToJson(obj);
            saveDictionary[key] = json;
        }

        public void SetString(string key, string value)
        {
            keyCheck(key);
            saveDictionary[key] = value;
        }

        public string GetString(string key, string _default)
        {
            keyCheck(key);

            if (!saveDictionary.ContainsKey(key))
                return _default;
            return saveDictionary[key];
        }

        public void SetInt(string key, int value)
        {
            keyCheck(key);
            saveDictionary[key] = value.ToString();
        }

        public int GetInt(string key, int _default)
        {
            keyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                return _default;
            int ret;
            if (!int.TryParse(saveDictionary[key], out ret))
            {
                ret = 0;
            }
            return ret;
        }

        public void SetFloat(string key, float value)
        {
            keyCheck(key);
            saveDictionary[key] = value.ToString();
        }

        public float GetFloat(string key, float _default)
        {
            float ret;
            keyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                ret = _default;

            if (!float.TryParse(saveDictionary[key], out ret))
            {
                ret = 0.0f;
            }
            return ret;
        }

        public void Clear()
        {
            saveDictionary.Clear();

        }

        public void Remove(string key)
        {
            keyCheck(key);
            if (saveDictionary.ContainsKey(key))
            {
                saveDictionary.Remove(key);
            }

        }

        public bool ContainsKey(string _key)
        {

            return saveDictionary.ContainsKey(_key);
        }

        public List<string> Keys()
        {
            return saveDictionary.Keys.ToList<string>();
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(path + fileName, false, Encoding.GetEncoding("utf-8")))
            {
                var serialDict = new Serialization<string, string>(saveDictionary);
                serialDict.OnBeforeSerialize();
                string dictJsonString = JsonUtility.ToJson(serialDict);
                writer.WriteLine(dictJsonString);
            }
        }

        public void Load()
        {
            try
            {
                if (File.Exists(path + fileName))
                {
                    using (StreamReader sr = new StreamReader(path + fileName, Encoding.GetEncoding("utf-8")))
                    {
                        if (saveDictionary != null)
                        {
                            var sDict = JsonUtility.FromJson<Serialization<string, string>>(sr.ReadToEnd());

                            if (sDict == null)
                            {
                                saveDictionary = new Dictionary<string, string>();
                            }
                            else
                            {
                                sDict.OnAfterDeserialize();
                                saveDictionary = sDict.ToDictionary();
                            }
                        }
                    }
                }
                else { saveDictionary = new Dictionary<string, string>(); }
            }
            catch (Exception ex)
            {
                Debug.Log("SaveData:Load");
                Debug.Log(ex);
            }

        }

        public string GetJsonString(string key)
        {
            keyCheck(key);
            if (saveDictionary.ContainsKey(key))
            {
                return saveDictionary[key];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// キーに不正がないかチェックします。
        /// </summary>
        private void keyCheck(string _key)
        {
            if (string.IsNullOrEmpty(_key))
            {
                throw new ArgumentException("invalid key!!");
            }
        }

        #endregion
    }


    #region Serialization Class

    // List<T>
    [Serializable]
    public class Serialization<T>
    {
        public List<T> target;

        public List<T> ToList()
        {
            return target;
        }

        public Serialization()
        {
        }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }
    // Dictionary<TKey, TValue>
    [Serializable]
    public class Serialization<TKey, TValue>
    {
        public List<TKey> keys;
        public List<TValue> values;
        private Dictionary<TKey, TValue> target;

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return target;
        }

        public Serialization()
        {
        }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            int count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            Enumerable.Range(0, count).ToList().ForEach(i => target.Add(keys[i], values[i]));
        }
    }

    #endregion
}
