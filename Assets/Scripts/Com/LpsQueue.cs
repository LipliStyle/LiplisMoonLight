//====================================================================
//  ClassName : LpsQueue
//  概要      : リプリスキュー
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2017 sachin.
//====================================================================
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Com
{
    [Serializable]
    public class LpsQueue<T> : List<T>
    {
        /// <summary>
        /// 話題リストにエンキューする
        /// </summary>
        /// <param name="topic"></param>
        public void Enqueue(T topic)
        {
            this.Add(topic);
        }

        /// <summary>
        /// デキューする
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            //取り出すものが無ければNULLを返す
            if (this.Count < 1)
            {
                return default(T);
            }

            //先頭を取り出す
            T topic = this[0];

            //先頭を除去する
            this.RemoveAt(0);

            //話題を返す
            return topic;
        }

    }
}
