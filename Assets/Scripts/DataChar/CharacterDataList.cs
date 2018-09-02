//====================================================================
//  ClassName : CharacterDataList
//  概要      : キャラクターのベース情報リスト
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.DataChar.Rabiits;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Web.Clalis.v40;
using System.Collections.Generic;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Mst;
using UnityEngine;

namespace Assets.Scripts.DataChar
{
    public class CharacterDataList
    {
        //キャラクターデータ管理
        public Dictionary<string, CharacterData> CharDataList { get; set; }
        public Dictionary<int, CharacterData> CharIdList { get; set; }
        public List<CharacterData> CharList { get; set; }

        //====================================================================
        //
        //                            初期化処理
        //                          
        //====================================================================
        #region 初期化処理
        /// <summary>
        /// コンストラクター
        /// </summary>
        public CharacterDataList()
        {
            CreateCharDataList();
        }
        #endregion




        //====================================================================
        //
        //                  キャラクターデータ取得関連処理
        //                         
        //====================================================================
        #region キャラクターデータ取得関連処理

        public CharacterData GetCharacter(int AllocationId)
        {
            return CharIdList[AllocationId];
        }

        /// <summary>
        /// キャラクターモデルを取得する
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <returns></returns>
        public LAppModel GetCharacterModel(int AllocationId)
        {
            return LAppLive2DManager.Instance.GetModel(CharIdList[AllocationId].NowModelName);
        }

        /// <summary>
        /// ランダムにキャラクターを取得する
        /// </summary>
        /// <returns></returns>
        public CharacterData GetCharacterRandam()
        {
            //シャッフルする
            CharList.Shuffle();

            //先頭1つを返す
            return CharList[0];
        }



        #endregion

        //====================================================================
        //
        //                          トーク関連処理
        //                         
        //====================================================================
        #region トーク関連処理
        /// <summary>
        /// 挨拶を生成する
        /// </summary>
        public List<MsgTopic> GetGreet() 
        {
            List<MsgTopic> greetList = new List<MsgTopic>();

            //キャラクターデータを回す
            foreach(KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                //挨拶を取得し、リストに入れる
                greetList.Add(kv.Value.Greet.GetGreet(kv.Value.Tone));
            }

            //生成した挨拶リストを返す
            return greetList;
        }


        public List<MsgTopic> GetAnniversary()
        {
            List<MsgTopic> greetList = new List<MsgTopic>();

            //キャラクターデータを回す
            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                //挨拶を取得し、リストに入れる
                greetList.Add(kv.Value.Greet.GetAnniversary());
            }

            //生成した挨拶リストを返す
            return greetList;
        }

        public List<MsgTopic> GetWether()
        {
            List<MsgTopic> greetList = new List<MsgTopic>();

            //キャラクターデータを回す
            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                //挨拶を取得し、リストに入れる
                greetList.Add(kv.Value.Greet.GetWether());
            }

