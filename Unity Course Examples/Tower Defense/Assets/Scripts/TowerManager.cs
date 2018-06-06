using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

    public TowerBtn towerBtnPressed { get; set; }
    private SpriteRenderer spriteRenderer;
    private List<Tower> towerList = new List<Tower>();
    private List<Collider2D> buildTileList = new List<Collider2D>();
    private Collider2D buildTile;

	// Use this for initialization
	void Start () {
        
        // Get the SpriteRenderer attached to TowerManager
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        buildTile = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {

        // When the user clicks a tower button
		if (Input.GetMouseButtonDown(0)) {

            // Get the point where the user clicked
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Send a raycast to find the tower button we're clicking on
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            // Place a new tower in that position, if it's on a build site
            if (hit.collider.tag == "BuildSite" && towerBtnPressed != null) {
                buildTile = hit.collider;
                buildTile.tag = "BuildSiteFull"; // Change the tag so we can only place one tower on each build site
                RegisterBuildSite(buildTile);
                PlaceTower(hit);
            }
        }

        // Set the tower sprite to follow the mouse cursor.
        if (spriteRenderer.enabled) {
            FollowMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag) {
        buildTileList.Add(buildTag);
    }

    public void RegisterTower(Tower tower) {
        towerList.Add(tower);
    }

    // Resets build site tags so we can place towers again.
    public void RenameTagsBuildSites() {
        foreach (Collider2D buildTag in buildTileList) {
            buildTag.tag = "BuildSite";
        }
        buildTileList.Clear();
    }

    public void DestroyAllTowers() {
        foreach (Tower tower in towerList) {
            Destroy(tower.gameObject);
        }
        towerList.Clear();
    }

    public void PlaceTower(RaycastHit2D hit) {

        // Create a new TowerObject and place it in the chosen location
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null) {
            Tower newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            BuyTower(towerBtnPressed.TowerPrice);
            RegisterTower(newTower);
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
            DisableDragSprite(); // Disable the sprite that follows the mouse
            towerBtnPressed = null; // Make sure we can't place multiple towers.
        }
    }

    public void BuyTower(int price) {
        GameManager.Instance.SubtractMoney(price);
    }

    public void SelectedTower(TowerBtn towerSelected) {

        // Don't let player buy a tower if they don't have the funds, or the game hasn't started.
        if (towerSelected.TowerPrice <= GameManager.Instance.TotalMoney &&
            GameManager.Instance.CurrentState != GameStatus.New) {

            // Set the tower type depending on the button
            towerBtnPressed = towerSelected;

            // Enable a sprite to be dragged around with the mouse
            EnableDragSprite(towerBtnPressed.DragSprite);
        }
    }

    public void FollowMouse() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void EnableDragSprite(Sprite sprite) {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void DisableDragSprite() {
        spriteRenderer.enabled = false;
    }
}
