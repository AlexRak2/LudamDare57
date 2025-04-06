using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Bridge : MonoBehaviour
    {
        [SerializeField] private WheelRotate[] _wheels;
        [SerializeField] private Transform[] _bridgePoints;

        public void DrawBrige()
        {
            foreach (var wheel in _wheels)
            {
                wheel.StartRotate();
            }

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