            //生成した挨拶リストを返す
            return greetList;
        }

        public List<MsgTopic> GetTemperature()
        {
            List<MsgTopic> greetList = new List<MsgTopic>();

            //キャラクターデータを回す
            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                //挨拶を取得し、リストに入れる
                greetList.Add(kv.Value.Greet.GetTemperature());
            }

            //生成した挨拶リストを返す
            return greetList;
        }

        public List<MsgTopic> GetChanceOfRain()
        {
            List<MsgTopic> greetList = new List<MsgTopic>();

            //キャラクターデータを回す
            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                //挨拶を取得し、リストに入れる
                greetList.Add(kv.Value.Greet.GetChanceOfRain());
            }

            //生成した挨拶リストを返す
            return greetList;
        }

        /// <summary>
        /// 適当なニュースを取得する
        /// </summary>
        /// <returns></returns>
        public MsgTopic CreateTopicFromShortNews()
        {
            //一人のデータを取得する
            CharacterData charData = GetCharacterRandam();

            //おしゃべりメッセージを取得
            MsgTalkMessage msg = ClalisShortNews.getShortNews("", "", "");

            //トピックを生成
            MsgTopic topic = new MsgTopic(charData.Tone,msg.sorce, msg.sorce, 0, 0, 0,charData.AllocationId);

            //ショートニュースからトピックを生成する
            return topic;
        }

        /// <summary>
        /// ウインドウネームを取得する
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public string GetWindowName(string modelName)
        {
            return CharDataList[modelName].WindowName;
        }

        /// <summary>
        /// ボイスを再生する
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <param name="acVoice"></param>
        public void StartVoice(int AllocationId, AudioClip acVoice)
        {
            LAppLive2DManager.Instance.GetModel(CharIdList[AllocationId].NowModelName).StartVoice(acVoice);
        }

        /// <summary>
        /// 現在のモデルが音声おしゃべり中か？
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <returns></returns>
        public bool IsPlaying(int AllocationId)
        {
            return LAppLive2DManager.Instance.GetModel(CharIdList[AllocationId].NowModelName).IsPlaying();
        }
        #endregion


        //====================================================================
        //
        //                          データ作成処理
        //                         
        //====================================================================
        #region データ作成処理
        /// <summary>
        /// キャラクターデータ作成
        /// </summary>
        private void CreateCharDataList()
        {
            //キャラクターデータリスト
            CharDataList = new Dictionary<string, CharacterData>();
            CharIdList = new Dictionary<int, CharacterData>();
            CharList = new List<CharacterData>();

            //リスト生成
            CreateCharData(new CharDataHazuki().CreateCharData(),CharDataList, CharIdList);
            CreateCharData(new CharDataShiroha().CreateCharData(), CharDataList, CharIdList);
            CreateCharData(new CharDataKuroha().CreateCharData(),CharDataList, CharIdList);
            CreateCharData(new CharDataMomoha().CreateCharData(),CharDataList, CharIdList);       
            
            //キャラクターの位置の初期化
            InitCharPosition();
        }

        /// <summary>
        /// キャラクターデータを生成する
        /// </summary>
        /// <param name="carData"></param>
        /// <param name="dic"></param>
        /// <param name="idDic"></param>
        private void CreateCharData(CharacterData carData ,Dictionary<string, CharacterData> dic, Dictionary<int, CharacterData> idDic)
        {
            dic.Add(carData.ModelName, carData);
            idDic.Add(carData.AllocationId, carData);
            CharList.Add(carData);
        }
        #endregion

        //====================================================================
        //
        //                        ウインドウ関連処理
        //                         
        //====================================================================
        #region ウインドウ関連処理

        /// <summary>
        /// すべてのウインドウを除去する
        /// </summary>
        public void DestroyAllWindow()
        {
            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                kv.Value.DestroyAllWindow();
            }
        }

        /// <summary>
        /// キューカウントを取得する
        /// </summary>
        /// <returns></returns>
        public int GetWindowQCount()
        {
            int qCount = 0;

            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                qCount = qCount + kv.Value.WindowTalkListQ.Count;
            }

            return qCount;
        }

        /// <summary>
        /// ウインドウメンテナンス
        /// </summary>
        public void WindowMaintenance(int windowLifeSpanTime)
        {
            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                kv.Value.WindowMaintenance(windowLifeSpanTime);
            }
        }



        #endregion

        //====================================================================
        //
        //                            モデル操作
        //                         
        //====================================================================
        #region モデル操作
        /// <summary>
        /// 全モデルをニュートラルに戻す
        /// </summary>
        public void NeutralAll()
        {
            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                foreach (var model in kv.Value.ModelList)
                {
                    LAppLive2DManager.Instance.SetExpression(model, EXPRESSION.NORMAL_01);
                    LAppLive2DManager.Instance.StartRandomMotion(model, MOTION.IDLE);
                }
            }
        }

        /// <summary>
        /// アイドリング
        /// </summary>
        public void IdleAll()
        {
            foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
            {
                LAppLive2DManager.Instance.StartRandomMotion(kv.Value.NowModelName, MOTION.IDLE);
            }
        }


        /// <summary>
        /// エクスプレッションを設定する
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <param name="sentence"></param>
        public void SetExpression(MsgSentence sentence)
        {
            int AllocationId = sentence.AllocationId;
            
            if(AllocationId < 0)
            {
                AllocationId = 1;
            }

            //一旦モーションをリセットする
            LAppLive2DManager.Instance.StartRandomMotion(CharIdList[AllocationId].NowModelName, MOTION.GetDefaultMotion(), LAppDefine.PRIORITY_FORCE);
            LAppLive2DManager.Instance.SetExpression(CharIdList[AllocationId].NowModelName, EXPRESSION.Instance.GetDefaultExpresssion());

            //表情設定
            LAppLive2DManager.Instance.SetExpressionNext(CharIdList[AllocationId].NowModelName, EXPRESSION.Instance.GetExpression(sentence.Emotion, sentence.Point));

            //モーション設定
            LAppLive2DManager.Instance.StartRandomMotion(CharIdList[AllocationId].NowModelName, MOTION.GetMotion(sentence.Emotion, sentence.Point), LAppDefine.PRIORITY_FORCE);

        }

        /// <summary>
        /// キャラクター位置を初期化する
        /// 
        /// 一度ムーブターゲットすると、初期位置に移動する。
        /// 
        /// </summary>
        public void InitCharPosition()
        {
            //移動する
            foreach (CharacterData c in CharList)
            {
                c.MoveTarget();
            }
        }

        /// <summary>
        /// キャラクター位置をシャッフルする
        /// </summary>
        public void ShuffleCharPosition(MsgTopic topic)
        {

            //位置リストを取得
            Dictionary<MST_CARACTER_POSITION, Vector3> locationList = SearchLocationList();

            //キャラクターロケーションYリスト
            Dictionary<MST_CARACTER_POSITION, float> CharLocationYList = SearchCharLocationYList();

            //前回キャラクター位置
            List<CharacterData> PrvCharList = new List<CharacterData>(CharList);

            //先頭アロケーションID取得
            int allocationId = topic.TalkSentenceList[0].AllocationId;

            //範囲内なら選択
            if (allocationId >= 0 && allocationId <= 3)
            {
                //司会設定
                CharIdList[allocationId].Position = MST_CARACTER_POSITION.Moderator;
            }
            else
            {
                //ポジションの初期化
                initPosition();

                //ここでタイトル追加は廃止　いずれこのコード自体削除
                //モデレーターに選ばれたキャラでタイトルセンテンスを追加
                //タイトルセンテンス追加
                //topic.TalkSentenceList.Insert(0, new MsgSentence(CharList[0].Tone, topic.Title, 0, 0,0 ,false, 0));

                foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
                {
                    kv.Value.MoveTarget();
                }

                return;
            }

            //司会キャラ
            CharacterData moderator = CharIdList[allocationId];

            //その他位置キャラリスト
            List<CharacterData> OtherCharList = SearchMember(allocationId);

            //もし司会がNULLなら、初期配置に戻す
            if (moderator == null)
            {
                //ポジションの初期化
                initPosition();

                //ここでタイトル追加は廃止　いずれこのコード自体削除
                //モデレーターに選ばれたキャラでタイトルセンテンスを追加
                //タイトルセンテンス追加
                //topic.TalkSentenceList.Insert(0, new MsgSentence(CharList[0].Tone, topic.Title, 0, 0,0 ,false, 0));

                foreach (KeyValuePair<string, CharacterData> kv in CharDataList)
                {
                    kv.Value.MoveTarget();
                }

                return;
            }
            
            //位置リスト
            List<MST_CARACTER_POSITION> PosList = CreatePosList();
            
            //選択されたもの以外の位置をほかのキャラクターにセットする
            foreach (CharacterData charData in OtherCharList)
            {
                //ポジション設定
                charData.Position = PosList.Dequeue();

                //ここでタイトル追加は廃止　いずれこのコード自体削除
                //モデレーターに選ばれたキャラでタイトルセンテンスを追加
                //if(charData.Position == MST_CARACTER_POSITION.Moderator)
                //{
                //    topic.TalkSentenceList.Insert(0, new MsgSentence(charData.Tone, topic.Title, 0, 0, 0, false, charData.AllocationId));
                //}
            }

            //Y座標の補正 身長差の分、位置がずれてしまうため補正
            int idx = 0;
            foreach (CharacterData charData in CharList)
            {
                //身長差を算出
                float diff = charData.LocationY - CharLocationYList[charData.Position];

                //移動後Y座標
                float afterMovementY = locationList[charData.Position].y + diff;

                //位置リストのY座標を更新
                locationList[charData.Position] = new Vector3(locationList[charData.Position].x, afterMovementY, locationList[charData.Position].z);

                //インデックスインクリメント
                idx++;
            }

            //移動
            foreach (CharacterData charData in CharList)
            {
                charData.NowModelLocation = locationList[charData.Position];
                charData.MoveTarget(locationList[charData.Position]);
            }
        }

        /// <summary>
        /// 司会者を探す
        /// </summary>
        /// <returns></returns>
        private CharacterData SearchModerator()
        {
            //位置リスト
            List<MST_CARACTER_POSITION> PosList = CreatePosList();

            //司会位置のキャラクターを探し、それ以外のキャラをリスト化する
            foreach (CharacterData charData in CharList)
            {
                if (charData.Position == MST_CARACTER_POSITION.Moderator)
                {
                    return charData;
                }
            }

            return null;
        }

        /// <summary>
        /// メンバーリスト生成する
        /// </summary>
        /// <returns></returns>
        private List<CharacterData> SearchMember(int allocationID)
        {
            //その他位置キャラリスト
            List<CharacterData> OtherCharList = new List<CharacterData>();

            //司会位置のキャラクターを探し、それ以外のキャラをリスト化する
            foreach (CharacterData charData in CharList)
            {
                if (charData.AllocationId == allocationID)
                {
  
                }
                else
                {
                    OtherCharList.Add(charData);
                }
            }


            return OtherCharList;
        }

        /// <summary>
        /// ロケーションリストを生成する
        /// </summary>
        /// <returns></returns>
        private Dictionary<MST_CARACTER_POSITION, Vector3> SearchLocationList()
        {
            //その他位置キャラリスト
            Dictionary<MST_CARACTER_POSITION,Vector3> locationList = new Dictionary<MST_CARACTER_POSITION, Vector3>();

            //位置リスト
            List<MST_CARACTER_POSITION> PosList = CreatePosList();

            foreach (MST_CARACTER_POSITION pos in PosList)
            {
                locationList.Add(pos,new Vector3());
            }

            foreach (CharacterData charData in CharList)
            {
                locationList[charData.Position] = charData.NowModelLocation;
            }

            return locationList;
        }

        /// <summary>
        /// 対象位置のキャラクターロケーションYリスト
        /// </summary>
        /// <returns></returns>
        private Dictionary<MST_CARACTER_POSITION, float> SearchCharLocationYList()
        {
            //その他位置キャラリスト
            Dictionary<MST_CARACTER_POSITION, float> locationList = new Dictionary<MST_CARACTER_POSITION, float>();

            //位置リスト
            List<MST_CARACTER_POSITION> PosList = CreatePosList();

            foreach (MST_CARACTER_POSITION pos in PosList)
            {
                locationList.Add(pos,0);
            }

            foreach (CharacterData charData in CharList)
            {
                locationList[charData.Position] = charData.LocationY;
            }

            return locationList;
        }


        /// <summary>
        /// 初期配置に戻す
        /// </summary>
        private void initPosition()
        {
            CharList[0].Position = MST_CARACTER_POSITION.Moderator;
            CharList[1].Position = MST_CARACTER_POSITION.Right;
            CharList[2].Position = MST_CARACTER_POSITION.Center;
            CharList[3].Position = MST_CARACTER_POSITION.Left;
        }


        /// <summary>
        /// キャラクター位置リストを生成する
        /// </summary>
        private List<MST_CARACTER_POSITION> CreatePosList()
        {
            List<MST_CARACTER_POSITION> PosList = new List<MST_CARACTER_POSITION>();

            PosList.Add(MST_CARACTER_POSITION.Center);
            PosList.Add(MST_CARACTER_POSITION.Right);
            PosList.Add(MST_CARACTER_POSITION.Left);

            PosList.Shuffle();

            return PosList;
        }




        #endregion


    }
}
