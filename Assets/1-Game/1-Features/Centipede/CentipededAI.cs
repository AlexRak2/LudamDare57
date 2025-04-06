using UnityEngine;
using UnityEngine.AI;

namespace Game.Centipede
{
    public class CentipededAI : MonoBehaviour
    {
        [Header("Wander Settings")]
        public float wanderRadius = 10f;
        public float wanderInterval = 5f;
        public float movementSpeed = 1f;

        private NavMeshAgent _agent;
        private float _timer;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = movementSpeed;
            _timer = wanderInterval;
        }

        void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= wanderInterval || _agent.velocity.sqrMagnitude == 0f)
            {
                Vector3 newPos = GetRandomNavMeshPosition(transform.position, wanderRadius);
                _agent.SetDestination(newPos);
                _timer = 0;
            }
        }

        private Vector3 GetRandomNavMeshPosition(Vector3 center, float radius)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPos = center + Random.insideUnitSphere * radius;
                if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return center;
        }
    }
}