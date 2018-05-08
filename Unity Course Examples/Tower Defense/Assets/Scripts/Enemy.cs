﻿using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float navigationUpdate;

    private int target = 0;
    private Transform enemy;
    private float navigationTime = 0;

	// Use this for initialization
	private void Start () {
        enemy = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	private void Update () {
        if (waypoints != null) {
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
            GameManager.Instance.RemoveEnemyFromScreen();
            Destroy(gameObject);
        }
    }
}
