//====================================================================
//  ClassName : LiplisModel
//  概要      : リプリスモデル
//              モデルのベースクラス
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Com;
using Assets.Scripts.LiplisSystem.Model.Event;
using Assets.Scripts.LiplisSystem.Model.Json;
using Assets.Scripts.LiplisSystem.Model.Setting;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.Utils;
using LiplisMoonlight;
using LiplisMoonlight.LiplisModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Model
{
    public class LiplisModel
    {
        ///=============================
        /// モデル設定
        private string ModelPath;
        private LiplisMoonlightModel modelSetting;
        private LiplisToneSetting toneSetting;      //口調設定
        private LiplisChatSetting chatSetting;      //固定セリフ設定

        //=============================
        //レンダリング階層
        public GameObject CanvasRendering;

        //=============================
        //定義プロパティ
        public string ModelName;                    //モデル名
        public List<IfsLiplisModel> ModelList;      //モデルリスト

        //=============================
        //制御プロパティ
        public int AllocationId;                    //割当ID
        public IfsLiplisModel ActiveModel;          //現在ロード中のモデル
        public MST_CARACTER_POSITION Position;      //現在のモデルの配置
        public float LocationY;                     //モデルのY座標(上面からの距離。ポジションが入れ替わった場合に、高さを保持するためのプロパティ)
        public Vector3 ModelLocation;               //モデルの位置

        //=============================
        //ウインドウキューリスト
        public Queue<TalkWindow> WindowTalkListQ;
        public TalkWindow NowTalkWindow;          //現在おしゃべりウインドウ

        //=============================
        //スプライト
        public Sprite SpriteWindow;
        public Sprite SpriteLogWindow;
        public Sprite SpriteCharIcon;

        //=============================
        //キャラクター口調設定
        public LiplisGreet Greet;                   //挨拶データ
        public LiplisTone Tone;                     //口調データ

        //=============================
        //ライブ2d 表情テーブル
        private MsgExpression Expression;

        //=============================
        //画像ウインスタンス
        public GameObject ImageInstanceWindow;
        public GameObject ImageInstanceLogWindowL;
        public GameObject ImageInstanceLogWindowR;

        //=============================
        //必須イベント
        public ModelEvents.OnNextTalkOrSkip CallbackOnNextTalkOrSkip { get; set; }  //スキップコールバック

        //====================================================================
        //
        //                             初期化処理
        //                         
        //====================================================================
        #region 初期化処理

        /// <summary>
        /// ファイルロード コンストラクター
        /// </summary>
        public LiplisModel(int AllocationId, 
            GameObject CanvasRendering,
            string ModelPath, 
            ModelEvents.
            OnNextTalkOrSkip CallbackOnNextTalkOrSkip)
        {
            //アロケーションID設定
            this.AllocationId = AllocationId;

            //レンダリング階層取得
            this.CanvasRendering = CanvasRendering;

            //コールバックメソッドの設定
            this.CallbackOnNextTalkOrSkip = CallbackOnNextTalkOrSkip;

            //モデル設定を読み込み
            LoadModelSetting(ModelPath);

            //イメージの読み込み
            LoadImage();

            //プレファブの初期化
            InitPrefab();

            //モデルの初期化
            InitModel();

            //トークウインドウの初期化
            InitTalkWindow();
        }

        /// <summary>
        /// プリセットモデルロード コンストラクター
        /// </summary>
        /// <param name="AllocationId"></param>
        /// <param name="CanvasRendering"></param>
        /// <param name="ModelPath"></param>
        /// <param name="CallbackOnNextTalkOrSkip"></param>
        /// <param name="modelSetting"></param>
        /// <param name="toneSetting"></param>
        /// <param name="chatSetting"></param>
        /// <param name="TextureWindow"></param>
        public LiplisModel(int AllocationId, 
            GameObject CanvasRendering,
            string ModelPath, 
            ModelEvents.OnNextTalkOrSkip 
            CallbackOnNextTalkOrSkip,  
            LiplisMoonlightModel modelSetting, 
            LiplisToneSetting toneSetting,
            LiplisChatSetting chatSetting, 
            Texture2D TextureWindow,
            Texture2D TextureLogWindow,
            Texture2D TextureCharIcon)
        {
            //アロケーションID設定
            this.AllocationId = AllocationId;

            //レンダリング階層取得
            this.CanvasRendering = CanvasRendering;

            //モデル設定を読み込み
            this.ModelPath = ModelPath;

            //コールバックメソッドの設定
            this.CallbackOnNextTalkOrSkip = CallbackOnNextTalkOrSkip;

            //モデル設定
            this.modelSetting = modelSetting;
            this.toneSetting = toneSetting;
            this.chatSetting = chatSetting;

            //イメージの読み込み
            this.SpriteWindow = CreateTalkWindowSprite(TextureWindow);
            this.SpriteLogWindow = CreateTalkWindowSprite(TextureLogWindow);
            this.SpriteCharIcon = CreateTalkWindowSprite(TextureCharIcon);

            //モデルの初期化
            InitModel();

            //プレファブの初期化
            InitPrefab();

            //トークウインドウの初期化
            InitTalkWindow();
        }

        /// <summary>
        /// モデルの初期化
        /// </summary>
        private void InitModel()
        {
            //モデル名の設定
            this.ModelName = modelSetting.ModelName;

            //表情の読み込み(モデル生成前に読み込んでおく)
            LoadExpression();

            //アロケーションIDから、位置を設定する。
            //モデルを表示する前に特定。モデルの引数として必要。
            SetPositionAndLocation();

            //モデルの読み込み
            LoadModel();

            //おしゃべり設定の読み込み
            LoadTalkSetting();

            //アクティブモデルをセットし、モデルを一つ表示する
            SetActiveModel();

            //アイドルモーション開始
            StartRandomMotion(MOTION.MOTION_IDLE);
        }

        /// <summary>
        /// プレファブの初期化
        /// </summary>
        private void InitPrefab()
        {
            //ウインドウプレハブインスタンス生成
            this.ImageInstanceWindow = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_TALK_S_1);
            this.ImageInstanceLogWindowL = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_L1);
            this.ImageInstanceLogWindowR = (GameObject)Resources.Load(PREFAB_NAMES.WINDOW_LOG_CHAR_R1);
        }

        /// <summary>
        /// トークウインドウの初期化
        /// </summary>
        private void InitTalkWindow()
        {
            WindowTalkListQ = new Queue<TalkWindow>();
        }

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        /// <param name="targetSettingFileName"></param>
        private void LoadModelSetting(string ModelPath)
        {
            try
            {
                //モデルパス取得
                this.ModelPath = ModelPath;
                string targetSettingFilePath = this.ModelPath + ModelPathDefine.LIPLIS_MODEL_JSON;
                string settingPath = ModelPath + ModelPathDefine.SETTINGS;
                string chatSettingPath = settingPath + ModelPathDefine.LIPLIS_CHAT_SETTING;
                string toneSettingPath = settingPath + ModelPathDefine.LIPLIS_TONE_SETTING;

                 // LiplisModel.jsonファイルを読み込む
                // 原則としてApplication.dataPath配下。
                // プリセットモデルの場合はリソース配下となる。
                modelSetting = JsonLoader<LiplisMoonlightModel>.Load(targetSettingFilePath);

                //口調設定
                toneSetting = JsonLoader<LiplisToneSetting>.Load(chatSettingPath);

                //固定セリフ設定
                chatSetting = JsonLoader<LiplisChatSetting>.Load(chatSettingPath);
            }
            catch (Exception ex)
            {
                //ファイル読み込みに失敗したらエクセプション発生
                throw new ApplicationException("LiplisModelLive2d:LoadModelSetting:モデルの読み込みに失敗しました！", ex);
            }
        }

        /// <summary>
        /// ウインドウイメージをロードする
        /// </summary>
        private void LoadImage()
        {
            //TODO LiplisModel:LoadImage要実装

            //this.SpriteWindow = CreateTalkWindowSprite(windowTexutre);
            //this.SpriteLogWindow = CreateTalkWindowSprite(windowTexutre);
            //this.SpriteCharWindow = CreateTalkWindowSprite(windowTexutre);

        }

        /// <summary>
        /// モデルの読み込み
        /// </summary>
        private void LoadModel()
        {
            //モデルリスト初夏
            ModelList = new List<IfsLiplisModel>();

            //モデルファイルのロード
            foreach (LiplisModelData modelData in modelSetting.ModelList)
            {
                LoadModel(modelData,modelSetting.ModelType);
            }
        }
        private void LoadModel(LiplisModelData modelData ,int ModelType)
        {
            //モデルタイプによってインスタンス化する実装クラスを可変
            if (ModelType == (int)LiplisModelType.VRM)
            {
                //TODO LiplisModelLive2d:LoadModel VRMモデル実装
            }
            else if (ModelType == (int)LiplisModelType.Live2d20)
            {
                //廃止予定
            }
            else
            {
                //デフォルトはLive2dとする。
                LoadModelLive2d30(modelData);
            }
        }
        private void LoadModelLive2d30(LiplisModelData modelData)
        {
            //Live2dモデルの読み込み
            IfsLiplisModel model = new LiplisModelLive2d(this.ModelPath, 
                modelData, 
                CanvasRendering, 
                ModelLocation, 
                Expression,
                CallbackOnNextTalkOrSkip
                );

            //モデルをリストに追加
            ModelList.Add(model);
        }

        /// <summary>
        /// 表情のロード
        /// </summary>
        private void LoadExpression()
        {
            //初期化
            Expression = new MsgExpression();

            //モデルに設定されている表情ファイルを読み込む
            foreach (LiplisMotion expression in modelSetting.ExpressionList)
            {
                Expression.Add(this.ModelPath,expression);
            }

            //TODO LiplisModel:LoadExpression:1件もExpressionを設定しなかった場合に、正しく動作するかテスト
            //リビルド
            Expression.FixAndRebuildTable();
        }

        /// <summary>
        /// おしゃべり設定の読み込み
        /// </summary>
        private void LoadTalkSetting()
        {
            //おしゃべり関連クラス初期化
            this.Tone = new LiplisTone(toneSetting);
            this.Greet = new LiplisGreet(this.Tone, this.AllocationId, chatSetting);
        }

        /// <summary>
        /// トークウインドウのスプライトを生成する
        /// </summary>
        /// <param name="windowTexutre"></param>
        /// <returns></returns>
        private Sprite CreateTalkWindowSprite(Texture2D windowTexutre)
        {
            //スプライト単位の中心
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Rect rect = new Rect(0, 0, windowTexutre.width, windowTexutre.height);
            Vector4 border = new Vector4(40, 40, 40, 40);

            //テクスチャをスプライトに変換
            return Sprite.Create(windowTexutre, rect, pivot, 100.0f, 0, SpriteMeshType.FullRect, border);
        }

        /// <summary>
        /// アクティブモデルをセットし、モデルを一つ表示する
        /// まずは正面をアクティブにする
        /// </summary>
        private void SetActiveModel()
        {
            //フロントモデルの検索
            foreach (IfsLiplisModel model in ModelList)
            {
                //正面のモデルが見つかったら採用
                if(model.Direction == (int)MODELE_DIRECTION.FRONT)
                {
                    this.ActiveModel = model;
                    break;
                }
            }

            //フロントモデルが見つからなければ1つ目をアクティブとする。
            if(this.ActiveModel == null)
            {
                this.ActiveModel = ModelList[0];
            }

            //ここでビジブル設定すると、初期にアクティブにしたモデル以外動かなくなる。
            //ビジブル設定
            InitVisible();
        }

        /// <summary>
        /// ポジション、ロケーション初期設定
        /// </summary>
        private void SetPositionAndLocation()
        {
            //位置を設定
            if (this.AllocationId == 1)
            {
                //右に配置
                Position = MST_CARACTER_POSITION.Right;
            }
            else if (this.AllocationId == 2)
            {
                //真ん中に配置
                Position = MST_CARACTER_POSITION.Center;
            }
            else if (this.AllocationId == 3)
            {
                //左に配置
                Position = MST_CARACTER_POSITION.Left;
            }
            else
            {
                //司会位置に設定
                Position = MST_CARACTER_POSITION.Moderator;
            }

            //デフォルトY座標設定
            //this.LocationY = -2.05F;
            this.LocationY = -90;

            //モデルロケーションの設定
            this.ModelLocation = MODEL_POS.GetPosLive2d20(this.Position, this.LocationY);
        }
        #endregion

        //====================================================================
        //
        //                              更新制御
        //                         
        //====================================================================
        #region 更新制御
        public void Update()
        {
            //各モデルのアップデート処理
            foreach (var model in ModelList)
            {
                model.Update();
            }
        }

        #endregion
        //====================================================================
        //
        //                             モデル操作
        //                         
        //====================================================================
        #region モデル操作
        /// <summary>
        /// モデル表示初期化
        /// </summary>
        private void InitVisible()
        {
            //全モデルを一旦非表示にする
            SetModelOffAll();

            //現在ロード中のモデルのビジブルON
            ActiveModel.SetFaidIn();
        }


        /// <summary>
        /// 現在ロード中のモデルのビジブルをONにする
        /// </summary>
        private void SetModelVisible()
        {
            //一旦すべてOFF
            SetModelVisibleAllOff();

            //現在ロード中のモデルのビジブルON
            ActiveModel.SetFaidIn();
        }

        /// <summary>
        /// すべてのモデルをOFFにする
        /// </summary>
        private void SetModelVisibleAllOff()
        {
            SetModelVisibleAll(false);
        }

        /// <summary>
        /// すべてのモデルのビジブルをセットする
        /// </summary>
        /// <param name="visible"></param>
        private void SetModelVisibleAll(bool visible)
        {
            foreach (var model in ModelList)
            {
                model.SetFadeOut();
            }
        }
        private void SetModelOffAll()
        {
            foreach (var model in ModelList)
            {
                model.SetFadeOut(0.8f);
            }
        }

        /// <summary>
        /// ニュートラルにする
        /// </summary>
        public void SetNeutralAll()
        {
            foreach (var model in ModelList)
            {
                model.StartRandomMotion(MOTION.MOTION_IDLE);
            }
        }

        /// <summary>
        /// ランダムに向きを変更する
        /// </summary>
        public void ChangeDirectionRandam()
        {
            //現在の座標を保存
            GetModelLocation();

            //選択モデル
            IfsLiplisModel selectModel = GetDirectionModel(this.Position);

            //選択名が空なら0番目を取得
            if (selectModel == null)
            {
                selectModel = ModelList[0];
            }

            //アクティブモデルを変更
            this.ActiveModel = selectModel;

            //位置の復元
            SetModelLocation();

            //モデルビジブル再設定
            SetModelVisible();
        }

        /// <summary>
        /// 方向モデルを取得する
        /// </summary>
        /// <returns></returns>
        public IfsLiplisModel GetDirectionModel(MST_CARACTER_POSITION Position)
        {
            //候補リスト
            List<IfsLiplisModel> CandidateModelList = new List<IfsLiplisModel>();

            //設定されたポジションから変更可能な方向を判断する。
            if (Position == MST_CARACTER_POSITION.Moderator)
            {
                //最右端 左向き禁止
                //変更候補検索
                foreach (var model in ModelList)
                {
                    //左向き以外を候補リストに追加
                    if(model.Direction != (int)MODELE_DIRECTION.LEFT)
                    {
                        CandidateModelList.Add(model);
                    }
                }
            }
            else if (Position == MST_CARACTER_POSITION.Left)
            {
                //最左端 右向き禁止
                //変更候補検索
                foreach (var model in ModelList)
                {
                    //左向き以外を候補リストに追加
                    if (model.Direction != (int)MODELE_DIRECTION.RIGNT)
                    {
                        CandidateModelList.Add(model);
                    }
                }
            }
            else
            {
                //それ以外はモデルリストすべて対象
                CandidateModelList.AddRange(ModelList);
            }

            //候補リストをシャッフルする
            CandidateModelList.Shuffle();

            //常に0番目を返す
            return CandidateModelList[0];
        }

        /// <summary>
        /// 向きを変更する
        /// </summary>
        public void ChengeDirection(MODELE_DIRECTION Direction)
        {
            //現在の座標を保存
            GetModelLocation();

            //候補リスト
            List<IfsLiplisModel> CandidateModelList = new List<IfsLiplisModel>();

            //対象の向きのモデルを検索
            foreach (var model in ModelList)
            {
                //左向き以外を候補リストに追加
                if (model.Direction != (int)Direction)
                {
                    CandidateModelList.Add(model);
                }
            }

            //空なら全対象
            if(CandidateModelList.Count == 0)
            {
                CandidateModelList.AddRange(ModelList);
            }

            //シャッフルする
            CandidateModelList.Shuffle();

            //アクティブモデルを変更
            this.ActiveModel = CandidateModelList[0];

            //モデルビジブル再設定
            SetModelVisible();
        }


        /// <summary>
        /// 現在モデルの位置を次のモデルに継承する
        /// </summary>
        /// <param name="selectModelName"></param>
        public void GetModelLocation()
        {
            this.ModelLocation = this.ActiveModel.ModelObject.transform.localPosition;
        }

        /// <summary>
        /// 現在モデルの位置を復元する
        /// </summary>
        public void SetModelLocation()
        {
           this.ActiveModel.ModelObject.transform.localPosition = this.ModelLocation;
        }


        /// <summary>
        /// 対象の位置に移動する
        /// </summary>
        public void MoveTarget()
        {
            ModelLocation = MODEL_POS.GetPosLive2d20(this.Position,this.LocationY);

            MoveTarget(ModelLocation);
        }
        public void MoveTarget(Vector3 pos)
        {
            //一旦フェードアウト
            SetModelVisibleAllOff();

            //移動する
            foreach (var model in ModelList)
            {
                model.SetPosition(pos);
            }

            //現在ロード中のモデルのビジブルON
            ActiveModel.SetFaidIn(0.01f);
        }

        /// <summary>
        /// ランダムモーション
        /// </summary>
        /// <param name="MotionCode"></param>
        public void StartRandomMotion(MOTION MotionCode)
        {
            foreach (var model in ModelList)
            {
                model.StartRandomMotion(MotionCode);
            }
        }

        /// <summary>
        /// 感情をセットする
        /// </summary>
        /// <param name="MotionCode"></param>
        /// <returns></returns>
        public IEnumerator SetExpression(MOTION MotionCode)
        {
            foreach (var model in ModelList)
            {
                yield return model.SetExpression(MotionCode);
            }
        }

        #endregion

        //====================================================================
        //
        //                           ウインドウ操作
        //                         
        //====================================================================
        #region ウインドウ操作

        /// <summary>
        /// ウインドウサイズ定義
        /// </summary>
        private const float HEIGHT_IMG_3 = 120;
        private const float HEIGHT_IMG_2 = 105;
        private const float HEIGHT_IMG_1 = 90;

        /// <summary>
        /// 横幅計算
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        const float MAX_WIDTH = 140;
        private Vector2 CulcWindowSize(string message)
        {
            float width = (float)message.Length * 10.0f;
            float height = HEIGHT_IMG_1;

            //横計算
            if (width >= MAX_WIDTH)
            {
                width = MAX_WIDTH;
            }

            //以下高さ計算
            double div = Math.Ceiling(message.Length / 17.0);

            if (div >= 3)
            {
                height = HEIGHT_IMG_3;
            }
            else if (div == 2)
            {
                height = HEIGHT_IMG_2;
            }
            else
            {
                height = HEIGHT_IMG_1;
            }

            //計算した結果を返す
            return new Vector2(width, height);
        }

        /// <summary>
        /// ウインドウを作成する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public TalkWindow CreateWindowTalk(string message)
        {
            //向き変更
            this.ChangeDirectionRandam();

            //インスタンティエイト
            GameObject window = UnityEngine.Object.Instantiate(ImageInstanceWindow) as GameObject;

            //ウインドウ
            return CreateWindowTalk(window, message);
        }
        public TalkWindow CreateWindowTalk(GameObject window, string message)
        {
            try
            {
                //ウインドウ名設定
                window.name = "TalkWindow" + WindowTalkListQ.Count;

                //親キャンバスに登録
                window.transform.SetParent(CanvasRendering.transform, false);

                //位置設定
                window.transform.localPosition = WINDOW_POS.GetPos(ModelLocation);

                //サイズ変更
                RectTransform windowRect = window.GetComponent<RectTransform>();

                //ウインドウサイズ設定
                windowRect.sizeDelta = CulcWindowSize(message);

                //ローカルスケール設定
                windowRect.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                //ウインドウインスタンス取得
                TalkWindow talkWindow = window.GetComponent<TalkWindow>();

                //モデル設定
                talkWindow.TargetModel = ActiveModel;

                //テキスト設定
                talkWindow.SetNextLine(message);

                //ペアレント設定
                talkWindow.SetParentWindow(window);

                //高さ設定
                talkWindow.SetHeightImg(windowRect.sizeDelta.y);

                //生成時刻設定
                talkWindow.SetCreateTime(DateTime.Now);

                //ウインドウ画像の設定
                talkWindow.SetWindowImage(SpriteWindow);

                //NULLでなければウインドウセット
                if (talkWindow != null)
                {
                    //ウインドウセット
                    SetWindow(talkWindow, AllocationId);
                }



                //結果を返す
                return talkWindow;
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// ウインドウをセットする
        /// </summary>
        /// <param name="talkWindow"></param>
        /// <param name="AllocationId"></param>
        public void SetWindow(TalkWindow talkWindow, int AllocationId)
        {
            //ターゲットモデルセット
            talkWindow.TargetModel = ActiveModel;

            //ウインドウを追加する
            AddWindow(talkWindow);

            //ウインドウ
            DestroyWindow();
        }

        /// <summary>
        /// ウインドウを追加する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="message"></param>
        private void AddWindow(TalkWindow talkWindow)
        {
            //1個以上ならスライドする
            if (WindowTalkListQ.Count >= 1)
            {
                //一つ前のウインドウの高さを取得する
                float prvHeight = this.NowTalkWindow.heightImg;

                float height = NowTalkWindow.ParentWindow.GetComponent<RectTransform>().sizeDelta.y;


                //ウインドウ移動量設定
                SlideWindow(talkWindow);
            }

            //キューに追加
            this.WindowTalkListQ.Enqueue(talkWindow);

            //現在おしゃべりウインドウ設置
            this.NowTalkWindow = talkWindow;
        }

        /// <summary>
        /// ウインドをスライドする
        /// </summary>
        private void SlideWindow(TalkWindow newTalkWindow)
        {
            //ウインドウが無ければ動かさない
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }

            //移動量算出
            float moveVal = GetWindowMoveValue(newTalkWindow.GetCurrentText());

            //回してスライド
            foreach (var talkWindow in WindowTalkListQ)
            {
                if (!talkWindow.flgEnd)
                {
                    //移動目標設定
                    talkWindow.SetMoveTarget(new Vector3(talkWindow.ParentWindow.transform.localPosition.x, talkWindow.ParentWindow.transform.localPosition.y + moveVal, talkWindow.ParentWindow.transform.localPosition.z));
                }
            }
        }

        /// <summary>
        /// これからしゃべる内容から、ウインドウ移動量を計算する
        /// 
        /// TODO LiplisModel:GetWindowMoveValue 要調整
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private float GetWindowMoveValue(string text)
        {
            //バイト数取得
            int lenB = Encoding.UTF8.GetByteCount(text);

            //行数計算
            float rowNum = (float)Math.Ceiling(lenB / 26d);

            //移動量を算出
            float moveVal = 22.5f + rowNum * 7.5f;

            //移動量を返す
            return moveVal;
        }


        /// <summary>
        /// 先頭ウインドウをサクジョする
        /// </summary>
        private void DestroyWindow()
        {
            Queue<TalkWindow> bufQ = new Queue<TalkWindow>();

            //カウントチェック
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }
            else
            {
                //まわして、範囲内のものはバッファキューに、範囲外のものは削除
                while (WindowTalkListQ.Count > 0)
                {
                    TalkWindow talkWindow = WindowTalkListQ.Dequeue();

                    if (WindowTalkListQ.Count > 2)
                    {
                        talkWindow.CloseWindow();
                    }
                    else
                    {
                        //1個取り出し、削除
                        bufQ.Enqueue(talkWindow);
                    }
                }

                //すげかえる
                WindowTalkListQ = bufQ;
            }
        }

        /// <summary>
        /// すべてのウインドウを除去する
        /// </summary>
        public void DestroyAllWindow()
        {
            //空なら何もしない
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }

            //ウインドウ数が1以上なら、デキューして除去
            while (WindowTalkListQ.Count > 0)
            {
                //1個取り出し、削除
                WindowTalkListQ.Dequeue().CloseWindow();
            }
        }

        public void WindowMaintenance(int windowLifeSpanTime)
        {
            //空の場合何もしない
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }

            //一定時間経過したウインドウは自動的に除去する
            if (WindowTalkListQ.Peek().CreateTime.AddSeconds(windowLifeSpanTime) < DateTime.Now)
            {
                WindowTalkListQ.Dequeue().CloseWindow();
            }


            //LiplisStatus.Instance.CharDataList.DestroyAllWindow();
        }
        #endregion

        //====================================================================
        //
        //                               音声関連
        //                         
        //====================================================================
        #region 音声関連
        public bool IsPlaying()
        {
            //再生中のモデルを検索
            foreach (var model in ModelList)
            {
                if (model.IsPlaying())
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
