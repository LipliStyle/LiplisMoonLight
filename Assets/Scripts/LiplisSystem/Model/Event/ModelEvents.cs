//====================================================================
//  ClassName : ModelEvents
//  概要      : モデル用イベント定義
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================

namespace Assets.Scripts.LiplisSystem.Model.Event
{
    public class ModelEvents
    {

        /// <summary>
        /// おしゃべりをスキップするトリガーが発火したときのイベント
        /// </summary>
        public delegate void OnNextTalkOrSkip();
    }
}
