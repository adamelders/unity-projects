using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private int maxEnemiesOnScreen;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesPerSpawn;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject[] enemies;

    public List<Enemy> enemyList = new List<Enemy>();

    // Use this for initialization
    private void Start () {
        StartCoroutine(Spawn());
	}

    public void RegisterEnemy(Enemy enemy) {
        enemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy) {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies() {
        foreach (Enemy enemy in enemyList)
            Destroy(enemy.gameObject);

        enemyList.Clear();
    }

    IEnumerator Spawn() {
        if (enemiesPerSpawn > 0 && enemyList.Count < totalEnemies) {
            for (int i = 0; i < enemiesPerSpawn; i++) {
                if (enemyList.Count < maxEnemiesOnScreen) {
                    int randomNumber = Random.Range(0, 3);
                    GameObject newEnemy = Instantiate(enemies[randomNumber]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }
}
