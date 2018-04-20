using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float jumpForce = 100f;

    private Animator anim;
    private Rigidbody rigidBody;

    private bool jump = false;

	// Initialize by getting the Animator and Rigidbody of the Player object.
	void Start () {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        // On left click (or touch)
        if (Input.GetMouseButtonDown(0)) {

            // Play jump animation
            anim.Play("Jump");

            // Turn on gravity for player object (when the game starts, this is off)
            rigidBody.useGravity = true;

            // Trigger jumping force in FixedUpdate.
            jump = true;
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
}
