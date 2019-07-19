//====================================================================
//  ClassName : SaveDataBaseBin
//  概要      : データをバイナリ保存する機能
//              
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using Assets.Scripts.Com;
using Assets.Scripts.LiplisSystem.Com;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class SaveDataSystemBin : SaveDataSystem
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_fileName"></param>
        public SaveDataSystemBin(string _path, string _fileName):base(_path,_fileName)
        {
        }

        /// <summary>
        /// 保存する
        /// </summary>
        public override void Save()
        {
            //ディクショナリシリアライズ
            var serialDict = new Serialization<string, string>(saveDictionary);
            serialDict.OnBeforeSerialize();
            string dictJsonString = JsonUtility.ToJson(serialDict);

            //圧縮
            byte[] data = LpsGzipUtil.Compress2(dictJsonString);

            //保存
            using (FileStream fs = new FileStream(path + fileName, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        public override void Load()
        {
            try
            {
                if (File.Exists(path + fileName))
                {

                    using (FileStream fs = new FileStream(path + fileName, FileMode.Open))
                    {
                        //ファイルを読み込むバイト型配列を作成する
                        byte[] bs = new byte[fs.Length];

                        //ファイルの内容をすべて読み込む
                        fs.Read(bs, 0, bs.Length);

                        //デシリアライズ
                        string jsonText = LpsGzipUtil.Decompress(bs);

                        if (saveDictionary != null)
                        {
                            var sDict = JsonUtility.FromJson<Serialization<string, string>>(jsonText);

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

    }
}
