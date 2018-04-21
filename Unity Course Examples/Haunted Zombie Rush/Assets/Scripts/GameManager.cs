using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;

    private bool playerActive = false;
    private bool gameOver = false;
    private bool gameStarted = false;
    private bool gameRestarted = false;
    private bool coinSpawningStarted = false;
    private bool playerReset = false;
    private bool rocksReset = false;
    private bool coinsReset = false;
    
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

    public bool GameRestarted {
        get {
            return gameRestarted;
        }
        set {

            // Do not set if the value is already the same.
            if (gameRestarted != value)
                gameRestarted = value;
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

    public bool PlayerReset {
        get {
            return playerReset;
        }
        set {

            // Do not set if the value is already the same.
            if (playerReset != value)
                playerReset = value;
        }
    }

    public bool RocksReset {
        get {
            return rocksReset;
        }
        set {

            // Do not set if the value is already the same.
            if (rocksReset != value)
                rocksReset = value;
        }
    }

    public bool CoinsReset {
        get {
            return coinsReset;
        }
        set {

            // Do not set if the value is already the same.
            if (coinsReset != value)
                coinsReset = value;
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
    }

    private void Update() {

        // Set "reset" values to false to make sure everything gets
        // reset before starting a new game.
        if (PlayerReset && RocksReset && CoinsReset) {
            gameRestarted = false;
            PlayerReset = false;
            RocksReset = false;
            CoinsReset = false;
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

        // Set game restarted to reset Player and Rock positions.
        gameRestarted = true;

        // Reset variables.
        gameOver = false;
        playerActive = false;
        coinSpawningStarted = false;

        // Disable the game over menu.
        gameOverMenu.SetActive(false);
    }

    // Player has clicked Main Menu after game over.
    public void ShowMainMenu() {

        // Set game restarted to reset Player and Rock positions.
        gameRestarted = true;

        // Reset variables.
        gameOver = false;
        playerActive = false;
        gameStarted = false;

        // Disable the game over menu.
        gameOverMenu.SetActive(false);

        // Enable the main menu.
        mainMenu.SetActive(true);

    }
}
