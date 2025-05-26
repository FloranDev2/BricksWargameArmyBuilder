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

        //[Header("")]

        //[Header("Add a unit")]

        [Header("Dynamic Scroller")]
        [SerializeField] private GameObject _dynScrollParentGo; //Activate / disable
        [SerializeField] private RectTransform _dynScrollerTf; //Relocate and change size
        [SerializeField] private Transform _dynScrollerWrapper;

        [Header("Dynamic Scroller test params")]
        [SerializeField] private RectTransform _testRt; //TMP!!!
        [SerializeField] private float _testHeight = 500f;

        //Hidden
        // - Managers
        private GameManager _gameMgr;

        // - Misc
        private bool _isReady = false;

        // - Dynamic scroller
        private List<GameObject> _dynamicScrollerElems = new List<GameObject>();

        // - Units
        private List<UnitElem> _unitElems = new List<UnitElem>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            HideDynamicScroller();
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

        [ContextMenu("Test dynamic scroll")]
        private void TestDynScroll()
        {
            ShowDynamicScroller(_testRt, _testHeight);
        }

        /// <summary>
        /// Source is the button from which the dynamic scroll view is opened.
        /// </summary>
        /// <param name="_src"></param>
        public void ShowDynamicScroller(RectTransform _src, float height = 500f)
        {
            //Show
            _dynScrollParentGo.SetActive(true);
            GameObject go = _dynScrollerTf.gameObject;

            //Vector3 pos = _src.position;
            Debug.Log("y : " + _src.position.y + " / Screen height: " + Screen.height);

            if (_src.position.y > Screen.height)
            {
                //Downward
                Vector3 pos = new Vector3(_src.position.x, _src.position.y - 0.5f * _src.rect.height - 0.5f * height, 0f);
                _dynScrollerTf.position = pos;                

                UIFitter.SetWidth(ref go, _src.rect.width);
                UIFitter.SetHeight(ref go, height);
            }
            else
            {
                //Upward
                Vector3 pos = new Vector3(_src.position.x, _src.position.y + 0.5f * _src.rect.height + 0.5f * height, 0f);
                _dynScrollerTf.position = pos;
            }
        }

        public void HideDynamicScroller()
        {
            _dynScrollParentGo.SetActive(false);
        }
        #endregion Public

        #endregion METHODS
    }
}