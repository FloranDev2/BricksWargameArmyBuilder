using System.Collections;
using System.Collections.Generic;
using Truelch.Managers;
using UnityEngine;

namespace Truelch.UI
{
    /// <summary>
    /// All children classes will implement the data they need to store and also the stuff they want to display in the scroll elements.
    /// </summary>
    public abstract class ExpandButtonBase : MonoBehaviour
    {
        #region ATTRIBUTES
        //Public
        //Note: stuff below need to be defined by the script that ask the dynamic scroller to show up and display a list of these elements.
        //I don't do a Init method because I might want to have different params.
        //Index is used to access the correct data. (in the list stored by the)
        //public int Index; //-> NO, move to DynScrollElem!!!
        //public

        //Inspector
        [SerializeField] protected Transform _arrowTf;
        [SerializeField] protected Vector3 _arrowDefaultRot = new Vector3(0f, 0f, 0f);
        [SerializeField] protected Vector3 _arrowExpandedRot = new Vector3(0f, 0f, 180f);

        //Hidden
        // - Misc
        protected bool _isReady = false;
        /*[SerializeField]*/ protected RectTransform _parentRt;

        // - Managers
        protected CanvasManager _canvasMgr;
        protected GameManager _gameMgr;
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        protected IEnumerator Start()
        {
            _parentRt = GetComponent<RectTransform>();

            yield return new WaitUntil(() =>
                CanvasManager.Instance != null &&
                GameManager.Instance != null
            );

            _canvasMgr = CanvasManager.Instance;
            _gameMgr = GameManager.Instance;

            _isReady = true;
        }
        #endregion Initialization

        #region Public
        /// <summary>
        /// You NEED to call base.OnClick for this!
        /// Also remember to check _isReady ;)
        /// 
        /// Moved from PeriodExpandButton:
        /// This only handle the fact to create a dynamic scroller and fill it with what we need.
        /// Closing it is done by the dynamic scroller itself, that reacts when we click outside.
        /// TODO: or when we actually click on one of the buttons.
        /// </summary>
        public virtual void OnClick()
        {
            if (!_isReady) return;

            //Rotate arrow
            _arrowTf.eulerAngles = _arrowDefaultRot;

            //Relocate dynamic scroller
            //(filling it is done by the children class)
            _canvasMgr.DynamicScroller.ShowDynamicScroller(_parentRt);
        }

        public abstract void OnElemClick(int index);

        //public abstract void OnExitDynamicScroller();
        #endregion Public

        #endregion METHODS
    }
}