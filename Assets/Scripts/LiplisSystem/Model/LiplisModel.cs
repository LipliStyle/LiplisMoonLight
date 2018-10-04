//====================================================================
//  ClassName : LiplisModel
//  概要      : リプリスモデル
//              モデルのベースクラス
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Model.Json;
using Assets.Scripts.LiplisSystem.Model.Setting;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.UI;
using Assets.Scripts.Utils;
using LiplisMoonlight;
using LiplisMoonlight.LiplisModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        public List<IfsLiplisModel> ListModel;      //モデルリスト

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
        public string WindowName;                   //ウインドウ名

        //=============================
        //キャラクター口調設定
        public LiplisGreet Greet;                   //挨拶データ
        public LiplisTone Tone;                     //口調データ

        //=============================
        //ライブ2d 表情テーブル
        private MsgExpression Expression;

        //=============================
        //ウインドウインスタンス
        public GameObject WindowInstances;

        //====================================================================
        //
        //                             初期化処理
        //                         
        //====================================================================
        #region 初期化処理

        /// <summary>
        /// コンストラクター
        /// </summary>
        public LiplisModel(int AllocationId, GameObject CanvasRendering,string ModelPath)
        {
            //アロケーションID設定
            this.AllocationId = AllocationId;

            //レンダリング階層取得
            this.CanvasRendering = CanvasRendering;

            //モデル設定を読み込み
            LoadModelSetting(ModelPath);

            //モデルの初期化
            InitModel();

            //トークウインドウの初期化
            InitTalkWindow();
        }
        public LiplisModel(int AllocationId, GameObject CanvasRendering,string ModelPath, LiplisMoonlightModel modelSetting, LiplisToneSetting toneSetting,LiplisChatSetting chatSetting)
        {
            //アロケーションID設定
            this.AllocationId = AllocationId;

            //レンダリング階層取得
            this.CanvasRendering = CanvasRendering;

            //モデル設定を読み込み
            this.ModelPath = ModelPath;

            //モデル設定
            this.modelSetting = modelSetting;
            this.toneSetting = toneSetting;
            this.chatSetting = chatSetting;

            //モデルの初期化
            InitModel();

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
        /// トークウインドウの初期化
        /// </summary>
        private void InitTalkWindow()
        {
            WindowTalkListQ = new Queue<TalkWindow>();

            this.WindowName = PREFAB_NAMES.WINDOW_TALK_S_1;

            //ウインドウプレハブインスタンス生成
            this.WindowInstances = (GameObject)Resources.Load(this.WindowName);
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
        /// モデルの読み込み
        /// </summary>
        private void LoadModel()
        {
            //モデルリスト初夏
            ListModel = new List<IfsLiplisModel>();

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
                //Live2dモデルの読み込み
                IfsLiplisModel model = new LiplisModelLive2d(this.ModelPath, modelData, ModelLocation, Expression);

                //レンダリング階層に移動
                model.ModelObject.transform.SetParent(CanvasRendering.transform);

                //モデルをリストに追加
                ListModel.Add(model);
            }
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
        /// アクティブモデルをセットし、モデルを一つ表示する
        /// まずは正面をアクティブにする
        /// </summary>
        private void SetActiveModel()
        {
            //フロントモデルの検索
            foreach (IfsLiplisModel model in ListModel)
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
                this.ActiveModel = ListModel[0];
            }

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
            this.LocationY = -2.05F;

            //モデルロケーションの設定
            this.ModelLocation = MODEL_POS.GetPosLive2d30(this.Position, this.LocationY);
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
            foreach (var model in ListModel)
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
            foreach (var model in ListModel)
            {
                model.SetFadeOut();
            }
        }
        private void SetModelOffAll()
        {
            foreach (var model in ListModel)
            {
                model.SetVisible(false);
            }
        }

        /// <summary>
        /// ニュートラルにする
        /// </summary>
        public void SetNeutralAll()
        {
            foreach (var model in ListModel)
            {
                model.SetExpression(MOTION.MOTION_NORMAL);
                model.StartRandomMotion(MOTION.MOTION_IDLE);
            }
        }

        /// <summary>
        /// 向きを変更する
        /// </summary>
        public void ChengeDirection(MODELE_DIRECTION Direction)
        {
            //TODO LiplisModel ChengeDirection 仕様検討
            ///☆☆☆☆☆☆☆☆☆仕様要検討☆☆☆☆☆☆☆☆☆

            ////選択モデル
            //string selectModelName = "";

            //if (Direction == MODELE_DIRECTION.RIGNT)
            //{
            //    selectModelName = this.RightModelName;
            //}
            //else if (Direction == MODELE_DIRECTION.LEFT)
            //{
            //    selectModelName = this.LeftModelName;
            //}
            //else
            //{
            //    selectModelName = this.FrontModelName;
            //}

            ////選択名がからなら、フロントをセットする
            //if (selectModelName == "")
            //{
            //    selectModelName = this.FrontModelName;
            //}

            ////前回モデルの座標を取得しておく
            //SetModelLocation(selectModelName);

            ////選択モデルをセットする
            //this.NowModelName = selectModelName;

            ////モデルビジブル再設定
            //SetModelVisible();
        }

        /// <summary>
        /// ランダムに向きを変更する
        /// </summary>
        public void ChengeDirectionRandam()
        {
            //TODO LiplisModel ChengeDirection 仕様検討
            ///☆☆☆☆☆☆☆☆☆仕様要検討☆☆☆☆☆☆☆☆☆

            ////選択モデル
            //string selectModelName = "";

            ////方向インデックスを取得する
            //int idx = GetDirectionIdx();

            ////モデル名変更
            //selectModelName = ListModel[idx];

            ////選択名がからなら、フロントをセットする
            //if (selectModelName == "")
            //{
            //    selectModelName = this.FrontModelName;
            //}

            ////前回モデルの座標を取得しておく
            //SetModelLocation(selectModelName);

            ////選択モデルをセットする
            //this.NowModelName = selectModelName;

            ////モデルビジブル再設定
            //SetModelVisible();
        }

        /// <summary>
        /// 現在モデルの位置を次のモデルに継承する
        /// </summary>
        /// <param name="selectModelName"></param>
        public void SetModelLocation(string selectModelName)
        {
            //TODO LiplisModel ChengeDirection 仕様検討
            ///☆☆☆☆☆☆☆☆☆仕様要検討☆☆☆☆☆☆☆☆☆

            ////前回モデルの座標を取得しておく
            //this.ModelLocation = LAppLive2DManager.Instance.GetModelLocation(this.NowModelName);

            //LAppLive2DManager.Instance.SetMove(selectModelName, this.NowModelLocation);

            this.ModelLocation = this.ActiveModel.ModelObject.transform.localPosition;
        }



        /// <summary>
        /// 方向インデックスを取得する
        /// </summary>
        /// <returns></returns>
        public int GetDirectionIdx()
        {
            System.Random r = new System.Random();

            if (Position == MST_CARACTER_POSITION.Moderator)
            {
                return r.Next(2);
            }
            else if (Position == MST_CARACTER_POSITION.Left)
            {
                return r.Next(1, 3);
            }
            else
            {
                return r.Next(3);
            }
        }


        /// <summary>
        /// 対象の位置に移動する
        /// </summary>
        public void MoveTarget()
        {
            ModelLocation = MODEL_POS.GetPosLive2d30(this.Position,this.LocationY);

            MoveTarget(ModelLocation);
        }
        public void MoveTarget(Vector3 pos)
        {
            //一旦フェードアウト
            SetModelVisibleAllOff();

            //移動する
            foreach (var model in ListModel)
            {
                model.SetMove(pos);
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
            this.ActiveModel.StartRandomMotion(MotionCode);
        }

        #endregion

        //====================================================================
        //
        //                           ウインドウ操作
        //                         
        //====================================================================
        #region ウインドウ操作

        /// <summary>
        /// 横幅計算
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        const float MAX_WIDTH = 140;
        private float CulcWindowWidth(string message)
        {
            float width = (float)message.Length * 10.0f;

            if (width >= MAX_WIDTH)
            {
                return MAX_WIDTH;
            }
            else
            {
                return width;
            }
        }

        /// <summary>
        /// ウインドウサイズ定義
        /// </summary>
        private const float HEIGHT_IMG_3 = 120;
        private const float HEIGHT_TXT_3 = 46;
        private const float POS_Y_TXT_3 = -8.0f;

        private const float HEIGHT_IMG_2 = 105;
        private const float HEIGHT_TXT_2 = 32;
        private const float POS_Y_TXT_2 = -7.0f;

        private const float HEIGHT_IMG_1 = 90;
        private const float HEIGHT_TXT_1 = 16;
        private const float POS_Y_TXT_1 = -5.5f;

        private const float HEIGHT_IMG_FIX = 60; //固定高さ

        /// <summary>
        /// ウインドウを作成する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public TalkWindow CreateWindowTalk(string message, Transform parentTransform)
        {
            //向き変更
            this.ChengeDirectionRandam();

            //インスタンティエイト
            GameObject window = UnityEngine.Object.Instantiate(WindowInstances) as GameObject;

            //ウインドウ生成
            TalkWindow lpsWindow = CreateWindowTalk(window, parentTransform, message);

            //NULLでなければウインドウセット
            if (lpsWindow != null)
            {
                //ウインドウセット
                SetWindow(lpsWindow, AllocationId);
            }
   
            //ウインドウ
            return lpsWindow;
        }
        public TalkWindow CreateWindowTalk(GameObject window, Transform canvasParent, string message)
        {
            try
            {
                //サイズ計算
                float width = CulcWindowWidth(message);

                double div = Math.Ceiling(message.Length / 17.0);

                float heightImg = HEIGHT_IMG_1;
                float heightText = HEIGHT_TXT_1;
                float posTextY = POS_Y_TXT_1;

                if (div >= 3)
                {
                    heightImg = HEIGHT_IMG_3;
                    heightText = HEIGHT_TXT_3;
                    posTextY = POS_Y_TXT_3;
                }
                else if (div == 2)
                {
                    heightImg = HEIGHT_IMG_2;
                    heightText = HEIGHT_TXT_2;
                    posTextY = POS_Y_TXT_2;
                }
                else
                {
                    heightImg = HEIGHT_IMG_1;
                    heightText = HEIGHT_TXT_1;
                    posTextY = POS_Y_TXT_1;
                }

                //ウインドウ名設定
                window.name = "TalkWindow" + WindowTalkListQ.Count;

                //位置設定
                window.transform.position = WINDOW_POS.GetPos(ModelLocation);

                //サイズ変更
                RectTransform windowRect = window.GetComponent<RectTransform>();
                windowRect.sizeDelta = new Vector2(width, heightImg);
                windowRect.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                //親キャンバスに登録
                window.transform.SetParent(canvasParent, false);

                //ウインドウインスタンス取得
                TalkWindow talkWindow = window.GetComponent<TalkWindow>();

                //モデル設定
                talkWindow.TargetModel = ActiveModel;

                //テキスト設定
                talkWindow.SetNextLine(message);

                //ペアレント設定
                talkWindow.SetParentWindow(window);

                //高さ設定
                talkWindow.SetHeightImg(heightImg);

                //生成時刻設定
                talkWindow.SetCreateTime(DateTime.Now);

                //結果を返す
                //return new LiplisWindow(window, heightImg, heightText, posTextY);
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

                //ウインドウ移動量設定
                SlideWindow(talkWindow.heightImg, prvHeight);

                foreach (var w in WindowTalkListQ)
                {
                    if (w._bubbleText.text.text == "")
                    {
                        Debug.Log("");
                    }
                }
            }

            //キューに追加
            this.WindowTalkListQ.Enqueue(talkWindow);

            //現在おしゃべりウインドウ設置
            this.NowTalkWindow = talkWindow;
        }

        /// <summary>
        /// ウインドをスライドする
        /// </summary>
        private const float WINDOW_MOVE_VAL = 100;
        private void SlideWindow(float heightImg, float prvHeight)
        {
            //ウインドウが無ければ動かさない
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }

            //移動量算出
            float moveVal = GetMoveValue(heightImg, prvHeight);

            //回してスライド
            foreach (var talkWindow in WindowTalkListQ)
            {
                if (!talkWindow.flgEnd)
                {
                    //移動目標設定
                    talkWindow.SetMoveTarget(new Vector3(talkWindow.ParentWindow.transform.position.x, talkWindow.ParentWindow.transform.position.y + moveVal, talkWindow.ParentWindow.transform.position.z));
                }
            }
        }

        /// <summary>
        /// ウインドウ移動量定義
        /// </summary>
        private const float MOVE_VAL_2 = 30f;
        private const float MOVE_VAL_3 = 37.5f;
        private const float MOVE_VAL_4 = 45f;
        private const float MOVE_VAL_5 = 52.5f;
        private const float MOVE_VAL_6 = 60f;

        /// <summary>
        /// ウインドウ移動量を計算する
        /// </summary>
        /// <param name="heightImg"></param>
        /// <param name="prvHeight"></param>
        /// <returns></returns>
        private float GetMoveValue(float heightImg, float prvHeight)
        {
            float moveVal = WINDOW_MOVE_VAL;

            //今回動かす先頭ウインドウのサイズをチェック
            if (heightImg == HEIGHT_IMG_1 && prvHeight == HEIGHT_IMG_1)
            {
                moveVal = MOVE_VAL_2;      //2
            }
            else if (heightImg == HEIGHT_IMG_1 && prvHeight == HEIGHT_IMG_2)
            {
                moveVal = MOVE_VAL_3;      //3
            }
            else if (heightImg == HEIGHT_IMG_1 && prvHeight == HEIGHT_IMG_3)
            {
                moveVal = MOVE_VAL_4;      //4
            }

            else if (heightImg == HEIGHT_IMG_2 && prvHeight == HEIGHT_IMG_1)
            {
                moveVal = MOVE_VAL_3;      //3
            }
            else if (heightImg == HEIGHT_IMG_2 && prvHeight == HEIGHT_IMG_2)
            {
                moveVal = MOVE_VAL_4;      //4
            }
            else if (heightImg == HEIGHT_IMG_2 && prvHeight == HEIGHT_IMG_3)
            {
                moveVal = MOVE_VAL_5;      //5
            }

            else if (heightImg == HEIGHT_IMG_3 && prvHeight == HEIGHT_IMG_1)
            {
                moveVal = MOVE_VAL_4;      //4
            }
            else if (heightImg == HEIGHT_IMG_3 && prvHeight == HEIGHT_IMG_2)
            {
                moveVal = MOVE_VAL_5;      //5
            }
            else if (heightImg == HEIGHT_IMG_3 && prvHeight == HEIGHT_IMG_3)
            {
                moveVal = MOVE_VAL_6;      //6
            }



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

        /// <summary>
        /// 自身のウインドウ名を返す
        /// </summary>
        /// <returns></returns>
        public string GetWindowName()
        {
            return this.WindowName;
        }



        #endregion

    }
}
