//=======================================================================
//  ClassName : ModelSettingJson
//  概要      : モデルセッティング
//              モデル設定のjsonをパースする
//
//  (c) Live2D Inc.All rights reserved.
//=======================================================================﻿﻿
using System ;
using System.Collections.Generic ;
using live2d ;

public class ModelSettingJson : ModelSetting 
{
    ///=============================
    ///文字定義
    private Value json ;
	private const string NAME				= "name";
	private const string ID					= "id";
	private const string MODEL				= "model";
	private const string TEXTURES			= "textures";
	private const string HIT_AREAS			= "hit_areas";
	private const string PHYSICS			= "physics";
	private const string POSE				= "pose";
	private const string EXPRESSIONS		= "expressions";
	private const string MOTION_GROUPS		= "motions";
	private const string SOUND				= "sound";
	private const string FADE_IN			= "fade_in";
	private const string FADE_OUT			= "fade_out";
	
	private const string VALUE				= "val";
	private const string FILE				= "file";
	private const string INIT_PARTS_VISIBLE	= "init_parts_visible";
	private const string INIT_PARAM			= "init_param";
	private const string LAYOUT				= "layout";
	
	
	/// <summary>
    /// コンストラクター
    /// </summary>
    /// <param name="str"></param>
	public ModelSettingJson( String str)
	{
		char[] buf = str.ToCharArray() ;
		json = Json.parseFromBytes( buf ) ;
	}
	
    /// <summary>
    /// モーション存在チェック
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
	public bool ExistMotion(string name) 
	{
		if(!json.getMap(null).ContainsKey(MOTION_GROUPS))
			return false;
		return json.get (MOTION_GROUPS).getMap (null).ContainsKey (name);
	}//json.motion_group[name]
	
    /// <summary>
    /// モーションサウンド存在チェック
    /// </summary>
    /// <param name="name"></param>
    /// <param name="n"></param>
    /// <returns></returns>
	public bool ExistMotionSound(string name,int n)	
	{
		if(!json.getMap(null).ContainsKey(MOTION_GROUPS))
			return false;
		return json.get(MOTION_GROUPS).get(name).get(n).getMap(null).ContainsKey(SOUND);
	}
	
    /// <summary>
    /// フェードインモーション存在チェック
    /// </summary>
    /// <param name="name"></param>
    /// <param name="n"></param>
    /// <returns></returns>
	public bool ExistMotionFadeIn(string name,int n)
	{
		if(!json.getMap(null).ContainsKey(MOTION_GROUPS))
			return false;
		return json.get(MOTION_GROUPS).get(name).get(n).getMap(null).ContainsKey(FADE_IN);
	}

    /// <summary>
    /// フェードアウトモーション存在チェック
    /// </summary>
    /// <param name="name"></param>
    /// <param name="n"></param>
    /// <returns></returns>
	public bool ExistMotionFadeOut(string name,int n)
	{
		if(!json.getMap(null).ContainsKey(MOTION_GROUPS))
			return false;
		return json.get(MOTION_GROUPS).get(name).get(n).getMap(null).ContainsKey(FADE_OUT);
	}
	
	/// <summary>
    /// モデル名取得
    /// </summary>
    /// <returns></returns>
	public string GetModelName()
	{
		if( !json.getMap(null).ContainsKey(NAME) )return null;
		return json.get(NAME).toString();
	}
	
	/// <summary>
    /// モデルファイルを取得する
    /// </summary>
    /// <returns></returns>
	public string GetModelFile()
	{
		if( !json.getMap(null).ContainsKey(MODEL) )return null;
		return json.get(MODEL).toString();
	}
	
	/// <summary>
    /// テクスチャー数を取得する
    /// </summary>
    /// <returns></returns>
	public int GetTextureNum()
	{
		if( !json.getMap(null).ContainsKey(TEXTURES) )return 0;
		return json.get(TEXTURES).getVector(null).Count ; // json.textures.length
	}
	
	/// <summary>
    /// テクスチャファイルを取得する
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetTextureFile(int n)
	{
		return json.get(TEXTURES).get(n).toString();//json.textures[n]
	}

    /// <summary>
    /// ヒットエリア数を取得する
    /// </summary>
    /// <returns></returns>
	public int GetHitAreasNum()
	{
		if( !json.getMap(null).ContainsKey(HIT_AREAS) )return 0;
		return json.get(HIT_AREAS).getVector(null).Count ; // json.hit_area.length
	}

    /// <summary>
    /// ヒットエリアIDを取得する
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetHitAreaID(int n)
	{
		return json.get(HIT_AREAS).get(n).get(ID).toString();//json.hit_area[n].id
	}

    /// <summary>
    /// ヒットエリア名を取得する
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetHitAreaName(int n)
	{
		return json.get(HIT_AREAS).get(n).get(NAME).toString();//json.hit_area[n].name
	}

    /// <summary>
    /// 物理演算ファイルを取得する
    /// </summary>
    /// <returns></returns>
	public string GetPhysicsFile()
	{
		if( !json.getMap(null).ContainsKey(PHYSICS) )return null;
		return json.get(PHYSICS).toString();
	}

    /// <summary>
    /// ポーズファイルを取得する
    /// </summary>
    /// <returns></returns>
	public string GetPoseFile()
	{
		if( !json.getMap(null).ContainsKey(POSE) )return null;
		return json.get(POSE).toString();
	}

    /// <summary>
    /// モーション数を取得する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
	public int GetMotionNum(string name)
	{
		if( ! ExistMotion(name))return 0;
		return json.get(MOTION_GROUPS).get(name).getVector(null).Count;//json.motion_group[name].length
	}

