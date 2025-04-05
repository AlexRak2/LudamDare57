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
        
        public override void RightInputInteract()
        {
            if(_isTurning) return;
            
            _isOn = !_isOn;
            _leverObj.DOLocalRotate(new Vector3(_isOn ? turnAmount : -turnAmount, 0, 0), 0.5f).OnComplete(() =>
            {
                _isTurning = false;
            });
        }
    }
}