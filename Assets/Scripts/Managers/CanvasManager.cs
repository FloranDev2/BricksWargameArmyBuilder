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
        // - Managers
        private DataManager _dataMgr;

        // - Misc
        private bool _isReady = false;
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        IEnumerator Start()
        {
            //Done by the Canvas Scaler
            Scale = transform.localScale.x;

            yield return new WaitUntil(() => DataManager.Instance);

            _dataMgr = DataManager.Instance;

            _isReady = true;
        }
        #endregion Init

        #region Public
        //ArmiesManagingUI -> ArmyBuilderUI
        public void OnEditClick(ArmyElem armyElem)
        {
            if (!_isReady) return;

            _dataMgr.CurrArmy = armyElem.ArmyData;
            //Debug.Log("_dataMgr.CurrArmy: " + _dataMgr.CurrArmy.Name);

            ArmiesManagingUI.Close();
            ArmyBuilderUI.Open();
        }

        public void OnReturnToArmiesManagementClicked()
        {
            ArmyBuilderUI.Close();
            ArmiesManagingUI.Open();            
        }

        //Save ALL the data, not only the army currently being edited
        public void OnSaveClick()
        {
            if (!_isReady) return;
            _dataMgr.OnSaveClick();
        }
        #endregion Public

        #endregion METHODS
    }
}