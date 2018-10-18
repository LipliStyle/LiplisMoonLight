//====================================================================
//  ClassName : CtrlModelController
//  概要      : Liplisモデルコントローラー
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.LiplisSystem.Model;
using Assets.Scripts.Data;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Web.Clalis.v40;
using SpicyPixel.Threading;
using System.Collections.Generic;
using UnityEngine;
using LiplisMoonlight;
using Assets.Scripts.LiplisSystem.Model.Priset;
using Assets.Scripts.LiplisSystem.Model.Setting;
using Assets.Scripts.Utils;
using Assets.Scripts.LiplisSystem.Model.Json;
using System.Collections;

namespace Assets.Scripts.Controller
{
    public class CtrlModelController : ConcurrentBehaviour
    {
        //=============================
        //キャラクターデータ管理
        public Dictionary<string, LiplisModel> TableModel { get; set; }          //モデルテーブル 名前のデータベース
        public Dictionary<int, LiplisModel> TableModelId { get; set; }           //モデルテーブル IDのデータベース
        public List<LiplisModel> ModelList { get; set; }                         //モデルリスト 

        //=============================
        //レンダリング階層
        public GameObject CanvasRendering;

        //=============================
        //会話コントローラー
        public CtrlTalk ctrlTalk;

        //====================================================================
        //
        //                            初期化処理
        //                          
        //====================================================================
        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        void Start()
        {
            //キャラクターリストを生成する
            CreateCharDataList(LiplisSetting.Instance.Setting.GetLstModelOnTheStage());
        }


          

        #endregion

        //====================================================================
        //
        //                            更新処理
        //                          
        //====================================================================
        #region 更新処理
        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            //各モデルの更新処理を実行する
            foreach (var model in ModelList)
            {
                model.Update();
            }
        }
        #endregion


        //====================================================================
        //
        //                  キャラクターデータ取得関連処理
        //                         
        //====================================================================
        #region キャラクターデータ取得関連処理
        /// <summary>
        /// キャラクターモデルを取得する
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <returns></returns>
        public LiplisModel GetCharacterModel(int AllocationId)
        {
            return TableModelId[AllocationId];
        }

        /// <summary>
        /// ランダムにキャラクターを取得する
        /// </summary>
        /// <returns></returns>
        public LiplisModel GetCharacterRandam()
        {
            //シャッフルする
            ModelList.Shuffle();

            //先頭1つを返す
            return ModelList[0];
        }

        /// <summary>
        /// モデル数を返す
        /// </summary>
        /// <returns></returns>
        public int GetModelCount()
        {
            return this.TableModelId.Count;
        }

