using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using UnityEngine;

namespace Truelch.UI
{
    public class ArmyElem : MonoBehaviour
    {
        #region ATTRIBUTES
        //Public
        public ArmyData ArmyData;

        //Inspector
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TextMeshProUGUI _armyFinalName; //as opposed to the temporary field "Armee sans nom"

        //Hidden
        private ArmiesManagingUI _ui;
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public void Init(ArmiesManagingUI ui, ArmyData armyData)
        {
            _ui = ui;
            ArmyData = armyData;

            if (!string.IsNullOrEmpty(armyData.Name))
            {
                ChangeName(armyData.Name);
            }
        }

        public void ChangeName(string newName)
        {
            _inputField.text = newName;
        }

        public void OnDeleteArmyClick()
        {
            _ui.OnRemoveArmyClicked(this);
        }

        public void OnDuplicateArmyClick()
        {
            _ui.OnDuplicateArmyClicked(ArmyData);
        }

        //Clicking on this will open the Army Builder UI
        public void OnEditArmyClick()
        {
            //Debug.Log("OnEditArmyClick()");

            _ui.OnEditArmyClicked(this);
        }

        // --- UI Events ---
        public void OnEndEdit(string name)
        {
            //Debug.Log("OnEndEdit(newName: " + name + ")");
            if (!string.IsNullOrEmpty(name))
            {
                _ui.OnArmyNameChanged(this, name);
            }
        }
        #endregion Public

        #endregion METHODS
    }
}