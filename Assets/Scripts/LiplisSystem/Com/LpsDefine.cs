using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.LiplisSystem.Com
{
    public class LpsDefine
    {

        ///=============================
        /// エンコード
        #region エンコード
        public const string ENCODING_SJIS         = "Shift-JIS";    //日本語
        public const string ENCODING_UTF8         = "UTF-8";        //日本語
        public const string ENCODING_GB2312       = "936";          //中国語 簡易
        public const string ENCODING_BIG5         = "950";          //中国語 繁
        public const string ENCODING_IBM860       = "860";          //ポルトガル
        public const int ENCODING_ISO_2022_KR     = 50225;          //朝鮮
        public const string ENCODING_WINDOWS_1256 = "1256";         //アラビア
        public const string ENCODING_IBM863       = "863";          //フランス語
        public const int ENCODING_UNICODE         = 1200;

        #endregion

        ///=============================
        /// URL定義
        #region URL定義
        public const string LIPLIS_API_SUMMARY_NEWS      = @"http://liplis.mine.nu/Clalis/v41/Liplis/ClalisForLiplis.aspx";                        //2014/04/07 ver4.0.0 Clalis4.0採用
        public const string LIPLIS_API_SUMMARY_NEWS_LIST = @"http://liplis.mine.nu/Clalis/v41/Liplis/ClalisForLiplisFx.aspx";                      //2014/04/07 ver4.0.0 Clalis4.0採用                  //2014/04/07 ver4.0.0 Clalis4.0採用
        public const string LIPLIS_API_SHORT_NEWS        = @"http://liplis.mine.nu/Clalis/v41/Liplis/ClalisForLiplisWeb.aspx";                     //2014/04/07 ver4.0.0 Clalis4.0採用         
        public const string LIPLIS_API_SHORT_NEWS_LIST   = @"http://liplis.mine.nu/Clalis/v41/Liplis/ClalisForLiplisWebFx.aspx";
        public const string LIPLIS_API_WEATHER_NEWS      = @"http://liplis.mine.nu/Clalis/v40/Liplis/ClalisForLiplisWeatherInfo.aspx";
        public const string LIPLIS_API_LOCATION_WEATHER  = @"http://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisLocationWetherList.aspx";
        public const string LIPLIS_API_LOCATION          = @"http://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisLocationInfomation.aspx";
        public const string LIPLIS_API_WHATDAY_ISTODAY   = @"http://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisWhatDayIsToday.aspx";
        public const string LIPLIS_API_NEW_TOPIC         = @"http://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopic.aspx";
        public const string LIPLIS_API_NEW_TOPIC_2       = @"http://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopic2.aspx";
        public const string LIPLIS_API_NEW_TOPIC_ONE     = @"http://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopicOne.aspx";

        public const string LIPLIS_API_NEW_TOPIC_2_MLT   = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopic2Mlt.aspx";
        public const string LIPLIS_API_NEW_TOPIC_ONE_MLT = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopicOneMlt.aspx";

        public const string LIPLIS_TONE_URL_HAZUKI  = @"little.pretty.hazuki.xml";
        public const string LIPLIS_TONE_URL_SHIROHA = @"little.pretty.shiroha.xml";
        public const string LIPLIS_TONE_URL_KUROHA  = @"little.pretty.kuroha.xml";
        public const string LIPLIS_TONE_URL_MOMOHA  = @"little.pretty.momoha.xml";


        #endregion

        ///=============================
        /// プリファレンス設定キー
        #region プリファレンス設定キー
        public const string SETKEY_LIPLIS_STATUS = "LiplisStatus";
        #endregion

        ///=============================
        /// アプリケーションタイトル
        #region アプリケーションタイトル
        public const string APPLICATION_TITLE = "Liplis MoonLight";
        #endregion

    }
}
