using System;
using Game.Player;
using Unity.Collections;
using UnityEngine;

namespace LD57.Player
{
    public class PlayerFallDamage : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _fallDamageThreshold = 10f;
        [SerializeField] private float _damage = 5f;
        [SerializeField] private float _health = 100f;

        private bool _isFalling = false;
        private float _lowestYVelocity = 0f;
        [SerializeField, ReadOnly] private float impactVelocity = 0f;

        private void FixedUpdate()
        {
            if (!PlayerMovement.Instance.IsGrounded)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                    _lowestYVelocity = float.MaxValue; // Reset for new fall
                }

                float currentY = _rigidbody.linearVelocity.y;

                // Capture the most negative Y velocity
                if (currentY < _lowestYVelocity)
                    _lowestYVelocity = currentY;
            }
            else if (_isFalling)
            {
                 impactVelocity = Mathf.Abs(_lowestYVelocity);

                if (impactVelocity > _fallDamageThreshold)
                {
                    float damage = (impactVelocity - _fallDamageThreshold) * _damage;
                    ApplyDamage(damage);
                }

                _isFalling = false;
                _lowestYVelocity = 0f;
            }
        }

        private void ApplyDamage(float amount)
        {
            _health -= amount;

            if (_health <= 0f)
            {
                Debug.Log("Player Died from Fall!");
                // Add death logic here
            }
        }
    }
}