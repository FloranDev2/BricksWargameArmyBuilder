using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    public class TestUIFitter : MonoBehaviour
    {
        #region ATTRIBUTES
        [SerializeField] private float _canvasScale = 1f;
        [SerializeField] private RectTransform _rt1;
        [SerializeField] private RectTransform _rt2;
        [SerializeField] private float _newHeight = 300f;
        //[SerializeField] private UIFitterComponent _uiFitter;
        #endregion ATTRIBUTES


        #region METHODS
        /*
        private void Awake()
        {
            DoTheThing(); //This actually works. Certainly because it's before the Canvas Scaler scale the UI?
        }
        */

        private void Start()
        {
            _canvasScale = transform.localScale.x; //scale x, y and even z are the same
            DoTheThing();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DoTheThing();
            }
        }

        void DoTheThing()
        {
            Debug.Log("Here!");
            GameObject go = _rt2.gameObject;
            UIFitter.SetSize(ref go, _newHeight);
            Debug.Log("_rt1.localScale.y");
            _rt2.position = new Vector3(_rt1.position.x, _rt1.position.y - 0.5f * _canvasScale * (_rt1.rect.height + _newHeight), 0f);
        }
        #endregion METHODS
    }
}