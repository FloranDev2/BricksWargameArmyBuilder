using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Localization;
using Truelch.Managers;
using Truelch.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    public class UnitElem : MonoBehaviour
    {
        #region ATTRIBUTES
        //Public
        [NonSerialized] public UnitData UnitData;

        //Inspector
        // - Top
        [Header("UI Refs")]
        [SerializeField] private Image _bgColorImg;
        [SerializeField] private Image _classIconImg; //Commando, Medic, etc.
        [SerializeField] private TextMeshProUGUI _placeHolderTxt;
        [SerializeField] private TextMeshProUGUI _finalTxt; //I may not use it

        // - Equipment
        [Header("Gear")]
        [SerializeField] private Transform _gearWrapper; //Call it gear instead?
        [SerializeField] private GearExpBtn _gearElemPrefab; //unless the gear elem prefab IS the placeholder??

        //Hidden
        // - Managers
        private GameManager _gameMgr; //only useful for tests?

        // - Misc
        private ArmyBuilderUI _armyBuilderUI;
        private List<GearExpBtn> _gearElems = new List<GearExpBtn>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance != null);
            _gameMgr = GameManager.Instance;
            //CM_AssignUnitData();
        }
        #endregion Init

        #region Context Menus
        //TMP!
        //[ContextMenu("Assign Unit Data")]
        //private void CM_AssignUnitData()
        //{
        //    if (_gameMgr == null)
        //    {
        //        Debug.Log("CM_AssignUnitData -> early return");
        //        return;
        //    }

        //    _gameMgr.ArmyUnits.Add(_gameMgr.UnitSOs[0].Data);
        //    UnitData = _gameMgr.ArmyUnits[0];

        //    Debug.Log("CM_AssignUnitData -> data assigned!");
        //}
        #endregion Context Menus

        #region Misc
        IEnumerator CR_RefreshClass()
        {
            yield return new WaitUntil(() => _gameMgr != null);

            //Name
            string name = "Unit";
            Language language = _gameMgr.GetCurrentLanguage();
            foreach (var foo in UnitData.LocNames)
            {
                if (foo.Language == language)
                {
                    name = foo.Txt;
                    break;
                }
            }
            _placeHolderTxt.text = name;

            //Icon
            _classIconImg.sprite = UnitData.Icon;
            _bgColorImg.color = UnitData.Color;
        }
        #endregion Misc

        #region Public
        public void Init(ArmyBuilderUI ui, UnitData unitData)
        {
            _armyBuilderUI = ui;

            UnitData = unitData; //it was already cloned, it's not the source itself
            StartCoroutine(CR_RefreshClass());
        }

        public void OnChangeClassClick(UnitSO unitSO)
        {
            UnitData = unitSO.Data;
            StartCoroutine(CR_RefreshClass());
        }

        public void OnShowInfosClick()
        {

        }

        public void OnDuplicateClick()
        {
            _gameMgr.AddUnit(UnitData);
        }

        public void OnDeleteClick()
        {
            _armyBuilderUI.OnRemoveUnitClick(this);
        }

        // --- UI Events ---
        public void OnEndEdit(string name)
        {
            Debug.Log("OnEndEdit(name: " + name + ")");
            //_gameMgr.ChangeUnitName(Index, name);
            UnitData.CurrentName = name;
        }
        #endregion Public

        #endregion METHODS
    }
}