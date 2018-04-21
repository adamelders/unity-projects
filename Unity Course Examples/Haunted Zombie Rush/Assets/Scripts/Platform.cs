using UnityEngine;

public class Platform : MonoBehaviour {

    [SerializeField] private float objectSpeed = 2;
    [SerializeField] private float resetPosition = 30f;
    [SerializeField] private float startPosition = -82f;
	
	// Update is called once per frame
	protected virtual void Update () {

        // If the game is over, or the game hasn't started, don't move any objects.
        if (!GameManager.instance.GameOver || GameManager.instance.GameStarted) {

            // Workaround because if I rotate the coin, it's blank on the other side.
            if (gameObject.tag == "coin")
                transform.Translate(Vector3.right * (objectSpeed * Time.deltaTime));

            // Move the platform left at objectSpeed.
            else
                transform.Translate(Vector3.left * (objectSpeed * Time.deltaTime));

            // When the first platform reaches a certain position on the X axis (off the screen),
            // we need to move that platform back to the right side, to make the platform seem never-ending.
            if (transform.localPosition.x > resetPosition) {
                Vector3 newPos = new Vector3(startPosition, transform.position.y, transform.position.z);
                transform.position = newPos;
            }
        }
	}
}
