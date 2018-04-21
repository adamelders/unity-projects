using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {

    public GameObject coin;
    private float randomYStart = 21f;
    private float randomYEnd = 8f;
    private float startX = -11.61f;
    private float startZ = -0.33f;
    private Quaternion startRotation = Quaternion.Euler(90, 0, 0);

    private void Update() {

        // Only spawn coins if the game has started.
        if (GameManager.instance.PlayerActive && !GameManager.instance.CoinSpawningStarted) {

            // Start spawning coins.
            StartCoroutine(SpawnCoin());

            // Set property so we don't keep spawning coins.
            GameManager.instance.CoinSpawningStarted = true;
        }
    }

    IEnumerator SpawnCoin() {

        // Wait a random amount of time to spawn new coins.
        yield return new WaitForSeconds(Random.Range(2f, 7f));

        // Only spawn a coin if spawning has started. Prevents
        // spawning after a reset.
        if (GameManager.instance.CoinSpawningStarted) {

            // Spawn a coin in a random Y position.
            float randomY = Random.Range(randomYStart, randomYEnd);
            Instantiate(coin, new Vector3(startX, randomY, startZ), startRotation);

            // Keep spawning coins, as long as the player is active,
            // and coins have started spawning.
            if (GameManager.instance.PlayerActive)
                StartCoroutine(SpawnCoin());

        }
    }
}
