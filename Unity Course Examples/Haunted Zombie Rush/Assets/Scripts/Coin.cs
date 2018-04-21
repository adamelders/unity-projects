using System.Collections;
using UnityEngine;

public class Coin : Platform {

    [SerializeField] private float coinResetPosition = 10f;

    // Update is called once per frame
    protected override void Update () {

        // Destroy all coins if the game is restarted.
        if (GameManager.instance.GameRestarted) {
            GameObject[] coins = GameObject.FindGameObjectsWithTag("coin");
            foreach (GameObject coin in coins) {
                Destroy(coin);
            }

            // Tell GameManager that Coins are reset.
            GameManager.instance.CoinsReset = true;

            // Reset coin spawning.
            GameManager.instance.CoinSpawningStarted = false;
        }

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
