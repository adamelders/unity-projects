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

    // Awake is called before Start
    private void Awake() {

        // Use assertions to make sure that the sound effects are not null.
        Assert.IsNotNull(sfxJump);
        Assert.IsNotNull(sfxDeath);
    }

    // Initialize by getting the Animator, Rigidbody, and AudioSource of the Player object.
    void Start () {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        // Do not allow jumping if the game is over.
        if (!GameManager.instance.GameOver) {

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

        // Player dies if they collide with an obstacle.
        if (collision.gameObject.tag == "obstacle") {
            
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
}
