//=======================================================================
//  ClassName : LAppModelProxy
//  概要      : モデルプロキシー
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿﻿﻿
using UnityEngine;
using System;
using live2d;

[ExecuteInEditMode]
public class LAppModelProxy : MonoBehaviour
{
	///=============================
	///モデルパス
	public String path = "";

    ///=============================
    ///透明度
    [Range(0.0f, 1.0f)]
    public float modelOpacity = 1.0f;   //透明度の調整
    public float targetOpacity = 1.0f;  //対象透明度
    public float incrimentRate = 0.05f;

    ///=============================
    ///モデル
    private LAppModel model;


    ///=============================
    ///ビジブル
    private bool isVisible = false ;

	///====================================================================
	///
	///                             プロパティ操作
	///                         
	///====================================================================
	#region プロパティ操作

	/// <summary>
	/// ビジブルのセット
	/// </summary>
	/// <param name="isVisible"></param>
	public void SetVisible(bool isVisible)
	{
		this.isVisible = isVisible;
	}

	/// <summary>
	/// ビジブルの取得
	/// </summary>
	/// <returns></returns>
	public bool GetVisible()
	{
		return isVisible;
	}

	#endregion

	///====================================================================
	///
	///                             モデル操作
	///                         
	///====================================================================
	#region モデル操作

	/// <summary>
	/// 起動時処理
	/// </summary>
	void Awake()
	{
        //パスチェック
		if (path == "") return;

        //モデル生成
		model = new LAppModel(this);

        //生成したモデルを追加
		LAppLive2DManager.Instance.AddModel(name,this);

        //モデルをロードする
        loadModel(this.path);

    }
	
    /// <summary>
    /// モデルをロードする
    /// </summary>
    /// <param name="path"></param>
    void loadModel(string pPath)
    {
        //ファイルマネージャーを用いて、物理パス取得
        var filename = FileManager.getFilename(path);

        //ディレクトリ取得
        var dir = FileManager.getDirName(path);

        //ロードファイル名表示
        Debug.Log("Load " + dir + "  filename:" + filename);

        //アセットストリームからモデルをロードする
        model.LoadFromStreamingAssets(dir, filename);
    }

    /// <summary>
    /// 描画処理
    /// </summary>
    void OnRenderObject()
    {
        // メインカメラ以外は処理を実行しない(負荷低減対策)　
        //カメラは1つしかないので、コメントとしておく
        //if (Camera.current.tag != "MainCamera")
        //{
        //    return;
        //}

        //非表示なら何もしない
        if (!isVisible) return;

        //モデルがNULLなら何もしない
        if (model == null) return;

        //ドローメッシュモードなら、このタイミングでドロー処理を行う
        if (model.GetLive2DModelUnity().getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW)
        {
            model.Draw();
        }

        //デバッグ設定チェック
        if (LAppDefine.DEBUG_DRAW_HIT_AREA)
        {
            // デバッグ用当たり判定の描画
            model.DrawHitArea();
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
            if(modelOpacity > 1.0f)
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
            if(modelOpacity == 0.0f)
            {
                SetVisible(false);
            }
        }

        //すべてのパーツの透明度調整
        var partList = model.getLive2DModel().getModelImpl().getPartsDataList();
        foreach (var item in partList)
        {
            model.getLive2DModel().setPartsOpacity(item.getPartsDataID().ToString(), modelOpacity);
        }
       
    }

    /// <summary>
    /// 描画処理
    /// </summary>
    void Update()
	{
        //非表示なら描画しない
		if(!isVisible) return;

        //モデルがNULLなら何もしない
		if (model == null) return;

        //モデルの描画処理
		model.Update();

        //レンダリングモードがドローメッシュに設定されていれば、ドロー処理も行う。
		if (model.GetLive2DModelUnity().getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH)
		{
            //モデルのドロー処理
			model.Draw();
		}
	}

	/// <summary>
	/// モデルを取得する
	/// </summary>
	/// <returns></returns>
	public LAppModel GetModel()
	{
		return model;
	}



	/// <summary>
	/// オーディオソースのリセット
	/// </summary>
	public void ResetAudioSource()
	{
		//ゲームオブジェクトのコンポーネント取得
		Component[] components = gameObject.GetComponents<AudioSource>();

        //リストを回して、削除
		for (int i = 0; i < components.Length; i++)
		{
			Destroy(components[i]);
		}
	}




	#endregion
}