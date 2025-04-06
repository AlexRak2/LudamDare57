using System;
using Game.Sounds;
using UnityEngine;

namespace Game
{
    public class WheelRotate : MonoBehaviour
    {
        public float Speed = 10;
        private bool _isRotating;

        public AudioClip _rotateClip;
        public void StartRotate()
        {
            _isRotating = true;

            SoundManager.PlayWorld(_rotateClip, transform.position, 0.5f);
        }

        public void StopRotate()
        {
            _isRotating = false;
        }
        
        private void Update()
        {
            if (_isRotating)
            {
                transform.Rotate(0, 0, Speed * Time.deltaTime);
            }
        }
    }
}