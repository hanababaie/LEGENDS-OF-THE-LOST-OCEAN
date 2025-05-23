using UnityEngine;

public class EnemySpawner:MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject spawnedEnemy;

    void Start()
    {
        if (enemyPrefab != null)
        {
            spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawnedEnemy.transform.parent = transform.parent; 
        }
    }
}
