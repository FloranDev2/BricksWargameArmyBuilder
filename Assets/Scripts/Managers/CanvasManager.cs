using System;
using System.Collections;
using System.Collections.Generic;
using Truelch.UI;
using UnityEngine;

namespace Truelch.Managers
{
    public class CanvasManager : Singleton<CanvasManager>
    {
        #region ATTRIBUTES
        //Public
        // - UIs
        [Header("UIs")]
        public ArmiesManagingUI ArmiesManagingUI;
        public ArmyBuilderUI ArmyBuilderUI;

        [Space(15)]
        public FeedbackUI FeedbackUI;

        [Space(15)]
        public DynamicScroller DynamicScroller;

        [NonSerialized] public float Scale = 1f;

        //Hidden
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        private void Start()
        {
            //Done by the Canvas Scaler
            Scale = transform.localScale.x;
        }
        #endregion Init

        #endregion METHODS
    }
}