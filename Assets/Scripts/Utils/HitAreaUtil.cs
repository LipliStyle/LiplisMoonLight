//=======================================================================
//  ClassName : HitAreaUtil
//  概要      : ヒットエリアテスト
//
//  (c) Live2D Inc.All rights reserved.
//=======================================================================﻿﻿
using System.IO;
using UnityEngine;

public class HitAreaUtil {
    private static Material material;
    private static Color color = new Color(1, 0, 0, 0.5f);
    private static Vector3[] vertex=new Vector3[8];


    /// <summary>
    /// ヒットエリアマテリアルを取得する
    /// エラーが発生する場合は、この処理をコメントアウト氏、下の処理に変更する
    /// </summary>
    static void CreateHitAreaMaterial()
    {
        //シェーダを生成する。
        //シェーダ設定はCreateShaderSettingメソッドで生成できる。あらかじめ設定しておく必要がある。
        Shader currentShader = Resources.Load("workingshader") as Shader;

        //カレントシェーダからマテリアルを生成する
        material = new Material(currentShader);

        material.hideFlags = HideFlags.HideAndDontSave;
        material.shader.hideFlags = HideFlags.HideAndDontSave;
    }

    //<summary>
    //SDKの以下の書式だと、警告が出るためコメントアウト。
    //代わりに上の処理を使用
    //</summary>
    //static void CreateHitAreaMaterial()
    //{
    //    material = new Material(
    //       "Shader \"Lines/HitArea\" {" +
    //       "SubShader {" +
    //       "    Pass { " +
    //       "       Blend SrcAlpha OneMinusSrcAlpha" +
    //       "       Cull Off" +
    //       "       ZWrite Off" +
    //       "       ZTest Less" +
    //       "       Fog { Mode Off }" +
    //       "       BindChannels {" +
    //       "           Bind \"Vertex\", vertex" +
    //       "           Bind \"Color\", color" +
    //       "       }" +
    //       "} } }");
    //    material.hideFlags = HideFlags.HideAndDontSave;
    //    material.shader.hideFlags = HideFlags.HideAndDontSave;
    //}

    /// <summary>
    /// 矩形描画
    /// </summary>
    /// <param name="m"></param>
    /// <param name="l"></param>
    /// <param name="r"></param>
    /// <param name="t"></param>
    /// <param name="b"></param>
    public static void DrawRect(Matrix4x4 m, float l, float r, float t, float b)
    {
        if (material==null)
        {
            CreateHitAreaMaterial();
        }
        material.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(m);

        GL.Begin(GL.LINES);
        GL.Color(color);

        vertex[0].x = l;
        vertex[0].y = t;

        vertex[1].x = r;
        vertex[1].y = t;

        vertex[2].x = r;
        vertex[2].y = b;

        vertex[3].x = l;
        vertex[3].y = b;

        GL.Vertex(vertex[0]);
        GL.Vertex(vertex[1]);

        GL.Vertex(vertex[1]);
        GL.Vertex(vertex[2]);

        GL.Vertex(vertex[2]);
        GL.Vertex(vertex[3]);

        GL.Vertex(vertex[3]);
        GL.Vertex(vertex[0]);

        GL.End();
        GL.PopMatrix();
    }
}