using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus {
    New,
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
    [SerializeField] private int totalEnemies = 3;
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
    private GameStatus currentState = GameStatus.New;
    private AudioSource audioSource;

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

    public int TotalEscaped { get; set; }

    public int RoundEscaped { get; set; }

    public int TotalKilled { get; set; }

    public GameStatus CurrentState {
        get {
            return currentState;
        }
        set {
            currentState = value;
        }
    }

    public AudioSource AudioSource {
        get {
            return audioSource;
        }
    }

    // Use this for initialization
    private void Start () {
        playButton.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
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

    public void IsWaveOver() {
        totalEscapedLabel.text = "Escaped: " + TotalEscaped + "/10";
        if ((RoundEscaped + TotalKilled) == totalEnemies) {
            
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState() {
        if (TotalEscaped >= 10)
            CurrentState = GameStatus.GameOver;
        else if (waveNumber == 0 && (TotalKilled + RoundEscaped) == 0)
            CurrentState = GameStatus.Play;
        else if (waveNumber >= totalWaves)
            CurrentState = GameStatus.Win;
        else
            CurrentState = GameStatus.Next;
    }

    public void ShowMenu() {
        switch(CurrentState) {
            case GameStatus.GameOver:
                playButtonLabel.text = "Play Again";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
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
            case GameStatus.New:
                playButtonLabel.text = "Play";
                break;
        }

        playButton.gameObject.SetActive(true);
    }

    public void PlayButtonPressed() {
        switch (CurrentState) {
            case GameStatus.Next:
                waveNumber++;
                totalEnemies += waveNumber;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagsBuildSites();
                break;
            default: // Reset values
                totalEnemies = 3;
                TotalEscaped = 0;
                TotalMoney = 10;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagsBuildSites();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Escaped " + TotalEscaped + "/10";
                audioSource.PlayOneShot(SoundManager.Instance.NewGame); // New Game sound
                break;
        }

        // If it's a new game, set state to Play
        if (CurrentState == GameStatus.New)
            CurrentState = GameStatus.Play;

        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLabel.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn()); // Start spawning enemies.
        playButton.gameObject.SetActive(false);
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
                if (enemyList.Count < totalEnemies) {

                    // Get random number based on wave number.
                    int endRandomNumber;
                    if (waveNumber >= 0 && waveNumber <= 2)
                        endRandomNumber = 1;
                    else if (waveNumber > 2 && waveNumber <= 5)
                        endRandomNumber = 2;
                    else
                        endRandomNumber = 3;
                    
                    int randomNumber = Random.Range(0, endRandomNumber);
                    GameObject newEnemy = Instantiate(enemies[randomNumber]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }
}
