using UnityEngine;

namespace LD57.Echo
{
public class EchoCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Particle collided with: " + other.name);
        other.GetComponent<EnemyEcho>().EmitEcho();
    }
}
}