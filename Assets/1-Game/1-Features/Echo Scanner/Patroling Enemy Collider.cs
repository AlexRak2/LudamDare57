using UnityEngine;
using System.Collections;

namespace LD57.Echo
{
public class PatrolingEnemyCollider : MonoBehaviour
{
    [SerializeField] private EnemyEcho _enemyModelPrefab;
    [SerializeField] private float _revealDuration = 2f; 

    private EnemyEcho _currentModelInstance; 

    private void OnTriggerEnter(Collider other)
    {
        if (_currentModelInstance != null) {
            Destroy(_currentModelInstance);
        }

        _currentModelInstance = Instantiate(_enemyModelPrefab, transform.position, transform.rotation);
        StartCoroutine(RevealModelRoutine());
    }

    private IEnumerator RevealModelRoutine()
    {
        yield return null;
        _currentModelInstance.EmitEcho();

        yield return new WaitForSeconds(_revealDuration);
        if (_currentModelInstance != null)
        {
            Destroy(_currentModelInstance);
            _currentModelInstance = null;
        }
    }
}
}