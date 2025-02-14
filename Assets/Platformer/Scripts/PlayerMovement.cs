using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public Camera camera;

	public Rigidbody rb;
	public Collider collider;
	public SpriteRenderer spriteRenderer;

	private float speed = 0.1f;
	private bool isRunning = false;
	private float jumpForce = 1.6f;
	private float currentJump = 0f;
	private float maxJump = 6f;
	private bool isJumping = false;

	private float maxSpeed = 2f;
	private float accel = 1f;

	private bool isGrounded = true;
	private bool hitHead = false;
	private bool superMario = false;
	private bool fireMario = false;


	public TextMeshProUGUI score;
	public TextMeshProUGUI coins;
	private int currentScore = 0;
	private int currentCoins = 0;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
		if(transform.position.x - camera.transform.position.x > 1) {
			camera.transform.position = new Vector3(transform.position.x - 1, camera.transform.position.y, camera.transform.position.z);
		}

		// Test hit block
		if(Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 100)) {
				Debug.Log(hit.transform.name);
				Debug.Log("hit");
				if(hit.transform.name == "Question(Clone)") {
					if(currentCoins < 99) {
						currentCoins++;
					} else {
						currentCoins = 0;
						currentScore += 1000;
					}
					
					currentScore += 200;
				}
				if(hit.transform.name == "Brick(Clone)") {
					currentScore += 100;
					Destroy(hit.transform.gameObject);
				}
			}
		}

		if(currentScore > 999900) {
			currentScore = 999900;
		}

		score.text = currentScore.ToString("D6");
		coins.text = "X" + currentCoins.ToString("D2");

		// Sprint
		if(isGrounded && Input.GetKeyDown(KeyCode.LeftShift)) {
			isRunning = true;
			speed *= 2;
		}
		if(isGrounded && isRunning && !Input.GetKey(KeyCode.LeftShift)) {
			isRunning = false;
			speed /= 2;
		}

		

		

		if(isGrounded) {
			currentJump = 0f;
		}
	}

	private void FixedUpdate()
	{
		float moveInput = 0f;

		// Move Right
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			spriteRenderer.flipX = false;
			moveInput = 1f;
		}

		// Move Left
		if(camera.transform.position.x - transform.position.x < 14.25 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) {
			spriteRenderer.flipX = true;
			moveInput = -1f;
		}

		//if(Mathf.Abs(rb.linearVelocity.x) < maxSpeed) {
		//	rb.AddForce(Vector3.right * moveInput * accel, ForceMode.VelocityChange);
		//}

		transform.Translate(Vector3.right * moveInput * speed);

		// Jump
		if((isJumping || isGrounded) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))) {
			isJumping = true;
			isGrounded = false;

			if(currentJump >= maxJump || HitHead()) {
				isJumping = false;
				currentJump = 0f;
			} else {
				rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
				currentJump += 1f;
			}
		}

		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow)) {
			isJumping = false;
			currentJump = 0f;
		}
	}

	private void HitBlock(RaycastHit block)
	{
		if(block.transform.name == "Brick(Clone)") {
			currentScore += 100;
			Destroy(block.transform.gameObject);
		}
		if(block.transform.name == "Question(Clone)") {
			if(currentCoins < 99) {
				currentCoins++;
			} else {
				currentCoins = 0;
				currentScore += 1000;
			}

			currentScore += 200;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if((collider.bounds.center.y - collider.bounds.extents.y) - (collision.collider.bounds.center.y - collision.collider.bounds.extents.y) > 0) {
			isGrounded = true;
		}
	}

	bool HitHead()
	{
		Ray ray = new Ray(transform.position, Vector2.up);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, collider.bounds.extents.y)) {
			HitBlock(hit);
			return true;
		}

		return false;
	}
}
