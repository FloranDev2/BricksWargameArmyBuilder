using System.Collections;
using System.Collections.Generic;
using Truelch.Managers;
using UnityEngine;

namespace Truelch.UI
{
    /// <summary>
    /// TODO:
    /// - Add a search bar?!
    /// - Maybe add a "minimum width" option (and expand toward the farthest side if needed)
    /// </summary>
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
        // - Managers
        private CanvasManager _canvasMgr;

        // - Misc
        private ExpandButtonBase _currExpandBtn;
        private List<DynScrollElem> _elems = new List<DynScrollElem>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            HideDynamicScroller();
            yield return new WaitUntil(() => CanvasManager.Instance != null);
            _canvasMgr = CanvasManager.Instance;

        }
        #endregion Initialization

        #region Public
        public void OnElemClick(int index)
        {
            //Close
            HideDynamicScroller();

            _currExpandBtn.OnElemClick(index);
        }

        //Same as "un-expand"
        public void OnOutsideClick()
        {
            //Close
            HideDynamicScroller();
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
            ShowDynamicScroller(null, _testRt, _testHeight);
        }

        /// <summary>
        /// Source is the button from which the dynamic scroll view is opened.
        /// </summary>
        /// <param name="_src"></param>
        public void ShowDynamicScroller(ExpandButtonBase btn, RectTransform _src, float height = 500f)
        {
            //Debug.Log("ShowDynamicScroller(btn: " + btn + ", _src: " + _src + ", height: " + height);

            _currExpandBtn = btn;

            //Show
            _dynScrollParentGo.SetActive(true);
            GameObject go = _dynScrollerTf.gameObject;

            //Debug.Log("y : " + _src.position.y + " / Screen w: " + Screen.width + ", h: " + Screen.height);

            if (_src.position.y > 0.5f * Screen.height)
            {
                //Downward
                UIFitter.SetWidth(ref go, _src.rect.width);
                UIFitter.SetHeight(ref go, height);
                Vector3 pos = new Vector3(_src.position.x, _src.position.y - 0.5f * _canvasMgr.Scale * (_src.rect.height + height), 0f);
                _dynScrollerTf.position = pos;
            }
            else
            {
                //Upward
                UIFitter.SetWidth(ref go, _src.rect.width);
                UIFitter.SetHeight(ref go, height);
                Vector3 pos = new Vector3(_src.position.x, _src.position.y + 0.5f * _canvasMgr.Scale * (_src.rect.height + height), 0f);
                _dynScrollerTf.position = pos;
            }
        }

        public void HideDynamicScroller()
        {
            //Destroy elems
            foreach (var elem in _elems)
            {
                Destroy(elem.gameObject);
            }
            _elems.Clear();

            //Hide
            _dynScrollParentGo.SetActive(false);

            //"Event"
            if (_currExpandBtn != null)
            {
                _currExpandBtn.OnFoldClick();
            }
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