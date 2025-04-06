using LD57.Player;
using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected by enemy line of sight!");
            // Add logic to handle player detection, e.g., alerting the enemy or triggering an event.
            StartCoroutine(other.GetComponent<PlayerStats>().GetJumpScared());
        }
    }
}
