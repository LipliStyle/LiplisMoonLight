//=======================================================================
//  ClassName : AccelHelper
//  概要      : 加速度関連情報管理
//
//
//  (c) Live2D Inc.All rights reserved.
//=======================================================================﻿﻿
using UnityEngine;
using live2d;

/*
 * 加速度センサの情報の管理。
 *
 */
public class AccelHelper
{
    ///=============================
    ///計算用フィールド
    private static float acceleration_x = 0 ;
	private static float acceleration_y = 0 ;
	private static float acceleration_z = 0 ;
	private static float dst_acceleration_x = 0 ;
	private static float dst_acceleration_y = 0 ;
	private static float dst_acceleration_z = 0 ;

	private static float last_dst_acceleration_x = 0 ;
	private static float last_dst_acceleration_y = 0 ;
	private static float last_dst_acceleration_z = 0 ;

    ///=============================
    ///移動量
    private static long lastTimeMSec = -1 ;
	private static float lastMove ;

    ///=============================
    ///加速度
    private float[] accel = new float[3] ;

    /// <summary>
    /// コンストラクター
    /// </summary>
	public AccelHelper() 
	{
		
	}


    /// <summary>
    /// 最終移動量取得
    /// 
    /// デバイスを振ったときなどにどのくらい揺れたかを取得。
    /// 1を超えるとそれなりに揺れた状態。
    /// resetShake()を使ってリセットできる。
    /// </summary>
    /// <returns></returns>
    public float GetShake()
	{
		return lastMove;
	}


    /// <summary>
    /// シェイクのリセット
    /// 
    /// シェイクイベントが連続で発生しないように揺れをリセットする。
    /// </summary>
    public void ResetShake()
	{
		lastMove=0;
	}


    /// <summary>
    /// 加速度が更新された時に呼ばれる
    /// </summary>
    /// <param name="acceleration"></param>
    public void SetCurAccel( Vector3 acceleration )
	{
		dst_acceleration_x = acceleration.x ;
		dst_acceleration_y = acceleration.y ;
		dst_acceleration_z = acceleration.z ;

		//  以下はシェイクイベント用の処理
		float move =
			Fabs(dst_acceleration_x-last_dst_acceleration_x) +
			Fabs(dst_acceleration_y-last_dst_acceleration_y) +
			Fabs(dst_acceleration_z-last_dst_acceleration_z) ;
		lastMove = lastMove * 0.7f + move * 0.3f ;

		last_dst_acceleration_x = dst_acceleration_x ;
		last_dst_acceleration_y = dst_acceleration_y ;
		last_dst_acceleration_z = dst_acceleration_z ;
	}


	/// <summary>
    /// 更新処理
    /// </summary>
	public void Update(){
        // setCurAccelの間隔が長い場合は、最大値を小さくする必要がある
        const float MAX_ACCEL_D = 0.04f ;

		float dx = dst_acceleration_x - acceleration_x ;
		float dy = dst_acceleration_y - acceleration_y ;
		float dz = dst_acceleration_z - acceleration_z ;

		if( dx >  MAX_ACCEL_D ) dx =  MAX_ACCEL_D ;
		if( dx < -MAX_ACCEL_D ) dx = -MAX_ACCEL_D ;

		if( dy >  MAX_ACCEL_D ) dy =  MAX_ACCEL_D ;
		if( dy < -MAX_ACCEL_D ) dy = -MAX_ACCEL_D ;

		if( dz >  MAX_ACCEL_D ) dz =  MAX_ACCEL_D ;
		if( dz < -MAX_ACCEL_D ) dz = -MAX_ACCEL_D ;

		acceleration_x += dx ;
		acceleration_y += dy ;
		acceleration_z += dz ;

		long time = UtSystem.getUserTimeMSec() ;
		long diff = time - lastTimeMSec ;

		lastTimeMSec = time ;

        // 経過時間に応じて、重み付けをかえる
        float scale = 0.2f * diff * 60 / (1000.0f) ;	
		const float MAX_SCALE_VALUE = 0.5f ;
		if( scale > MAX_SCALE_VALUE ) scale = MAX_SCALE_VALUE ;

		accel[0] = (acceleration_x * scale) + (accel[0] * (1.0f - scale)) ;
		accel[1] = (acceleration_y * scale) + (accel[1] * (1.0f - scale)) ;
		accel[2] = (acceleration_z * scale) + (accel[2] * (1.0f - scale)) ;
    }


    /// <summary>
    /// 絶対値計算
    /// @param v
    /// @return
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private float Fabs(float v)
	{
		return v > 0 ? v : -v ;
	}


    /// <summary>
    /// 横方向回転を取得
    /// 寝かせた状態で0。(表裏関係なく)
    /// 左に回転させると-1,右に回転させると1になる。
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetAccelX() {
		return accel[0];
	}

    /// <summary>
    /// 上下回転取得
    /// 
    /// 寝かせた状態で0。(表裏関係なく)
    /// デバイスが垂直に立っているときに-1、逆さまにすると1になる。
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetAccelY() {
		return accel[1];
	}

    /// <summary>
    /// 上下の回転を取得。
    /// 立たせた状態で0。
    /// 表向きに寝かせると-1、裏向きに寝かせると1になる
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetAccelZ() 
	{
		return accel[2];
	}
}