using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private bool playerActive = false;
    private bool gameOver = false;
    
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

    private void Awake() {

        // Make sure there is only one instance of GameManager running.
        // If another instance is created, destroy that object.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Don't destroy this object between scenes.
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Player has collided with a rock, set game over.
    public void PlayerCollided() {
        gameOver = true;
    }

    // Player has started the game (touched/clicked for the first time).
    public void PlayerStartedGame() {
        playerActive = true;
    }
}
