//====================================================================
//  ClassName : DatNewTopic
//  概要      : 最新話題データ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.SubData
{
    [Serializable]
    public class DatNewTopic
    {
        ///=============================
        ///プロパティ
        public uint LastUpdateTime;

        ///=============================
        ///最新ニュースデータ
        public ResLpsTopicList LastData;

        ///=============================
        /// メッセージリスト
        public List<MsgTopic> TalkTopicList;            //トークメッセージリスト
        public List<MsgTopic> InterruptTopicList;       //割り込みトークメッセージリスト
        public List<MsgTopic> ChattedKeyList;           //おしゃべり済みトピックリスト

        ///=============================
        /// トーンURLリスト
        public List<string> ToneUrlList;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DatNewTopic()
        {
            TalkTopicList = new List<MsgTopic>();
            InterruptTopicList = new List<MsgTopic>();
            ChattedKeyList = new List<MsgTopic>();

            //トーンURLの更新
            InitToneUrlList();
        }

        /// <summary>
        /// トーンURLリストの更新
        /// </summary>
        private void InitToneUrlList()
        {
            ToneUrlList = new List<string>();

            ToneUrlList.Add(LpsDefine.LIPLIS_TONE_URL_HAZUKI);      //アロケーションID : 0
            ToneUrlList.Add(LpsDefine.LIPLIS_TONE_URL_SHIROHA);     //アロケーションID : 1
            ToneUrlList.Add(LpsDefine.LIPLIS_TONE_URL_KUROHA);      //アロケーションID : 2
            ToneUrlList.Add(LpsDefine.LIPLIS_TONE_URL_MOMOHA);      //アロケーションID : 3
        }

        /// <summary>
        /// 保持件数が少ない場合に、件数を初期化する
        /// </summary>
        public void InitLastUpdateTime()
        {
            if(TalkTopicList.Count < 100)
            {
                LastUpdateTime = 0;
            }
        }

        /// <summary>
        /// トピックリストからデータを取得する
        /// </summary>
        /// <returns></returns>
        public MsgTopic TopicListDequeue(List<LiplisModel> ModelList)
        {
            return TopicListDequeue(ModelList, 0);
        }
        public MsgTopic TopicListDequeue(List<LiplisModel> ModelList, int retryCnt)
        {
            //次の話題をロードする
            MsgTopic result = LiplisStatus.Instance.NewTopic.TalkTopicList.Dequeue();

            TopicUtil.SetAllocationIdAndTone(result, ModelList);

            //現在ロード中の話題をおしゃべり済みに入れる
            if (!result.FlgNotAddChatted)
            {
                LiplisStatus.Instance.NewTopic.ChattedKeyList.AddToNotDuplicate(result.Clone());
            }

            //話題一致ならそのまま帰す
            if (LiplisSetting.Instance.Setting.CatCheck(result.TopicClassification))
            {
                return result;
            }
            else
            {
                if(retryCnt > 10)
                {
                    //10回やってダメならそのまま帰す
                    return result;
                }
                else
                {
                    retryCnt++;
                    return TopicListDequeue(ModelList, retryCnt);
                }

            }
        }

        /// <summary>
        /// 対象データをリストから削除し、おしゃべり済みに移行する
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public MsgTopic TopicListDequeueTargetTopic(MsgTopic topic)
        {
            //次の話題をロードする
            LiplisStatus.Instance.NewTopic.TalkTopicList.Remove(topic);

            //現在ロード中の話題をおしゃべり済みに入れる
            if (!topic.FlgNotAddChatted)
            {
                LiplisStatus.Instance.NewTopic.ChattedKeyList.AddToNotDuplicate(topic.Clone());
            }

            return topic;
        }

        /// <summary>
        /// ラストトピックリストからトピックを取得する
        /// </summary>
        /// <param name="retryCnt"></param>
        /// <returns></returns>
        public MsgTopic GetTopicFromResLpsTopicList()
        {
            //次の話題をロードする
            int count = LiplisStatus.Instance.NewTopic.LastData.topicList.Count;

            //乱数取得
            System.Random r = new System.Random();

            //インデックス取得
            int idx = r.Next(0, count-1);

            //ラストトピックからランダムに1件取得する
            return LiplisStatus.Instance.NewTopic.LastData.topicList[idx].Clone();
        }

        /// <summary>
        /// トピックを検索する
        /// </summary>
        /// <param name="DataKey"></param>
        /// <returns></returns>
        public MsgTopic SearchTopic(string DataKey, List<LiplisModel> ModelList)
        {
            MsgTopic result = null;

            //トピックリストから検索。あればトピックリストから返す
            foreach (var topic in TalkTopicList)
            {
                if(topic.DataKey == DataKey)
                {
                    result = TopicListDequeueTargetTopic(topic);
                    break;
                }
            }

            //おしゃべり済みデータから検索
            if(result == null)
            {
                foreach (var topic in ChattedKeyList)
                {
                    if (topic.DataKey == DataKey)
                    {
                        result = topic.Clone();
                        break;
                    }
                }
            }

            //アロケーションID設定
            if(result != null)
            {
                TopicUtil.SetAllocationIdAndTone(result, ModelList);
            }
            
            //見つからない場合はNULLを返す
            return result;
        }

        /// <summary>
        /// キーリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyList()
        {
            List<string> keyList = new List<string>();

            foreach (MsgTopic topic in TalkTopicList)
            {
                keyList.Add(topic.DataKey);
            }

            //重複回避挿入
            foreach (MsgTopic topic in ChattedKeyList)
            {
                keyList.AddToNotDuplicate(topic.DataKey);
            }

            return keyList;
        }


        /// <summary>
        /// 重複排除
        /// </summary>
        /// <param name="NewTopic"></param>
        public List<MsgTopic> RemoveDuplicate(List<MsgTopic> topicList)
        {
            List<MsgTopic> newList = new List<MsgTopic>();

            List<string> keyList = new List<string>();

            foreach (MsgTopic topic in topicList)
            {
                if(!keyList.Contains(topic.DataKey))
                {
                    newList.Add(topic);
                    keyList.Add(topic.DataKey);
                }
            }

            return newList;
        }
        /// <summary>
        /// トークトピックリストの重複排除
        /// </summary>
        public void RemoveDuplicateTopicList()
        {
            this.TalkTopicList = RemoveDuplicate(this.TalkTopicList);
        }
        /// <summary>
        /// チャッティッドリストの重複排除
        /// </summary>
        public void RemoveDuplicateChattedList()
        {
            this.ChattedKeyList = RemoveDuplicate(this.ChattedKeyList);
        }



        /// <summary>
        /// 削除処理
        /// </summary>
        private const int MINIMUM_NUMBER = 100;
        public void DeleteOldData()
        {
            //下処理に変更
            //指定条件に合致するデータを削除する
            //LiplisStatus.Instance.NewTopic.TalkTopicList.RemoveAll(TermOldTopicDelete);
            //LiplisStatus.Instance.NewTopic.ChattedKeyList.RemoveAll(TermOldTopicDelete);

            while (TalkTopicList.Count > MINIMUM_NUMBER)
            {
                if(LpsDatetimeUtil.dec(TalkTopicList[0].CreateTime) <= DateTime.Now.AddHours(-24))
                {
                    MsgTopic topic = TalkTopicList.Dequeue();
                    topic.TalkSentenceList.Clear();
                    topic.TalkSentenceList = null;
                    topic = null;
                }
                else
                {
                    break;
                }
            }
            while (ChattedKeyList.Count > MINIMUM_NUMBER)
            {
                if (LpsDatetimeUtil.dec(ChattedKeyList[0].CreateTime) <= DateTime.Now.AddHours(-24))
                {
                    MsgTopic topic = ChattedKeyList.Dequeue();
                    topic.TalkSentenceList.Clear();
                    topic.TalkSentenceList = null;
                    topic = null;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// ラストデータから入れる
        /// </summary>
        public void SetTopicListFromChattedKeyList()
        {
            //重複排除
            RemoveDuplicateChattedList();

            //シャッフル
            ChattedKeyList.Shuffle();

            foreach (MsgTopic topic in ChattedKeyList)
            {
                TalkTopicList.Add(topic.Clone());

                //100件追加したら抜ける
                if(TalkTopicList.Count > 100)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// ラストデータから取得する
        /// </summary>
        public void SetTopicListFromLastData()
        {
            foreach (var topic in LastData.topicList)
            {
                this.TalkTopicList.Add(topic.Clone());
            }
        }

        /// <summary>
        /// 古データ 削除条件
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private bool TermOldTopicDelete(MsgTopic topic)
        {
            return LpsDatetimeUtil.dec(topic.CreateTime) <= DateTime.Now.AddHours(-24);
        }

    }
}
