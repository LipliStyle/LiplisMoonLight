//====================================================================
//  ClassName : CharacterData
//  概要      : キャラクターのベース情報
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.DataChar.CharacterTalk;
using Assets.Scripts.LiplisSystem.Msg;
using Assets.Scripts.LiplisSystem.Mst;
using Assets.Scripts.LiplisSystem.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DataChar
{
    public class CharacterData
    {
        //現在ロード中モデル定義名
        public string ModelName;

        //モデル定義名
        public string FrontModelName;
        public string RightModelName;
        public string LeftModelName;
        public List<string> ModelList;

        //現在ロード中モデル定義名
        public string NowModelName;
        //割り当てID
        public int AllocationId;
        //ウインドウキューリスト
        public Queue<LiplisWindow> WindowTalkListQ;
        //一つ前ウインドウ
        public LiplisWindow NowTalkWindow;
        //一つ前ウインドウ
        public string WindowName;
        //挨拶
        public CharDataGreet Greet;
        //口調
        public CharDataTone Tone;
        //配置位置
        public MST_CARACTER_POSITION Position;
        public float LocationY;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="FrontModelName"></param>
        /// <param name="AllocationId"></param>
        public CharacterData(string FrontModelName, string RightModelName, string LeftModelName, int AllocationId,CharDataGreet Greet, CharDataTone Tone, MST_CARACTER_POSITION DefaultPosition, string WindowName, float LocationY)
        {
            //モデル名取得
            this.FrontModelName           = FrontModelName;
            this.RightModelName           = RightModelName;
            this.LeftModelName            = LeftModelName;

            //モデルリストの初期化
            initModelList();

            //現在ロード中モデルにフロントを設定
            this.ModelName                = this.FrontModelName;
            this.NowModelName             = this.FrontModelName;

            //アロケーションID設定
            this.AllocationId             = AllocationId;

            //必要インスタンス設定
            this.Greet                    = Greet;
            this.Tone                     = Tone;
            this.Position                 = DefaultPosition;
            this.WindowName               = WindowName;
            this.LocationY                = LocationY;
            WindowTalkListQ               = new Queue<LiplisWindow>();

            //モデル表示の初期化
            InitVisible();
        }

        /// <summary>
        /// モデルリストの初期化
        /// </summary>
        private void initModelList()
        {
            this.ModelList = new List<string>();
            this.ModelList.Add(RightModelName);
            this.ModelList.Add(FrontModelName);
            this.ModelList.Add(LeftModelName);
        }

        //====================================================================
        //
        //                           モデル操作
        //                         
        //====================================================================
        #region モデル操作
        /// <summary>
        /// モデル表示初期化
        /// </summary>
        private void InitVisible()
        {
            //全モデルを一旦非表示にする
            SetModelOffAll();

            //現在ロード中のモデルのビジブルON
            LAppLive2DManager.Instance.SetFadeIn(this.NowModelName);
        }


        /// <summary>
        /// 現在ロード中のモデルのビジブルをONにする
        /// </summary>
        private void SetModelVisible()
        {
            //一旦すべてOFF
            SetModelVisibleAllOff();

            //現在ロード中のモデルのビジブルON
            LAppLive2DManager.Instance.SetFadeIn(this.NowModelName);
        }

        /// <summary>
        /// すべてのモデルをOFFにする
        /// </summary>
        private void SetModelVisibleAllOff()
        {
            SetModelVisibleAll(false);
        }

        /// <summary>
        /// すべてのモデルのビジブルをセットする
        /// </summary>
        /// <param name="visible"></param>
        private void SetModelVisibleAll(bool visible)
        {
            LAppLive2DManager.Instance.SetFadeOut(this.FrontModelName);
            LAppLive2DManager.Instance.SetFadeOut(this.RightModelName);
            LAppLive2DManager.Instance.SetFadeOut(this.LeftModelName);
        }
        private void SetModelOffAll()
        {
            LAppLive2DManager.Instance.SetVisible(this.FrontModelName,false);
            LAppLive2DManager.Instance.SetVisible(this.RightModelName, false);
            LAppLive2DManager.Instance.SetVisible(this.LeftModelName, false);
        }

        /// <summary>
        /// 向きを変更する
        /// </summary>
        public void ChengeDirection(MODELE_DIRECTION Direction)
        {
            //選択モデル
            string selectModelName = "";

            if(Direction == MODELE_DIRECTION.RIGNT)
            {
                selectModelName = this.RightModelName;
            }
            else if(Direction == MODELE_DIRECTION.LEFT)
            {
                selectModelName = this.LeftModelName;
            }
            else
            {
                selectModelName = this.FrontModelName;
            }

            //選択名がからなら、フロントをセットする
            if(selectModelName == "")
            {
                selectModelName = this.FrontModelName;
            }

            //選択モデルをセットする
            this.NowModelName = selectModelName;

            //モデルビジブル再設定
            SetModelVisible();
        }

        /// <summary>
        /// ランダムに向きを変更する
        /// </summary>
        public void ChengeDirectionRandam()
        {
            //選択モデル
            string selectModelName = "";

            //方向インデックスを取得する
            int idx　= GetDirectionIdx();
            
            //モデル名変更
            selectModelName = ModelList[idx];

            //選択名がからなら、フロントをセットする
            if (selectModelName == "")
            {
                selectModelName = this.FrontModelName;
            }

            //選択モデルをセットする
            this.NowModelName = selectModelName;

            //モデルビジブル再設定
            SetModelVisible();
        }

        /// <summary>
        /// 方向インデックスを取得する
        /// </summary>
        /// <returns></returns>
        public int GetDirectionIdx()
        {
            System.Random r = new System.Random();

            if (Position == MST_CARACTER_POSITION.Moderator)
            {
                return r.Next(2);
            }
            else if (Position == MST_CARACTER_POSITION.Left)
            {
                return r.Next(1, 3);
            }
            else
            {
                return r.Next(3);
            }
        }


        /// <summary>
        /// 対象の位置に移動する
        /// </summary>
        public void MoveTarget()
        {
            //一旦フェードアウト
            SetModelVisibleAllOff();


            //移動する
            LAppLive2DManager.Instance.SetMove(this.FrontModelName, MODEL_POS.GetPos(Position, LocationY));
            LAppLive2DManager.Instance.SetMove(this.RightModelName, MODEL_POS.GetPos(Position, LocationY));
            LAppLive2DManager.Instance.SetMove(this.LeftModelName, MODEL_POS.GetPos(Position, LocationY));

            //現在ロード中のモデルのビジブルON
            LAppLive2DManager.Instance.SetFadeIn(this.NowModelName,0.01f);
        }

        #endregion
        //====================================================================
        //
        //                           ウインドウ操作
        //                         
        //====================================================================
        #region ウインドウ操作

        /// <summary>
        /// 横幅計算
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        const float MAX_WIDTH = 140;
        private float CulcWindowWidth(string message)
        {
            float width = (float)message.Length * 10.0f;

            if (width >= MAX_WIDTH)
            {
                return MAX_WIDTH;
            }
            else
            {
                return width;
            }
        }


        //private const float HEIGHT_IMG_3 = 126;
        //private const float HEIGHT_TXT_3 = 46;
        //private const float POS_Y_TXT_3 = -5.5f;

        //private const float HEIGHT_IMG_2 = 102;
        //private const float HEIGHT_TXT_2 = 33;
        //private const float POS_Y_TXT_2 = -7.0f;

        //private const float HEIGHT_IMG_1 = 70;
        //private const float HEIGHT_TXT_1 = 20;
        //private const float POS_Y_TXT_1 = -4.5f;

        private const float HEIGHT_IMG_3 = 116;
        private const float HEIGHT_TXT_3 = 46;
        private const float POS_Y_TXT_3 = -8.0f;

        private const float HEIGHT_IMG_2 = 98;
        private const float HEIGHT_TXT_2 = 32;
        private const float POS_Y_TXT_2 = -7.0f;

        private const float HEIGHT_IMG_1 = 60;
        private const float HEIGHT_TXT_1 = 16;
        private const float POS_Y_TXT_1 = -5.5f;


        /// <summary>
        /// ウインドウを作成する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public LiplisWindow CreateWindowTalk(string message,  MsgWindowInfo windowInfo)
        {
            try
            {
                //サイズ計算
                float width = CulcWindowWidth(message);

                double div = Math.Ceiling(message.Length / 17.0);

                float heightImg = HEIGHT_IMG_1;
                float heightText = HEIGHT_TXT_1;
                float posTextY = POS_Y_TXT_1;

                if (div >= 3)
                {
                    heightImg = HEIGHT_IMG_3;
                    heightText = HEIGHT_TXT_3;
                    posTextY = POS_Y_TXT_3;
                }
                else if (div == 2)
                {
                    heightImg = HEIGHT_IMG_2;
                    heightText = HEIGHT_TXT_2;
                    posTextY = POS_Y_TXT_2;
                }
                else
                {
                    heightImg = HEIGHT_IMG_1;
                    heightText = HEIGHT_TXT_1;
                    posTextY = POS_Y_TXT_1;
                }

                //ウインドウ名設定
                windowInfo.window.name = "TalkWindow" + WindowTalkListQ.Count;

                //位置設定
                windowInfo.window.transform.position = WINDOW_POS.GetPos(Position);

                //サイズ変更
                RectTransform windowRect = windowInfo.window.GetComponent<RectTransform>();
                windowRect.sizeDelta = new Vector2(width, heightImg);

                //テキスト　サイズ、位置調整
                Vector3 txtPos = windowInfo.windowText.transform.position;
                windowInfo.windowText.transform.position = new Vector3(txtPos.x, txtPos.y + posTextY, txtPos.z);
                RectTransform textRect = windowInfo.windowText.GetComponent<RectTransform>();
                textRect.sizeDelta = new Vector2(width, (float)(heightText));

                //スケール設定
                windowInfo.window.transform.localScale = new Vector3(1, 1, 1);

                //ウインドウインスタンス取得
                TalkWindow imgWindow = windowInfo.window.GetComponent<TalkWindow>();

                //モデル設定
                imgWindow.TargetModelName = NowModelName;

                //テキスト設定
                imgWindow.SetNextLine(message);

                //親キャンバスに登録
                windowInfo.window.transform.SetParent(windowInfo.canvasParent, false);

                //結果を返す
                return new LiplisWindow(windowInfo.window, heightImg, heightText, posTextY);
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// ウインドウをセットする
        /// </summary>
        /// <param name="window"></param>
        /// <param name="AllocationId"></param>
        public void SetWindow(LiplisWindow window, int AllocationId)
        {
            //ターゲットモデルセット
            window.imgWindow.TargetModelName = NowModelName;

            //ウインドウを追加する
            AddWindow(window);

            //ウインドウ
            DestroyWindow();
        }

        /// <summary>
        /// ウインドウを追加する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="message"></param>
        private void AddWindow(LiplisWindow window)
        {
            //1個以上ならスライドする
            if (WindowTalkListQ.Count >= 1)
            {
                //一つ前のウインドウの高さを取得する
                float prvHeight = this.NowTalkWindow.heightImg;

                //ウインドウ移動量設定
                SlideWindow(window.heightImg, prvHeight);
            }

            //キューに追加
            this.WindowTalkListQ.Enqueue(window);

            //現在おしゃべりウインドウ設置
            this.NowTalkWindow = window;
        }

        /// <summary>
        /// ウインドをスライドする
        /// </summary>
        private const float WINDOW_MOVE_VAL = 50;
        private void SlideWindow(float heightImg, float prvHeight)
        {
            //ウインドウが無ければ動かさない
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }

            //移動量算出
            float moveVal = GetMoveValue(heightImg, prvHeight);

            //回してスライド
            foreach (var WindowData in WindowTalkListQ)
            {
                if (!WindowData.imgWindow.flgEnd)
                {
                    //移動目標設定
                    WindowData.SetMoveTarget(new Vector3(WindowData.imgWindow.ParentWindow.transform.position.x, WindowData.imgWindow.ParentWindow.transform.position.y + moveVal, WindowData.imgWindow.ParentWindow.transform.position.z));
                }
            }
        }

        /// <summary>
        /// ウインドウ移動量を計算する
        /// </summary>
        /// <param name="heightImg"></param>
        /// <param name="prvHeight"></param>
        /// <returns></returns>
        private float GetMoveValue(float heightImg, float prvHeight)
        {
            float moveVal = WINDOW_MOVE_VAL;

            //今回動かす先頭ウインドウのサイズをチェック
            if (heightImg == HEIGHT_IMG_1 && prvHeight == HEIGHT_IMG_1)
            {
                moveVal = 10.0f * CulcWidthRate();
            }
            else if (heightImg == HEIGHT_IMG_2 && prvHeight == HEIGHT_IMG_2)
            {
                moveVal = 14.0f * CulcWidthRate();
            }
            else if (heightImg == HEIGHT_IMG_3 && prvHeight == HEIGHT_IMG_3)
            {
                moveVal = 20.0f * CulcWidthRate();
            }

            else if (heightImg == HEIGHT_IMG_2 && prvHeight == HEIGHT_IMG_1)
            {
                moveVal = 10.0f * CulcWidthRate();
            }
            else if (heightImg == HEIGHT_IMG_3 && prvHeight == HEIGHT_IMG_1)
            {
                moveVal = 14.0f * CulcWidthRate();
            }

            else if (heightImg == HEIGHT_IMG_1 && prvHeight == HEIGHT_IMG_2)
            {
                moveVal = 14.0f * CulcWidthRate();
            }
            else if (heightImg == HEIGHT_IMG_3 && prvHeight == HEIGHT_IMG_2)
            {
                moveVal = 18.5f * CulcWidthRate();
            }

            else if (heightImg == HEIGHT_IMG_1 && prvHeight == HEIGHT_IMG_3)
            {
                moveVal = 18.5f * CulcWidthRate();
            }
            else if (heightImg == HEIGHT_IMG_2 && prvHeight == HEIGHT_IMG_3)
            {
                moveVal = 18.5f * CulcWidthRate();
            }


            return moveVal;
        }

        /// <summary>
        /// 幅方向比率を取得する
        /// </summary>
        /// <returns></returns>
        private float CulcWidthRate()
        {
            return Screen.width / 380.0f;
        }


        /// <summary>
        /// 先頭ウインドウをサクジョする
        /// </summary>
        private void DestroyWindow()
        {
            Queue<LiplisWindow> bufQ = new Queue<LiplisWindow>();

            //カウントチェック
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }
            else
            {
                //まわして、範囲内のものはバッファキューに、範囲外のものは削除
                while (WindowTalkListQ.Count > 0)
                {
                    LiplisWindow window = WindowTalkListQ.Dequeue();

                    if (WindowTalkListQ.Count > 2)
                    {
                        window.CloseWindow();
                    }
                    else
                    {
                        //1個取り出し、削除
                        bufQ.Enqueue(window);
                    }
                }

                //すげかえる
                WindowTalkListQ = bufQ;
            }
        }

        /// <summary>
        /// すべてのウインドウを除去する
        /// </summary>
        public void DestroyAllWindow()
        {
            //空なら何もしない
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }

            //ウインドウ数が1以上なら、デキューして除去
            while (WindowTalkListQ.Count > 0)
            {
                //1個取り出し、削除
                WindowTalkListQ.Dequeue().CloseWindow();
            }
        }

        public void WindowMaintenance(int windowLifeSpanTime)
        {
            //空の場合何もしない
            if (WindowTalkListQ.Count < 1)
            {
                return;
            }

            //一定時間経過したウインドウは自動的に除去する
            if (WindowTalkListQ.Peek().CreateTime.AddSeconds(windowLifeSpanTime) < DateTime.Now)
            {
                WindowTalkListQ.Dequeue().CloseWindow();
            }


            //LiplisStatus.Instance.CharDataList.DestroyAllWindow();
        }

        /// <summary>
        /// 自身のウインドウ名を返す
        /// </summary>
        /// <returns></returns>
        public string GetWindowName()
        {
            return this.WindowName;
        }



        #endregion

    }
}
