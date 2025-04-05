using System;
using DG.Tweening;
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

        private void Start()
        {
            _movingOriginalPos = _movingObj.localPosition;
        }

        public override void LeftInputInteract()
        {
            if(_isTurning) return;
            
            _isOn = !_isOn;
            _leverObj.DOLocalRotate(new Vector3(_isOn ? turnAmount : -turnAmount, 0, 0), 0.5f).OnComplete(() =>
            {
                _movingObj.DOLocalMove(_isOn ? _movingTarget : _movingOriginalPos, _movingDuration).OnComplete(() =>
                {
                    _isTurning = false;
                });
            });
        }

        public override string LeftInteractionMessage => "Turn Lever";
    }
}