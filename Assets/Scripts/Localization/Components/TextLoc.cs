using System.Collections;
using System.Collections.Generic;
using Truelch.Managers;
using TMPro;
using UnityEngine;

namespace Truelch.Localization
{
    /// <summary>
    /// Important note: don't disable GameObjects with TextLoc.
    /// Please find another way to hide them, like putting a canvas manager with alpha = 0.
    /// Otherwise, they won't receive the Language changed event.
    /// </summary>
    public class TextLoc : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private TextLocSO _locData;

        [Header("Debug")]
        [SerializeField] private bool _showLogs = false;

        //Hidden
        private GameManager _gameManager;
        private TextMeshProUGUI _textUI;
        private TextMeshPro _textWorldSPace;
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            _textUI = GetComponent<TextMeshProUGUI>();
            _textWorldSPace = GetComponent<TextMeshPro>();

            if (GameManager.Instance == null)
                yield return new WaitUntil(() => GameManager.Instance != null);

            _gameManager = GameManager.Instance;

            UpdateLoc(_gameManager.GetCurrentLanguage());
        }
        #endregion Initialization

        #region Loc
        public void UpdateLoc(Language lang)
        {
            if (_showLogs)
                Debug.Log("UpdateLoc(lang: " + lang + ") for " + gameObject);

            if (_locData == null)
            {
                Debug.Log("No localization data loc for: " + gameObject);
                return;
            }

            foreach (var loc in _locData.data)
            {
                if (loc.Language == lang)
                {
                    if (_textUI != null)
                    {
                        _textUI.text = loc.Txt;
                    }
                    else if (_textWorldSPace != null)
                    {
                        _textWorldSPace.text = loc.Txt;
                    }

                    return;
                }
            }

            Debug.Log("Couldn't find fitting loc for this language: " + lang + " for this object: " + gameObject);
        }
        #endregion Loc

        #region Delegate Event
        private void OnEnable()
        {
            GameManager.onLanguageChanged += UpdateLoc;
        }

        private void OnDisable()
        {
            GameManager.onLanguageChanged -= UpdateLoc;
        }
        #endregion Delegate Event

        #endregion METHODS
    }
}