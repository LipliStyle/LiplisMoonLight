/*
 * Copyright(c) Live2D Inc. All rights reserved.
 * 
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at http://live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;


namespace Live2D.Cubism.Framework.MouthMovement
{
    /// <summary>
    /// Automatic mouth movement.
    /// </summary>
    public sealed class CubismAutoMouthInput : MonoBehaviour
    {
        /// <summary>
        /// Timescale.
        /// </summary>
        [SerializeField]
        public float Timescale = 10f;


        /// <summary>
        /// Target controller.
        /// </summary>
        private CubismMouthController Controller { get; set; }


        /// <summary>
        /// Current time.
        /// </summary>
        private float T { get; set; }


        /// <summary>
        /// Resets the input.
        /// </summary>
        public void Reset()
        {
            T = 0f;
        }

        #region Unity Event Handling

        /// <summary>
        /// Called by Unity. Initializes input.
        /// </summary>
        private void Start()
        {
            Controller = GetComponent<CubismMouthController>();
        }


        /// <summary>
        /// Called by Unity. Updates controller.
        /// </summary>
        /// <remarks>
        /// Make sure this method is called after any animations are evaluated.
        /// </remarks>
        private void LateUpdate()
        {
            // Fail silently.
            if (Controller == null)
            {
                return;
            }

            //TimeScale0に設定されたら、口パクストップと判断
            if(Timescale == 0)
            {
                return;
            }

            // Progress time.
            T += (Time.deltaTime * Timescale);


            // Evaluate.
            Controller.MouthOpening = Mathf.Abs(Mathf.Sin(T));
        }

        #endregion

        /// <summary>
        /// リップシンクをONにする
        /// </summary>
        public void LipSyncOn()
        {
            Timescale = 10;
        }

        /// <summary>
        /// リップシンクをOFFにする
        /// </summary>
        public void LipSyncOff()
        {
            Timescale  = 0;
            this.Controller.MouthOpening = 0;
            this.Controller.Refresh();
        }
    }
}
