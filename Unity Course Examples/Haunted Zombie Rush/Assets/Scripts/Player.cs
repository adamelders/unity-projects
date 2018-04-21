using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {

    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private AudioClip sfxJump;
    [SerializeField] private AudioClip sfxDeath;

    private Animator anim;
    private Rigidbody rigidBody;
    private bool jump = false;
    private AudioSource audioSource;
    private Vector3 originalPostion;

    // Awake is called before Start
    private void Awake() {

        // Use assertions to make sure that the sound effects are not null.
        Assert.IsNotNull(sfxJump);
        Assert.IsNotNull(sfxDeath);

        // Set the original position so we can reset it.
        originalPostion = gameObject.transform.position;
    }

    // Initialize by getting the Animator, Rigidbody, and AudioSource of the Player object.
    void Start () {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        // Reset player position if the game is restarted.
        if (GameManager.instance.GameRestarted)
            ResetPlayer();

        // Do not allow jumping if the game is over or hasn't started yet.
        if (!GameManager.instance.GameOver && GameManager.instance.GameStarted) {

            // On left click (or touch)
            if (Input.GetMouseButtonDown(0)) {

                // Player has started the game.
                GameManager.instance.PlayerStartedGame();

                // Play jump animation
                anim.Play("Jump");

                // Play the jump sound once.
                audioSource.PlayOneShot(sfxJump);

                // Turn on gravity for player object (when the game starts, this is off)
                rigidBody.useGravity = true;

                // Trigger jumping force in FixedUpdate.
                jump = true;
            }
        }
	}

    // FixedUpdate must be used with RigidBody, to counter variable frame rates
    private void FixedUpdate() {

        // If jump is triggered (touch/left click)
        if (jump) {

            // Change jump variable to false, so we can keep jumping
            jump = false;

            // Reset the velocity of the player object, so when you jump while falling,
            // the velocity is not a factor in applying force
            rigidBody.velocity = new Vector2(0, 0);

            // Add force impulse to player object
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
        }
    }

    // Called when any collision is detected, if detectCollisions on the object is true.
    private void OnCollisionEnter(Collision collision) {

        // Player dies if they collide with an obstacle or the platform.
        if (collision.gameObject.tag == "obstacle" || collision.gameObject.tag == "platform") {
            
            // Apply force to the player to knock them back on death.
            rigidBody.AddForce(new Vector2(50, 20), ForceMode.Impulse);

            // Turn off collisions for the player, so we don't do anything else.
            rigidBody.detectCollisions = false;

            // Play the death audio once.
            audioSource.PlayOneShot(sfxDeath);

            // End the game.
            GameManager.instance.PlayerCollided();
        }
    }

    public void ResetPlayer() {

        // Disable gravity on Player.
        rigidBody.useGravity = false;

        // Re-activate collision detection on Player.
        rigidBody.detectCollisions = true;

        // Reset Player position, rotation, and velocity.
        gameObject.transform.position = originalPostion;
        gameObject.transform.rotation = Quaternion.Euler(0, -60, 0);
        rigidBody.velocity = new Vector2(0, 0);
        rigidBody.angularVelocity = Vector3.zero;

        // Tell GameManager that Player is reset.
        GameManager.instance.PlayerReset = true;
    }
}
