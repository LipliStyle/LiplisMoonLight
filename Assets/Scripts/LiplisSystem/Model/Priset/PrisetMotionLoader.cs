﻿//====================================================================
//  ClassName : PrisetExpressionLoader
//  概要      : プリセット表情データを読み込む
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//====================================================================

using Live2D.Cubism.Framework.Json;
using UnityEngine;

namespace Assets.Scripts.LiplisSystem.Model.Priset
{
    public class PrisetMotionLoader
    {
        /// <summary>
        /// プリセットモデルの設定を読み出す。
        /// リソースからの相対パスを指定
        /// </summary>
        /// <param name="pathResources"></param>
        public static AnimationClip Load(string pathResources)
        {
            //モデルの設定データをロードする。
            TextAsset textasset = Resources.Load(pathResources) as TextAsset;

            //キュービズムJosnデシリアライズ
            //CubismMotion3Json model3Json = JsonConvert.DeserializeObject<CubismMotion3Json>(textasset.text);
            CubismMotion3Json model3Json = JsonUtility.FromJson<CubismMotion3Json>(textasset.text);

            //アニメーションクリップを生成し、返す
            return model3Json.ToAnimationClip();
        }

    }
}
