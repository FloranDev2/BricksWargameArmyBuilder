using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Localization;
using Truelch.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    /// <summary>
    /// WILL BE REMOVED.
    /// LOGIC WILL BE SPLIT BETWEEN:
    /// - UNIT ELEM (PARENT)
    /// - CHANGE UNIT CLASS
    /// 
    /// This actually needs to be an ExpandButton, so we can choose the class.
    /// Unless I'm doing a script for a children component.
    /// 
    /// NEW
    /// Depending on the rows it needs, it'll self-expand.
    /// For example, Megafigs need another row for their type (Ground, Flyer, etc.)
    /// 
    /// Oooor, it's just an hidden row that is enabled when needed.
    /// So I can switch type between megafig and minifig.
    /// </summary>
    public class UnitExpBtn : ExpandButtonBase
    {
        #region ATTRIBUTES
        //Public
        [Header("Public")]
        public int Index;

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
        [SerializeField] private GearExpBtn _addGearExpBtnPrefab; //"Add a gear" button
        [SerializeField] private GearExpBtn _gearElemPrefab; //unless the gear elem prefab IS the placeholder??

        // - Bottom

        //Hidden
        private ArmyBuilderUI _armyBuilderUI;
        private List<GearExpBtn> _gearElems = new List<GearExpBtn>();

        private UnitData _unitData;
        #endregion ATTRIBUTES


        #region METHODS

        #region Misc
        //We do that when we change class?
        //But we might risk to lose all the gears while just changing from Commando to Medic (for example)
        private void RefreshGearButtons(UnitData data)
        {
            //Logic 1: Destroy existing gear, then create new ones (placeholders)

            //L1A Destroy existing gear
            foreach (var gear in _gearElems)
            {
                Destroy(gear.gameObject);
            }
            _gearElems.Clear();

            //L1B Create new gear buttons
            //Or, instead of GearList.Count, use a new variable like "MaxGear".
            for (int i = 0; i < data.GearList.Count; i++)
            {
                var gearElem = Instantiate(_gearElemPrefab, _gearWrapper); //is a placeholder FOR NOW
                _gearElems.Add(gearElem);
            }

            //Logic 2: See if we need more or less gear buttons than we already have
            //Assuming that gear exp button have a placeholder mode, but it's still the same prefab
            //L2A: Create if needed
        }

        IEnumerator CR_RefreshClass(UnitSO unitSO)
        {
            yield return new WaitUntil(() => _isReady == true);

            //Name
            string name = "Unit";
            Language language = _gameMgr.GetCurrentLanguage();
            foreach (var foo in unitSO.Data.LocNames)
            {
                if (foo.Language == language)
                {
                    name = foo.Txt;
                    break;
                }
            }
            _placeHolderTxt.text = name;

            //Icon
            _classIconImg.sprite = unitSO.Data.Icon;
            _bgColorImg.color = unitSO.Data.Color;
        }
        #endregion Misc

        #region Public
        public void Init(ArmyBuilderUI ui, UnitSO unitSO)
        {
            _armyBuilderUI = ui;
            _unitData = unitSO.Data;
            RefreshClass(unitSO);
        }

        public void RefreshClass(UnitSO unitSO)
        {
            StartCoroutine(CR_RefreshClass(unitSO));
        }

        // --- INHERITED FROM EXPAND BUTTON BASE ---
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            Language language = _gameMgr.GetCurrentLanguage();

            int index = 0;

            for (int i = 0; i < _gameMgr.UnitSOs.Count; i++)
            {
                UnitData data = _gameMgr.UnitSOs[i].Data;

                //TODO: concatenate this function, somewhere...
                string name = "Unit";
                foreach (var d in data.LocNames)
                {
                    if (d.Language == language)
                    {
                        name = d.Txt;
                        break;
                    }
                }
                _canvasMgr.DynamicScroller.CreateElem(index, name);
                index++;
            }
        }

        // --- BUTTONS CALLBACKS ----
        public override void OnElemClick(int index)
        {
            //Look for new class

        }

        public void OnDeleteClick()
        {
            //_armyBuilderUI.OnRemoveUnitClick(this);
        }

        public void OnDuplicateClick()
        {

        }

        //From Gear Elem

        /// <summary>
        /// Only if I do an add / remove gear logic, but maybe I can just place as many placeholders as there are options available
        /// </summary>
        public void OnDeleteGearClick()
        {

        }



        // --- UI Events ---
        public void OnEndEdit(string name)
        {
            //_gameMgr.ChangeUnitName(Index, name);
        }
        #endregion Public

        #endregion METHODS
    }
}