        /// <summary>
        /// モデルリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<LiplisModel> GetModelList()
        {
            return this.ModelList;
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
        private void CreateCharDataList(List<string> LstModelOnTheStage)
        {
            //キャラクターデータリスト
            TableModel   = new Dictionary<string, LiplisModel>();
            TableModelId = new Dictionary<int, LiplisModel>();
            ModelList    = new List<LiplisModel>();

            int AllocationId = 0;

            //モデル設定を回し、読み込む
            foreach (var modelPath in LstModelOnTheStage)
            {
                //モデル読み込み
                LoadModel(modelPath, AllocationId);

                AllocationId++;
            }

            //キャラクターの位置の初期化
            //InitCharPosition();
        }

        /// <summary>
        /// モデルをロードする
        /// </summary>
        /// <param name="LPS-DB2\浩介"></param>
        private void LoadModel(string ModelSettingPath, int AllocationId)
        {
            //デフォルトモデルの場合は、プレファブから召喚
            if(ModelSettingPath.StartsWith(LIPLIS_MODEL_RABBITS.RABBITS_KEY))
            {
                LoadModelRabbits(ModelSettingPath, AllocationId);
            }
            else
            {
                LoadModelFromPath(ModelSettingPath, AllocationId);
            }
        }

        /// <summary>
        /// プリセットモデルを読み込む
        /// </summary>
        /// <param name="ModelSettingPath"></param>
        private void LoadModelRabbits(string ModelSettingPath, int AllocationId)
        {
            if (ModelSettingPath == LIPLIS_MODEL_RABBITS.SHIROHA)
            {
                //白葉
                AddModel(CreateModelRabbits(ModelPathDefine.PRISET_MODEL_SETTING_SHIROHA, AllocationId));
            }
            else if (ModelSettingPath == LIPLIS_MODEL_RABBITS.KUROHA)
            {
                //黒葉
                AddModel(CreateModelRabbits(ModelPathDefine.PRISET_MODEL_SETTING_KUROHA, AllocationId));
            }
            else if (ModelSettingPath == LIPLIS_MODEL_RABBITS.MOMOHA)
            {
                //桃葉
                AddModel(CreateModelRabbits(ModelPathDefine.PRISET_MODEL_SETTING_MOMOHA, AllocationId));
            }
            else
            {
                //葉月
                AddModel(CreateModelRabbits(ModelPathDefine.PRISET_MODEL_SETTING_HAZUKI, AllocationId));
            }
        }

        /// <summary>
        /// ラビッツモデルを生成する
        /// </summary>
        /// <param name="modelPathAnderResource"></param>
        /// <returns></returns>
        private LiplisModel CreateModelRabbits(string modelPathAnderResource, int AllocationId)
        {
            //モデルパス生成
            string modelPath = UtilUnityPath.GetStreamingAssetsPath() + "/" + modelPathAnderResource;

            //モデル設定を取得する
            LiplisMoonlightModel lmm = PrisetModelSettingLoader.LoadClassFromJson<LiplisMoonlightModel>(modelPath + ModelPathDefine.LIPLIS_MODEL_JSON);
            LiplisToneSetting ltn = PrisetModelSettingLoader.LoadClassFromJson<LiplisToneSetting>(modelPath + ModelPathDefine.SETTINGS + ModelPathDefine.LIPLIS_TONE_SETTING);
            LiplisChatSetting lch = PrisetModelSettingLoader.LoadClassFromJson<LiplisChatSetting>(modelPath + ModelPathDefine.SETTINGS + ModelPathDefine.LIPLIS_CHAT_SETTING);

            //ウインドウイメージの読み込み
            Texture2D TextureWindow = PrisetModelSettingLoader.LoadTexture(modelPath + ModelPathDefine.IMAGES + ModelPathDefine.IMG_WINDOW) as Texture2D;
            Texture2D TextureLogWindow = PrisetModelSettingLoader.LoadTexture(modelPath + ModelPathDefine.IMAGES + ModelPathDefine.IMG_WINDOW_LOG) as Texture2D;
            Texture2D TextureCharIcon = PrisetModelSettingLoader.LoadTexture(modelPath + ModelPathDefine.IMAGES + ModelPathDefine.IMG_ICON_CHAR) as Texture2D;

            //モデルを追加する
            return new LiplisModel(AllocationId, CanvasRendering, modelPath, ctrlTalk.NextTalkOrSkip, lmm, ltn, lch, TextureWindow, TextureLogWindow, TextureCharIcon);

        }

        /// <summary>
        /// パスからユーザーモデルを読み込む
        /// </summary>
        /// <param name="ModelSettingPath"></param>
        private void LoadModelFromPath(string ModelSettingPath, int AllocationId)
        {
            //TODO 要実装 CtrlModelController LoadModelFromPath
            ///☆☆☆☆☆☆☆☆☆要実装☆☆☆☆☆☆☆☆☆
        }

        /// <summary>
        /// モデルを追加する
        /// </summary>
        /// <param name="model"></param>
        private void AddModel(LiplisModel model)
        {
            //TODO CtrlModelController AddModel 登録キーチェックすべきか？
            TableModel.Add(model.ModelName, model);
            TableModelId.Add(model.AllocationId, model);
            ModelList.Add(model);
        }

        /// <summary>
        /// モデルにアロケーションIDを付与する
        /// </summary>
        private void AttachAllocationIdToModel()
        {

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
            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
            {
                kv.Value.SetNeutralAll();
            }
        }

        /// <summary>
        /// アイドリング
        /// </summary>
        public void IdleAll()
        {
            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
            {
                kv.Value.StartRandomMotion(MOTION.MOTION_IDLE);
            }
        }


        /// <summary>
        /// エクスプレッションを設定する
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <param name="sentence"></param>
        public IEnumerator SetExpression(MsgSentence sentence)
        {
            int AllocationId = sentence.AllocationId;

            if (AllocationId < 0)
            {
                AllocationId = 0;
            }

            //モデルに感情をセットする
            yield return TableModelId[sentence.AllocationId].ActiveModel.SetExpression(MotionMap.GetMotion(sentence.Emotion,sentence.Point));
        }

        /// <summary>
        /// おしゃべり開始
        /// </summary>
        public void StartTalking(MsgSentence sentence)
        {
            ////リップシンク有効
            this.TableModelId[sentence.AllocationId].ActiveModel.StartTalking();

            //foreach (var model in TableModelId[sentence.AllocationId].ListModel)
            //{
            //    model.StartTalking();
            //}
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
            foreach (LiplisModel model in ModelList)
            {
                model.MoveTarget();
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
            List<LiplisModel> PrvCharList = new List<LiplisModel>(ModelList);

            //先頭アロケーションID取得
            int allocationId = topic.TalkSentenceList[0].AllocationId;

            //範囲内なら選択
            if (allocationId >= 0 && allocationId <= 3)
            {
                //司会設定
                TableModelId[allocationId].Position = MST_CARACTER_POSITION.Moderator;
            }
            else
            {
                //ポジションの初期化
                initPosition();

                foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
                {
                    kv.Value.MoveTarget();
                }

                return;
            }

            //司会キャラ
            LiplisModel moderator = TableModelId[allocationId];

            //その他位置キャラリスト
            List<LiplisModel> OtherCharList = SearchMember(allocationId);

            //もし司会がNULLなら、初期配置に戻す
            if (moderator == null)
            {
                //ポジションの初期化
                initPosition();

                foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
                {
                    kv.Value.MoveTarget();
                }

                return;
            }

            //位置リスト
            List<MST_CARACTER_POSITION> PosList = CreatePosList();

            //選択されたもの以外の位置をほかのキャラクターにセットする
            foreach (LiplisModel charData in OtherCharList)
            {
                //ポジション設定
                charData.Position = PosList.Dequeue();
            }

            //Y座標の補正 身長差の分、位置がずれてしまうため補正
            int idx = 0;
            foreach (LiplisModel model in ModelList)
            {
                //身長差を算出
                float diff = model.LocationY - CharLocationYList[model.Position];

                //移動後Y座標
                float afterMovementY = locationList[model.Position].y + diff;

                //位置リストのY座標を更新
                locationList[model.Position] = new Vector3(locationList[model.Position].x, afterMovementY, locationList[model.Position].z);

                //インデックスインクリメント
                idx++;
            }

            //移動
            foreach (LiplisModel model in ModelList)
            {
                model.ModelLocation = locationList[model.Position];
                model.MoveTarget(locationList[model.Position]);
            }
        }

        /// <summary>
        /// 司会者を探す
        /// </summary>
        /// <returns></returns>
        private LiplisModel SearchModerator()
        {
            //位置リスト
            List<MST_CARACTER_POSITION> PosList = CreatePosList();

            //司会位置のキャラクターを探し、それ以外のキャラをリスト化する
            foreach (LiplisModel charData in ModelList)
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
        private List<LiplisModel> SearchMember(int allocationID)
        {
            //その他位置キャラリスト
            List<LiplisModel> OtherCharList = new List<LiplisModel>();

            //司会位置のキャラクターを探し、それ以外のキャラをリスト化する
            foreach (LiplisModel model in ModelList)
            {
                if (model.AllocationId == allocationID)
                {

                }
                else
                {
                    OtherCharList.Add(model);
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
            Dictionary<MST_CARACTER_POSITION, Vector3> locationList = new Dictionary<MST_CARACTER_POSITION, Vector3>();

            //位置リスト
            List<MST_CARACTER_POSITION> PosList = CreatePosList();

            foreach (MST_CARACTER_POSITION pos in PosList)
            {
                locationList.Add(pos, new Vector3());
            }

            foreach (LiplisModel model in ModelList)
            {
                locationList[model.Position] = model.ModelLocation;
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
                locationList.Add(pos, 0);
            }

            foreach (LiplisModel charData in ModelList)
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
            ModelList[0].Position = MST_CARACTER_POSITION.Moderator;
            ModelList[1].Position = MST_CARACTER_POSITION.Right;
            ModelList[2].Position = MST_CARACTER_POSITION.Center;
            ModelList[3].Position = MST_CARACTER_POSITION.Left;
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
            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
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

            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
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
            //まだ初期化されていなければスルーする。
            if(TableModel == null)
            {
                return;
            }

            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
            {
                kv.Value.WindowMaintenance(windowLifeSpanTime);
            }
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
            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
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
            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
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
            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
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
            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
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
            foreach (KeyValuePair<string, LiplisModel> kv in TableModel)
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
            LiplisModel charData = GetCharacterRandam();

            //おしゃべりメッセージを取得
            MsgTalkMessage msg = ClalisShortNews.getShortNews("", "", "");

            //トピックを生成
            MsgTopic topic = new MsgTopic(charData.Tone, msg.sorce, msg.sorce, 0, 0, false, charData.AllocationId);

            //ショートニュースからトピックを生成する
            return topic;
        }

        /// <summary>
        /// ボイスを再生する
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <param name="acVoice"></param>
        public void StartVoice(int AllocationId, AudioClip acVoice)
        {
            TableModelId[AllocationId].ActiveModel.StartVoice(acVoice);
        }

        /// <summary>
        /// 現在のモデルが音声おしゃべり中か？
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <returns></returns>
        public bool IsPlaying(int AllocationId)
        {
            return TableModelId[AllocationId].ActiveModel.IsPlaying();
        }
        #endregion

        //====================================================================
        //
        //                          音声再生関連
        //                         
        //====================================================================
        #region 音声再生関連

        public bool IsPlaying()
        {
            //再生中のモデルを検索
            foreach (var model in ModelList)
            {
                if(model.IsPlaying())
                {
                    return true;
                }
            }

            //すべてのモデルが再生中でなければFalse
            return false;
        }

        #endregion
    }
}
