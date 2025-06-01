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
        public ArmyBuilderUI ArmyBuilderUI;
        public FeedbackUI FeedbackUI;

        [Space(15)]
        public DynamicScroller DynamicScroller;
        #endregion ATTRIBUTES


        #region METHODS

        #endregion METHODS
    }
}