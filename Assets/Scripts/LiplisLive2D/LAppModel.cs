//=======================================================================
//  ClassName : LAppModel
//  概要      : ライブ2Dモデル
//
//LAppModel は低レベルのLive2Dモデル定義クラス Live2DModelUnity をラップし
//簡便に扱うためのユーティリティクラスです。
// 
// 機能一覧
//  アイドリングモーション
//  表情
//  音声
//  物理演算によるアニメーション
//  モーションが無いときに自動で目パチ
//  パーツ切り替えによるポーズの変更
//  当たり判定
//  呼吸のアニメーション
//  ドラッグによるアニメーション
//  デバイスの傾きによるアニメーション
// 
// 
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Msg;
using live2d;
using live2d.framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LAppModel :L2DBaseModel
{
    ///=============================
    ///モデル管理
    private LAppModelProxy parent;      //シーンに張り付けているLive2DCanvas
    private LAppView view;
    private Transform parentTransForm;  //一番上の位置設定している値　(これをいじれば移動させられる)

    ///=============================
    ///モデル関連
	private String modelHomeDir;
	private ModelSetting modelSetting = null;   //  モデルファイルやモーションの定義

    ///=============================
    ///ヒットエリア描画用マトリックス
    private Matrix4x4 matrixHitArea;

    ///=============================
    ///移動制御
    private bool FlgMoving;

    ///=============================
    ///モーション
    private bool FlgMotioning;
    private string NextEmotionName;

    ///=============================
    ///ドラッグ制御
    private bool FlgDragging = false;

    ///=============================
    /// 音声ソース
    private AudioSource asVoice;

	System.Random rand = new System.Random();

    ///=============================
    /// Live2Dモデルの 3D空間の範囲
    private Bounds bounds;
    private Vector3 DragStartPoint;
    private Vector3 DragStartPointModel;

    ///====================================================================
    ///
    ///                             初期化処理
    ///                         
    ///====================================================================
    #region 初期化処理

    /// <summary>
    /// コンストラクター
    /// </summary>
    /// <param name="parent"></param>
    public LAppModel(LAppModelProxy parent)
	{
		if (isInitialized()) return;

        //親ゲームオブジェクト取得(シーンに張り付けているLive2DCanvas)
		this.parent = parent;

        //トランスフォーム取得
        this.parentTransForm = parent.gameObject.GetComponent<Transform>();

        if (this.parent.GetComponent<AudioSource>() != null)
		{
            asVoice = this.parent.gameObject.GetComponent<AudioSource>();
            asVoice.playOnAwake = false;
		}
		else
		{
			if (LAppDefine.DEBUG_LOG)
			{
                Debug.Log("Live2D : AudioSource Component is NULL !");
			}
		}

        bounds = this.parent.GetComponent<MeshFilter>().sharedMesh.bounds;

        view = new LAppView(this, this.parent.transform);
		view.StartAccel();


		//if (LAppDefine.DEBUG_LOG) mainMotionManager.setMotionDebugMode(true);
	}

    /// <summary>
    /// ストリームからアセットをロードする
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="filename"></param>
	public void LoadFromStreamingAssets(String dir,String filename)
	{
		if (LAppDefine.DEBUG_LOG) Debug.Log(dir + filename);
		modelHomeDir = dir;
		var data = Live2DFramework.getPlatformManager().loadString(modelHomeDir + filename);
		Init(data);
	}
   

    /// <summary>
    /// モデルを初期化する
    /// </summary>
    /// <param name="modelJson"></param>
	public void Init(String modelJson)
	{
		updating = true;
		initialized = false;

		modelSetting = new ModelSettingJson(modelJson);

		if (LAppDefine.DEBUG_LOG) Debug.Log("Start to load model");

		// Live2D Model
		if (modelSetting.GetModelFile() != null)
		{
			loadModelData(modelHomeDir + modelSetting.GetModelFile());

			var len = modelSetting.GetTextureNum();
			for (int i = 0; i < len; i++)
			{
				loadTexture(i, modelHomeDir + modelSetting.GetTextureFile(i));
			}
		}
	   
		// Expression
		if (modelSetting.GetExpressionNum() != 0)
		{
			var len = modelSetting.GetExpressionNum();
			for (int i = 0; i < len; i++)
			{
				loadExpression(modelSetting.GetExpressionName(i), modelHomeDir + modelSetting.GetExpressionFile(i));
			}
		}

		// Physics
		if (modelSetting.GetPhysicsFile()!=null)
		{
			loadPhysics(modelHomeDir + modelSetting.GetPhysicsFile());            
		}

		// Pose
		if (modelSetting.GetPoseFile()!=null)
		{
			loadPose(modelHomeDir + modelSetting.GetPoseFile());    
		}

		// レイアウトはUnity上で設定できるのでJSONからは読み込まない
		//Dictionary<string, float> layout = new Dictionary<string, float>();
		//if (modelSetting.GetLayout(layout))
		//{
		//    if (layout.ContainsKey("width")) modelMatrix.setWidth(layout["width"]);
		//    if (layout.ContainsKey("height")) modelMatrix.setHeight(layout["height"]);
		//    if (layout.ContainsKey("x")) modelMatrix.setX(layout["x"]);
		//    if (layout.ContainsKey("y")) modelMatrix.setY(layout["y"]);
		//    if (layout.ContainsKey("center_x")) modelMatrix.centerX(layout["center_x"]);
		//    if (layout.ContainsKey("center_y")) modelMatrix.centerY(layout["center_y"]);
		//    if (layout.ContainsKey("top")) modelMatrix.top(layout["top"]);
		//    if (layout.ContainsKey("bottom")) modelMatrix.bottom(layout["bottom"]);
		//    if (layout.ContainsKey("left")) modelMatrix.left(layout["left"]);
		//    if (layout.ContainsKey("right")) modelMatrix.right(layout["right"]);
		//}


		// 初期パラメータ
		for (int i = 0; i < modelSetting.GetInitParamNum(); i++)
		{
			string id = modelSetting.GetInitParamID(i);
			float value = modelSetting.GetInitParamValue(i);
			live2DModel.setParamFloat(id, value);
		}

		for (int i = 0; i < modelSetting.GetInitPartsVisibleNum(); i++)
		{
			string id = modelSetting.GetInitPartsVisibleID(i);
			float value = modelSetting.GetInitPartsVisibleValue(i);
			live2DModel.setPartsOpacity(id, value);
		}

		// 自動目パチ
		eyeBlink = new L2DEyeBlink();

		view.SetupView(
			live2DModel.getCanvasWidth(),
			live2DModel.getCanvasHeight());

		updating = false;// 更新状態の完了
		initialized = true;// 初期化完了
	}

    #endregion

    //====================================================================
    //
    //                           モデル取得処理
    //                         
    //====================================================================
    #region モデル取得処理

    /// <summary>
    /// モデル取得
    /// </summary>
    /// <returns></returns>
    public Live2DModelUnity GetLive2DModelUnity()
    {
        return (Live2DModelUnity)live2DModel;
    }

    /// <summary>
    /// 3D空間範囲取得
    /// </summary>
    /// <returns></returns>
    public Bounds GetBounds()
    {
        return bounds;
    }

    #endregion

    ///====================================================================
    ///
    ///                             描画処理
    ///                         
    ///====================================================================
    #region 描画処理
    /// <summary>
    /// モデル更新メソッド
    /// 1フレームにつき1回呼ばれる
    /// </summary>
    public void Update()
	{
		if ( ! isInitialized() || isUpdating())
		{
			return;
		}

		view.Update(Input.acceleration);
		if (live2DModel == null)
		{
			if (LAppDefine.DEBUG_LOG) Debug.Log("Can not update there is no model data");
			return;
		}

		if (!Application.isPlaying)
		{
			live2DModel.update();
			return;
		}

		long timeMSec = UtSystem.getUserTimeMSec() - startTimeMSec;
		double timeSec = timeMSec / 1000.0;
		double t = timeSec * 2 * Math.PI;// 2πt

		// 待機モーション判定
		if (mainMotionManager.isFinished())
		{
            // モーションの再生がない場合、待機モーションの中からランダムで再生する
            //StartRandomMotion(MOTION.IDLE, LAppDefine.PRIORITY_IDLE);

            if(FlgMotioning)
            {
                FlgMotioning = false;

                //モーション終了イベント
                OnMotionEnd();
            }
		}

        //-----------------------------------------------------------------
        live2DModel.loadParam();// 前回セーブされた状態をロード

		bool update = mainMotionManager.updateParam(live2DModel);// モーションを更新

		if (!update)
		{
			// メインモーションの更新がないとき
			eyeBlink.updateParam(live2DModel);// 目パチ
		}
        else
        {
            FlgMotioning = true;
        }

		live2DModel.saveParam();// 状態を保存
		//-----------------------------------------------------------------

		if (expressionManager != null) expressionManager.updateParam(live2DModel);//  表情でパラメータ更新（相対変化）


		// ドラッグによる変化
		// ドラッグによる顔の向きの調整
		live2DModel.addToParamFloat(L2DStandardID.PARAM_ANGLE_X, dragX * 30, 1);// -30から30の値を加える
		live2DModel.addToParamFloat(L2DStandardID.PARAM_ANGLE_Y, dragY * 30, 1);
		live2DModel.addToParamFloat(L2DStandardID.PARAM_ANGLE_Z, (dragX * dragY) * -30, 1);

		// ドラッグによる体の向きの調整
		live2DModel.addToParamFloat(L2DStandardID.PARAM_BODY_ANGLE_X, dragX, 10);// -10から10の値を加える

		// ドラッグによる目の向きの調整
		live2DModel.addToParamFloat(L2DStandardID.PARAM_EYE_BALL_X, dragX, 1);// -1から1の値を加える
		live2DModel.addToParamFloat(L2DStandardID.PARAM_EYE_BALL_Y, dragY, 1);

		// 呼吸など
		live2DModel.addToParamFloat(L2DStandardID.PARAM_ANGLE_X, (float)(15 * Math.Sin(t / 6.5345)), 0.5f);
		live2DModel.addToParamFloat(L2DStandardID.PARAM_ANGLE_Y, (float)(8 * Math.Sin(t / 3.5345)), 0.5f);
		live2DModel.addToParamFloat(L2DStandardID.PARAM_ANGLE_Z, (float)(10 * Math.Sin(t / 5.5345)), 0.5f);
		live2DModel.addToParamFloat(L2DStandardID.PARAM_BODY_ANGLE_X, (float)(4 * Math.Sin(t / 15.5345)), 0.5f);
		live2DModel.setParamFloat(L2DStandardID.PARAM_BREATH, (float)(0.5f + 0.5f * Math.Sin(t / 3.2345)), 1);


		// 加速度による変化
		live2DModel.addToParamFloat(L2DStandardID.PARAM_ANGLE_X, 90 * accelX, 0.5f);
		live2DModel.addToParamFloat(L2DStandardID.PARAM_ANGLE_Z, 10 * accelX, 0.5f);


		if (physics != null) physics.updateParam(live2DModel);// 物理演算でパラメータ更新

		// リップシンクの設定
		if (lipSync)
		{
			live2DModel.setParamFloat(L2DStandardID.PARAM_MOUTH_OPEN_Y, lipSyncValue, 0.8f);
		}

        //移動処理
        Moving();

        // ポーズの設定
        if (pose != null) pose.updateParam(live2DModel);

		live2DModel.update();
	}

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Moving()
    {
        if(FlgMoving)
        {
            //移動処理
            Move(this.parentTransForm.localPosition.x - 0.001f, this.parentTransForm.localPosition.y, this.parentTransForm.localPosition.z);
        }
    }

    /// <summary>
    /// 描画処理
    /// </summary>
	public void Draw()
	{
		Matrix4x4 planeLocalToWorld = parent.transform.localToWorldMatrix;

		//  modelMatrixによってPlaneの縮尺に合わせたモデルを、Planeの向きに合わせる変換
		Matrix4x4 rotateModelOnToPlane = Matrix4x4.identity;
		rotateModelOnToPlane.SetTRS(Vector3.zero, Quaternion.Euler(90, 0, 0), Vector3.one);

		Matrix4x4 scale2x2ToPlane = Matrix4x4.identity;
		// planeは xz平面を張っている。その向きに２ｘ２平面を回転した上でスケール
		Vector3 scale = new Vector3(bounds.size.x / 2.0f, -1, bounds.size.z / 2.0f);
		scale2x2ToPlane.SetTRS(Vector3.zero, Quaternion.identity, scale);

		//  -1..1 のサイズで描画されるマトリックス
		Matrix4x4 modelMatrix4x4 = Matrix4x4.identity;
		float[] matrix = modelMatrix.getArray();
		for (int i = 0; i < 16; i++)
		{
			modelMatrix4x4[i] = matrix[i];
		}

		Matrix4x4 modelCanvasToWorld = planeLocalToWorld * scale2x2ToPlane * rotateModelOnToPlane * modelMatrix4x4;

		GetLive2DModelUnity().setMatrix(modelCanvasToWorld);

		live2DModel.draw();

		matrixHitArea = modelCanvasToWorld;
	}


    /// <summary>
    /// デバッグ用当たり判定の表示
    /// </summary>
    public void DrawHitArea()
	{
		
		int len = modelSetting.GetHitAreasNum();
		for (int i = 0; i < len; i++)
		{
			string drawID = modelSetting.GetHitAreaID(i);
			float left = 0;
			float right = 0;
			float top = 0;
			float bottom = 0;

			if (!getSimpleRect(drawID, out left, out right, out top, out bottom))
			{
				continue;
			}

			HitAreaUtil.DrawRect(matrixHitArea,left, right, top, bottom);
		}
	}
    #endregion

    //====================================================================
    //
    //                             モーション関連処理
    //                         
    //====================================================================
    #region モーション関連処理

    /// <summary>
    /// ランダムモーションを開始する
    /// </summary>
    /// <param name="name"></param>
    /// <param name="priority"></param>
    public void StartRandomMotion(string name, int priority)
	{
		int max = modelSetting.GetMotionNum(name);
		int no = (int)(rand.NextDouble() * max);
		StartMotion(name, no, priority);
	}


    /// <summary>
    /// モーションの開始
    /// 
    /// 再生できる状態かチェックして、できなければ何もしない。
    /// 再生出来る場合は自動でファイルを読み込んで再生。
    /// 音声付きならそれも再生。
    /// フェードイン、フェードアウトの情報があればここで設定。なければ初期値。
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <param name="no"></param>
    /// <param name="priority"></param>
    public void StartMotion(string group, int no, int priority)
	{
		string motionName = modelSetting.GetMotionFile(group, no);

		if (motionName == null || motionName.Equals(""))
		{
			if (LAppDefine.DEBUG_LOG) Debug.Log("Motion name is invalid");
			return;//
		}

		// 新しいモーションのpriorityと、再生中のモーション、予約済みモーションのpriorityと比較して
		// 予約可能であれば（優先度が高ければ）再生を予約します。
		//
		// 予約した新モーションは、このフレームで即時再生されるか、もしくは音声のロード等が必要な場合は
		// 以降のフレームで再生開始されます。
		if (priority == LAppDefine.PRIORITY_FORCE)
		{
			mainMotionManager.setReservePriority(priority);
		}
		else if (!mainMotionManager.reserveMotion(priority))
		{
			if (LAppDefine.DEBUG_LOG) { Debug.Log("Do not play because book already playing, or playing a motion already." + motionName); }
			return;
		}

		AMotion motion=null;
		string name = group + "_" + no;

		if (motions.ContainsKey(name))
		{
			motion = motions[name];            
		}
		if (motion==null)
		{
			motion = loadMotion(name, modelHomeDir+motionName);
		}
		if (motion == null)
		{
			Debug.Log("Failed to read the motion."+motionName);
			mainMotionManager.setReservePriority(0);
			return;
		}

        //Live2D jsonから読み出す場合は以下の記述とすべき。
        // フェードイン、フェードアウトの設定
        //motion.setFadeIn(modelSetting.GetMotionFadeIn(group, no));
        //motion.setFadeOut(modelSetting.GetMotionFadeOut(group, no));

        //負荷低減のため、モーションタイム調整
        motion.setFadeIn(500);
        motion.setFadeOut(500);


        if ((modelSetting.GetMotionSound(group, no)) == null)
		{
			// 音声が無いモーションは即時再生を開始します。
			if (LAppDefine.DEBUG_LOG) Debug.Log("Start motion : " + motionName);
			mainMotionManager.startMotionPrio(motion, priority);
		}
		else
		{
			// 音声があるモーションは音声のロードを待ってから再生を開始します。
			string soundPath = modelSetting.GetMotionSound(group, no);
			soundPath = Regex.Replace(soundPath, ".mp3$", "");// 不要な拡張子を削除

			AudioClip acVoice = FileManager.LoadAssetsSound(modelHomeDir+soundPath);
			if (LAppDefine.DEBUG_LOG) Debug.Log("Start motion : " + motionName + "  voice : " + soundPath);
			StartVoice( acVoice);
			mainMotionManager.startMotionPrio(motion, priority);
		}
	}

    /// <summary>
    /// モーション終了検知
    /// </summary>
    private void OnMotionEnd()
    {
        //表情初期化
        SetExpression(EXPRESSION.NORMAL_01);
        StartRandomMotion("MOTION_NORMAL", LAppDefine.PRIORITY_FORCE);

        //モーション終了したら、ネクストエモーションを実行
        if(NextEmotionName !=null)
        {
            SetExpression(NextEmotionName);
        }

        //アイドルモーション
        StartRandomMotion("MOTION_IDLE", LAppDefine.PRIORITY_FORCE);
    }

    /// <summary>
    /// 音声とモーションの同時再生
    /// </summary>
    /// <param name="pVoice">優先度。使用しないなら0で良い。</param>
    public void StartVoice( AudioClip pVoice)
	{
		if (asVoice == null)
		{
			Debug.Log("Live2D : AudioSource Component is NULL !");
			return;
		}
		asVoice.clip = pVoice;
		asVoice.loop = false;
        asVoice.spatialBlend = 0;
        asVoice.Play();
	}

    /// <summary>
    /// 再生中か？
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        return asVoice.isPlaying;
    }

    #endregion

    ///====================================================================
    ///
    ///                            表情関連処理
    ///                         
    ///====================================================================
    #region 表情関連処理

    /// <summary>
    /// 表情設定
    /// </summary>
    /// <param name="name">表情名を指定する</param>
    public void SetExpression(string name)
	{
        try
        {
            if (!expressions.ContainsKey(name)) return;// 無効な指定ならなにもしない
            if (LAppDefine.DEBUG_LOG) Debug.Log("Setting expression : " + name);
            AMotion motion = expressions[name];
            expressionManager.startMotion(motion, false);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }

	}

    /// <summary>
    /// 次の表情をセットする
    /// </summary>
    /// <param name="name"></param>
    public void SetExpressionNext(string name)
    {
        if (!expressions.ContainsKey(name)) return;// 無効な指定ならなにもしない
        this.NextEmotionName = name;
    }

    /// <summary>
    /// 表情をランダムに切り替える
    /// </summary>
	public void SetRandomExpression()
	{
		int no = (int)(rand.NextDouble() * expressions.Count);

		string[] keys = new string[expressions.Count];
		expressions.Keys.CopyTo(keys, 0);

		SetExpression(keys[no]);
	}

    #endregion


    //====================================================================
    //
    //                            イベントハンドラ
    //                         
    //====================================================================
    #region イベントハンドラ
    /// <summary>
    /// フリックした時のイベント
    /// 
    /// LAppView側でフリックイベントを感知した時に呼ばれ
    /// フリック時のモデルの動きを開始します。
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void FlickEvent(float x, float y)
	{
		if (LAppDefine.DEBUG_LOG) Debug.Log("flick x:" + x + " y:" + y);

		if (HitTest(LAppDefine.HIT_AREA_HEAD, x, y))
		{
			if (LAppDefine.DEBUG_LOG) Debug.Log("Flick face");
			//StartRandomMotion(MOTION.FLICK_HEAD, LAppDefine.PRIORITY_NORMAL);
		}
	}

    /// <summary>
    /// タップしたときのイベント
    /// </summary>
    /// <param name="x">タップの座標 x</param>
    /// <param name="y">タップの座標 y</param>
    /// <returns></returns>
    public bool TapEvent(float x, float y)
	{
		if (LAppDefine.DEBUG_LOG) Debug.Log("tapEvent view x:" + x + " y:" + y);

		if (HitTest(LAppDefine.HIT_AREA_HEAD, x, y))
		{
			// 顔をタップしたら表情切り替え
			if (LAppDefine.DEBUG_LOG) Debug.Log("顔をタップしました！");
            CtrlTalk.Instance.NextTalkOrSkip();
        }
        else if (HitTest(LAppDefine.HIT_AREA_OPPAI, x, y))
        {
            if (LAppDefine.DEBUG_LOG) Debug.Log("胸をタップしました！");
            CtrlTalk.Instance.NextTalkOrSkip();
            StartRandomMotion("MOTION_PROUD_M", LAppDefine.PRIORITY_FORCE);
        }
        else if (HitTest(LAppDefine.HIT_AREA_BODY, x, y))
		{
			if (LAppDefine.DEBUG_LOG) Debug.Log("体をタップしました！");
            CtrlTalk.Instance.NextTalkOrSkip();

        }

        return true;
	}

    /// <summary>
    /// ドラッグ移動中
    /// 
    /// 開始座標との差の分を移動
    /// </summary>
    /// <param name="inputPos"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// 
    private const float rate = 0.8f;
    public void DragMove(Vector3 inputPos, float x, float y)
    {
        if (HitTest(LAppDefine.HIT_AREA_BODY, x, y))
        {
            //時モデルがドラッグ中でなければ移動させない
            if (!this.FlgDragging)
            {
                return;
            }

            float moveValX = (this.DragStartPoint.x - inputPos.x) * rate;
            float moveValY = (this.DragStartPoint.y - inputPos.y) * rate;

            //0.5以上動いたら動かす。クリックで動かさない対策
            if(Math.Abs(moveValX) >= 1.0f && Math.Abs(moveValY) >= 1.0f)
            {
                Move(new Vector3(this.DragStartPointModel.x - moveValX, this.DragStartPointModel.y - moveValY, this.DragStartPointModel.z));
            }
            
        }
    }

    /// <summary>
    /// ドラッグ開始
    /// 
    /// 開始時に開始座標を記録
    /// </summary>
    /// <param name="inputPos"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void DragStart(Vector3 inputPos, float x, float y)
    {
        if (HitTest(LAppDefine.HIT_AREA_BODY, x, y))
        {
            //既にドラッグ中なら何もしない
            if (LAppLive2DManager.Instance.FlgDrag)
            {
                return;
            }

            //親コンポーネントが無効ならドラッグを開始しない
            if(!parent.transform.parent.gameObject.activeSelf)
            {
                return;
            }

            //ドラッグ開始位置記録
            this.DragStartPoint = inputPos;
            this.DragStartPointModel = this.parentTransForm.localPosition;

            //ドラッグ終了
            LAppLive2DManager.Instance.FlgDrag = true;

            //自分モデル ドラッグ開始
            this.FlgDragging = true;
        }
    }

    /*
	 * シェイクイベント
	 *
	 * LAppView側でシェイクイベントを感知した時に呼ばれ、
	 * シェイク時のモデルの動きを開始します。
	 */

    /// <summary>
    /// シェイクイベント
    /// 
    /// LAppView側でシェイクイベントを感知した時に呼ばれ、
    /// シェイク時のモデルの動きを開始します。
    /// </summary>
    public void ShakeEvent()
	{
		if (LAppDefine.DEBUG_LOG) Debug.Log("Shake Event");

		StartRandomMotion("MOTION_SHAKE", LAppDefine.PRIORITY_FORCE);
	}

    /// <summary>
    /// タッチ開始イベント
    /// </summary>
    /// <param name="inputPos"></param>
    internal void TouchesBegan(Vector3 inputPos)
    {
        view.TouchesBegan(inputPos);
    }

    /// <summary>
    /// タッチ中イベント
    /// </summary>
    /// <param name="inputPos"></param>
    internal void TouchesMoved(Vector3 inputPos)
    {
        view.TouchesMoved(inputPos);
    }

    /// <summary>
    /// タッチ終了イベント
    /// </summary>
    /// <param name="inputPos"></param>
    internal void TouchesEnded(Vector3 inputPos)
    {
        view.TouchesEnded(inputPos);

        //タッチ終了時、フラグを寝かす
        FlgDragging = false;
    }

    #endregion

    //====================================================================
    //
    //                              アタリ判定
    //                         
    //====================================================================
    #region アタリ判定
    /// <summary>
    /// 当たり判定
    /// 
    ///
    ///当たり判定との簡易テスト。
    ///指定IDの頂点リストからそれらを含む最大の矩形を計算し、点がそこに含まれるか判定
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="testX"></param>
    /// <param name="testY"></param>
    /// <returns></returns>
    public bool HitTest(string id, float testX, float testY)
    {
        if (modelSetting == null) return false;
        int len = modelSetting.GetHitAreasNum();
        for (int i = 0; i < len; i++)
        {
            if (id.Equals(modelSetting.GetHitAreaName(i)))
            {
                string drawID = modelSetting.GetHitAreaID(i);
                return hitTestSimple(drawID, testX, testY);
            }
        }
        return false;// 存在しない場合はfalse
    }

    #endregion

    //====================================================================
    //
    //                      モーション設定、表情設定取得
    //                         
    //====================================================================
    #region モーション設定、表情設定取得

    /// <summary>
    /// 表情リストを取得する
    /// </summary>
    /// <returns></returns>
    public List<string> GetExpressionList()
    {
        //表情リストを取得する
        return new List<string>(modelSetting.GetExpressionNames());
    }

    /// <summary>
    /// モーションリストを取得する
    /// </summary>
    /// <returns></returns>
	public List<MsgMotion> GetMotionList()
    {
        //結果リスト
        List<MsgMotion> resList = new List<MsgMotion>();

        //グループリストを取得する
        string[] gropuList = modelSetting.GetMotionGroupNames();

        //グループを回してリストを生成する
        foreach (string name in gropuList)
        {
            int num = modelSetting.GetMotionNum(name);

            for (int i = 0; i < num; i++)
            {
                resList.Add(new MsgMotion(name, i));
            }
        }

        return resList;
    }


    public string GetModelName()
    {
        return parent.gameObject.name;
    }


    #endregion

    //====================================================================
    //
    //                          モデル移動など
    //                         
    //====================================================================
    #region モデル移動など
    public void Move(Vector3 moveTarget)
    {
        parentTransForm.localPosition = moveTarget;
    }
    public void Move(float x,float y,float z)
    {
        Move(new Vector3(x, y, z));
    }
    #endregion
}
