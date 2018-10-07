//====================================================================
//  ClassName : LiplisModelLive2d
//  概要      : Liplisモデル Live2d
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Model.Event;
using Assets.Scripts.LiplisSystem.Model.Setting;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.Utils;
using LiplisMoonlight.LiplisModel;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Framework.MouthMovement;
using Live2D.Cubism.Framework.Raycasting;
using Live2D.Cubism.Rendering;
using System;
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

        //=============================
        //アタッチオブジェクト
        public CubismRenderController RendererController { get; set; }
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
        //表情オブジェクト


        ///=============================
        ///透明度
        [Range(0.0f, 1.0f)]
        public float modelOpacity = 1.0f;   //透明度の調整
        public float targetOpacity = 1.0f;  //対象透明度
        public float incrimentRate = 0.05f;

        //=============================
        //モデルデータ設定
        LiplisModelData modelData;
        public int Direction { get; set; }

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
                                LiplisModelData modelData,  //モデルデータ
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
     
            //表情テーブルにデフォルトをセット
            this.TableMotion = Expression.TableExpression;

            //モデルのロード
            LoadModel(modelPath, modelData.FileName, targetPosition);

            //表情のロード
            LoadExpression(Expression);
        }

        /// <summary>
        /// モデルをロードする
        /// TODO Exceptionの実装 
        /// </summary>
        /// <param name="targetSettinPath"></param>
        public void LoadModel(string modelPath, string targetSettingFileName, Vector3 targetPosition)
        {
            //Load model.
            var path = modelPath + ModelPathDefine.MODELS + "/" + targetSettingFileName;

            var model3Json = CubismModel3Json.LoadAtPath(path, AssetLoader.LoadAsset);

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
            this.RendererController = model.gameObject.GetComponent<CubismRenderController>();

            //当たり判定クラス取得
            this.RayCaster = model.gameObject.GetComponent<CubismRaycaster>();

            //当たり判定の設定
            SetHitArea();

            //レンダリング階層に移動
            this.ModelObject.transform.SetParent(CanvasRendering.transform);

            //サイズの設定
            //this.SetScale(new Vector3(4.55f, 4.55f, 5f));
            this.SetScale(new Vector3(200f, 200f, 200f));

            //位置の設定
            this.SetPosition(targetPosition);

            //モーションのロード
            LoadMotion(this.model, modelPath);

            //口パクを初期化
            StopTalking();
        }

        /// <summary>
        /// サイズの設定
        /// </summary>
        /// <param name="targetScale"></param>
        public void SetScale(Vector3 targetScale)
        {
            this.model.transform.localScale = targetScale;
        }

        /// <summary>
        /// モーションのロード
        /// TODO Exceptionの実装 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelPath"></param>
        private void LoadMotion(CubismModel model, string modelPath)
        {
            //モーションテーブルの初期化
            TableMotion = new Dictionary<int, List<string>>();

            //モーションリストを回して登録
            foreach (LiplisMotion motion in modelData.MotionList)
            {
                //モーションパス取得
                var path = modelPath + ModelPathDefine.MOTIONS + "/" + motion.FileName;

                //ModelJsonオブジェクト取得
                var model3Json = CubismMotion3Json.LoadFrom(AssetLoader.LoadAsset<string>(path));

                //モーションロード
                AnimationClip clip = model3Json.ToAnimationClip();

                //ワラップモード設定
                clip.wrapMode = WrapMode.Loop;

                //レガシーアニメーションに設定(この設定は必須)
                clip.legacy = true;

                //アニメーターに登録
                this.Animator.AddClip(clip, motion.FileName);

                //モーションテーブル キー追加
                if(!TableMotion.ContainsKey(motion.Emotion))
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
        /// </summary>
        private void SetHitArea()
        {
            //ドローエイブル取得
            var drawables = this.model.Drawables;

            //ドローエイブルリストを回し、当たり判定クラスをアタッチする。
            foreach (CubismDrawable drawable in drawables)
            {
                //当たり判定の範囲であれば、アタッチ
                if(drawable.name.StartsWith("D_REF"))
                {
                    //レイキャストエイブルをアッドコンポーネント
                    drawable.gameObject.AddComponent<CubismRaycastable>();
                }
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
            //対照勘定のリストを取得
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
                //設定モーションが無い場合はノーマルを返す
                return TableMotion[(int)MOTION.MOTION_NORMAL][0];
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

        public void SetExpression(MOTION ExpressionCode)
        {
            Animator.Blend(GetMotionNameTargetMotionRndam(ExpressionCode));
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
            if(this.LipSync == null)
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
        /// モーションをランダムに再生する
        /// </summary>
        /// <param name="MotionCode"></param>
        public void StartRandomMotion(MOTION MotionCode)
        {
            Animator.Stop();
            Animator.Play(GetMotionNameTargetMotionRndam(MotionCode));
        }

        /// <summary>
        /// 音声の再生
        /// </summary>
        /// <param name="pVoice">優先度。使用しないなら0で良い。</param>
        public void StartVoice(AudioClip pVoice)
        {
            if (Audio == null)
            {
                Debug.Log("Live2D : AudioSource Component is NULL !");
                return;
            }
            Audio.clip = pVoice;
            Audio.loop = false;
            Audio.spatialBlend = 0;
            Audio.Play();
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
            }

            //モデルオパシティの更新
            RendererController.Opacity = modelOpacity;
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
            if(hitCount < 1)
            {
                return;
            }

            //当たり判定結果
            int hitTestResult = 0; 

            //判定結果を走査
            foreach (CubismRaycastHit result in results)
            {
                if(result.Drawable == null)
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

            if(hitTestResult == 1)
            {
                Debug.Log("頭をタップしました！");

                //次の話題
                CallbackOnNextTalkOrSkip();
            }
            else if (hitTestResult == 2)
            {
                Debug.Log("胸をタップしました！");
            }
            else if (hitTestResult == 3)
            {
                Debug.Log("体をタップしました！");

                //次の話題
                CallbackOnNextTalkOrSkip();
            }
        }



        #endregion
    }
}
