using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Clalis.v40.Res;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace Assets.Scripts.LiplisSystem.Web.Clalis.v40
{
    public static class ClalisShortNews
    {
        public static MsgTalkMessage getShortNews(string uid, string toneUrl, string newsFlg)
        {
            MsgTalkMessage msg = new MsgTalkMessage();
            try
            {
                NameValueCollection ps = new NameValueCollection();
                ps.Add("tone", toneUrl);                //TONE_URLの指定
                ps.Add("newsFlg", newsFlg);             //NEWS_FLGの指定

                //Jsonで結果取得
                string jsonText = HttpPost.sendPost(LpsDefine.LIPLIS_API_SHORT_NEWS, ps);

                //APIの結果受け取り用クラス
                ResLpsShortNews2Json result = JsonConvert.DeserializeObject<ResLpsShortNews2Json>(jsonText);

                //結果を返す
                return LiplisNewsJpJson.getShortNews(result);
            }
            catch
            {
                return msg;
            }
        }
    }
}
