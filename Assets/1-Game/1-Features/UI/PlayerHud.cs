using System;
using DG.Tweening;
using Game.Waypoint;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using LD57.UI;
using Game;

namespace Game.UI
{
    public class PlayerHud : MonoBehaviour
    {
        public static PlayerHud Instance;

        [SerializeField] private GameObject _rightInteractionObj;
        [SerializeField] private GameObject _leftInteractionObj;
        [SerializeField] private TMP_Text _interactionRightText;
        [SerializeField] private TMP_Text _interactionLeftText, echoCountText, deathReasonText;
        [SerializeField] private CanvasGroup _deathCanvasGroup;
        bool isFadingToMainMenu = false;
        private void Awake()
        {
            Instance = this;
        }
        private void Start() 
        {
            LD57.GameManager.Instance.OnFadeComplete += OnFadingCanvasComplete;
        }

        public void ShowRightInteractionUI(string message)
        {
            _rightInteractionObj.SetActive(true);
            _interactionRightText.text = $"Right Click to {message}";
        }
        
        public void ShowLeftInteractionUI(string message)
        {
            _leftInteractionObj.SetActive(true);
            _interactionLeftText.text  = $"Left Click to {message}";
        }

        public void HideRightInteractionUI()
        {
            _rightInteractionObj.SetActive(false);
        }
        
        public void HideLeftInteractionUI()
        {
            _leftInteractionObj.SetActive(false);
        }
        public void UpdateEchoCount(int2 count)
        {
            echoCountText.text = $"{count.x}/{count.y}";
        }

        public void Quit()
        {
            isFadingToMainMenu = true;
            StartCoroutine(LD57.GameManager.Instance.FadeCanvas(true));
        }

        public void RestartFromLastCheckPoint()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            _deathCanvasGroup.DOFade(0, 0.5f);
            GameManager.Instance.RestartFromCheckPoint(WaypointHandler.Instance.CurrentWaypoint);

        }

        public void OpenDeathUI(string _reason)
        {
            deathReasonText.text = _reason;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            _deathCanvasGroup.DOFade(1, 0.5f);
        }

        public void OnExitGameButtonClicked()
        {
            Application.Quit();
        }
        private void OnFadingCanvasComplete(bool isActive)
        {
            if (isActive && isFadingToMainMenu) {
                isFadingToMainMenu = false;
                LD57.GameManager.Instance.LoadMainMenu();
            }
        }
        private void OnDestroy()
        {
            LD57.GameManager.Instance.OnFadeComplete -= OnFadingCanvasComplete;
        }
    }
}