using UnityEngine;

public class Coin : Platform {

    [SerializeField] private float coinResetPosition = 10f;

    // Update is called once per frame
    protected override void Update () {

        // If the player is not active, the coins should not move.
        if (GameManager.instance.PlayerActive) {

            // Move the coins along with the platform.
            base.Update();

            // If the coins are missed, destroy them at a certain point.
            if (transform.localPosition.x > coinResetPosition)
                Destroy(gameObject);
        }
	}
}
