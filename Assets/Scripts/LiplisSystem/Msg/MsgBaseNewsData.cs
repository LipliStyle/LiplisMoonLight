//=======================================================================
//  ClassName : MsgBaseNewsData
//  概要      : ニュースのベースメッセージ
//
//  SatelliteServer
//  Copyright(c) 2009-2017 sachin. All Rights Reserved. 
//=======================================================================

using System;

namespace Assets.Scripts.LiplisSystem.Msg
{
    [Serializable]
    public class MsgBaseNewsData
    {
        ///=============================
        /// ベース情報
        public uint CREATE_TIME;         //ニュースソース作成時刻
        public string DATA_TYPE;         //データタイプ ニュース、ツイートなど
        public int RANK;                 //ランキング
        public string DATAKEY;           //データキー   対象テーブルでのキー
        public string TITLE;             //ニュースタイトル
        public string URL;               //ニュースURL
        public string THUMBNAIL_URL;     //サムネURL
        public int THUMBNAIL_HEIGHT;     //サムネ高さ
        public int THUMBNAIL_WIDTH;      //サムネ幅
        public int THUMBNAIL_SIZE;       //サムネサイズ
        public byte[] THUMBNAIL_DATA;    //サムネデータ
    }
}