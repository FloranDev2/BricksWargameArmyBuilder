using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.UI
{
    public class DynamicScroller : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private GameObject _dynScrollParentGo; //Activate / disable
        [SerializeField] private RectTransform _dynScrollerTf; //Relocate and change size


        [Header("Elems")]
        [SerializeField] private DynScrollElem _elemPrefab;
        [SerializeField] private Transform _dynScrollerWrapper;

        [Header("Dynamic Scroller test params")]
        [SerializeField] private RectTransform _testRt; //TMP!!!
        [SerializeField] private float _testHeight = 500f;

        //Hidden
        // - Dynamic scroller
        private List<DynScrollElem> _elems = new List<DynScrollElem>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        private void Awake()
        {
            HideDynamicScroller();
        }
        #endregion Initialization

        #region Public
        public void OnOutsideClick()
        {
            //Close
            HideDynamicScroller();

            //Destroy elems
            foreach (var elem in _elems)
            {
                Destroy(elem.gameObject);
            }
            _elems.Clear();

            //Event

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Here!");
                TestDynScroll();
            }
        }

        //If I use the context menu, for some reason, the screen size will be incorrect...
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

            //Debug.Log("y : " + _src.position.y + " / Screen w: " + Screen.width + ", h: " + Screen.height);

            if (_src.position.y > 0.5f * Screen.height)
            {
                //Debug.Log("Downward");
                //Downward
                Vector3 pos = new Vector3(_src.position.x, _src.position.y - 0.5f * _src.rect.height - 0.5f * height, 0f);
                _dynScrollerTf.position = pos;

                UIFitter.SetWidth(ref go, _src.rect.width);
                UIFitter.SetHeight(ref go, height);
            }
            else
            {
                //Debug.Log("Upward");
                //Upward
                Vector3 pos = new Vector3(_src.position.x, _src.position.y + 0.5f * _src.rect.height + 0.5f * height, 0f);
                _dynScrollerTf.position = pos;

                UIFitter.SetWidth(ref go, _src.rect.width);
                UIFitter.SetHeight(ref go, height);
            }
        }

        public void HideDynamicScroller()
        {
            _dynScrollParentGo.SetActive(false);
        }

        public DynScrollElem CreateElem(int index, string msg)
        {
            var elem = Instantiate(_elemPrefab, _dynScrollerWrapper);
            elem.DynamicScroller = this;
            elem.Index = index;
            elem.Text.text = msg;
            _elems.Add(elem);
            return elem;
        }

        public DynScrollElem CreateElem(int index, string msg, Color color)
        {
            DynScrollElem elem = CreateElem(index, msg);
            elem.Image.color = color;
            return elem;
        }
        #endregion Public

        #endregion METHODS
    }
}
