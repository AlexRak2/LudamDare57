using UnityEngine;
using UnityEngine.AI;

public class DefaultEnemy : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public float maxMoveDist;
    public float maxStartFollowDist;
    public float minStopFollowDist;

    private bool isFollowingPlayer;
    
    public void UpdatePosition(Vector3 playerPosition)
    {
        if (Vector3.Distance(playerPosition, transform.position) <= maxStartFollowDist)
        {
            isFollowingPlayer = true;
        }
        else if (Vector3.Distance(playerPosition, transform.position) > minStopFollowDist)
        {
            isFollowingPlayer = false;
        }
        
        if (isFollowingPlayer)
        {
            navAgent.destination = playerPosition;
            Vector3[] corners = navAgent.path.corners;
            float curDistance = 0f;
            
            for (int i = 0; i < corners.Length - 1; i++)
            {
                curDistance += Vector3.Distance(corners[i], corners[i + 1]);
                float targetDistance = Mathf.Min(navAgent.remainingDistance / 2, maxMoveDist);
                if (curDistance >= targetDistance)
                {
                    float offsetDistance = Mathf.Abs(targetDistance - curDistance);
                    Vector3 cornerOffset = (corners[i + 1] - corners[i]).normalized * offsetDistance;
                    transform.position = corners[i + 1] - cornerOffset;
                    break;
                }
            }
        }
        
        // TODO: Choose a random walk pose
    }
}
