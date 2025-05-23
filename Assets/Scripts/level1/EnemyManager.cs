using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private List<EnemyController> enemies = new List<EnemyController>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public void SyncEnemyHealth(EnemyController enemy, int health)
    {
        Debug.Log($"Enemy {enemy.gameObject.name} health synced to {health}");

        if (health <= 0)
        {
            UnregisterEnemy(enemy);
        }
    }

    public void ResetAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.SetHealth(enemy.maxHealth);
        }
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public List<EnemyController> GetAllEnemies()
    {
        return new List<EnemyController>(enemies);
    }
}