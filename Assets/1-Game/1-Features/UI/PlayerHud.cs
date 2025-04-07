using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private CanvasGroup _checkpointCanvasGroup;
        [SerializeField] private TMP_Text _checkpointText;

        bool isFadingToMainMenu = false;
        
        public static bool IsDeathScreenOn => Instance._deathCanvasGroup.alpha > 0.6f;
        private void Awake()
        {
            Instance = this;
        }
        private void Start() 
        {
            LD57.GameManager.Instance.OnFadeComplete += OnFadingCanvasComplete;

            RestartFromLastCheckPoint();
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
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void RestartFromLastCheckPoint()
        {
            ICheckpoint checkpoint = WaypointHandler.Instance.CurrentWaypoint;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _checkpointText.text = checkpoint == null ? "Dark Cave" : checkpoint.CheckpointName;
            _deathCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                _checkpointCanvasGroup.DOFade(1, 1f);
                StartCoroutine(FadeOutCheckpoint());
            });

            GameManager.Instance.RestartFromCheckPoint(checkpoint);
        }

        IEnumerator FadeOutCheckpoint()
        {
            yield return new WaitForSeconds(2f);
            _checkpointCanvasGroup.DOFade(0, 1f);
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