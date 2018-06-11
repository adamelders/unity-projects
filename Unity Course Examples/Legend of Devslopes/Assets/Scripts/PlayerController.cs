using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private LayerMask layerMask;

    private CharacterController characterController;
    private Vector3 currentLookTarget = Vector3.zero;
    private Animator anim;

	// Use this for initialization
	void Start () {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);

        // Set walking animation
        if (moveDirection == Vector3.zero)
            anim.SetBool("IsWalking", false);
        else
            anim.SetBool("IsWalking", true);

        // Left mouse button clicked
        if (Input.GetMouseButtonDown(0))
            anim.Play("DoubleChop");

        // Right mouse button clicked
        if (Input.GetMouseButtonDown(1))
            anim.Play("SpinAttack");
	}

    private void FixedUpdate() {

        // Create a ray from the main camera to the mouse cursor position.
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Draw a ray in the Scene window so we can see it for debugging purposes.
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.blue);

        // Create the raycast, ignore any other trigger interactions.
        if (Physics.Raycast(ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore)) {

            // Set the current look target, if not set already.
            if (hit.point != currentLookTarget)
                currentLookTarget = hit.point;

            // Rotate the character towards the raycast hit point.
            Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
}
