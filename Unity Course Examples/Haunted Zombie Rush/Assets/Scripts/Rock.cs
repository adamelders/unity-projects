using System.Collections;
using UnityEngine;

public class Rock : Platform {

    [SerializeField] Vector3 topPosition;
    [SerializeField] Vector3 bottomPosition;
    [SerializeField] float speed;

    // Use this for initialization
    void Start () {

        // Start moving rocks.
        StartCoroutine(Move(bottomPosition));
	}

    // Update is called once per frame
    protected override void Update() {

        // If the player is not active, the rocks should not move.
        if (GameManager.instance.PlayerActive) {

            // Move the Rock along with the platform
            base.Update();
        }
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
