using System.Collections;
using System.Collections.Generic;
using Truelch.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    public class ArmyBuilderUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Hidden
        // - Managers
        private GameManager _gameMgr;

        // - Misc
        private bool _isReady = false;

        // - Units
        private List<UnitExpBtn> _unitElems = new List<UnitExpBtn>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance);
            _gameMgr = GameManager.Instance;
            _isReady = true;
        }
        #endregion Initialization

        #region Public
        //Options
        public void OnSaveClick()
        {
            if (!_isReady) return;
            _gameMgr.OnSaveClick();
        }

        public void OnPrintExportClick()
        {
            if (!_isReady) return;
            //TODO
        }
        #endregion Public

        #endregion METHODS
    }
}