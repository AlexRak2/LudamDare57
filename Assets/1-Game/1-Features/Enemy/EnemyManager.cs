using System;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform[] enemySpawnPoints;
    public GameObject enemyPrefab;

    public static EnemyManager Instance;
    
    private List<DefaultEnemy> spawnedEnemies = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var curSpawnPoint in enemySpawnPoints)
        {
            GameObject curEnemy = Instantiate(enemyPrefab, curSpawnPoint.position, curSpawnPoint.rotation);
            spawnedEnemies.Add(curEnemy.GetComponent<DefaultEnemy>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateEnemyPositions(PlayerMovement.Instance.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var curSpawnPoint in enemySpawnPoints)
        {
            Gizmos.DrawWireSphere(curSpawnPoint.position, 0.1f);
        }
    }

    public void UpdateEnemyPositions(Vector3 playerPosition)
    {
        foreach (var curEnemy in spawnedEnemies)
        {
            curEnemy.UpdatePosition(playerPosition);
        }
    }
}
