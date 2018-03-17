//====================================================================
//  ClassName : CharDataGreet
//  概要      : キャラクターあいさつデータ
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Common;
using Assets.Scripts.Data;
using Assets.Scripts.LiplisSystem.Cif.v60.Res;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.DataChar.CharacterTalk
{
    public class CharDataGreet
    {
        ///=============================
        ///あいさつディクショナリ
        List<MsgGreet> GreetList;
        List<MsgTopic> AnniversaryDaysList;

        ///=============================
        ///口調設定
        CharDataTone Tone;

        //割り当てID
        public int AllocationId { get; set; }

        //====================================================================
        //
        //                          シングルトン管理
        //                         
        //====================================================================
        #region シングルトン管理

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        //private static CharDataGreet instance;
        //public static CharDataGreet Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new CharDataGreet();
        //        }

        //        return instance;
        //    }
        //}

        /// <summary>
        /// コンストラクター
        /// </summary>
        public CharDataGreet(CharDataTone Tone,int allocationID)
        {
            this.Tone = Tone;
            this.AllocationId = allocationID;
            this.GreetList = new List<MsgGreet>();
        }
        #endregion

        //====================================================================
        //
        //                             データロード
        //                         
        //====================================================================
        #region データロード

        /// <summary>
        /// データロードする
        /// </summary>
        //public void DataLoad()
        //{
        //    GreetList.Add(new MsgGreet(CreateDatetime(6, 0, 0), CreateDatetime(10, 0, 0), "おはようございます、ご主人様。", EMOTION.JOY, 3));
        //    GreetList.Add(new MsgGreet(CreateDatetime(6, 0, 0), CreateDatetime(10, 0, 0), "おはようございます、ご主人様。", EMOTION.NORMAL, 0));
        //    GreetList.Add(new MsgGreet(CreateDatetime(11, 0, 0), CreateDatetime(16, 0, 0), "こんにちは、ご主人様。", EMOTION.JOY, 3));
        //    GreetList.Add(new MsgGreet(CreateDatetime(11, 0, 0), CreateDatetime(16, 0, 0), "こんにちは、ご主人様。", EMOTION.NORMAL, 0));
        //    GreetList.Add(new MsgGreet(CreateDatetime(17, 0, 0), CreateDatetime(22, 0, 0), "こんばんは、ご主人様。", EMOTION.JOY, 3));
        //    GreetList.Add(new MsgGreet(CreateDatetime(17, 0, 0), CreateDatetime(22, 0, 0), "こんばんは、ご主人様。", EMOTION.NORMAL, 3));
        //    GreetList.Add(new MsgGreet(CreateDatetime(22, 0, 0), CreateDatetime(3, 0, 0), "夜遅くまでお疲れ様です。", EMOTION.PEACE, 3));
        //    GreetList.Add(new MsgGreet(CreateDatetime(22, 0, 0), CreateDatetime(3, 0, 0), "夜遅くまでお疲れ様です。", EMOTION.NORMAL, 3));
        //    GreetList.Add(new MsgGreet(CreateDatetime(18, 0, 0), CreateDatetime(21, 0, 0), "今日も1日お疲れ様です。", EMOTION.PEACE, 3));
        //    GreetList.Add(new MsgGreet(CreateDatetime(18, 0, 0), CreateDatetime(21, 0, 0), "今日も1日お疲れ様です。", EMOTION.NORMAL, 3));
        //    GreetList.Add(new MsgGreet(CreateDatetime(6, 0, 0), CreateDatetime(10, 0, 0), "おはようございます、こんな朝早くからどうされたんですか！", EMOTION.AMAZEMENT, 3));
        //}


        //データ追加
        public void AddGreetData(MsgGreet data)
        {
            GreetList.Add(data);
        }


        #endregion

        //====================================================================
        //
        //                             パブリックメソッド
        //                         
        //====================================================================
        #region パブリックメソッド
        /// <summary>
        /// 時間、季節、条件から適切なあいさつを取得する
        /// </summary>
        public MsgTopic GetGreet(CharDataTone Tone)
        {
            //現在時刻取得
            DateTime dt = DateTime.Now;

            //トピックを生成する
            MsgTopic topic = CreateGreetTopic(dt, Tone);

            //今日は～の日です～
            //SetAnniversarySentence(topic.TalkSentenceList);

            //今日の天気は～です
            //SetWetherSentence(topic.TalkSentenceList);

            //生成したトピックを返す
            return topic;
        }

        /// <summary>
        /// 今日はなんの日の話題取得
        /// </summary>
        /// <returns></returns>
        public MsgTopic GetAnniversary()
        {
            //トピックを生成する
            MsgTopic topic = new MsgTopic();

            //今日は～の日です～
            SetAnniversarySentence(topic.TalkSentenceList);

            return topic;
        }

        /// <summary>
        /// 天気の話題セット
        /// </summary>
        /// <returns></returns>
        public MsgTopic GetWether()
        {
            //トピックを生成する
            MsgTopic topic = new MsgTopic();

            //今日は～の日です～
            SentenceCreatorWether.SetWetherSentence(topic.TalkSentenceList, Tone ,AllocationId);

            return topic;
        }

        /// <summary>
        /// あいさつデータからトピックを生成する
        /// </summary>
        /// <returns></returns>
        public MsgTopic CreateGreetTopic(DateTime dt, CharDataTone Tone)
        {
            //ピックアップリスト生成
            List<MsgTopic> PicupList = new List<MsgTopic>();

            //現在時刻作成
            DateTime nowTime = DateUtil.CreateDatetime(dt.Hour, dt.Minute, dt.Second);
            DateTime dayEndTime = DateUtil.CreateDatetime(23, 59, 59);
            DateTime daySrtTime = DateUtil.CreateDatetime(0, 0, 0);

            foreach (MsgGreet greet in GreetList)
            {
                //日またぎを考慮
                if (greet.SrtTime <= greet.EndTime)
                {
                    //日またがない
                    if (nowTime >= greet.SrtTime && nowTime <= greet.EndTime)
                    {
                        PicupList.Add(greet.GetTopic());
                    }
                }
                else
                {
                    //日またぎ
                    if ((nowTime >= greet.SrtTime && nowTime <= dayEndTime) || (nowTime >= daySrtTime && nowTime <= greet.EndTime))
                    {
                        PicupList.Add(greet.GetTopic());
                    }
                }
            }

            //0以下の場合は適当に返す
            if (PicupList.Count < 1)
            {
                if (GreetList.Count > 0)
                {
                    return GreetList[0].GetTopic();
                }
                else
                {
                    return new MsgTopic(Tone,"こんにちは", 0, 0);
                }
            }

            //シャッフルする
            PicupList.Shuffle();

            //選択されたあいさつを返す
            return PicupList[0];
        }

        /// <summary>
        /// 気温の話題セット
        /// </summary>
        /// <returns></returns>
        public MsgTopic GetTemperature()
        {
            //トピックを生成する
            MsgTopic topic = new MsgTopic();

            //今日の気温に対するコメント
            SetTemperatureSentenceToday(topic.TalkSentenceList, DateTime.Now);

            //
            return topic;
        }

        /// <summary>
        /// 降水確率の話題セット
        /// </summary>
        /// <returns></returns>
        public MsgTopic GetChanceOfRain()
        {
            //トピックを生成する
            MsgTopic topic = new MsgTopic();

            //今日は～の日です～
            SetChanceOfRainSentenceToday(topic.TalkSentenceList, DateTime.Now);

            //
            return topic;
        }



        #endregion
      


        //====================================================================
        //
        //                          今日の日情報関連
        //                         
        //====================================================================
        #region 今日の日情報関連

        /// <summary>
        /// ランダムにその日の情報を取得する
        /// </summary>
        /// <returns></returns>
        public void SetAnniversarySentence(List<MsgSentence> q)
        {
            //本日データのチェック
            CheckTodayDataAndCreate();

            //センテンスチェック
            if (AnniversaryDaysList.Count > 0)
            {
                //シャッフルする
                AnniversaryDaysList.Shuffle();

                //センテンスを入れる
                foreach (MsgSentence sentence in AnniversaryDaysList[0].TalkSentenceList)
                {
                    q.Add(sentence);
                }
            }
        }

        public void CheckTodayDataAndCreate()
        {
            if(CheckTodayDataExists())
            {
                return;
            }

            //クリア
            AnniversaryDaysList.Clear();

            //データ取得
            ResWhatDayIsToday DataList = LiplisStatus.Instance.InfoAnniversary.DataList;

            if(DataList == null)
            {
                return; 
            }

            foreach (var data in DataList.AnniversaryDaysList)
            {
                int sentenceIdx = 0;

                foreach (var sentence in data.TalkSentenceList)
                {
                    if (sentenceIdx == 0)
                    {
                        sentence.BaseSentence = "今日は" + sentence.BaseSentence + "みたいです～♪";
                        sentence.TalkSentence = sentence.BaseSentence;
                    }
                    else
                    {
                        sentence.ToneConvert(Tone);
                    }

                    sentenceIdx++;
                }


                //データ追加
                AnniversaryDaysList.Add(data);
            }
        }

        /// <summary>
        /// 保持データに本日データがあるかどうかチェックする
        /// </summary>
        /// <returns></returns>
        public bool CheckTodayDataExists()
        {
            //nullチェック
            if (AnniversaryDaysList == null)
            {
                AnniversaryDaysList = new List<MsgTopic>();
                return false;
            }

            //本日データ検索
            foreach (MsgTopic daysData in AnniversaryDaysList)
            {
                if (LpsDatetimeUtil.dec(daysData.CreateTime).Day == DateTime.Now.Day)
                {
                    return true;
                }
            }

            //クリアする
            AnniversaryDaysList.Clear();

            //本日データが無かった。
            return false;
        }


        #endregion


        //====================================================================
        //
        //                          現在温度関連
        //                          
        //====================================================================
        #region 現在温度関連
        /// <summary>
        /// 現在の温度状況を取得する
        /// </summary>
        /// <returns></returns>
        public void SetTemperatureSentenceToday(List<MsgSentence> q, DateTime dt)
        {
            int? maxTemp = LiplisStatus.Instance.InfoWether.GetMaxTemperature(dt);

            if (maxTemp == null)
            {

            }
            else
            {
                CreateTemperatureSentence(q, maxTemp.Value);
            }
        }

        /// <summary>
        /// 最高気温からコメントを生成する
        /// </summary>
        /// <param name="maxTemp"></param>
        /// <returns></returns>
        public void CreateTemperatureSentence(List<MsgSentence> q, int maxTemp)
        {
            if (maxTemp <= 0)
            {
                string msg = "寒すぎです・・・！";

                MsgSentence sentence = new MsgSentence(Tone,msg, msg, 1, -3, 1,AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 10)
            {
                string msg = "寒いですね～。";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 4, -3, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 15)
            {
                string msg = "少し寒いですね。";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 0, 0, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 20)
            {
                string msg = "涼しいですね。";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 0, 0, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 25)
            {
                string msg = "これくらいの気温が過ごしやすいですね。";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 1, 3, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 30)
            {
                string msg = "ちょっと暑いですね～。";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 3, -3, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 35)
            {
                string msg = "暑くて溶けてしまいそうです・・・";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 6, -3, 1, AllocationId);

                q.Add(sentence);
            }
            else
            {
                string msg = "暑い、暑い、暑いぃぃぃ！";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 6, 3, 1, AllocationId);

                q.Add(sentence);
            }



        }
        #endregion


        //====================================================================
        //
        //                         現在降水確率関連
        //                          
        //====================================================================
        #region 現在降水確率関連
        /// 現在の温度状況を取得する
        /// </summary>
        /// <returns></returns>
        public void SetChanceOfRainSentenceToday(List<MsgSentence> q, DateTime dt)
        {
            int? chanceOfRain = LiplisStatus.Instance.InfoWether.GetChanceOfRain(dt);

            if (chanceOfRain == null)
            {

            }
            else
            {
                CreatChanceOfRainSentence(q, chanceOfRain.Value);
            }
        }

        /// <summary>
        /// 最高気温からコメントを生成する
        /// </summary>
        /// <param name="maxTemp"></param>
        /// <returns></returns>
        public void CreatChanceOfRainSentence(List<MsgSentence> q, int maxTemp)
        {
            if (maxTemp <= 0)
            {
                string msg = "雨の心配は全く無いですね！";

                MsgSentence sentence = new MsgSentence(Tone,msg, msg, 1, 3, 1,AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 10)
            {
                string msg = "雨が降る可能性は低そうですね～。";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 3, 3, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 20)
            {
                string msg = "雨、降るかもしれませんね。";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 0, 0, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 30)
            {
                string msg = "うーん、傘は大丈夫ですかね・・・？念のため持って行きますか？";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 0, 0, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 50)
            {
                string msg = "雨が降るか振らないか、五分五分ですねぇ。";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 3, 3, 1, AllocationId);

                q.Add(sentence);
            }
            else if (maxTemp < 80)
            {
                string msg = "傘を持っていったほうがいいですね！";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 1, -3, 1, AllocationId);

                q.Add(sentence);
            }
            else
            {
                string msg = "傘を持っていったほうがいいですね！";

                MsgSentence sentence = new MsgSentence(Tone, msg, msg, 1, 3, 1, AllocationId);

                q.Add(sentence);
            }



        }


        #endregion

    }
}
