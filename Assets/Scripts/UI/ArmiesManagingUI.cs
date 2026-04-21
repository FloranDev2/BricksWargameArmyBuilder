using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Managers;
using UnityEngine;

namespace Truelch.UI
{
    public class ArmiesManagingUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private Transform _wrapper;
        [SerializeField] private ArmyElem _elemPrefab;

        //Hidden
        // - Managers
        private DataManager _dataMgr;

        // - Misc
        private bool _isReady = false;

        // - Army Elems
        [SerializeField] private List<ArmyElem> _armyElems = new List<ArmyElem>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            yield return new WaitUntil(() => DataManager.Instance);
            _dataMgr = DataManager.Instance;
            _isReady = true;

            //Load data

            CreateArmyElems();
        }

        void CreateArmyElems()
        {
            if (!_isReady) return;

            foreach (var army in _dataMgr.ArmyUnits)
            {
                var elem = Instantiate(_elemPrefab, _wrapper);
                //elem.Init();
            }
        }

        void DestroyArmyElems()
        {
            foreach (var elem in _armyElems)
            {
                Destroy(elem);
            }
            _armyElems.Clear();
        }
        #endregion Initialization

        #region Public
        public void OnNewArmyClicked()
        {
            if (!_isReady) return;

            ArmyData army = new ArmyData();
        }
        #endregion Public

        #endregion METHODS
    }
}