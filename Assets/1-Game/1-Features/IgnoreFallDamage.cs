using Game.Waypoint;
using LD57.Player;
using UnityEngine;

namespace Game
{
    public class IgnoreFallDamage : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            
            if(!other.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
                return; 
            print("Ignore FallDamage");
            playerStats.IgnoreFallDamage = true;
        }
    }
}