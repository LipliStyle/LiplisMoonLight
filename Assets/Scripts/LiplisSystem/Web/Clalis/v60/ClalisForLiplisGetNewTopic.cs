//====================================================================
//  ClassName : ClalisForLiplisGetNewTopic
//  概要      : 最新話題データ取得
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;

namespace Assets.Scripts.LiplisSystem.Web.Clalis.v60
{
    public class ClalisForLiplisGetNewTopic
    {
        /// <summary>
        /// 最新話題データ取得
        /// 現在未使用　廃止予定
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static ResLpsTopicList GetNewTopic()
        {
            try
            {
                NameValueCollection ps = new NameValueCollection();

                //Jsonで結果取得
                string jsonText = HttpPost.sendPostAndDecompressUtf8(LpsDefine.LIPLIS_API_NEW_TOPIC, ps);

                //APIの結果受け取り用クラス
                return JsonConvert.DeserializeObject<ResLpsTopicList>(jsonText);
            }
            catch
            {
                return null;
            }
        }
    }
}
