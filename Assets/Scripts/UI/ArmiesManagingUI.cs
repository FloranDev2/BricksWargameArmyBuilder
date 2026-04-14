using System.Collections;
using System.Collections.Generic;
using Truelch.Managers;
using UnityEngine;

namespace Truelch.UI
{
    public class ArmiesManagingUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private Transform _wrapper;

        //Hidden
        // - Managers
        private DataManager _dataMgr;

        // - Misc
        private bool _isReady = false;

        // - Units
        //[SerializeField] private List<UnitElem> _unitElems = new List<UnitElem>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            yield return new WaitUntil(() => DataManager.Instance);
            _dataMgr = DataManager.Instance;
            _isReady = true;
        }
        #endregion Initialization

        #region Public
        public void OnNewArmyClicked()
        {
            if (!_isReady) return;


        }
        #endregion Public

        #endregion METHODS
    }
}