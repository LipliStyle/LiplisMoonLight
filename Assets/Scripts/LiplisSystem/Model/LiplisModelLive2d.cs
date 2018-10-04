//====================================================================
//  ClassName : LiplisModelLive2d
//  概要      : Liplisモデル Live2d
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Model.Setting;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.Utils;
using LiplisMoonlight.LiplisModel;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Framework.MouthMovement;
using Live2D.Cubism.Rendering;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Model
{
    public class LiplisModelLive2d : IfsLiplisModel
    {
        //=============================
        //モデルオブジェクト
        public GameObject ModelObject { get; set; }
        public CubismModel model { get; set; }


        //=============================
        //アタッチオブジェクト
        public CubismRenderController rendererController { get; set; }
        public CubismAutoEyeBlinkInput EyeBlink { get; set; }
        public CubismMouthController MouthController { get; set; }
        public CubismAutoMouthInput LipSync { get; set; }
        public Animation animator { get; set; }
        public AudioSource audio { get; set; }


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
        public LiplisModelLive2d(string modelPath, LiplisModelData modelData, Vector3 targetPosition, MsgExpression Expression)
        {
            //モデルデータ取得
            this.modelData = modelData;
            this.Direction = modelData.Direction;

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
            this.animator = model.GetComponent<Animation>();

            //オーディオソース取得
            this.audio = model.GetComponent<AudioSource>();

            //目パチ制御
            this.EyeBlink = model.GetComponent<CubismAutoEyeBlinkInput>();

            //口パク制御
            this.MouthController = model.GetComponent<CubismMouthController>();
            this.LipSync = model.GetComponent<CubismAutoMouthInput>();

            //レンダラーコントローラの取得
            this.rendererController = model.gameObject.GetComponent<CubismRenderController>();

            //サイズの設定
            this.SetScale(new Vector3(4.55f, 4.55f, 5f));

            //位置の設定
            this.SetPosition(targetPosition);

            //モーションのロード
            LoadMotion(this.model, modelPath);

            //アイドルモーションを開始しておく。
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
        /// 位置の設定
        /// </summary>
        /// <param name="targetPosition"></param>
        public void SetPosition(Vector3 targetPosition)
        {
            this.model.transform.localPosition = targetPosition;
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
                this.animator.AddClip(clip, motion.FileName);

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
                this.animator.AddClip(kv.Value, kv.Key);
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
            return this.audio.isPlaying;
        }

        public void SetExpression(MOTION ExpressionCode)
        {
            animator.Blend(GetMotionNameTargetMotionRndam(ExpressionCode));
        }

        /// <summary>
        /// おしゃべりを停止する
        /// </summary>
        public void StartTalking()
        {
            this.LipSync.LipSyncOn();
        }

        /// <summary>
        /// おしゃべりを停止する
        /// </summary>
        public void StopTalking()
        {
            this.LipSync.LipSyncOff();
        }


        /// <summary>
        /// モデルのビジブルをセットする。
        /// </summary>
        /// <param name="flg"></param>
        public void SetVisible(bool flg)
        {
            ModelObject.SetActive(flg);
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
        /// 指定の座標に移動する。
        /// </summary>
        /// <param name="ModelLocation"></param>
        public void SetMove(Vector3 ModelLocation)
        {
            this.model.transform.position = ModelLocation;
        }

        /// <summary>
        /// モーションをランダムに再生する
        /// </summary>
        /// <param name="MotionCode"></param>
        public void StartRandomMotion(MOTION MotionCode)
        {
            animator.Stop();
            animator.Play(GetMotionNameTargetMotionRndam(MotionCode));
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
            rendererController.Opacity = modelOpacity;
        }
        #endregion
    }
}
