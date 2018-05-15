using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private int maxEnemiesOnScreen;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesPerSpawn;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject[] enemies;

    private int enemiesOnScreen = 0;

    // Use this for initialization
    private void Start () {
        StartCoroutine(Spawn());
	}

    public void RemoveEnemyFromScreen() {
        if (enemiesOnScreen > 0)
            enemiesOnScreen--;
    }

    IEnumerator Spawn() {
        if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies) {
            for (int i = 0; i < enemiesPerSpawn; i++) {
                if (enemiesOnScreen < maxEnemiesOnScreen) {
                    int randomNumber = Random.Range(0, 3);
                    GameObject newEnemy = Instantiate(enemies[randomNumber]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    enemiesOnScreen++;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }
}
