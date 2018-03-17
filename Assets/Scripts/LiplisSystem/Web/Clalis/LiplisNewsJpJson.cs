using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Clalis.v40.Res;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.LiplisSystem.Web.Clalis
{
    public class LiplisNewsJpJson
    {
        //============================================================
        //
        //                       パブリックメソッド
        //
        //============================================================
        #region 変換処理

        /// <summary>
        /// サマリーニュースの結果をMsgTalkMessageに変換して取得する
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static MsgTalkMessage getSummaryNews(string jsonText)
        {
            return json2MsgTalk(jsonText);
        }

        /// <summary>
        /// サマリーニュースリストの結果をMsgTalkMessageのリストにして取得する
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static List<MsgTalkMessage> getSummaryNewsList(string jsonText)
        {
            return json2MsgNewsList(jsonText);
        }

        /// <summary>
        /// ショートニュースを取得する
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static MsgTalkMessage getShortNews(string jsonText)
        {
            return json2MsgShortNews(jsonText);
        }
        public static MsgTalkMessage getShortNews(ResLpsShortNews2Json rlsn2)
        {
            return json2MsgShortNews(rlsn2);
        }

        #endregion

        //============================================================
        //
        //                            変換処理
        //
        //============================================================
        #region 変換処理
        /// <summary>
        /// サマリーニュースの取得結果をMsgTalkMessageに変換する
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
            protected static MsgTalkMessage json2MsgTalk(string jsonText)
        {
            return json2MsgSummaryNews(JsonConvert.DeserializeObject<ResLpsSummaryNews2Json>(jsonText));
        }
        protected static MsgTalkMessage json2MsgSummaryNews(ResLpsSummaryNews2Json rlsn2)
        {
            //結果メッセージを作成
            MsgTalkMessage msg = new MsgTalkMessage();

            //リザルトSB
            StringBuilder sbResult = new StringBuilder();

            //ネームリスト、等作成
            foreach (string desc in rlsn2.descriptionList)
            {
                //ネームリストに追記
                msg.createList(desc);

                //全文章追記
                sbResult.Append(msg.result);
            }

            //最後のあっとまーくを除去する
            if (msg.nameList.Count > 0)
            {
                if (msg.nameList[msg.nameList.Count - 1] == "@")
                {
                    int targetIndex = msg.nameList.Count - 1;
                    msg.nameList.RemoveAt(targetIndex);
                    msg.emotionList.RemoveAt(targetIndex);
                    msg.pointList.RemoveAt(targetIndex);
                }
            }

            //EOSの除去
            string result = sbResult.ToString().Replace("EOS", "");

            //結果をメッセージに格納
            msg.url = LpsLiplisUtil.nullCheck(rlsn2.url);
            msg.title = LpsLiplisUtil.nullCheck(rlsn2.title);
            msg.result = result;
            msg.sorce = result;
            msg.jpgUrl = "";
            msg.calcNewsEmotion();

            ///jpgURLのセット
            if (rlsn2.jpgUrl != null)
            {
                if (!rlsn2.jpgUrl.Equals(""))
                {
                    msg.jpgUrl = rlsn2.jpgUrl;
                }

            }
            return msg;
        }

        /// <summary>
        /// json2MsgNewsList
        /// サマリーニュースの取得結果をトMsgTalkMessageリストに変換する
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static List<MsgTalkMessage> json2MsgNewsList(string jsonText)
        {
            //結果リスト
            List<MsgTalkMessage> resList = new List<MsgTalkMessage>();

            //JSON内容取得結果
            ResLpsSummaryNews2JsonList result = JsonConvert.DeserializeObject<ResLpsSummaryNews2JsonList>(jsonText);

            foreach (ResLpsSummaryNews2Json rlsn2 in result.lstNews)
            {
                resList.Add(json2MsgSummaryNews(rlsn2));
            }

            return resList;
        }

        #endregion


        //============================================================
        //
        //                            ショートニュース
        //
        //============================================================
        #region ショートニュース

        protected static MsgTalkMessage json2MsgShortNews(string jsonText)
        {
            return json2MsgShortNews(JsonConvert.DeserializeObject<ResLpsShortNews2Json>(jsonText));
        }
        protected static MsgTalkMessage json2MsgShortNews(ResLpsShortNews2Json rlsn2)
        {
            //結果メッセージを作成
            MsgTalkMessage msg = new MsgTalkMessage();

            //リザルトSB
            StringBuilder sbResult = new StringBuilder();

            //ネームリスト、等作成
            string[] bufList = rlsn2.result.Split(';');

            foreach (string buf in bufList)
            {
                try
                {
                    string[] bufList2 = buf.Split(',');

                    if (buf.Length < 3) { break; }

                    msg.nameList.Add(bufList2[0]);
                    msg.emotionList.Add(int.Parse(bufList2[1]));
                    msg.pointList.Add(int.Parse(bufList2[2]));
                    sbResult.Append(bufList2[0]);
                }
                catch
                {

                }
            }

            string result = sbResult.ToString().Replace("EOS", "");

            //結果をメッセージに格納
            msg.url = rlsn2.url;
            msg.title = result;
            msg.result = result;
            msg.sorce = result;
            msg.calcNewsEmotion();
            msg.jpgUrl = "";

            return msg;
        }

        #endregion
    }
}
