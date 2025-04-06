using System;
using DG.Tweening;
using Game.Sounds;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class ButtonInteraction : BaseInteractable
    {
        public UnityEvent ClickEvent;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _resetSound;
        private Vector3 _originalPosition;

        private bool _isClicked;
        private void Start()
        {
            _originalPosition = transform.localPosition;
        }

        public override void LeftInputInteract()
        {
            if(_isClicked) return;
            
            
            if(_clickSound)
                SoundManager.PlayWorld(_clickSound, transform.position);

            transform.DOLocalMove(new Vector3(transform.localPosition.x, 0, transform.localPosition.z), 0.4f);
            _isClicked = true;
            
            ClickEvent.Invoke();
        }

        public void ResetButton()
        {
            transform.DOKill();
            transform.DOLocalMove(_originalPosition, 0.4f);
            _isClicked = false;
            
            if(_resetSound)
                SoundManager.PlayWorld(_resetSound, transform.position);
        }

        public override string LeftInteractionMessage => _isClicked ? string.Empty : "Press";
    }
}