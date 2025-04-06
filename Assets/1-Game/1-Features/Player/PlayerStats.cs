using System;
using DG.Tweening;
using Game;
using Game.Player;
using Game.UI;
using Game.Waypoint;
using Unity.Collections;
using UnityEngine;

namespace LD57.Player
{
    public class PlayerStats : MonoBehaviour
    {
        public static PlayerStats Instance;
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _fallDamageThreshold = 10f;
        [SerializeField] private float _damage = 5f;
        [SerializeField] private float _health = 100f;

        private bool _isFalling = false;
        [SerializeField, ReadOnly]private float _lowestYVelocity = 0f;
        [SerializeField, ReadOnly] private float impactVelocity = 0f;
        public bool IgnoreFallDamage { get; set; }
        [SerializeField] private AudioSource _fallingAudioSource;

        private void Awake()
        {
            Instance = this;
        }

        private void FixedUpdate()
        {
            if (!PlayerMovement.Instance.IsGrounded)
            {
                if (!_isFalling && _lowestYVelocity < -_fallDamageThreshold)
                {
                    _isFalling = true;
                    _lowestYVelocity = float.MaxValue; // Reset for new fall
                    _fallingAudioSource.volume = 0;
                    _fallingAudioSource.Play();
                    _fallingAudioSource.DOFade(0.5f, 0.5f);
                }
            
                float currentY = _rigidbody.linearVelocity.y;

                // Capture the most negative Y velocity
                if (currentY < _lowestYVelocity)
                    _lowestYVelocity = currentY;
            }
            else if (_isFalling)
            {
                _fallingAudioSource.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    _fallingAudioSource.Stop();
                });
                
                if (IgnoreFallDamage)
                {
                    _isFalling = false;
                    _lowestYVelocity = 0f;
                    IgnoreFallDamage = false;
                    
                    
                    return;
                }

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
                Die();
            }
        }

        public static void Die()
        {
            PlayerMovement.Freeze(true);
            PlayerHud.Instance.OpenDeathUI();
        }

        public void Respawn(Vector3 pos)
        {
            transform.position = pos;
            PlayerMovement.Freeze(false);
        }
    }
}