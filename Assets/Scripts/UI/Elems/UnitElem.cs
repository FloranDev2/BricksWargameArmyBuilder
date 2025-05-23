using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    public class UnitElem : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        // - Top
        [SerializeField] private Image _classIcon; //Commando, Medic, etc.
        [SerializeField] private TextMeshProUGUI _className;

        // - Equipment
        [SerializeField] private Transform _equipmentWrapper; //Call it gear instead?

        // - Bottom
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public void OnChangeTypeClick()
        {

        }

        //Change the gear. Example: change from armor to heal.
        public void OnEditOptionClick(int slot)
        {

        }

        //Click on the "?" to the right, opening a big pop up filled with text.
        public void OnShowOptionClick()
        {

        }

        public void OnDeleteClick()
        {

        }

        public void OnDuplicateClick()
        {

        }
        #endregion Public

        #endregion METHODS
    }
}