using UnityEngine;
using System.Collections;

namespace LD57.Enemy
{
    public class PatrolingEnemy : MonoBehaviour
    {
        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _turnSpeed = 2f;
        [SerializeField] private float _stopDistance = 0.1f;

        private int _currentPointIndex = 0;
        private bool _isTurning = false;

        private void Start()
        {
            if (_patrolPoints.Length < 2) {
                Debug.LogError("PatrolingEnemy needs at least 2 patrol points!");
                return;
            }
            transform.position = _patrolPoints[0].position;
            StartCoroutine(PatrolRoutine());
        }

        private IEnumerator PatrolRoutine()
        {
            while (true)
            {
                Vector3 currentPoint = _patrolPoints[_currentPointIndex].position;
                int nextPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
                Vector3 nextPoint = _patrolPoints[nextPointIndex].position;

                while (Vector3.Distance(transform.position, currentPoint) > _stopDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentPoint, _speed * Time.deltaTime);
                    yield return null;
                }
                transform.position = currentPoint;

                _isTurning = true;
                Quaternion startRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.LookRotation(nextPoint - transform.position);
                float t = 0f;

                while (t < 1f && _isTurning)
                {
                    t += Time.deltaTime * _turnSpeed;
                    transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                    yield return null;
                }
                transform.rotation = targetRotation; 
                _isTurning = false;

                _currentPointIndex = nextPointIndex;
            }
        }

        private void OnDrawGizmos()
        {
            if (_patrolPoints == null || _patrolPoints.Length < 2) return;

            Gizmos.color = Color.red;
            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                Gizmos.DrawSphere(_patrolPoints[i].position, 0.2f);
                int nextIndex = (i + 1) % _patrolPoints.Length;
                Gizmos.DrawLine(_patrolPoints[i].position, _patrolPoints[nextIndex].position);
            }
        }
    }
}