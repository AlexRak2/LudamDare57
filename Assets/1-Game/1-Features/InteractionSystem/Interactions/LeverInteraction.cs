using System;
using DG.Tweening;
using Game.Sounds;
using UnityEngine;

namespace Game.Interactions.Lever
{
    public class LeverInteraction : BaseInteractable
    {
        [SerializeField] private Transform _leverObj;
        [SerializeField] private float turnAmount = 100;
        private bool _isOn;
        private bool _isTurning;

        [SerializeField] private Transform _movingObj;
        [SerializeField] private float _movingDuration = 3f;
        [SerializeField] private Vector3 _movingTarget;
        private Vector3 _movingOriginalPos;
        
        [SerializeField] private AudioClip _movingSound;
        [SerializeField] private AudioClip _leverSound;

        private void Start()
        {
            if (_movingObj)
                _movingOriginalPos = _movingObj.localPosition;
        }

        public override void LeftInputInteract()
        {
            if(_isTurning) return;

            SoundManager.PlayWorld(_leverSound, transform.position, 0.5f, 1f, false);
            _isOn = !_isOn;
            _leverObj.DOLocalRotate(new Vector3(_isOn ? turnAmount : -turnAmount, -90, 0), 0.5f).OnComplete(() =>
            {
                if (_movingObj)
                {
                    SoundManager.PlayWorld(_movingSound, _movingObj.position, maxDistance: 200f);
                    _movingObj.DOLocalMove(_isOn ? _movingTarget : _movingOriginalPos, _movingDuration).OnComplete(() =>
                    {
                        _isTurning = false;
                    });
                }
                else
                    _isTurning = false;
                
            });
        }

        public override string LeftInteractionMessage => "Turn Lever";
    }
}