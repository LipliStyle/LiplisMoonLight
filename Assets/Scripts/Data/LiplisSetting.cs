//====================================================================
//  ClassName : LiplisSetting
//  概要      : 設定情報
//              
//
//  LiplisMoonlight
//  Copyright(c) 2017-2018 sachin.
//====================================================================

using Assets.Scripts.Data.SubData;

namespace Assets.Scripts.Data
{
    public class LiplisSetting
    {
        //====================================================================
        //
        //                          シングルトン管理
        //                         
        //====================================================================
        #region シングルトン管理

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static LiplisSetting instance;
        public static LiplisSetting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LiplisSetting();
                }

                return instance;
            }
        }

        /// <summary>
        /// インスタンスをセットする
        /// </summary>
        /// <param name="Instance"></param>
        public static void SetInstance(LiplisSetting Instance)
        {
            //インスタンスセット
            instance = Instance;

            //リストが初期化されない可能性があるので、データロードを呼んでおく。
            instance.DataLoad();
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public LiplisSetting()
        {
            DataLoad();
        }

        /// <summary>
        /// データロードする
        /// </summary>
        public void DataLoad()
        {
            if (this.Setting == null) { this.Setting = new DatSetting(); }
        }



        #endregion

        //環境設定
        public DatSetting Setting;


    }
}
