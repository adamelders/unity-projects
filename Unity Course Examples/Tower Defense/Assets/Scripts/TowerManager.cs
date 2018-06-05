using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

    public TowerBtn towerBtnPressed { get; set; }
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        
        // Get the SpriteRenderer attached to TowerManager
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            if (hit.collider.tag == "BuildSite") {
                hit.collider.tag = "BuildSiteFull"; // Change the tag so we can only place one tower on each build site
                PlaceTower(hit);
            }
        }

        // Set the tower sprite to follow the mouse cursor.
        if (spriteRenderer.enabled) {
            FollowMouse();
        }
    }

    public void PlaceTower(RaycastHit2D hit) {

        // Create a new TowerObject and place it in the chosen location
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null) {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            DisableDragSprite(); // Disable the sprite that follows the mouse
        }
    }

    public void SelectedTower(TowerBtn towerSelected) {

        // Set the tower type depending on the button
        towerBtnPressed = towerSelected;

        // Enable a sprite to be dragged around with the mouse
        EnableDragSprite(towerBtnPressed.DragSprite);
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
