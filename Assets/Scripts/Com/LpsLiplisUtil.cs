using Assets.Scripts.LiplisSystem.Com;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace Assets.Scripts.Com
{
    public static class LpsLiplisUtil
    {
        /// <summary>
        /// IsNumeric
        /// 数値チェック
        /// </summary>
        /// <param name="stTarget">ターゲットストリング</param>
        /// <returns>結果の真偽</returns>
        #region IsNumeric
        public static bool IsNumeric(string stTarget)
        {
            double dNullable;

            return double.TryParse(
                stTarget,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }
        #endregion

        /// <summary>
        /// IsNumericReturnResut
        /// 数値チェック&数値変換
        /// </summary>
        /// <param name="stTarget">ターゲットストリング</param>
        /// <param name="defaultVal">デフォルト値</param>
        /// <returns>真:ターゲットをIntに変換して返す。</returns>
        /// <returns>偽:デフォルト値を返す。</returns>
        #region IsNumericReturnResut
        public static int IsNumericReturnResut(string stTarget, int defaultVal)
        {
            if (stTarget == null)       //nullならデフォルトを返す
            {
                return defaultVal;
            }
            if (IsNumeric(stTarget))     //数値なら数値に変換して返す
            {
                return int.Parse(stTarget);
            }
            return defaultVal;          //数値でなければデフォルト値を返す
        }
        #endregion

        /// <summary>
        /// nullChekc
        /// ヌルチェッカー
        /// </summary>
        #region nullCheck
        public static string nullCheck(string str)
        {
            try
            {
                string result = "";

                //ヌルでなければ結果をそのまま返す
                if (str != null)
                {
                    result = str;
                }

                return result;
            }
            catch
            {
                return "";
            }

        }
        public static string nullCheck(object str)
        {
            try
            {
                if (str == null)
                {
                    return "";
                }

                string result = "";

                //ヌルでなければ結果をそのまま返す
                if (str != null)
                {
                    result = str.ToString();
                }

                return result;
            }
            catch
            {
                return "";
            }

        }
        public static string nullCheck(int val)
        {
            try
            {

                string result = "";

                if (!val.Equals(null))
                {
                    result = val.ToString();
                }

                return result;
            }
            catch
            {
                return "0";
            }

        }

        public static int nullCheckInt(int val)
        {
            try
            {
                int result = 0;

                if (val != 0)
                {
                    result = val;
                }

                return result;
            }
            catch
            {
                return 0;
            }

        }
        #endregion

        /// <summary>
        /// checkSqlParam
        /// SQLパラメータチェック
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        #region checkSqlParam
        public static string checkSqlParam(string param)
        {
            return param.Replace("\'", "\'\'");
        }
        #endregion


        /// <summary>
        /// int数値をフラグに変換する
        /// ※[1]をtrueとする場合に限る!!
        /// </summary>
        /// <param name="flg"></param>
        /// <returns></returns>
        #region bitToBool
        public static bool bitToBool(int flg)
        {
            try
            {
                return flg == 1;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// ブール型をビットに変換する
        /// </summary>
        #region boolToBit
        public static int boolToBit(bool flg)
        {
            if (flg)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// ブル型のリストをビットのリストに変換する
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        #region boolToBitList
        public static List<int> boolToBitList(List<bool> boolList)
        {
            List<int> result = new List<int>();
            foreach (bool flg in boolList)
            {
                result.Add(boolToBit(flg));
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 正反対のフラグを返す
        /// </summary>
        /// <param name="flg"></param>
        /// <returns></returns>
        #region returnReverseFlg
        public static int returnReverseFlg(bool flg)
        {
            if (flg)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        #endregion

        /// <summary>
        /// calcSecToMin
        /// 秒から分への変換
        /// </summary>
        /// <returns></returns>
        #region calcSecToMin
        public static double calcSecToMin(double min)
        {
            return min / 60;
        }
        #endregion

        /// <summary>
        /// calcMinToSec
        /// 分から秒への変換
        /// </summary>
        /// <returns></returns>
        #region calcMinToSec
        public static double calcMinToSec(double min)
        {
            return min * 60;
        }
        #endregion

        /// <summary>
        /// calcMinToMilsec
        /// 分からミリ秒への変換
        /// </summary>
        /// <returns></returns>
        #region calcMinToMilsec
        public static double calcMinToMilsec(double min)
        {
            return min * 60000;
        }
        #endregion

        /// <summary>
        /// 時刻ストリングから秒数を計算する
        /// </summary>
        /// <returns></returns>
        #region culcSecond
        public static int culcSecond(string time)
        {
            int result = 0;

            try
            {
                string[] hms = time.Split(':');

                //数値チェック
                foreach (string str in hms)
                {
                    if (!IsNumeric(str))
                    {
                        return 0;
                    }
                }

                for (int idx = 0; idx < hms.Length; idx++)
                {
                    if (idx > 2) { return result; }
                    if (idx == 0)
                    {
                        result = result + int.Parse(hms[0]) * 1000;
                    }
                    else if (idx == 1)
                    {
                        result = result + int.Parse(hms[1]) * 60000;
                    }
                    else if (idx == 2)
                    {
                        result = result + int.Parse(hms[2]) * 3600000;
                    }
                }


                return result;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// 正しい年月かチェックする
        /// </summary>
        /// <returns></returns>
        #region checkDateTime
        public static bool checkDateTime(string timeString)
        {
            DateTime dt;
            return DateTime.TryParse(timeString, out dt);
        }
        #endregion

        /// <summary>
        /// ストリングを日付に変換する
        /// </summary>
        /// <returns></returns>
        #region strToDateTime
        public static DateTime strToDateTime(string timeString)
        {
            try
            {
                return DateTime.Parse(timeString);
            }
            catch
            {
                return DateTime.Now;
            }

        }
        #endregion

        /// <summary>
        /// ストリングの日付をデイとに変換して比較する
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>-1 : 失敗,0 : 左大,1 : 等しい,2 : 右大</returns>
        #region comPairToDate
        public static int comPairToDate(string left, string right)
        {
            return comPairToDate(DateTime.Parse(left), DateTime.Parse(right));
        }
        public static int comPairToDate(DateTime left, DateTime right)
        {
            try
            {
                //左大
                if (left > right)
                {
                    return 0;
                }
                //右代
                else if (left < right)
                {
                    return 2;
                }
                //イコール
                else
                {
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        /// <summary>
        /// getYobi
        /// 日付から曜日を取得する
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        #region getYobi
        public static string getYobi(DateTime date)
        {
            return ("日月火水木金土").Substring(int.Parse(date.DayOfWeek.ToString("d")), 1);
        }
        public static string getYobi(string date)
        {
            try
            {
                return getYobi(DateTime.Parse(date));
            }
            catch
            {
                return getYobi(DateTime.Now);
            }
        }
        #endregion


        /// <summary>
        /// atetimeFromCompleteTimeDateTime
        /// </summary>
        /// <param name="completeTime"></param>
        /// <returns></returns>
        #region atetimeFromCompleteTimeDateTime
        public static DateTime atetimeFromCompleteTimeDateTime(string completeTime)
        {
            return DateTime.Parse(atetimeFromCompleteTimeString(completeTime));
        }
        public static string atetimeFromCompleteTimeString(string completeTime)
        {
            try
            {
                string date = "";
                //int hour = 0;
                //int min = 0;
                //int sec = 9;
                string[] buf = completeTime.Split('T');
                string[] buf2 = buf[1].Split('+');
                string[] buf3 = buf2[0].Split(':');
                string[] buf4 = buf2[1].Split(':');

                //日時の算出
                date = buf[0].Replace("-", "/");

                //時刻の算出
                //hour = int.Parse(buf3[0]) + int.Parse(buf4[0]);
                //min = int.Parse(buf3[1]) + int.Parse(buf4[1]);
                //sec = int.Parse(buf3[2]);

                //結果を返す
                //return date + " " + hour + ":" + min + ":" + sec;

                return date + " " + buf2[0];
            }
            catch
            {
                return DateTime.Now.ToString();
            }


        }


        #endregion

        /// <summary>
        /// a/bの結果を切り上げで返す
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        #region divaideRoundUp
        public static int divaideRoundUp(int a, int b)
        {
            decimal A = new decimal(a);
            decimal B = new decimal(b);
            decimal result = Math.Ceiling(A / B);
            return (int)result;
        }
        #endregion

        /// <summary>
        /// 文中に含まれる文字数をカウントする
        /// </summary>
        /// <returns></returns>
        #region countTargetStr
        public static int countTargetStr(string targetStr, string countStr)
        {
            int idx = 0;
            int cnt = 0;

            try
            {

                if (targetStr.Length < 1)
                {
                    return 0;
                }

                while (idx >= 0)
                {
                    idx = targetStr.IndexOf(countStr, idx + 1);

                    if (idx >= 0)
                    {
                        cnt++;
                    }

                    //バカよけ
                    if (cnt > 10000)
                    {
                        return 0;
                    }
                }
                return cnt;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// ストリングのバイト数を取得する
        /// </summary>
        #region getStringByte
        public static int getStringByte(string str)
        {
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            return sjisEnc.GetByteCount(str);
        }
        #endregion

        /// <summary>
        /// ストリングをintに変換する
        /// </summary>
        #region stringToInt
        public static int stringToInt(string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// ストリングをlongに変換する
        /// </summary>
        #region stringToLong
        public static long stringToLong(string str)
        {
            try
            {
                return long.Parse(str);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// ディレクトリをコピーする
        /// </summary>
        /// <param name="sourceDirName">コピーするディレクトリ</param>
        /// <param name="destDirName">コピー先のディレクトリ</param>
        #region CopyDirectory
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            //コピー先のディレクトリがないときは作る
            if (!System.IO.Directory.Exists(destDirName))
            {
                System.IO.Directory.CreateDirectory(destDirName);
                //属性もコピー
                System.IO.File.SetAttributes(destDirName,
                    System.IO.File.GetAttributes(sourceDirName));
            }

            //コピー先のディレクトリ名の末尾に"\"をつける
            if (destDirName[destDirName.Length - 1] !=
                    System.IO.Path.DirectorySeparatorChar)
                destDirName = destDirName + System.IO.Path.DirectorySeparatorChar;

            //コピー元のディレクトリにあるファイルをコピー
            string[] files = System.IO.Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                System.IO.File.Copy(file,
                    destDirName + System.IO.Path.GetFileName(file), true);
            }

            //コピー元のディレクトリにあるディレクトリについて、
            //再帰的に呼び出す
            string[] dirs = System.IO.Directory.GetDirectories(sourceDirName);
            {
                foreach (string dir in dirs)
                    CopyDirectory(dir, destDirName + System.IO.Path.GetFileName(dir));
            }
        }
        #endregion

        /// <summary>
        /// ファイルコピー
        /// </summary>
        /// <param name="sourceFileName">コピー元ファイルパス</param>
        /// <param name="destFileName">コピー先ファイルパス</param>
        #region CopyFile
        public static bool CopyFile(string sourceFileName, string destFileName)
        {
            try
            {
                File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// フォルダにあるファイルを全てコピー
        /// </summary>
        /// <param name="sourceFileName">コピー元ファイルパス</param>
        /// <param name="destFileName">コピー先ファイルパス</param>
        #region CopyFileDirAll
        public static bool CopyFileDirAll(string sourcePath, string destPath)
        {
            try
            {
                foreach (string stCopyFrom in System.IO.Directory.GetFiles(sourcePath))
                {
                    string stCopyTo = System.IO.Path.Combine(destPath, System.IO.Path.GetFileName(stCopyFrom));
                    System.IO.File.Copy(stCopyFrom, stCopyTo, true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// RenameFile
        /// ファイルコピー
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        #region RenameFile
        public static bool RenameFile(string sourceFileName, string loadFile)
        {
            try
            {
                System.IO.File.Move(sourceFileName, loadFile);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// ファイルでリート
        /// </summary>
        /// <param name="sourveFileName"></param>
        /// <returns></returns>
        #region DeleteFile
        public static bool DeleteFile(string sourveFileName)
        {
            try
            {
                System.IO.File.Delete(sourveFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// ディレクトリでリート
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        #region DeleteDir
        public static bool DeleteDir(string dirName)
        {
            try
            {
                System.IO.Directory.Delete(dirName, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 大正ディレクトリのファイルをすべて削除する
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static bool DeleteFileTargetDir(string dirName)
        {
            try
            {
                foreach (var item in getFileList(dirName, "*.*"))
                {
                    DeleteFile(item);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// getFileList
        /// ファイルリストを取得する
        /// ファイルは名前順に取得される。
        /// ソースはここ:http://msdn.microsoft.com/library/en-us/fileio/fs/findnextfile.asp
        /// </summary>
        /// <param name="sourveFileName"></param>
        /// <returns></returns>
        #region getFileList
        public static List<string> getFileList(string dir, string ptn)
        {
            List<string> fileList = new List<string>();

            string[] files = Directory.GetFiles(dir, ptn);
            foreach (string s in files)
            {
                fileList.Add(s);
            }

            string[] dirs = Directory.GetDirectories(dir);
            foreach (string s in dirs)
            {
                fileList.AddRange(getFileList(s, ptn));
            }

            return fileList;
        }
        public static List<string> getFileListNonCicle(string dir, string ptn)
        {
            List<string> fileList = new List<string>();

            string[] files = Directory.GetFiles(dir, ptn);
            foreach (string s in files)
            {
                fileList.Add(s);
            }

            return fileList;
        }
        #endregion


        /// <summary>
        /// ファイル存在チェック
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        #region ExistsFile
        public static bool ExistsFile(string sourceFileName)
        {
            return File.Exists(sourceFileName);
        }
        #endregion

        /// <summary>
        /// ディレクトリ存在チェック
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        #region ExistsDir
        public static bool ExistsDir(string sourceDirName)
        {
            return Directory.Exists(sourceDirName);
        }
        #endregion

        /// <summary>
        /// getTextFileText
        /// テキストファイルの内容をリストで返す
        /// </summary>
        /// <param name="sourveFileName"></param>
        /// <returns></returns>
        #region getTextFileText
        public static List<string> getTextFileText(string path)
        {
            string line = "";
            List<string> resList = new List<string>();
            //テキストファイル読み込み
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(LpsDefine.ENCODING_SJIS)))
            {
                //処理
                while ((line = sr.ReadLine()) != null)
                {
                    resList.Add(line);
                }
            }

            return resList;
        }
        public static List<string> getTextFileText(string path, Encoding enc)
        {
            string line = "";
            List<string> resList = new List<string>();
            //テキストファイル読み込み
            using (StreamReader sr = new StreamReader(path, enc))
            {
                //処理
                while ((line = sr.ReadLine()) != null)
                {
                    resList.Add(line);
                }
            }

            return resList;
        }
        #endregion

        /// <summary>
        /// writeTextFile
        /// リストの内容をテキストファイルに書き出す
        /// </summary>
        /// <param name="sourveFileName"></param>
        /// <returns></returns>
        #region writeTextFile
        public static bool writeTextFile(List<string> list, string path)
        {
            try
            {
                Encoding sjis = Encoding.GetEncoding("Shift-JIS");
                using (StreamWriter w = new StreamWriter(path, false, sjis))
                {
                    foreach (string line in list)
                    {
                        w.WriteLine(line);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static bool writeTextFile(List<string> list, string path, Encoding enc)
        {
            try
            {
                using (StreamWriter w = new StreamWriter(path, false, enc))
                {
                    foreach (string line in list)
                    {
                        w.WriteLine(line);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static bool writeTextFile(string msg, string path, Encoding enc)
        {
            try
            {
                using (StreamWriter w = new StreamWriter(path, false, enc))
                {
                    w.Write(msg);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        /// <summary>
        /// 引数文字列をUnicode表現
        /// </summary>
        /// <remarks>
        /// 引数文字列をunicode表現(\u+XXXX)に変換します。
        /// </remarks>
        /// <param name="str">文字列</param>
        /// <returns>unicode表現文字列</returns>
        #region toUnicodeExpression
        public static string toUnicodeExpression(string str)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(str);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < byteArray.Length; i += 2)
            {
                sb.Append(@"\u");
                sb.Append(string.Format("{0:X2}{1:X2}", byteArray[i + 1], byteArray[i]));
            }

            return sb.ToString();
        }
        #endregion


        /// <summary
        /// Unicode表現文字列をUnicodeに変換します。
        /// </summary>
        /// <remarks>
        /// unicode表現(\u+XXXX)をUnicode文字列に変換します。
        /// ToUnicodeExpressionの対になるメソッドです
        /// </remarks>
        /// <param name="str">Unicode表現文字列(\uXXXX)</param>
        /// <returns>Unicode文字列</returns>
        #region reverseToUniCode
        public static string reverseToUniCode(string str)
        {

            //正規表現でユニコード表現文字列を検索・抽出します。

            IList codeList = new ArrayList();

            Regex regUnicode = new Regex(@"(\\u){1}[0-9a-fA-F]{4}");

            for (Match matchUniCode = regUnicode.Match(str);

                        matchUniCode.Success;

                                matchUniCode = matchUniCode.NextMatch())
            {

                codeList.Add(matchUniCode.Groups[0].Value.Replace(@"\u", ""));

            }

            StringBuilder sb = new StringBuilder();

            //リトルエンディアン方式を前提にしているので

            //0,1文字目と2,3文字目の組を16進文字列とみなし、数値に変換します。

            //0,1文字目の組と、2,3文字目の組の順序を入れ替えてbyte配列を作成し、

            //エンコーディングを行います。

            //上記正規表現にマッチしている以上、char配列は4個あることが保証されています。

            //(配列数のチェックはしません)

            foreach (string unicode in codeList)
            {

                char[] codeArray = unicode.ToCharArray();

                int intVal1 = Convert.ToByte(codeArray[0].ToString(), 16) *

                                     16 + Convert.ToByte(codeArray[1].ToString(), 16);

                int intVal2 = Convert.ToByte(codeArray[2].ToString(), 16) *

                                     16 + Convert.ToByte(codeArray[3].ToString(), 16);

                sb.Append(Encoding.Unicode.GetString(

                                new byte[] { (byte)intVal2, (byte)intVal1 }));

            }

            return sb.ToString();

        }
        #endregion

        /// <summary>
        /// ストリングを文字数で分割する
        /// </summary>
        /// <param name="self"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        #region stringSplitAtCount
        public static string[] stringSplitAtCount(this string self, int count)
        {
            var result = new List<string>();
            var length = (int)Math.Ceiling((double)self.Length / count);

            for (int i = 0; i < length; i++)
            {
                int start = count * i;
                if (self.Length <= start)
                {
                    break;
                }
                if (self.Length < start + count)
                {
                    result.Add(self.Substring(start));
                }
                else
                {
                    result.Add(self.Substring(start, count));
                }
            }

            return result.ToArray();
        }
        #endregion



        /// <summary>
        /// ニコニコドメインのURLかチェックする
        /// </summary>
        /// <returns></returns>
        #region domainCheck
        public static bool domainCheck(string uri, string targetDomain)
        {
            //ニコニコドメインチェック
            if (!uri.Equals("") && uri != null)            //uri未設定でなければチェック
            {
                return uri.StartsWith(targetDomain);
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 最後の一文字がスラッシュかバックスラッシュなら削る
        /// </summary>
        #region removeLast
        public static string removeLast(string target)
        {
            string result = target;
            if (target.Substring(target.Length - 1, 1).Equals("\\"))
            {
                result = target.Remove(target.Length - 1, 1);
            }
            else if (target.Substring(target.Length - 1, 1).Equals("/"))
            {
                result = target.Remove(target.Length - 1, 1);
            }

            return result;
        }
        #endregion

        /// -----------------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の左端から指定したバイト数分の文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。<param>
        /// <param name="iByteSize">
        ///     取り出すバイト数。</param>
        /// <returns>
        ///     左端から指定されたバイト数分の文字列。</returns>
        /// -----------------------------------------------------------------------------------------
        #region　LeftB メソッド


        public static string LeftB(string stTarget, int iByteSize)
        {
            return MidB(stTarget, 1, iByteSize);
        }

        #endregion

        /// -----------------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の指定されたバイト位置以降のすべての文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iStart">
        ///     取り出しを開始する位置。</param>
        /// <returns>
        ///     指定されたバイト位置以降のすべての文字列。</returns>
        /// -----------------------------------------------------------------------------------------
        #region　MidB メソッド (+1)


        public static string MidB(string stTarget, int iStart)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            return hEncoding.GetString(btBytes, iStart - 1, btBytes.Length - iStart + 1);
        }

        /// -----------------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の指定されたバイト位置から、指定されたバイト数分の文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iStart">
        ///     取り出しを開始する位置。</param>
        /// <param name="iByteSize">
        ///     取り出すバイト数。</param>
        /// <returns>
        ///     指定されたバイト位置から指定されたバイト数分の文字列。</returns>
        /// -----------------------------------------------------------------------------------------
        public static string MidB(string stTarget, int iStart, int iByteSize)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            if (btBytes.Length <= iByteSize)
            {
                iByteSize = btBytes.Length;
            }

            return hEncoding.GetString(btBytes, iStart - 1, iByteSize);
        }

        #endregion


        /// -----------------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の右端から指定されたバイト数分の文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iByteSize">
        ///     取り出すバイト数。</param>
        /// <returns>
        ///     右端から指定されたバイト数分の文字列。</returns>
        /// -----------------------------------------------------------------------------------------
        #region　RightB メソッド


        public static string RightB(string stTarget, int iByteSize)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            return hEncoding.GetString(btBytes, btBytes.Length - iByteSize, iByteSize);
        }

        #endregion
        
        /// <summary>
        /// createLiplisGuid
        /// 独自Guidを作成する
        /// </summary>
        #region createLiplisGuid
        public static string createLiplisGuid()
        {
            return Guid.NewGuid().ToString() + String.Format("{0:000}", DateTime.Now.Millisecond);
        }
        #endregion

        /// <summary>
        /// getEisuOnly
        /// 英数字のみを抽出する
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        #region getEisuOnly
        public static string getEisuOnly(string str)
        {
            //結果
            string res = str;

            //Regexオブジェクトを作成
            Regex r = new Regex(@"[A-Z0-9]", RegexOptions.IgnoreCase);

            //TextBox1.Text内で正規表現と一致する対象を1つ検索
            Match m = r.Match(str);

            //次のように一致する対象をすべて検索することもできる
            //System.Text.RegularExpressions.MatchCollection mc = r.Matches(TextBox1.Text);

            while (m.Success)
            {
                str += m.Value.ToString();
                m = m.NextMatch();
            }

            return res;
        }

        #endregion

        /// <summary>
        /// getEisuOnly
        /// 英数字のみを抽出する
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        #region getEisuOnly
        public static string getEisuJpOnly(string str)
        {
            //結果
            string res = "";
            string resb = "";

            //Regexオブジェクトを作成
            Regex r = new Regex(@"[^\x01-\x7E]", RegexOptions.IgnoreCase);

            //TextBox1.Text内で正規表現と一致する対象を1つ検索
            Match m = r.Match(str);

            //次のように一致する対象をすべて検索することもできる
            //System.Text.RegularExpressions.MatchCollection mc = r.Matches(TextBox1.Text);

            while (m.Success)
            {
                resb += m.Value.ToString();
                m = m.NextMatch();
            }

            res += resb;
            resb = "";

            r = new Regex(@"[a-zA-Z0-9]", RegexOptions.IgnoreCase);

            //TextBox1.Text内で正規表現と一致する対象を1つ検索
            m = r.Match(str);

            //次のように一致する対象をすべて検索することもできる
            //System.Text.RegularExpressions.MatchCollection mc = r.Matches(TextBox1.Text);

            while (m.Success)
            {
                resb += m.Value.ToString();
                m = m.NextMatch();
            }

            res += resb;

            return res;
        }

        #endregion
        
        /// <summary>
        /// ストリームからデータを読み込み、バイト配列に格納
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        #region ReadBinaryData
        public static byte[] readBinaryData(Stream st)
        {
            byte[] buf = new byte[32768]; // 一時バッファ

            using (MemoryStream ms = new MemoryStream())
            {

                while (true)
                {
                    // ストリームから一時バッファに読み込む
                    int read = st.Read(buf, 0, buf.Length);

                    if (read > 0)
                    {
                        // 一時バッファの内容をメモリ・ストリームに書き込む
                        ms.Write(buf, 0, read);
                    }
                    else
                    {
                        break;
                    }
                }
                // メモリ・ストリームの内容をバイト配列に格納
                return ms.ToArray();
            }
        }
        #endregion
        
        /// LenB
        /// 文字列のバイト数を取得する
        /// </summary>
        /// <param name="stTarget"></param>
        /// <returns></returns>
        #region　LenB メソッド
        public static int LenB(string stTarget)
        {
            return System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(stTarget);
        }
        #endregion

        /// <summary>
        /// deepClone
        /// オブジェクトをディープコピーする
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        #region deepClone
        public static T deepClone<T>(T source)
        {
            object target = null;

            using (MemoryStream stream = new MemoryStream())
            {

                // コピー元オブジェクトをシリアライズします。

                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, source);

                stream.Position = 0;

                // シリアライズデータをコピー先オブジェクトにデシリアライズします。
                target = formatter.Deserialize(stream);
            }


            return (T)target;
        }
        #endregion

        /// <summary>
        /// ランダムな文字列を生成する
        /// </summary>
        #region getName
        private static readonly string passwordChars = "0123456789abcdefghijklmnopqrstuvwxyz";
        public static string getName(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random r;
            try
            {
                System.Threading.Thread.Sleep(10);
                r = new Random(Environment.TickCount + length);
            }
            catch
            {
                r = new Random();
            }


            for (int i = 0; i < length; i++)
            {
                //文字の位置をランダムに選択
                int pos = r.Next(passwordChars.Length);
                //選択された位置の文字を取得
                char c = passwordChars[pos];
                //パスワードに追加
                sb.Append(c);
            }

            return sb.ToString();
        }
        #endregion

        ///====================================================================
        ///
        ///                           SQL関連
        ///                         
        ///====================================================================

        // 基本的なエスケープ
        #region SafeSqlLiteral
        public static string SafeSqlLiteral(string inputSql)
        {
            return inputSql.Replace("'", "’");
        }
        #endregion

        // LIKE句を使用するする文字列については、
        // さらにいくつかの文字列エスケープが必要
        #region SafeSqlLikeClauseLiteral
        public static string SafeSqlLikeClauseLiteral(string inpurSql)
        {
            return inpurSql.Replace("'", "’")
            .Replace("[", "[[]")
            .Replace("%", "[%]")
            .Replace("_", "[_]");
        }
        #endregion
    }
}
