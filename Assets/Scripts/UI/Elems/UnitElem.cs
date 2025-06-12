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
        /*[NonSerialized]*/ public UnitData UnitData;

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
        }
        #endregion Init

        #region Misc
        IEnumerator CR_RefreshClass()
        {
            if (_gameMgr == null) yield return new WaitUntil(() => _gameMgr != null);

            //Name
            string name = "Unit";
            Language language = _gameMgr.GetCurrentLanguage();
            foreach (var locName in UnitData.LocNames)
            {
                if (locName.Language == language)
                {
                    name = locName.Txt;
                    break;
                }
            }
            _placeHolderTxt.text = string.IsNullOrEmpty(UnitData.CurrentName) ? name : UnitData.CurrentName;

            //Icon
            _classIconImg.sprite = UnitData.Icon;
            _bgColorImg.color = UnitData.Color;

            //Gears! (new!)
            // - TODO: check if we can keep previous upgrades?

            // - Destroy previous ?
            foreach (GearExpBtn gearBtn in _gearElems)
            {
                Destroy(gearBtn.gameObject);
            }

            // - Create new
            for (int i = 0; i < UnitData.MaxGear; i++)
            {
                GearExpBtn gearBtn = Instantiate(_gearElemPrefab, _gearWrapper);
                gearBtn.Init(this, i);
                _gearElems.Add(gearBtn);
            }

            // - Update Data
            UnitData.GearList.Clear();
            for (int i = 0; i < UnitData.MaxGear; i++)
            {
                UnitData.GearList.Add(null);
            }
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
            if (_gameMgr.ArmyUnits.Contains(UnitData))
            {
                int index = _gameMgr.ArmyUnits.IndexOf(UnitData);
                string name = UnitData.CurrentName;
                UnitData = unitSO.Data.GetClone();
                if (!string.IsNullOrEmpty(name))
                {
                    UnitData.CurrentName = name;
                }
                _gameMgr.ArmyUnits[index] = UnitData;
                StartCoroutine(CR_RefreshClass());
            }
            else
            {
                Debug.Log("WTF");
            }
        }

        public void OnShowInfosClick()
        {

        }

        public void OnDuplicateClick()
        {
            UnitData cloneData = UnitData.GetClone();
            _gameMgr.AddUnit(cloneData);
        }

        public void OnDeleteClick()
        {
            _armyBuilderUI.OnRemoveUnitClick(this);
        }

        // --- Gear ---
        public void OnGearChanged(int gearIndex, GearSO newGear)
        {
            GearData clonedGear = newGear.Data.GetClone();
            UnitData.GearList[gearIndex] = clonedGear;
        }

        //public void OnDestroyGear(GearExpBtn gear)
        //{

        //}

        // --- UI Events ---
        public void OnEndEdit(string name)
        {
            UnitData.CurrentName = name;
        }
        #endregion Public

        #endregion METHODS
    }
}