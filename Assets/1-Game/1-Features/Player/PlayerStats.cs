using System;
using DG.Tweening;
using Game;
using Game.Player;
using Game.UI;
using Game.Waypoint;
using Unity.Collections;
using UnityEngine;
using System.Collections;

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
        [SerializeField] private GameObject _jumpScareObject;

        public static Action OnRespawn;
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
                    ApplyFallDamage(damage);
                }

                _isFalling = false;
                _lowestYVelocity = 0f;
            }
        }

        private void ApplyEnemyDamage(float amount)
        {
            _health -= amount;

            if (_health <= 0f)
            {
                Die("The dweller in the depths has claimed another soul!");
            }
        }
        private void ApplyFallDamage(float amount)
        {
            _health -= amount;

            if (_health <= 0f)
            {
                Die("You fell too hard!");
            }
        }

        public static void Die(string _reason)
        {
            PlayerMovement.Freeze(true);
            PlayerHud.Instance.OpenDeathUI(_reason);
        }

        public void Respawn(Vector3 pos)
        {
            PlayerMovement.Instance.enabled = false;
            PlayerMovement.Rigidbody.isKinematic = true;
            
            PlayerMovement.Rigidbody.position = pos;
    
            PlayerMovement.Instance.enabled = true;
            PlayerMovement.Freeze(false);
            
            OnRespawn?.Invoke();
        }
        public IEnumerator GetJumpScared()
        {
            PlayerMovement.Freeze(true);
            _jumpScareObject.SetActive(true);

            yield return new WaitForSeconds(1.25f);
            ApplyEnemyDamage(100f);
        }
    }
}