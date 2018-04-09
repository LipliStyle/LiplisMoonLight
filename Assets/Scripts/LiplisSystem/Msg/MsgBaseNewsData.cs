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
    }
}