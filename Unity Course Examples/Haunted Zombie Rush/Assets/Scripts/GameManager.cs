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

        // Disable the game over menu.
        gameOverMenu.SetActive(false);
    }
}
