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
        ///デフォルト画像サイズ
        #region エンコード
        public const float SCREAN_DEFAULT_WIDTH = 1920f;
        public const float SCREAN_SIZE_DEFAULT_HEIGTH = 1080f;
        //public const float SCREAN_DEFAULT_WIDTH = 960f;
        //public const float SCREAN_SIZE_DEFAULT_HEIGTH = 540f;
        public const float SCREAN_SIZE_DEFAULT_HEIGTH90 = 972;
        public const float SCREAN_SIZE_DEFAULT_HEIGTH80 = 864;
        #endregion

        ///=============================
        /// URL定義
        #region URL定義
        public const string LIPLIS_API_SHORT_NEWS        = @"https://liplis.mine.nu/Clalis/v41/Liplis/ClalisForLiplisWeb.aspx";//HTTPSにしてはならない //2014/04/07 ver4.0.0 Clalis4.0採用         
        public const string LIPLIS_API_WEATHER_NEWS      = @"https://liplis.mine.nu/Clalis/v40/Liplis/ClalisForLiplisWeatherInfo.aspx";
        public const string LIPLIS_API_LOCATION_WEATHER  = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisLocationWetherList.aspx";
        public const string LIPLIS_API_LOCATION          = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisLocationInfomation.aspx";
        public const string LIPLIS_API_WHATDAY_ISTODAY   = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisWhatDayIsToday.aspx";
        public const string LIPLIS_API_NEW_TOPIC         = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopic.aspx";
        public const string LIPLIS_API_NEW_TOPIC_2       = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopic2.aspx";

        public const string LIPLIS_API_NEW_TOPIC_2_MLT         = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopic2Mlt.aspx";
        public const string LIPLIS_API_NEW_TOPIC_2_MLT_LIGHT   = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopic2MltLight.aspx";
        public const string LIPLIS_API_NEW_TOPIC_ONE_MLT       = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopicOneMlt.aspx";

        //トピック取得API
        public const string LIPLIS_API_NEW_TOPIC_ML       = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopicMl.aspx";
        public const string LIPLIS_API_NEW_TOPIC_ML_LIGHT = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewTopicMlLight.aspx";

        //ニュースリスト取得API
        public const string LIPLIS_API_NEWS_LIST                  = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetNewsList.aspx";

        //サムネイルプロキシ関連
        public const string LIPLIS_API_THUMBNAIL_PROXY_LIST       = @"https://liplis.mine.nu/Clalis/v60/Liplis/ClalisForLiplisThumbnailProxyList.aspx";
        public const string LIPLIS_API_THUMBNAIL_PROXY_SMALL      = @"https://liplis.mine.nu/Clalis/v60/Liplis/ClalisForLiplisThumbnailProxySmall.aspx";
        public const string LIPLIS_API_THUMBNAIL_PROXY            = @"https://liplis.mine.nu/Clalis/v60/Liplis/ClalisForLiplisThumbnailProxy.aspx";

        //ボイス関連
        public const string LIPLIS_API_VOICE_MP3          = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetVoiceMp3.aspx";
        public const string LIPLIS_API_VOICE_MP3_ONDEMAND = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetVoiceMp3Ondemand.aspx";
        public const string LIPLIS_API_VOICE_WAV          = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetVoiceWav.aspx";
        public const string LIPLIS_API_VOICE_WAV_ONDEMAND = @"https://liplis.mine.nu/Clalis/v60/Compress/ClalisForLiplisGetVoiceWavOndemand.aspx";

        //トーンURL
        public const string LIPLIS_TONE_URL_HAZUKI  = @"little.pretty.hazuki.xml";
        public const string LIPLIS_TONE_URL_SHIROHA = @"little.pretty.shiroha.xml";
        public const string LIPLIS_TONE_URL_KUROHA  = @"little.pretty.kuroha.xml";
        public const string LIPLIS_TONE_URL_MOMOHA  = @"little.pretty.momoha.xml";


        #endregion

        ///=============================
        /// プリファレンス設定キー
        #region プリファレンス設定キー
        public const string SETKEY_LIPLIS_STATUS = "LiplisStatus";
        public const string SETKEY_LIPLIS_SETTING = "LiplisSetting";
        public const string SETKEY_LIPLIS_CACHE = "LiplisCache";
        #endregion

        ///=============================
        /// アプリケーションタイトル
        #region アプリケーションタイトル
        public const string APPLICATION_TITLE = "Liplis MoonLight";
        #endregion

        ///=============================
        /// プリファレンス設定キー
        #region
        public const string UN_CANVAS_RENDERING = "CanvasRendering";
        public const string UN_CANVAS_FRONT = "CanvasFront";
        public const string UN_CANVAS_BACKGROUND = "CanvasBackGround";



        #endregion



    }
}
