using DG.Tweening;
using Game.Sounds;
using UnityEngine;

namespace Game
{
    public class Bridge : MonoBehaviour
    {
        [SerializeField] private WheelRotate[] _wheels;
        [SerializeField] private Transform[] _bridgePoints;
        [SerializeField] private AudioClip _bridgeClip;
        public void DrawBrige()
        {
            foreach (var wheel in _wheels)
            {
                wheel.StartRotate();
            }
            
            SoundManager.PlayWorld(_bridgeClip, _bridgePoints[0].position, 0.5f);

            _bridgePoints[0].DOLocalRotate(new Vector3(0, 140, -90), 3f);
            _bridgePoints[1].DOLocalRotate(new Vector3(0, -40, -90), 3f).OnComplete(
                () =>
                {
                    StopBridge();
                });

        }

        private void StopBridge()
        {
            foreach (var wheel in _wheels)
            {
                wheel.StopRotate();
            }
        }
    }
} 