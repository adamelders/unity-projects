using System.Collections;
using UnityEngine;

public class Rock : Platform {

    [SerializeField] Vector3 topPosition;
    [SerializeField] Vector3 bottomPosition;
    [SerializeField] float speed;

    private GameObject[] rocks;
    private Vector3 rock1OriginalPosition;
    private Vector3 rock2OriginalPosition;
    private Vector3 rock3OriginalPosition;

    // Use this for initialization
    void Start () {

        // Store the rock positions for restting later.
        rocks = GameObject.FindGameObjectsWithTag("obstacle");
        rock1OriginalPosition = rocks[0].transform.position;
        rock2OriginalPosition = rocks[1].transform.position;
        rock3OriginalPosition = rocks[2].transform.position;

        StartCoroutine(Move(bottomPosition));
	}

    // Update is called once per frame
    protected override void Update() {

        // If the game has been restarted, reset the rock positions.
        if (GameManager.instance.GameRestarted)
            ResetRocks();

        // If the player is not active, the rocks should not move.
        if (GameManager.instance.PlayerActive) {

            // Move the Rock along with the platform
            base.Update();
        }
    }

    private void ResetRocks() {

        // Reset rock positions.
        rocks[0].transform.position = rock1OriginalPosition;
        rocks[1].transform.position = rock2OriginalPosition;
        rocks[2].transform.position = rock3OriginalPosition;
    }

    // Coroutine to move the rock up and down.
    IEnumerator Move(Vector3 target) {

        // While the y axis of the rock object has not reached our target yet
        while (Mathf.Abs((target - transform.localPosition).y) > 0.20f) {

            // If we're at the top limit, our direction is up, otherwise down
            Vector3 direction = target.y == topPosition.y ? Vector3.up : Vector3.down;

            // Move the rock object in our current direction
            transform.localPosition += (direction * speed) * Time.deltaTime;

            // Return null so the rocks never stop moving
            yield return null;
        }

        // Wait half a second when we reach our target
        yield return new WaitForSeconds(0.5f);

        // Set our new target/direction to the opposite side
        Vector3 newTarget = target.y == topPosition.y ? bottomPosition : topPosition;

        // Start moving the rock to the new target
        StartCoroutine(Move(newTarget));
    }
}
