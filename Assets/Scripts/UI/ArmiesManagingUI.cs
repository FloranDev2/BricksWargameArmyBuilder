using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Managers;
using UnityEngine;

namespace Truelch.UI
{
    public class ArmiesManagingUI : FadePanel
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private Transform _wrapper;
        [SerializeField] private ArmyElem _elemPrefab;

        //Hidden
        // - Managers
        private CanvasManager _canvasMgr;
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
            yield return new WaitUntil(() =>
                CanvasManager.Instance &&
                DataManager.Instance
            );

            _canvasMgr = CanvasManager.Instance;
            _dataMgr   = DataManager.Instance;

            _isReady = true;

            //Wait for DataManager to load
            yield return new WaitUntil(() => _dataMgr.IsReady);

            //Create army
            CreateArmyElems();
        }
        #endregion Initialization

        #region Delegate Event
        private void OnEnable()
        {
            DataManager.onArmyAdded += OnArmyAdded;
        }

        private void OnDisable()
        {
            DataManager.onArmyAdded -= OnArmyAdded;
        }

        private void OnArmyAdded(ArmyData armyData)
        {
            var armyElem = Instantiate(_elemPrefab, _wrapper);
            //armyElem.Init(this);
            armyElem.Init(this, armyData);
            _armyElems.Add(armyElem);
        }
        #endregion Delegate Event

        #region Elems
        void CreateArmyElems()
        {
            if (!_isReady) return;

            //Debug.Log("ArmiesManagingUI.CreateArmyElems()");

            foreach (var army in _dataMgr.Armies)
            {
                ArmyElem elem = Instantiate(_elemPrefab, _wrapper);
                elem.Init(this, army);
                if (!string.IsNullOrEmpty(army.Name))
                {
                    elem.ChangeName(army.Name);
                }
                _armyElems.Add(elem);
            }
        }

        [ContextMenu("Destroy Army Elems")]
        void DestroyArmyElems()
        {
            //Debug.Log("ArmiesManagingUI.DestroyArmyElems()");
            foreach (var elem in _armyElems)
            {
                Destroy(elem.gameObject);
            }
            _armyElems.Clear();
        }
        #endregion Elems

        #region Public
        public void OnNewArmyClicked()
        {
            if (!_isReady) return;
            _dataMgr.AddArmy();
        }

        public void OnEditArmyClicked(ArmyElem armyElem)
        {
            if (!_isReady) return;
            _canvasMgr.OnEditClick(armyElem);
        }

        //From ArmyElem
        public void OnArmyNameChanged(ArmyElem armyElem, string newName)
        {
            //Debug.Log("OnArmyNameChanged(armyElem: " + armyElem + ", newName: " + newName + ")");
            armyElem.ArmyData.Name = newName;
            //_dataMgr.
        }

        //From exterior
        public void RefreshMe()
        {
            DestroyArmyElems();
            CreateArmyElems();
        }

        public void Open()
        {
            RefreshMe(); //or just create if we destroy when we close?
            ShowInstantly();
        }

        public void Close()
        {
            DestroyArmyElems(); //useless?
            HideInstantly();
        }
        #endregion Public

        #endregion METHODS
    }
}