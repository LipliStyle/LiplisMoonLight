//====================================================================
//  ClassName : DatImagePath
//  概要      : イメージパス管理クラス
//              Web上から画像をダウンロードし、テクスチャとして返す。
//              ダウンロード後、本クラスのディクショナリにキャッシュとして登録する。
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================

using Assets.Scripts.Com;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Model.Setting;
using Assets.Scripts.LiplisSystem.Web.Clalis;
using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatImagePath
    {
        //=====================================
        // URLディクショナリー
        public Dictionary<string, DatImageFileInfo> DicImage;

        public List<DatImageFileInfo> DicImageList;

        //=====================================
        // URLディクショナリー
        public Texture2D TexNoImage;

        //=====================================
        // ダウンロード中コード
        public string DOWNLOAD_NOW = "downloadnow2234";

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DatImagePath()
        {
            DicImage = new Dictionary<string, DatImageFileInfo>();
            DicImageList = new List<DatImageFileInfo>();
        }




        /// <summary>
        /// Web上からテクスチャを取得する。
        /// キャッシュにあれば、キャッシュから返す
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IEnumerator GetWebTexutre(string url)
        {
            //ディクショナリ存在チェック
            if (DicImage.ContainsKey(url))
            {
                //ファイルパス取得
                string filePath = DicImage[url].FilePath;

                //ファイルパスが空の場合は、NULL
                if (filePath == "" || filePath == DOWNLOAD_NOW)
                {
                    yield return GetNoImageTex();
                    goto End;
                }

                //存在チェック
                if (File.Exists(filePath))
                {
                    //ディクショナリからファイルパスを取得する
                    yield return TextureUtil.GetTextureFromFile(filePath);
                    goto End;
                }
            }
            else
            {
                //とりあえず登録
                this.Add(new DatImageFileInfo(url, DOWNLOAD_NOW, DateTime.Now));
            }

            //ファイル名、ファイルパス生成
            string path = CreateFilePath(url);

            //ファイル名取得失敗時
            if (path != "")
            {
                //最新ニュースデータ取得
                var Async = WebImage.GetImage(url);

                //非同期実行
                yield return Async;

                //データ取得
                Texture2D texture = (Texture2D)Async.Current;

                if (texture != null)
                {
                    try
                    {
                        if (!texture.isBogus())
                        {
                            //PNGで保存
                            TextureUtil.SavePng(texture, path);

                            //辞書に登録
                            this.Add(new DatImageFileInfo(url, path, DateTime.Now));
                        }
                        else
                        {
                            //ノーイメージをセット
                            texture = GetNoImageTex();

                            //辞書に登録
                            this.Add(new DatImageFileInfo(url, "", DateTime.Now));
                        }
                    }
                    catch
                    {
                        Debug.Log("ファイル保存失敗:" + url);
                    }
                }
                else
                {
                    //ノーイメージをセット
                    texture = GetNoImageTex();

                    //辞書に登録
                    this.Add(new DatImageFileInfo(url, "", DateTime.Now));
                }

                //セーブ
                Save();

                //取得したテクスチャを返す
                yield return texture;
            }
            else
            {
                //空を返す
                yield return null;
            }

        End:;
        }



        /// <summary>
        /// ファイルからテクスチャを取得する
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Texture2D GetWebTexutreFromFile(string url)
        {
            //ディクショナリ存在チェック
            if (DicImage.ContainsKey(url))
            {
                //ファイルパス取得
                string filePath = DicImage[url].FilePath;

                //ファイルパスが空の場合は、NULL
                if (filePath == "")
                {
                    return GetNoImageTex();
                }

                //存在チェック
                if (File.Exists(filePath))
                {
                    //ディクショナリからファイルパスを取得する
                    return TextureUtil.GetTextureFromFile(filePath);
                }
            }

            ///見つからなければNULLを返す
            return null;
        }

        /// <summary>
        /// ファイルパスを生成する
        /// </summary>
        /// <returns></returns>
        private string CreateFilePath(string url)
        {
            try
            {
                //イメージキャッシュフォルダ 存在チェック
                string dirPath = Application.persistentDataPath + "/ImageCache";

                //フォルダ存在チェック
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                //ファイル名定義
                string fileNameFromUrl = GetFileName(url);

                //イコールチェック
                if (fileNameFromUrl.Contains("="))
                {
                    //イコールで分割
                    string[] buf = fileNameFromUrl.Split('=');

                    //最後のファイル名取得
                    fileNameFromUrl = buf[buf.Length - 1];
                }

                //ファイル名生成
                string fileName = fileNameFromUrl + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".png";

                //ファイル名生成
                return string.Format("{0}/{1}",dirPath, fileName);
            }
            catch(Exception ex)
            {
                Debug.Log(ex);

                return "";
            }

        }

        /// <summary>
        /// ファイル名取得
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFileName(string url)
        {
            try
            {
                //ファイル無効文字を除去する
                var invalidChars = Path.GetInvalidFileNameChars();
                var removed = string.Concat(url.Where(c => !invalidChars.Contains(c)));

                //ファイル名を生成して返す
                return Path.GetFileName(removed);
            }
            catch
            {
                return "NonFileName";
            }
        }


        /// <summary>
        /// データを追加する
        /// </summary>
        /// <param name="data"></param>
        public void Add(DatImageFileInfo data)
        {
            //辞書に登録
            if (!DicImage.ContainsKey(data.Url))
            {
                DicImage.Add(data.Url, data);
            }
            else
            {
                //既存ファイルパスが存在し、空で上書きしようとした場合、スルー
                if(data.Url == "" && DicImage[data.Url].Url != "")
                {
                    return;
                }

                //更新
                DicImage[data.Url] = data;
            }
        }


        /// <summary>
        /// 辞書と実ファイルをクリーンする
        /// </summary>
        public void Clean()
        {
            //辞書に存在しないファイルを削除する
            CleanNotExistsDictionaryFile();

            //ファイルが実在しない辞書のデータを削除する
            CleanNotExistsFile();
        }

        /// <summary>
        /// 辞書に存在してない画像ファイルを削除する
        /// </summary>
        public void CleanNotExistsDictionaryFile()
        {
            //ファイルディクショナリを取得する
            Dictionary<string, string> fileDictionary = CreateFileDic();

            string imageChachePath = Application.persistentDataPath + ModelPathDefine.IMAGE_CACHE;

            //イメージキャッシュパスの存在チェック
            if(!Directory.Exists(imageChachePath))
            {
                Directory.CreateDirectory(imageChachePath);
            }

            //ファイルリストを取得する
            string[] files = Directory.GetFiles(imageChachePath, "*", System.IO.SearchOption.TopDirectoryOnly);

            //ファイルリストを回し、辞書に存在しないふぁいるを検索する
            foreach (var file in files)
            {
                try
                {
                    //ファイル名修正
                    string fixFile = file.Replace("\\", "/");

                    //ファイルリストディクショナリに存在しなければ、削除対象リストに追加する。
                    if (!fileDictionary.ContainsKey(fixFile))
                    {
                        FileInfo fi = new FileInfo(fixFile);

                        fi.Delete();
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 辞書に存在するが、実ファイルが存在しないファイルを削除する
        /// </summary>
        public void CleanNotExistsFile()
        {
            //ファイルディクショナリ
            Dictionary<string, DatImageFileInfo> NewDicImage = new Dictionary<string, DatImageFileInfo>();

            foreach (var item in DicImage)
            {
                if(File.Exists(item.Value.FilePath))
                {
                    NewDicImage.Add(item.Value.Url, item.Value);
                }
            }

            //ファイルが実在するディクショナリに置き換える。
            this.DicImage = NewDicImage;
        }

        /// <summary>
        /// ファイルディクショナリを生成する
        /// </summary>
        /// <returns></returns>
        public Dictionary<string , string> CreateFileDic()
        {
            Dictionary<string, string> fileDictionary = new Dictionary<string, string>();

            foreach (var item in DicImage)
            {
                if(!fileDictionary.ContainsKey(item.Value.FilePath))
                {
                    fileDictionary.Add(item.Value.FilePath, "");
                }
            }

            return fileDictionary;
        }

        /// <summary>
        /// ノーイメージテクステゃを取得する
        /// </summary>
        /// <returns></returns>
        public Texture2D GetNoImageTex()
        {
            if(!UnityNullCheck.IsNull(TexNoImage))
            {
                return TexNoImage;
            }
            else
            {
                TexNoImage = Resources.Load("Image/NoImage") as Texture2D;

                return TexNoImage;
            }
        }

        //====================================================================
        //
        //                        パブリックメソッド
        //                         
        //====================================================================
        #region データ保存

        /// <summary>
        /// データセーブ
        /// </summary>
        public void Save()
        {
            //リストに保存
            EscapeList();

            //指定キー「LiplisStatus」でリプリスステータスのインスタンスを保存する
            SaveDataCache.SetClass(LpsDefine.SETKEY_LIPLIS_CACHE, LiplisCache.Instance);

            //セーブ発動
            SaveDataCache.Save();
        }

        /// <summary>
        /// リカバリー
        /// </summary>
        public void Recovery()
        {
            //ディクショナリ初期化
            DicImage = new Dictionary<string, DatImageFileInfo>();

            //復元
            foreach (var data in DicImageList)
            {
                DicImage.Add(data.Url, data);
            }

            //イメージ初期化
            TexNoImage = Resources.Load("Image/NoImage") as Texture2D;
        }

        /// <summary>
        /// リストにエスケープする
        /// </summary>
        public void EscapeList()
        {
            DicImageList = new List<DatImageFileInfo>();

            foreach (var data in DicImage)
            {
                DicImageList.Add(data.Value);
            }
        }


        #endregion

    }

    /// <summary>
    /// イメージファイル情報
    /// </summary>
    [Serializable]
    public class DatImageFileInfo
    {
        public string Url;
        public string FilePath;
        public DateTime CreateTime;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="CreateTime"></param>
        public DatImageFileInfo(string Url, string FilePath,DateTime CreateTime)
        {
            this.Url = Url;
            this.FilePath = FilePath;
            this.CreateTime = CreateTime;
        }
    }
}
