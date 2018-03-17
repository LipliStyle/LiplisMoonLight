//=======================================================================
//  ClassName : LAppLive2DManager
//  概要      : Live2モデルを管理する
//              シングルトン
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using UnityEngine;
using System.Collections.Generic;
using live2d;
using live2d.framework;

public class LAppLive2DManager
{

    ///=============================
    ///モデル表示用ゲームオブジェクト
    private Dictionary<string, LAppModelProxy> lstModel;

    ///=============================
    ///タッチモード
    private bool touchMode2D;

    //====================================================================
    //
    //                          シングルトン管理
    //                         
    //====================================================================
    #region シングルトン管理

    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    private static LAppLive2DManager instance;
    public static LAppLive2DManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LAppLive2DManager();
            }

            return instance;
        }
    }

    #endregion

    //====================================================================
    //
    //                            初期化処理
    //                         
    //====================================================================
    #region 初期化処理
    /// <summary>
    /// コンストラクター
    /// </summary>
    public LAppLive2DManager()
    {
        Live2D.init();
        Live2DFramework.setPlatformManager(new PlatformManager());
        lstModel = new Dictionary<string, LAppModelProxy>();
    }

    /// <summary>
    /// モデル追加
    /// </summary>
    /// <param name="item"></param>
    public void AddModel(string key, LAppModelProxy item)
    {
        //ログ出力
        if (LAppDefine.DEBUG_LOG)
        {
            Debug.Log("Add Live2D Model : " + key);
        }

        //モデルリストに追加
        if(!lstModel.ContainsKey(key))
        {
            lstModel.Add(key, item);
        }
        else
        {
            lstModel[key] =item;
        }

        lstModel[key].SetVisible(true);
    }

    /// <summary>
    /// タッチ2D設置
    /// </summary>
    /// <param name="value"></param>
    public void SetTouchMode2D(bool value)
    {
        touchMode2D = value;
    }

    /// <summary>
    /// タッチモードの取得
    /// </summary>
    /// <returns></returns>
    public bool IsTouchMode2D()
    {
        return touchMode2D;
    }
    #endregion

    //====================================================================
    //
    //                            タッチ制御
    //                         
    //====================================================================
    #region タッチ制御

    /// <summary>
    /// タッチ開始
    /// </summary>
    /// <param name="inputPos"></param>
    public void TouchesBegan(Vector3 inputPos)
    {
        foreach (var modelData in lstModel)
        {
            if (modelData.Value.GetVisible())
            {
                modelData.Value.GetModel().TouchesBegan(inputPos);
            }
        }
    }

    /// <summary>
    /// タッチ移動
    /// </summary>
    /// <param name="inputPos"></param>
    public void TouchesMoved(Vector3 inputPos)
    {
        foreach (var modelData in lstModel)
        {
            if (modelData.Value.GetVisible())
            {
                modelData.Value.GetModel().TouchesMoved(inputPos);
            }
        }
    }

    /// <summary>
    /// タッチ終了
    /// </summary>
    /// <param name="inputPos"></param>
    public void TouchesEnded(Vector3 inputPos)
    {
        foreach (var modelData in lstModel)
        {
            if (modelData.Value.GetVisible())
            {
                modelData.Value.GetModel().TouchesEnded(inputPos);
            }
        }
    }

    #endregion

    //====================================================================
    //
    //                            表情制御
    //                         
    //====================================================================
    #region 表情制御

    /// <summary>
    /// 表情をセットする
    /// </summary>
    /// <param name="expressionName"></param>
    public void SetExpression(string key, string expressionName)
    {
        if (!lstModel.ContainsKey(key))
        {
            return;
        }

        if (lstModel[key].GetVisible())
        {
            lstModel[key].GetModel().SetExpression(expressionName);
        }
    }

    /// <summary>
    /// モーションを開始する
    /// </summary>
    /// <param name="group"></param>
    /// <param name="no"></param>
    /// <param name="priority"></param>
    public void StartMotion(string key, string group, int no, int priority)
    {
        if (!lstModel.ContainsKey(key))
        {
            return;
        }

        if (lstModel[key].GetVisible())
        {
            lstModel[key].GetModel().StartMotion(group, no, priority);
        }
    }
    public void StartRandomMotion(string key, string group, int priority)
    {
        lstModel[key].GetModel().StartRandomMotion(group, priority);
    }
    public void StartRandomMotion(string key, string group)
    {
        StartRandomMotion(key, group, LAppDefine.PRIORITY_NORMAL);
    }



    /// <summary>
    /// リップシンクセット
    /// </summary>
    /// <param name="key"></param>
    /// <param name="lipVal"></param>
    public void SetLip(string key, float lipVal)
    {
        if (!lstModel.ContainsKey(key))
        {
            return;
        }

        lstModel[key].GetModel().lipSyncValue = lipVal;
    }

    /// <summary>
    /// おしゃべり開始
    /// </summary>
    public void StopTalking(string key)
    {
        if (!lstModel.ContainsKey(key))
        {
            return;
        }

        if (lstModel[key].GetVisible())
        {
            lstModel[key].GetModel().lipSyncValue = 0;
        }
    }

    /// <summary>
    /// ビジブルセット
    /// </summary>
    public void SetVisible(string key, bool visible)
    {
        if (!lstModel.ContainsKey(key))
        {
            return;
        }

        //ビジブルセット
        lstModel[key].SetVisible(visible);
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="key"></param>
    public void SetFadeOut(string key)
    {
        SetFadeOut(key, 0.05f);
    }
    public void SetFadeOut(string key,float incrimentRate)
    {
        if (!lstModel.ContainsKey(key))
        {
            return;
        }

        lstModel[key].incrimentRate = incrimentRate;
        lstModel[key].targetOpacity = 0;
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="key"></param>
    public void SetFadeIn(string key)
    {
        SetFadeIn(key, 0.05f);
    }
    public void SetFadeIn(string key, float incrimentRate)
    {
        if (!lstModel.ContainsKey(key))
        {
            return;
        }

        lstModel[key].SetVisible(true);
        lstModel[key].incrimentRate = incrimentRate;
        lstModel[key].targetOpacity = 1.0f;
    }

    /// <summary>
    /// 対象座標に移動する
    /// </summary>
    /// <param name="key"></param>
    /// <param name="moveTarget"></param>
    public void SetMove(string key,Vector3 moveTarget)
    {
        if (!lstModel.ContainsKey(key))
        {
            return;
        }


        lstModel[key].GetModel().Move(moveTarget);
    }


    /// <summary>
    /// モデル変更
    /// </summary>
    /// <param name="path"></param>
    public void ChangeModel(string key, string path)
    {
        if (lstModel[key].GetVisible())
        {
            loadModel(lstModel[key].GetModel(), path);
        }
    }

    /// <summary>
    /// モデルをロードする
    /// </summary>
    /// <param name="path"></param>
    void loadModel(LAppModel model, string pPath)
    {
        //ファイルマネージャーを用いて、物理パス取得
        var filename = FileManager.getFilename(pPath);

        //ディレクトリ取得
        var dir = FileManager.getDirName(pPath);

        //ロードファイル名表示
        Debug.Log("Load " + dir + "  filename:" + filename);

        //アセットストリームからモデルをロードする
        model.LoadFromStreamingAssets(dir, filename);
    }


    #endregion
}