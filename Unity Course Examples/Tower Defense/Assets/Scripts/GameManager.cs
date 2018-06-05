using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus {
    Next,
    Play,
    GameOver,
    Win
}

public class GameManager : Singleton<GameManager> {

    [SerializeField] private int totalWaves = 10;
    [SerializeField] private Text totalMoneyLabel;
    [SerializeField] private Text currentWaveLabel;
    [SerializeField] private Text playButtonLabel;
    [SerializeField] private Text totalEscapedLabel;
    [SerializeField] private Button playButton;
    [SerializeField] private int maxEnemiesOnScreen;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesPerSpawn;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject[] enemies;

    private int waveNumber = 0;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int whichEnemiesToSpawn = 0;
    private GameStatus currentState = GameStatus.Play;

    public List<Enemy> enemyList = new List<Enemy>();

    public int TotalMoney {
        get {
            return totalMoney;
        }
        set {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }

    // Use this for initialization
    private void Start () {
        playButton.gameObject.SetActive(false);
        ShowMenu();
	}

    private void Update() {
        HandleEscape();
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

    // Adds to total player money.
    public void AddMoney(int amount) {
        TotalMoney += amount;
    }

    // Subtracts from total player money.
    public void SubtractMoney(int amount) {
        TotalMoney -= amount;
    }

    public void ShowMenu() {
        switch(currentState) {
            case GameStatus.GameOver:
                playButtonLabel.text = "Play Again";
                // game over sound
                break;
            case GameStatus.Next:
                playButtonLabel.text = "Next Wave";
                break;
            case GameStatus.Play:
                playButtonLabel.text = "Play";
                break;
            case GameStatus.Win:
                playButtonLabel.text = "Play";
                break;
        }

        playButton.gameObject.SetActive(true);
    }

    private void HandleEscape() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TowerManager.Instance.DisableDragSprite();
            TowerManager.Instance.towerBtnPressed = null;
        }
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
