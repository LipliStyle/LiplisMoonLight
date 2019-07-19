//====================================================================
//  ClassName : JsonLoader
//  概要      : Json設定ファイルを読み込む
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//====================================================================
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Assets.Scripts.Utils
{
    public class JsonLoader<T>
    {
        /// <summary>
        /// Jsonパスを指定すると、ロードし、してクラスのインスタンスとして返す
        /// </summary>
        /// <param name="josnFilePath"></param>
        /// <returns></returns>
        public static T Load(string josnFilePath)
        {
            FileInfo fi = new FileInfo(josnFilePath);

            // Json読み込み
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                //モデルファイルシリアライズ
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
    }
}
