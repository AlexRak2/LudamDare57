using UnityEngine;
using UnityEngine.AI;

public class DefaultEnemy : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public float minMoveDist;
    
    public void UpdatePosition(Vector3 playerPosition)
    {
        navAgent.destination = playerPosition;
        Vector3[] corners = navAgent.path.corners;
        float curDistance = 0f;
        
        for (int i = 0; i < corners.Length - 1; i++)
        {
            curDistance += Vector3.Distance(corners[i], corners[i + 1]);
            float targetDistance = Mathf.Max(navAgent.remainingDistance / 2, minMoveDist);
            if (curDistance >= targetDistance)
            {
                float offsetDistance = Mathf.Abs(targetDistance - curDistance);
                Vector3 cornerOffset = (corners[i + 1] - corners[i]).normalized * offsetDistance;
                transform.position = corners[i + 1] - cornerOffset;
                break;
            }
        }
        
        // TODO: Choose a random walk pose
    }
}
