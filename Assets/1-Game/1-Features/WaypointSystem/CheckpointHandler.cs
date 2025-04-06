using System;
using LD57.Player;
using UnityEngine;

namespace Game.Waypoint
{
    public class WaypointHandler : MonoBehaviour
    {
        public static WaypointHandler Instance;
        
        public ICheckpoint CurrentWaypoint => _lastCheckpoint; 
        ICheckpoint _lastCheckpoint;
        
        private void Awake()
        {
            Instance = this;
        }

        public static void SetCheckpoint(ICheckpoint checkpoint)
        { 
            Instance._lastCheckpoint = checkpoint; 
            print($"Checkpoint Triggered: {checkpoint.CheckpointName}");
        }
    }
}