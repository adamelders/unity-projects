using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;
    private GameObject[] rocks;
    private Vector3[] rockPositions;

    private bool playerActive = false;
    private bool gameOver = false;
    private bool gameStarted = false;
    private bool coinSpawningStarted = false;
    
    public bool PlayerActive {
        get {
            return playerActive;
        }
    }

    public bool GameOver {
        get {
            return gameOver;
        }
    }

    public bool GameStarted {
        get {
            return gameStarted;
        }
    }

    public bool CoinSpawningStarted {
        get {
            return coinSpawningStarted;
        }
        set {

            // Do not set if the value is already the same.
            if (coinSpawningStarted != value)
                coinSpawningStarted = value;
        }
    }

    private void Awake() {

        // Make sure there is only one instance of GameManager running.
        // If another instance is created, destroy that object.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Don't destroy this object between scenes.
        DontDestroyOnLoad(gameObject);

        Assert.IsNotNull(mainMenu);
        Assert.IsNotNull(gameOverMenu);

        // Store rock positions.
        this.rocks = GameObject.FindGameObjectsWithTag("obstacle");
        this.rockPositions = new Vector3[3];

        for (int i = 0; i < rocks.Length; i++) {
            this.rockPositions[i] = rocks[i].transform.position;
        }
    }

    // Player has collided with a rock, set game over.
    public void PlayerCollided() {

        // Set gameOver to true, so Player can't jump.
        gameOver = true;

        // Show the game over menu.
        gameOverMenu.SetActive(true);
    }

    // Player has started the game (touched/clicked for the first time).
    public void PlayerStartedGame() {
        playerActive = true;
    }

    // Player has clicked Play on the main menu.
    public void EnterGame() {

        // Disable the main menu camera & UI.
        mainMenu.SetActive(false);
        gameStarted = true;
    }

    // Player has clicked Restart after game over.
    public void RestartGame() {

        // Reset Rocks.
        this.ResetRocks();

        // Reset Player.
        this.ResetPlayer();

        // Reset Coins.
        this.ResetCoins();

        // Reset variables.
        gameOver = false;
        playerActive = false;
        coinSpawningStarted = false;

        // Disable the game over menu.
        gameOverMenu.SetActive(false);
    }

    // Player has clicked Main Menu after game over.
    public void ShowMainMenu() {

        // Reset Rocks.
        this.ResetRocks();

        // Reset Player.
        this.ResetPlayer();

        // Reset Coins.
        this.ResetCoins();

        // Reset variables.
        gameOver = false;
        playerActive = false;
        gameStarted = false;

        // Disable the game over menu.
        gameOverMenu.SetActive(false);

        // Enable the main menu.
        mainMenu.SetActive(true);

    }

    private void ResetRocks() {

        // Reset Rocks to original positions.
        for (int i = 0; i < rocks.Length; i++) {
            rocks[i].transform.position = rockPositions[i];
        }
    }

    private void ResetPlayer() {

        // Reset Player to original position.
        Player player = FindObjectOfType<Player>();
        player.ResetPlayer();
    }

    private void ResetCoins() {

        // Find all Coin objects and destroy them.
        GameObject[] coins = GameObject.FindGameObjectsWithTag("coin");
        foreach (GameObject coin in coins)
            Destroy(coin);
    }
}
