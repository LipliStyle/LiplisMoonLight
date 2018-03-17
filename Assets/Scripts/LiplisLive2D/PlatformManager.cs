//=======================================================================
//  ClassName : PlatformManager
//  概要      : プラットフォームマネージャー
//
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//=======================================================================﻿
using live2d;
using live2d.framework;
using UnityEngine;

class PlatformManager : IPlatformManager
{
    /// <summary>
    /// jsonファイルをバイナリでロードする
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public byte[] loadBytes(string path)
	{
        //パス取得
        var assetsPath = path.Replace(".json","");

        //ファイルマネージャーを使ってロード
		return FileManager.LoadBin(assetsPath);
    }

    /// <summary>
    /// jsonファイルをStringでロードする
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string loadString(string path)
	{
        //パス取得
        var assetsPath = path.Replace(".json","");

        //ファイルマネージャーを使ってロード
		return FileManager.LoadString(assetsPath);
    }

    /// <summary>
    /// live2Dモデルをロードする
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public ALive2DModel loadLive2DModel(string path)
    {
        //バイナリでロード
		var data = FileManager.LoadBin(path);

        //Live2Dモデルロード
        var live2DModel = Live2DModelUnity.loadModel(data);

        //Live2Dを返す
        return live2DModel;
    }

    /// <summary>
    /// テクスチャをロードする
    /// </summary>
    /// <param name="model"></param>
    /// <param name="no"></param>
    /// <param name="path"></param>
    public void loadTexture(live2d.ALive2DModel model, int no, string path)
    {
        //デバッグモードならログ出力
        if (LAppDefine.DEBUG_LOG) Debug.Log("Load texture " + path);

        //パス取得
		var texPath = path.Replace (".png", "");

        //テクスチャ生成
		Texture2D texture = FileManager.LoadTexture(texPath);

        //テクスチャセット
        ((Live2DModelUnity)model).setTexture(no, texture);
    }

    /// <summary>
    /// デバッグログ出力
    /// </summary>
    /// <param name="txt"></param>
    public void log(string txt)
    {
        Debug.Log(txt);
    }
}