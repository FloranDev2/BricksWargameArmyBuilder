using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    public class UnitElem : MonoBehaviour
    {
        #region ATTRIBUTES
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
        //[SerializeField] private GearExpBtn _addGearExpBtnPrefab; //"Add a gear" button
        [SerializeField] private GearExpBtn _gearElemPrefab; //unless the gear elem prefab IS the placeholder??

        //Hidden
        private ArmyBuilderUI _armyBuilderUI;
        private List<GearExpBtn> _gearElems = new List<GearExpBtn>();
        private UnitData _unitData;
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public void Init(ArmyBuilderUI ui, UnitSO unitSO)
        {
            _armyBuilderUI = ui;
            _unitData = unitSO.Data;
            //RefreshClass(unitSO);
        }
        #endregion Public

        #endregion METHODS
    }
}