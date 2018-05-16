using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float navigationUpdate;
    [SerializeField] private int healthPoints;

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
            GameManager.Instance.UnregisterEnemy(this);
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
            animator.Play("Hurt"); // Play Hurt animation
        }
        else {
            animator.SetTrigger("DidDie"); // Play Dying animation
            Die();
        }
    }

    public void Die() {
        isDead = true;
        enemyCollider.enabled = false;
    }
}
