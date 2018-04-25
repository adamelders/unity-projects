using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject spawnPoint;
    public GameObject[] enemies;
    public int maxEnemiesOnScreen;
    public int totalEnemies;
    public int enemiesPerSpawn;
    public float spawnDelay;

    private int enemiesOnScreen = 0;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
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
