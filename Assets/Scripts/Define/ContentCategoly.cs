using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Define
{

    /// <summary>
    /// コンテンツ定義
    /// 
    /// ここで設定するインデックスは、Unity上に設定したページの順番と揃える必要あり！
    /// </summary>
    public enum ContentCategoly
    {
        home = 0,
        news = 2,
        matome = 3,
        retweet = 4,
        hotPicture = 5,
        hotHash = 6
    }

    /// <summary>
    /// コンテンツカテゴリーテキスト
    /// </summary>
    public class ContentCategolyText
    {
        /// <summary>
        /// コンテンツに対応するテキストを返す
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static string GetContentText(ContentCategoly cat)
        {
            switch (cat)
            {
                case ContentCategoly.home:
                    return "おまかせ";
                case ContentCategoly.news:
                    return "ニュース";
                case ContentCategoly.matome:
                    return "まとめ";
                case ContentCategoly.retweet:
                    return "リツイート";
                case ContentCategoly.hotPicture:
                    return "話題の画像";
                case ContentCategoly.hotHash:
                    return "ハッシュ";
                default:
                    return "";

            }
        }

        public static string GetContentText(int cat)
        {
            switch (cat)
            {
                case (int)ContentCategoly.home:
                    return "おまかせ";
                case (int)ContentCategoly.news:
                    return "ニュース";
                case (int)ContentCategoly.matome:
                    return "まとめ";
                case (int)ContentCategoly.retweet:
                    return "リツイート";
                case (int)ContentCategoly.hotPicture:
                    return "話題の画像";
                case (int)ContentCategoly.hotHash:
                    return "ハッシュ";
                default:
                    return "";

            }
        }


        public static string GetContentText(string sCat)
        {
            try
            {
                int cat = int.Parse(sCat);

                return GetContentText(cat);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// コンテントカテゴリを取得する
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ContentCategoly GetContentCategoly(string code)
        {
            switch (code)
            {
                case "0":
                    return ContentCategoly.home;
                case "2":
                    return ContentCategoly.news;
                case "3":
                    return ContentCategoly.matome;
                case "4":
                    return ContentCategoly.retweet;
                case "5":
                    return ContentCategoly.hotPicture;
                case "6":
                    return ContentCategoly.hotHash;
                default:
                    return ContentCategoly.home;
            }
        }
    }




}
