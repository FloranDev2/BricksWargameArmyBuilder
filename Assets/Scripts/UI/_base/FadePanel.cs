using Truelch.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.UI
{
    /// <summary>
    /// TODO: register if originally, the canvas group was set to ignore raycast
    /// and apply that value when the panel is shown.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadePanel : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [Header("Fade params")]
        [SerializeField] protected float _appearSpeed = 2f;
        [SerializeField] protected float _disappearSpeed = 2f;

        //Hidden
        protected float _fadeValue = 0f;
        protected FadeState _fadeState = FadeState.Default;
        protected CanvasGroup _canvasGroup;
        #endregion ATTRIBUTES


        #region PROPERTIES
        public float AppearSpeed
        {
            get
            {
                return _appearSpeed;
            }
        }

        public float DisappearSpeed
        {
            get
            {
                return _disappearSpeed;
            }
        }
        #endregion PROPERTIES


        #region METHODS

        #region Initialization
        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        #endregion Initialization

        #region Update
        protected virtual void Update()
        {
            ComputeFade();
        }

        void ComputeFade()
        {
            if (_fadeState == FadeState.Appear)
            {
                _fadeValue += _appearSpeed * Time.deltaTime;
                if (_fadeValue >= 1f)
                {
                    _fadeValue = 1f;
                    _fadeState = FadeState.Default;

                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;
                }
            }
            else if (_fadeState == FadeState.Disappear)
            {
                _fadeValue -= _disappearSpeed * Time.deltaTime;
                if (_fadeValue <= 0f)
                {
                    _fadeValue = 0f;
                    _fadeState = FadeState.Default;
                }
            }
            else
            {
                return;
            }
            _canvasGroup.alpha = _fadeValue;
        }
        #endregion Update

        #region Public
        public virtual void Show()
        {
            //Debug.Log(gameObject + ".Show()");
            _fadeState = FadeState.Appear;
        }

        public virtual void Hide()
        {
            //Debug.Log(gameObject + ".Hide()");
            _fadeState = FadeState.Disappear;

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public virtual void ShowInstantly()
        {
            //Debug.Log(gameObject + ".ShowInstantly()");
            _fadeState = FadeState.Default;
            _fadeValue = 1f;
            _canvasGroup.alpha = _fadeValue;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public virtual void HideInstantly()
        {
            //Debug.Log(gameObject + ".HideInstantly()");
            _fadeState = FadeState.Default;
            _fadeValue = 0f;
            _canvasGroup.alpha = _fadeValue;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        #endregion Public

        #endregion METHODS
    }
}