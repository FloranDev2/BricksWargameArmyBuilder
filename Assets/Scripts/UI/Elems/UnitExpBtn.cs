using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    /// <summary>
    /// This actually needs to be an ExpandButton, so we can choose the class.
    /// Unless I'm doing a script for a children component.
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
        [SerializeField] private Image _classIcon; //Commando, Medic, etc.
        [SerializeField] private TextMeshProUGUI _placeHolderTxt;
        [SerializeField] private TextMeshProUGUI _finalTxt; //I may not use it

        // - Equipment
        [Header("Gear")]
        [SerializeField] private Transform _gearWrapper; //Call it gear instead?
        //[SerializeField] private GearElem _gearPlaceholder;
        [SerializeField] private GearExpBtn _gearElemPrefab; //unless the gear elem prefab IS the placeholder??

        // - Bottom
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public void Init(string name)
        {
            _placeHolderTxt.text = name;
        }

        public override void OnElemClick(int index)
        {
            //Look for new class

        }

        public void OnEndEdit(string name)
        {
            _gameMgr.ChangeUnitName(Index, name);
        }

        //This will be for each individual button option
        /*
        //Change the gear. Example: change from armor to heal.
        public void OnEditOptionClick(int slot)
        {

        }

        //Click on the "?" to the right, opening a big pop up filled with text.
        public void OnShowOptionClick()
        {

        }
        */

        public void OnDeleteClick()
        {

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
        #endregion Public

        #endregion METHODS
    }
}