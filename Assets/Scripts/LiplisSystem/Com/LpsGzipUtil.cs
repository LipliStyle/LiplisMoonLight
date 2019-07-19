//=======================================================================
//  ClassName : LpsGzipUtil
//  概要      : Gzip圧縮クラス
//     
//              Unityでは「System.IO.Compression」は使えない。
//              代わりのアセット「System.IO.Compression」を使用している。
//              https://www.assetstore.unity3d.com/jp/#!/content/31902
//  Copyright(c) 2010-2017 sachin. All Rights Reserved. 
//=======================================================================
using ICSharpCode.SharpZipLib.GZip;
using System.IO;
using System.Text;
using Unity.IO.Compression;
//using System.IO.Compression;

namespace Assets.Scripts.LiplisSystem.Com
{
    public class LpsGzipUtil
    {
        /// <summary>
        /// 文字列をUTF-8のバイト配列に変換し、Gzip圧縮する
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] Compress(string text)
        {
            //UTF8エンコード
            byte[] source = Encoding.UTF8.GetBytes(text);

            // 入力ストリーム
            using (MemoryStream inStream = new MemoryStream(source))
            {
                // 出力ストリーム
                using (MemoryStream outStream = new MemoryStream())
                {

                    // 圧縮ストリーム
                    using (GZipStream compStream = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        compStream.Write(source, 0, source.Length);
                    }

                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// バイト配列の圧縮データを解凍してUTF-8エンコードで返す
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static string Decompress(byte[] destination)
        {
            using (MemoryStream ms = new MemoryStream(destination))
            {
                return Decompress(ms);
            }
        }

        /// <summary>
        /// ストリームの圧縮データを解凍してUTF-8エンコードで返す
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static string Decompress(Stream stream)
        {
            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress))
            {
                StringBuilder sb = new StringBuilder();
                byte[] buff = new byte[4096];
                int length = 0;
                while ((length = gzip.Read(buff, 0, buff.Length)) > 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buff, 0, length));
                }
                return sb.ToString();
            };
        }



















        /// <summary>
        /// 圧縮します
        /// </summary>
        public static byte[] Compress2(string rawString)
        {
            var rawData = Encoding.UTF8.GetBytes(rawString);
            using (var memoryStream = new MemoryStream())
            {
                Compress2(memoryStream, rawData);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 解凍します
        /// </summary>
        public static string Decompress2(byte[] compressedData)
        {
            using (var memoryStream = new MemoryStream())
            {
                Decompress2(memoryStream, compressedData);
                var bytes = memoryStream.ToArray();
                return Encoding.UTF8.GetString(bytes);
            }
        }

        /// <summary>
        /// 圧縮します
        /// </summary>
        private static void Compress2(Stream stream, byte[] rawData)
        {
            using (var gzipOutputStream = new GZipOutputStream(stream))
            {
                gzipOutputStream.Write(rawData, 0, rawData.Length);
            }
        }

        /// <summary>
        /// 解凍します
        /// </summary>
        private static void Decompress2(Stream stream, byte[] compressedData)
        {
            var buffer = new byte[4096];
            using (var memoryStream = new MemoryStream(compressedData))
            using (var gzipOutputStream = new GZipInputStream(memoryStream))
            {
                for (int r = -1; r != 0; r = gzipOutputStream.Read(buffer, 0, buffer.Length))
                {
                    if (r > 0)
                    {
                        stream.Write(buffer, 0, r);
                    }
                }
            }
        }







    }
}
