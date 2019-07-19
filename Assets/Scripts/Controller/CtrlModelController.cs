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
using System.IO;

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
            CreateCharDataList(CreateModelPathList());
        }

        /// <summary>
        /// モデルパスリストを生成する
        /// </summary>
        /// <returns></returns>
        private List<string> CreateModelPathList()
        {
            List<string> result = new List<string>();

            //ストリーミングアセットのパスを取得する
            string path = UtilUnityPath.GetStreamingAssetsPath();

            ////フォルダリスト取得
            //string[] dirs = Directory.GetDirectories(path);

            ////リスト生成
            //foreach(string dir in dirs)
            //{
            //    result.Add(dir);
            //}

            result.Add(path + "/Hazuki");
            result.Add(path + "/Kuroha");
            result.Add(path + "/Momoha");
            result.Add(path + "/Shiroha");

            return result;
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
        /// 次のキャラクターモデルを取得する
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <returns></returns>
        public LiplisModel GetCharacterModelNext(int AllocationId)
        {
            //アロケーションIDを送る
            AllocationId++;

            //アロケーションIDコントロール
            if (AllocationId > GetMaxAllocationId())
            {
                return TableModelId[0];
            }
            else
            {
                return TableModelId[AllocationId];
            }
            
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
        /// 最大アロケーションIDを取得する
        /// </summary>
        /// <returns></returns>
        public int GetMaxAllocationId()
        {
            return this.TableModelId.Count -1;
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
                AddModel(LoadModel(modelPath, AllocationId, LstModelOnTheStage.Count));

                AllocationId++;
            }

            //キャラクターの位置の初期化
            //InitCharPosition();
        }

        /// <summary>
        /// ラビッツモデルを生成する
        /// </summary>
        /// <param name="modelPathAnderResource"></param>
        /// <returns></returns>
        private LiplisModel LoadModel(string modelPath, int AllocationId, int modelNum)
        {
            //モデル設定を取得する
            LiplisMoonlightModel lmm = PrisetModelSettingLoader.LoadClassFromJson<LiplisMoonlightModel>(modelPath + ModelPathDefine.LIPLIS_MODEL_JSON);
            LiplisToneSetting ltn = PrisetModelSettingLoader.LoadClassFromJson<LiplisToneSetting>(modelPath + ModelPathDefine.SETTINGS + ModelPathDefine.LIPLIS_TONE_SETTING);
            LiplisChatSetting lch = PrisetModelSettingLoader.LoadClassFromJson<LiplisChatSetting>(modelPath + ModelPathDefine.SETTINGS + ModelPathDefine.LIPLIS_CHAT_SETTING);

            //ウインドウイメージの読み込み
            Texture2D TextureWindow = PrisetModelSettingLoader.LoadTexture(modelPath + ModelPathDefine.IMAGES + ModelPathDefine.IMG_WINDOW) as Texture2D;
            Texture2D TextureLogWindow = PrisetModelSettingLoader.LoadTexture(modelPath + ModelPathDefine.IMAGES + ModelPathDefine.IMG_WINDOW_LOG) as Texture2D;
            Texture2D TextureCharIcon = PrisetModelSettingLoader.LoadTexture(modelPath + ModelPathDefine.IMAGES + ModelPathDefine.IMG_ICON_CHAR) as Texture2D;

            //モデルを追加する
            return new LiplisModel(AllocationId, CanvasRendering, modelPath, ctrlTalk.NextTalkOrSkip, lmm, ltn, lch, TextureWindow, TextureLogWindow, TextureCharIcon, modelNum);

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
            Dictionary<int, Vector3> locationList = SearchLocationList();

            //キャラクターロケーションYリスト
            Dictionary<int, float> CharLocationYList = SearchCharLocationYList();

            //前回キャラクター位置
            List<LiplisModel> PrvCharList = new List<LiplisModel>(ModelList);

            //先頭アロケーションID取得
            int allocationId = topic.TalkSentenceList[0].AllocationId;

            //位置リスト
            Queue<int> PosList = new Queue<int>(CreatePosList());

            foreach (LiplisModel model in ModelList)
            {
                model.Position = (PosList.Dequeue());
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
            List<int> PosList = CreatePosList();

            //司会位置のキャラクターを探し、それ以外のキャラをリスト化する
            foreach (LiplisModel charData in ModelList)
            {
                if (charData.Position == LiplisModel.MODEL_POS_RIGHT)
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
        private Dictionary<int, Vector3> SearchLocationList()
        {
            //その他位置キャラリスト
            Dictionary<int, Vector3> locationList = new Dictionary<int, Vector3>();

            //位置リスト
            List<int> PosList = CreatePosList();

            foreach (int pos in PosList)
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
        private Dictionary<int, float> SearchCharLocationYList()
        {
            //その他位置キャラリスト
            Dictionary<int, float> locationList = new Dictionary<int, float>();

            //位置リスト
            List<int> PosList = CreatePosList();

            foreach (int pos in PosList)
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
            foreach (var model in ModelList)
            {
                model.Position = model.AllocationId;
            }
        }


        /// <summary>
        /// キャラクター位置リストを生成する
        /// </summary>
        private List<int> CreatePosList()
        {
            List<int> PosList = new List<int>();

            foreach (var model in ModelList)
            {
                PosList.Add(model.AllocationId);
            }

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
            TableModelId[AllocationId].StartVoice(acVoice);
        }

        /// <summary>
        /// 音声停止
        /// </summary>
        public void StopVoiceAll()
        {
            foreach (var model in ModelList)
            {
                model.StopVoice();
            }
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
