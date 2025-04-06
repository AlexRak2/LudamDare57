using System;
using LD57.Player;
using UnityEngine;

namespace Game.Waypoint
{
    public class Checkpoint : MonoBehaviour, ICheckpoint
    {
        [SerializeField] private string _waypointName;
        public string CheckpointName => _waypointName;
        public Vector3 RespawnPosition { get; set; }
        public bool SavedCheckpoint { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if(SavedCheckpoint) return;
            
            if(!other.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
                return; 
            
            WaypointHandler.SetCheckpoint(this);
            RespawnPosition = playerStats.transform.position;
            SavedCheckpoint = true;
        }
    }
}