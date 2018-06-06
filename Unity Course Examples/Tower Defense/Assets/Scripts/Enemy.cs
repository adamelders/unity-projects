using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float navigationUpdate;
    [SerializeField] private int healthPoints;
    [SerializeField] private int rewardAmount;

    private int target = 0;
    private Transform enemy;
    private Collider2D enemyCollider;
    private Animator animator;
    private float navigationTime = 0;
    private bool isDead = false;

    public bool IsDead {
        get {
            return isDead;
        }
    }

	// Use this for initialization
	private void Start () {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        GameManager.Instance.RegisterEnemy(this);
	}
	
	// Update is called once per frame
	private void Update () {
        if (waypoints != null && !isDead) {
            navigationTime += Time.deltaTime;
            
            if (navigationTime > navigationUpdate) {

                if (target < waypoints.Length)
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);
                else
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);

                navigationTime = 0;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "Checkpoint")
            target++;
        else if (other.tag == "Finish") {
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.UnregisterEnemy(this);
            GameManager.Instance.IsWaveOver();
        }
        else if (other.tag == "Projectile") {
            Projectile newP = other.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackStrength);
            Destroy(other.gameObject);
        }
    }

    public void EnemyHit(int hitPoints) {
        if ((healthPoints - hitPoints) > 0) {
            healthPoints -= hitPoints;
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
            animator.Play("Hurt"); // Play Hurt animation
        }
        else {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
            animator.SetTrigger("DidDie"); // Play Dying animation
            Die();
        }
    }

    public void Die() {
        isDead = true;
        enemyCollider.enabled = false;
        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.AddMoney(rewardAmount);
        GameManager.Instance.IsWaveOver();
    }
}
