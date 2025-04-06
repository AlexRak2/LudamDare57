using System;
using DG.Tweening;
using Game.Waypoint;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Game.UI
{
    public class PlayerHud : MonoBehaviour
    {
        public static PlayerHud Instance;

        [SerializeField] private GameObject _rightInteractionObj;
        [SerializeField] private GameObject _leftInteractionObj;
        [SerializeField] private TMP_Text _interactionRightText;
        [SerializeField] private TMP_Text _interactionLeftText, echoCountText;
        [SerializeField] private CanvasGroup _deathCanvasGroup;
        private void Awake()
        {
            Instance = this;
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
            //back to main menu
        }

        public void RestartFromLastCheckPoint()
        {
            GameManager.Instance.RestartFromCheckPoint(WaypointHandler.Instance.CurrentWaypoint);

        }

        public void OpenDeathUI()
        {
            _deathCanvasGroup.DOFade(1, 0.5f);
        }
    }
}