//====================================================================
//  ClassName : LiplisModelLive2d
//  概要      : Liplisモデル Live2d
//              
//              ドラッグ移動は「DragGameObject」で行っている。インスタンス生成時、アタッチしている。
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Model.Event;
using Assets.Scripts.LiplisSystem.Model.Setting;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisUi.uGuiUtil;
using Assets.Scripts.Utils;
using LiplisMoonlight.LiplisModel;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Framework.MouthMovement;
using Live2D.Cubism.Framework.Raycasting;
using Live2D.Cubism.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Model
{
    public class LiplisModelLive2d : IfsLiplisModel
    {
        //=============================
        //描画領域
        public GameObject CanvasRendering { get; set; }

        //=============================
        //モデルオブジェクト
        public GameObject ModelObject { get; set; }
        public CubismModel model { get; set; }
        public float ModelScale { get; set; }

        //=============================
        //アタッチオブジェクト
        public CubismRenderController RenderController { get; set; }
        public CubismAutoEyeBlinkInput EyeBlink { get; set; }
        public CubismMouthController MouthController { get; set; }
        public CubismAutoMouthInput LipSync { get; set; }
        public CubismRaycaster RayCaster { get; set; }
        public Animation Animator { get; set; }
        public AudioSource Audio { get; set; }

        //=============================
        //必須イベント
        public ModelEvents.OnNextTalkOrSkip CallbackOnNextTalkOrSkip { get; set; }  //スキップコールバック 体、顔タップ時に呼ぶ

        //=============================
        //モーションオブジェクト
        public Dictionary<int, List<string>> TableMotion;
        public Dictionary<int, List<string>> TableExpression;

        //=============================
        //方向オブジェクト
        MODELE_DIRECTION NowDirection;
        CubismParameter PramAngleX;
        CubismParameter ParamBodyAngleX;

        //=============================
        //方向定義
        private const float PARAM_ANGLE_LEFT = 30;
        private const float PARAM_ANGLE_RIGTH = -30;
        private const float PARAM_BODY_ANGLE_LEFT = 10;
        private const float PARAM_BODY_ANGLE_RIGTH = -10;
        private const float PARAM_NEWTRAL = 0;

        ///=============================
        ///透明度
        [Range(0.0f, 1.0f)]
        public float modelOpacity = 1.0f;   //透明度の調整
        public float targetOpacity = 1.0f;  //対象透明度
        public float incrimentRate = 0.05f;

        //=============================
        //モデルデータ設定
        private LiplisModelData modelData;
        public int Direction { get; set; }
        private bool flgResource { get; set; }

        //====================================================================
        //
        //                            初期化処理
        //====================================================================
        #region 初期化処理

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="targetPath"></param>
        public LiplisModelLive2d(string modelPath,          //モデルパス
                                bool flgResource,
                                LiplisModelData modelData,  //モデルデータ
                                float ModelScale,
                                GameObject CanvasRendering, //親キャンバス
                                Vector3 targetPosition,     //ターゲット座標
                                MsgExpression Expression,   //表情データ
                                ModelEvents.OnNextTalkOrSkip CallbackOnNextTalkOrSkip //次へクリックコールバック
                                )
        {
            //モデルデータ取得
            this.modelData = modelData;
            this.CanvasRendering = CanvasRendering;
            this.Direction = modelData.Direction;
            this.CallbackOnNextTalkOrSkip = CallbackOnNextTalkOrSkip;
            this.flgResource = flgResource;
            this.ModelScale = ModelScale;

            //モデルのロード
            LoadModel(modelPath, modelData.FileName, targetPosition);

            //表情のロード
            LoadExpression(Expression);
        }

        /// <summary>
        /// モデルロード
        /// </summary>
        /// <param name="modelPath"></param>
        /// <param name="model3Json"></param>
        /// <param name="targetPosition"></param>
        public void LoadModel(string modelPath, string targetSettingFileName, Vector3 targetPosition)
        {
            //Load model.
            string path = modelPath + ModelPathDefine.MODELS + "/" + targetSettingFileName;

            //モデル3json
            CubismModel3Json model3Json = CubismModel3Json.LoadAtPath(path, AssetLoader.LoadAsset);

            //モデル生成
            this.model = model3Json.ToModel();

            //アニメーターアタッチ
            this.model.gameObject.AddComponent<Animation>();

            //モデルゲームオブジェクトを取得する
            this.ModelObject = this.model.gameObject;

            //アニメーションを取得する
            this.Animator = model.gameObject.GetComponent<Animation>();

            //オーディオソース取得
            this.Audio = model.GetComponent<AudioSource>();

            //目パチ制御
            this.EyeBlink = model.gameObject.GetComponent<CubismAutoEyeBlinkInput>();

            //口パク制御
            this.MouthController = model.gameObject.GetComponent<CubismMouthController>();
            this.LipSync = model.gameObject.GetComponent<CubismAutoMouthInput>();

            //レンダラーコントローラの取得
            this.RenderController = model.gameObject.GetComponent<CubismRenderController>();

            //当たり判定クラス取得
            this.RayCaster = model.gameObject.GetComponent<CubismRaycaster>();

            //当たり判定の設定
            SetHitArea();

            //レンダリング階層に移動
            this.ModelObject.transform.SetParent(CanvasRendering.transform);

            //サイズの設定
            SetScale();

            //位置の設定
            this.SetPosition(targetPosition);

            //モーションのロード
            LoadMotion(this.model, modelPath);

            //方向パラメータの取得
            SetDirectionParam();

            //口パクを初期化
            StopTalking();

            //ドラッグオブジェクトのセット
            SetDragObject();
        }

        /// <summary>
        /// 方向パラメータを取得する
        /// </summary>
        private void SetDirectionParam()
        {
            PramAngleX = model.Parameters.FindById("ParamAngleX");
            ParamBodyAngleX = model.Parameters.FindById("ParamBodyAngleX");

            //NULLなら休パラメータを読んでみる
            if (PramAngleX == null)
            {
                PramAngleX = model.Parameters.FindById("PARAM_ANGLE_X");
            }

            if (ParamBodyAngleX == null)
            {
                ParamBodyAngleX = model.Parameters.FindById("PARAM_BODY_ANGLE_X");
            }
        }

        /// <summary>
        /// ドラッグオブジェクトをセットする
        /// </summary>
        private void SetDragObject()
        {
            //ドラッグドロップアタッチ
            this.model.gameObject.AddComponent<CapsuleCollider>();
            this.model.gameObject.AddComponent<DragGameObjectLive2d>();
        }

        /// <summary>
        /// サイズの設定
        /// </summary>
        /// <param name="targetScale"></param>
        public void SetScale(Vector3 targetScale)
        {
            this.model.transform.localScale = targetScale;
        }

        //アイドルモーション先頭
        public const string MOTION_IDLE_HEAD = "MOTION_IDLE";

        /// <summary>
        /// モーションのロード
        /// TODO Exceptionの実装 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelPath"></param>
        private void LoadMotion(CubismModel model, string modelPath)
        {
            //モーションテーブルの初期化
            this.TableMotion = new Dictionary<int, List<string>>();

            //モーションリストを回して登録
            foreach (LiplisMotion motion in modelData.MotionList)
            {
                //モーションパス取得
                var path = modelPath + ModelPathDefine.MOTIONS + "/" + motion.FileName;

                //ModelJsonオブジェクト取得
                CubismMotion3Json model3Json = CubismMotion3Json.LoadFrom(AssetLoader.LoadAsset<string>(path));

                //モーションロード
                AnimationClip clip = model3Json.ToAnimationClip();

                //レガシーに設定
                clip.legacy = true;

                //ワラップモード設定
                //アイドルモーションはループ、それ以外はワンスを設定
                if (motion.FileName.StartsWith(MOTION_IDLE_HEAD))
                {
                    //アイドルモーションに登録
                    clip.wrapMode = WrapMode.Loop;
                }
                else
                {
                    //通常モーションに登録
                    clip.wrapMode = WrapMode.Once;
                }

                //アニメーターに登録
                this.Animator.AddClip(clip, motion.FileName);

                //モーションテーブル キー追加
                if (!TableMotion.ContainsKey(motion.Emotion))
                {
                    TableMotion.Add(motion.Emotion, new List<string>());
                }

                //モーションテーブル 値追加
                TableMotion[motion.Emotion].Add(motion.FileName);
            }
        }

        /// <summary>
        /// エクスプレッションをロードする
        /// </summary>
        /// <param name="Expression"></param>
        private void LoadExpression(MsgExpression Expression)
        {
            //テーブルのロード
            this.TableExpression = Expression.TableExpression;

            //アニメメーター登録
            foreach (KeyValuePair<string, AnimationClip> kv in Expression.TabelAnimationClip)
            {
                //ワラップモード設定
                kv.Value.wrapMode = WrapMode.Loop;

                //レガシーアニメーションに設定(この設定は必須)
                kv.Value.legacy = true;

                //アニメーターに登録
                this.Animator.AddClip(kv.Value, kv.Key);
            }
        }

        /// <summary>
        /// 当たり判定の設定
        /// 
        /// TODO LiplisModelLived2:SetHitArea 当たり判定のパーツの名称を確定させる、デファインに定義
        /// </summary>
        private void SetHitArea()
        {
            //ドローエイブル取得
            var drawables = this.model.Drawables;

            //ドローエイブルリストを回し、当たり判定クラスをアタッチする。
            foreach (CubismDrawable drawable in drawables)
            {
                //当たり判定の範囲であれば、アタッチ
                if (drawable.name.StartsWith("D_REF"))
                {
                    //レイキャストエイブルをアッドコンポーネント
                    drawable.gameObject.AddComponent<CubismRaycastable>();
                }
            }
        }

        /// <summary>
        /// スケールを設定する
        /// </summary>
        private void SetScale()
        {
            this.SetScale(new Vector3(ModelScale, ModelScale, ModelScale));
        }

        /// <summary>
        /// パラメータの初期化
        /// </summary>
        private void InitParamator()
        {
            foreach (CubismParameter param in model.Parameters)
            {
                param.Value = param.DefaultValue;
            }
        }

        #endregion

        //====================================================================
        //
        //                            モーションテーブル操作
        //                         
        //====================================================================
        #region モーションテーブル操作
        /// <summary>
        /// モーションコードに指定されたモーションに紐づくモーション名を返す。
        /// 複数件登録されている場合はランダムで返す。
        /// </summary>
        /// <param name="MotionCode"></param>
        /// <returns></returns>
        private string GetMotionNameTargetMotionRndam(MOTION MotionCode)
        {
            try
            {
                //キーチェック
                if (!TableMotion.ContainsKey((int)MotionCode))
                {
                    Debug.Log("GetMotionNameTargetMotionRndam 存在なし:" + MotionCode);

                    //存在しなければNULLを返す
                    return null;
                }

                //対照感情のリストを取得
                List<string> motionList = TableMotion[(int)MotionCode];

                if (motionList.Count != 0)
                {
                    //取得したリストのカウント値から、ランダムでインデクス生成
                    int targetIdx = UnityEngine.Random.Range(0, motionList.Count);

                    //選択されたモーション名を返す
                    return motionList[targetIdx];
                }
                else
                {
                    //設定モーションが無い場合はNULLを返す
                    return null;
                }
            }
            catch
            {
                Debug.Log(MotionCode);
                return null;
            }

        }
        private string GetMotionNameTargetIdleMotionRndam()
        {
            try
            {
                //対照勘定のリストを取得
                List<string> motionList = TableMotion[(int)MOTION.MOTION_IDLE];

                if (motionList.Count != 0)
                {
                    //取得したリストのカウント値から、ランダムでインデクス生成
                    int targetIdx = UnityEngine.Random.Range(0, motionList.Count);

                    //選択されたモーション名を返す
                    return motionList[targetIdx];
                }
                else
                {
                    //設定モーションが無い場合はノーマルを返す
                    return TableMotion[(int)MOTION.MOTION_NORMAL][0];
                }
            }
            catch
            {
                Debug.Log("アイドルモーションでエラー");
                return TableMotion[(int)MOTION.MOTION_IDLE][0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MotionCode"></param>
        /// <returns></returns>
        private string GetExpressionNameTargetMotionRndam(MOTION MotionCode)
        {
            //対照勘定のリストを取得
            List<string> motionList = TableExpression[(int)MotionCode];

            if (motionList.Count != 0)
            {
                //取得したリストのカウント値から、ランダムでインデクス生成
                int targetIdx = UnityEngine.Random.Range(0, motionList.Count);

                //選択されたモーション名を返す
                return motionList[targetIdx];
            }
            else
            {
                //設定モーションが無い場合はノーマルを返す
                return TableExpression[(int)MOTION.MOTION_NORMAL][0];
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
        /// オーディオ再生中かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return this.Audio.isPlaying;
        }

        /// <summary>
        /// エクスプレッションの設定
        /// </summary>
        /// <param name="ExpressionCode"></param>
        public IEnumerator SetExpression(MOTION ExpressionCode)
        {
            //対照感情のモーション取得
            string motionName = GetMotionNameTargetMotionRndam(ExpressionCode);

            //取得モーションがNULLでなければ実行する。
            if (motionName != null)
            {
                //モーション実行
                Animator.Play(motionName);
                Animator.Blend(GetExpressionNameTargetMotionRndam(ExpressionCode));

                //モーション完了待ち
                yield return new WaitForSeconds(Animator.GetClip(motionName).length);

                //パラメーター初期化
                //InitParamator();

                //表情とアイドル設定
                Animator.Blend(GetMotionNameTargetIdleMotionRndam());

            }
            else
            {
                //様子見中
                //一旦停止
                //Animator.Stop();

                //パラメーター初期化
                //InitParamator();

                //表情とアイドル設定
                Animator.Play(GetExpressionNameTargetMotionRndam(ExpressionCode));
                Animator.Blend(GetMotionNameTargetIdleMotionRndam());
            }

        }

        /// <summary>
        /// 本インスタンスからセットエクスプレッションを呼ぶための目セッド
        /// </summary>
        /// <param name="ExpressionCode"></param>
        public void SetExpressionLocal(MOTION ExpressionCode)
        {
            GlobalCoroutine.Go(SetExpression(ExpressionCode));
        }


        /// <summary>
        /// おしゃべりを停止する
        /// </summary>
        public void StartTalking()
        {
            if (this.LipSync == null)
            {
                return;
            }

            this.LipSync.LipSyncOn();
        }

        /// <summary>
        /// おしゃべりを停止する
        /// </summary>
        public void StopTalking()
        {
            if (this.LipSync == null)
            {
                return;
            }

            this.LipSync.LipSyncOff();
        }


        /// <summary>
        /// モデルのビジブルをセットする。
        /// </summary>
        /// <param name="flg"></param>
        public void SetVisible(bool flg)
        {
            this.ModelObject.SetActive(flg);
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        public void SetFadeOut()
        {
            SetFadeOut(0.05f);
        }
        public void SetFadeOut(float interval)
        {
            incrimentRate = interval;
            targetOpacity = 0.0f;
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        public void SetFaidIn()
        {
            SetFaidIn(0.05f);
        }
        public void SetFaidIn(float interval)
        {
            SetVisible(true);
            incrimentRate = interval;
            targetOpacity = 1.0f;
        }

        /// <summary>
        /// ビジブルONかどうか
        /// </summary>
        public bool IsVisble()
        {
            return ModelObject.activeInHierarchy;
        }

        /// <summary>
        /// 位置の設定
        /// </summary>
        /// <param name="targetPosition"></param>
        public void SetPosition(Vector3 targetPosition)
        {
            this.model.transform.localPosition = targetPosition;
        }

        /// <summary>
        /// ソーティングオーダーを設定する
        /// </summary>
        /// <param name="SortingOrder"></param>
        public void SetOrder(int SortingOrder)
        {
            RenderController.SortingOrder = SortingOrder;
        }

        /// <summary>
        /// モーションをランダムに再生する
        /// </summary>
        /// <param name="MotionCode"></param>
        public void StartRandomMotion(MOTION MotionCode)
        {
            try
            {
                string motionName = GetMotionNameTargetMotionRndam(MotionCode);

                if (motionName == null)
                {
                    return;
                }

                //Animator.Stop();
                Animator.Play(motionName);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

        }

        /// <summary>
        /// 音声の再生
        /// </summary>
        /// <param name="pVoice">優先度。使用しないなら0で良い。</param>
        public void StartVoice(AudioClip pVoice)
        {
            if (Audio == null)
            {
                Debug.Log("オーディオがNULL！");
                return;
            }
            Audio.clip = pVoice;
            Audio.loop = false;
            Audio.spatialBlend = 0;
            Audio.Play();
        }

        /// <summary>
        /// 音声おしゃべりを停止する
        /// </summary>
        public void StopVoice()
        {
            if (Audio == null)
            {
                Debug.Log("オーディオがNULL！");
                return;
            }
            Audio.Stop();

            Audio.clip = null;
        }

        /// <summary>
        /// 方向変換
        /// </summary>
        /// <param name="Direction"></param>
        public void ChengeDirection(MODELE_DIRECTION Direction)
        {
            this.NowDirection = Direction;
        }

        #endregion

        //====================================================================
        //
        //                             更新制御
        //                         
        //====================================================================
        #region 更新制御
        public void Update()
        {
            //非表示なら何もしない
            if (!IsVisble()) return;

            //モデルがNULLなら何もしない
            if (model == null) return;

            //当たり判定チェック
            if (Input.GetMouseButtonDown(0))
            {
                HitTest();
            }

            //透明度の調整
            OnRenderObjectOpacity();

            //方向のセット
            SetDirection();
        }

        /// <summary>
        /// 透明度の調整
        /// </summary>
        void OnRenderObjectOpacity()
        {
            //ターゲット透明度に寄せる
            if (targetOpacity != modelOpacity)
            {
                if (modelOpacity > 1.0f)
                {
                    modelOpacity = 1.0f;
                }
                else if (modelOpacity < 0.0f)
                {
                    modelOpacity = 0;
                }
                else if (targetOpacity > modelOpacity)
                {
                    modelOpacity += incrimentRate;
                }
                else
                {
                    modelOpacity += (-1 * incrimentRate);
                }

                //モデルオパシティが0になったら、非表示にする。(負荷低減対策)
                if (modelOpacity == 0.0f)
                {
                    SetVisible(false);
                }

                //モデルオパシティの更新
                RenderController.Opacity = modelOpacity;
            }
        }


        /// <summary>
        /// 当たり判定
        /// </summary>
        void HitTest()
        {
            //当たり判定 4点まで許容
            var results = new CubismRaycastHit[4];

            //マウスポジションレイ取得
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //当たり判定チェック
            var hitCount = this.RayCaster.Raycast(ray, results);

            //あたってなければ抜ける
            if (hitCount < 1)
            {
                return;
            }

            //当たり判定結果
            int hitTestResult = 0;

            //判定結果を走査
            foreach (CubismRaycastHit result in results)
            {
                if (result.Drawable == null)
                {
                    continue;
                }

                if (result.Drawable.name == "D_REF_OPPAI")
                {
                    hitTestResult = 2;
                    break;
                }
                else if (result.Drawable.name == "D_REF_BODY")
                {
                    hitTestResult = 3;
                    //体は優先度低。続行
                    continue;
                }
                else if (result.Drawable.name == "D_REF_HEAD")
                {
                    hitTestResult = 1;
                    break;
                }
            }

            if (hitTestResult == 1)
            {
                //次の話題
                CallbackOnNextTalkOrSkip();
            }
            else if (hitTestResult == 2)
            {
                //恥じらいを発動
               　SetExpressionLocal(MOTION.MOTION_PROUD_M);
            }
            else if (hitTestResult == 3)
            {
                //次の話題
                CallbackOnNextTalkOrSkip();
            }
        }

        /// <summary>
        /// 方向をセットする。
        /// </summary>
        private void SetDirection()
        {
            //いずれかの方向がNULLなら
            if (PramAngleX == null || ParamBodyAngleX == null)
            {
                return;
            }

            if (this.NowDirection == MODELE_DIRECTION.LEFT)
            {
                PramAngleX.Value = PARAM_ANGLE_LEFT;
                ParamBodyAngleX.Value = PARAM_BODY_ANGLE_LEFT;
            }
            else if (this.NowDirection == MODELE_DIRECTION.RIGNT)
            {
                PramAngleX.Value = PARAM_ANGLE_RIGTH;
                ParamBodyAngleX.Value = PARAM_BODY_ANGLE_RIGTH;
            }
            else
            {
                PramAngleX.Value = PARAM_NEWTRAL;
                ParamBodyAngleX.Value = PARAM_NEWTRAL;
            }
        }


        #endregion
    }
}
