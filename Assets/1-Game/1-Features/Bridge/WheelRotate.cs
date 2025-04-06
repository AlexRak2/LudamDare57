using System;
using UnityEngine;

namespace Game
{
    public class WheelRotate : MonoBehaviour
    {
        public float Speed = 10;
        private bool _isRotating;
        public void StartRotate()
        {
            _isRotating = true;
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