using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float attackRadius;
    [SerializeField] private Projectile projectile;

    private Enemy targetEnemy = null;
    private float attackCounter;
    private bool isAttacking = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        attackCounter -= Time.deltaTime;

        if (targetEnemy == null || targetEnemy.IsDead) {
            Enemy nearestEnemy = GetNearestEnemyInRange();
            if (nearestEnemy != null &&
                Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius) {
                targetEnemy = nearestEnemy;
            }
        }

        else {
            if (attackCounter <= 0) {
                isAttacking = true;

                // Reset attack counter
                attackCounter = timeBetweenAttacks;
            }
            else
                isAttacking = false;

            // Un-set the target enemy if they go out of range.
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
                targetEnemy = null;
        }
	}

    private void FixedUpdate() {
        if (isAttacking)
            Attack();
    }

    public void Attack() {
        isAttacking = false;

        // Create a new projectile
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;

        // If the enemy does not exist anymore, destroy the new projectile
        if (targetEnemy == null)
            Destroy(newProjectile);
        else {

            // Move the projectile towards the enemy
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    private float GetTargetDistance(Enemy thisEnemy) {
        if (thisEnemy == null) {
            thisEnemy = GetNearestEnemyInRange();

            if (thisEnemy == null)
                return 0f;
        }

        // Return the distance between the tower and nearest enemy
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    IEnumerator MoveProjectile(Projectile projectile) {
        while(GetTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null) {
            Vector3 dir = targetEnemy.transform.localPosition - transform.localPosition;
            float angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition,
                targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }

        if (projectile != null && targetEnemy == null)
            Destroy(projectile);
    }

    // Finds all enemies within range of this tower
    private List<Enemy> GetEnemiesInRange() {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in GameManager.Instance.enemyList) {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
                enemiesInRange.Add(enemy);
        }

        return enemiesInRange;
    }

    // Finds the nearest enemy in the list of enemies in range
    private Enemy GetNearestEnemyInRange() {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach (Enemy enemy in GetEnemiesInRange()) {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance) {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
