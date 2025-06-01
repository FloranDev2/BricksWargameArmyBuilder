using System.Collections;
using System.Collections.Generic;
using Truelch.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    public class ArmyBuilderUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [Header("UI refs")]
        [SerializeField] private Image _medFanToggleImg;
        [SerializeField] private Image _modFutToggleImg;

        [Header("Sprites")]
        [SerializeField] private Sprite _toggleOnSprite;
        [SerializeField] private Sprite _toggleOffSprite;

        //Hidden
        // - Managers
        private GameManager _gameMgr;

        // - Misc
        private bool _isReady = false;

        // - Units
        private List<UnitElem> _unitElems = new List<UnitElem>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance);
            _gameMgr = GameManager.Instance;
            UpdatePeriodToggles();
            _isReady = true;
        }
        #endregion Initialization

        #region Misc
        private void UpdatePeriodToggles()
        {
            if (!_isReady) return;

            _medFanToggleImg.sprite = _gameMgr.MedFanOn ? _toggleOnSprite : _toggleOffSprite;
            _modFutToggleImg.sprite = _gameMgr.ModFutOn ? _toggleOnSprite : _toggleOffSprite;
        }
        #endregion Misc

        #region Public
        //Options
        public void OnSaveClick()
        {
            if (!_isReady) return;
            _gameMgr.OnSaveClick();
        }

        public void OnPrintExportClick()
        {
            if (!_isReady) return;
            //TODO
        }

        //Toggle Medieval / Fantasy period
        public void OnMedFanToggleClick()
        {
            if (!_isReady) return;
            _gameMgr.OnMedFanToggleClick();
            UpdatePeriodToggles();
        }

        //Toggle Modern / Sci-Fi period
        public void OnModFutToggleClick()
        {
            if (!_isReady) return;
            _gameMgr.OnModFutToggleClick();
            UpdatePeriodToggles();
        }
        #endregion Public

        #endregion METHODS
    }
}