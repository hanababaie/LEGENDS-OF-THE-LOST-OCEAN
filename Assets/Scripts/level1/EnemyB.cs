using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyB : EnemyController
{
    public float shootCooldown = 2f;
    public float detectionRadius = 8f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float shootTimer;
    private GameObject currentTarget;

    void Update()
    {
        if (currentTarget == null || Vector2.Distance(transform.position, currentTarget.transform.position) > detectionRadius)
        {
            FindTarget();
        }

        if (currentTarget != null)
        {
            shootTimer += Time.deltaTime;
            GetComponent<Animator>().SetTrigger("attack");
            // چرخش به سمت پلیر
            Vector3 scale = transform.localScale;
            if ((currentTarget.transform.position.x > transform.position.x && scale.x < 0) ||
                (currentTarget.transform.position.x < transform.position.x && scale.x > 0))
            {
                scale.x *= -1;
                transform.localScale = scale;
            }

            if (shootTimer >= shootCooldown)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
    }

    void FindTarget()
    {
        List<GameObject> players = new List<GameObject>();
        players.AddRange(GameObject.FindGameObjectsWithTag("player1"));
        players.AddRange(GameObject.FindGameObjectsWithTag("player2"));

        List<GameObject> inRange = new List<GameObject>();
        foreach (GameObject player in players)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= detectionRadius)
                inRange.Add(player);
        }

        if (inRange.Count > 0)
            currentTarget = inRange[Random.Range(0, inRange.Count)];
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (currentTarget.transform.position - firePoint.position).normalized;

        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }
}