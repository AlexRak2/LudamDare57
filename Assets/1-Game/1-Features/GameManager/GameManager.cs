using System;
using Game.Waypoint;
using LD57.Player;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private Vector3 _startPosition;
        private void Awake()
        {
            Instance = this;
        }

        public void RestartFromCheckPoint(ICheckpoint checkpoint)
        {
            //perform ui stuff
            if (checkpoint == null)
            {
                PlayerStats.Instance.Respawn(_startPosition);
                return;
            }

            PlayerStats.Instance.Respawn(checkpoint.RespawnPosition);
        }
    }
}