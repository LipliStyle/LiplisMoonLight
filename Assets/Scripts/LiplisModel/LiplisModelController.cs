//====================================================================
//  ClassName : LiplisModelController
//  概要      : Liplisモデルコントローラー
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2018 sachin. All Rights Reserved. 
//====================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.LiplisModel
{
    public class LiplisModelController
    {
        //=============================
        //キャラクターデータ管理
        public Dictionary<string, IfsLiplisModel> TableModel { get; set; }          //モデルテーブル 名前のデータベース
        public Dictionary<int, IfsLiplisModel> TableModelId { get; set; }           //モデルテーブル IDのデータベース
        public List<IfsLiplisModel> ModelList { get; set; }                         //


    }
}