    /// <summary>
    /// モーションファイルを取得する
    /// </summary>
    /// <param name="name"></param>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetMotionFile(string name,int n)
	{
		if( ! ExistMotion(name))return null;
		return json.get(MOTION_GROUPS).get(name).get(n).get(FILE).toString();//json.motion_group[name][n].file
	}

    /// <summary>
    /// モーションサウンドを取得する
    /// </summary>
    /// <param name="name"></param>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetMotionSound(string name,int n)
	{
		if( ! ExistMotionSound(name,n))return null;	
		return json.get(MOTION_GROUPS).get(name).get(n).get(SOUND).toString();//json.motion_group[name][n].sound
	}

    /// <summary>
    /// フェードインモーションを取得する
    /// </summary>
    /// <param name="name"></param>
    /// <param name="n"></param>
    /// <returns></returns>
	public int GetMotionFadeIn(string name,int n)
	{
		return (! ExistMotionFadeIn(name,n))? 1000 :  json.get(MOTION_GROUPS).get(name).get(n).get(FADE_IN).toInt();//json.motion_group[name][n].fade_in
	}

    /// <summary>
    /// フェードアウトモーションを取得する
    /// </summary>
    /// <param name="name"></param>
    /// <param name="n"></param>
    /// <returns></returns>
	public int GetMotionFadeOut(string name,int n)
	{
		return (! ExistMotionFadeOut(name,n))? 1000 :json.get(MOTION_GROUPS).get(name).get(n).get(FADE_OUT).toInt();//json.motion_group[name][n].fade_out
	}

	/// <summary>
    /// モーショングルームネームのリストを取得する
    /// </summary>
    /// <returns></returns>
	public string[] GetMotionGroupNames()
	{
		if( !json.getMap(null).ContainsKey(MOTION_GROUPS) )return null;
		System.Object[] keys = json.get(MOTION_GROUPS).keySet().ToArray();

		if( keys.Length == 0 )return null;

		string[] names = new string[keys.Length];

		for (int i = 0; i < names.Length; i++)
		{
			names[i] = (string)keys[i];
		}
		return  names;
	}
	
	
    /// <summary>
    /// レイアウトを取得する
    /// </summary>
    /// <param name="layout"></param>
    /// <returns></returns>
	public bool GetLayout(Dictionary<string, float> layout)
	{
		if(!json.getMap(null).ContainsKey(LAYOUT))return false;

		Dictionary<string,Value> map = json.get(LAYOUT).getMap(null);
		string[] keys = new string[map.Count] ;
		map.Keys.CopyTo(keys,0);

		for(int i=0;i<keys.Length;i++)
		{
			layout.Add(keys[i], json.get(LAYOUT).get(keys[i]).toFloat() );
		}
		return true;
	}
	
	/// <summary>
    /// 初期化パラメーター数を取得する
    /// </summary>
    /// <returns></returns>
	public int GetInitParamNum()
	{
		if(!json.getMap(null).ContainsKey(INIT_PARAM))return 0;
        return json.get(INIT_PARAM).getVector(null).Count;
	}

    /// <summary>
    /// 初期化パラメータを取得する
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public float GetInitParamValue(int n)
	{
		return json.get(INIT_PARAM).get(n).get(VALUE).toFloat();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetInitParamID(int n)
	{
		return json.get(INIT_PARAM).get(n).get(ID).toString();
	}

	
	/// <summary>
    /// 初期パーツ数を取得する
    /// </summary>
    /// <returns></returns>
	public int GetInitPartsVisibleNum()
	{
		if(!json.getMap(null).ContainsKey(INIT_PARTS_VISIBLE) )return 0;
        return json.get(INIT_PARTS_VISIBLE).getVector(null).Count;
	}

    /// <summary>
    /// 初期パーツ表示値を取得する
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public float GetInitPartsVisibleValue(int n)
	{
		return json.get(INIT_PARTS_VISIBLE).get(n).get(VALUE).toFloat();
	}

    /// <summary>
    /// 初期化パーツ表示値ID取得
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetInitPartsVisibleID(int n)
	{
		return json.get(INIT_PARTS_VISIBLE).get(n).get(ID).toString();
	}

    /// <summary>
    /// 表情数取得
    /// </summary>
    /// <returns></returns>
	public int GetExpressionNum()
	{
		if(!json.getMap(null).ContainsKey(EXPRESSIONS))return 0;
		return json.get(EXPRESSIONS).getVector(null).Count;
	}

    /// <summary>
    /// 表情ファイル取得
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetExpressionFile(int n)
	{
		return json.get(EXPRESSIONS).get(n).get(FILE).toString();
	}

    /// <summary>
    /// 表情名取得
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
	public string GetExpressionName(int n)
	{
		return json.get(EXPRESSIONS).get(n).get(NAME).toString();
	}

    /// <summary>
    /// テクスチャファイル取得
    /// </summary>
    /// <returns></returns>
	public string[] GetTextureFiles() 
	{
		string[] ret=new string[GetTextureNum()];
		for (int i = 0; i < ret.Length; i++)
		{
			ret[i] = GetTextureFile(i);
		}
		return ret;
	}

    /// <summary>
    /// テクスチャファイル取得
    /// </summary>
    /// <returns></returns>
	public string[] GetExpressionFiles() 
	{
		string[] ret=new string[GetExpressionNum()];
		for (int i = 0; i < ret.Length; i++)
		{
			ret[i] = GetExpressionFile(i);
		}
		return ret;
	}

    /// <summary>
    /// 表情名リスト取得
    /// </summary>
    /// <returns></returns>
	public string[] GetExpressionNames() 
	{
		string[] ret=new string[GetExpressionNum()];
		for (int i = 0; i < ret.Length; i++)
		{
			ret[i] = GetExpressionName(i);
		}
		return ret;
	}
	
}