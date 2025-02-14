using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public Camera camera;

	public Rigidbody rb;
	public Collider collider;
	public LayerMask blockLayer;

	private float speed = 5f;
	private bool isRunning = false;
	private float jumpForce = 10f;
	private float currentJump = 0f;
	private float maxJump = 90f;
	private bool isJumping = false;

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

		// Move Right
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}

		// Move Left
		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}

		Debug.Log("currentJump: " + currentJump);
		// Jump
		if((isJumping || isGrounded) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))) {
			isJumping = true;
			isGrounded = false;
			
			if(currentJump >= maxJump || HitHead()) {
				isJumping = false;
				currentJump = 0f;
			} else {
				transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
				currentJump += 1f;
			}
		}

		if(isGrounded) {
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
		isGrounded = true;
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
