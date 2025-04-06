using UnityEngine;

namespace Game.Waypoint
{
    public interface ICheckpoint
    {
        string CheckpointName { get; } 
        Vector3 RespawnPosition { get; }
        bool SavedCheckpoint { get; }
    }
}