using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public Camera camera;

	public Rigidbody rb;

	private float speed = 3f;
	private Boolean isRunning = false;
	private float jumpForce = 10f;
	private float currentJump = 0f;
	private float maxJump = 100f;

	private Boolean isGrounded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 currentPosition = transform.position;

		if(Input.GetKeyDown(KeyCode.LeftShift)) {
			isRunning = true;
			speed *= 2;
		}
		if(Input.GetKeyUp(KeyCode.LeftShift)) {
			isRunning = false;
			speed /= 2;
		}

		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}

		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}

		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) {
			isGrounded = false;

			if(currentJump < maxJump) {
				transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
				currentJump += 1f;
			}
		}
	}

	private void FixedUpdate()
	{
		//Debug.Log(currentJump);

		//if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) {
		//	if(currentJump < maxJump) {
		//		rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		//		currentJump += 1f;
		//	}
		//}
	}

	private void OnCollisionEnter(Collision collision)
	{
		currentJump = 0f;
		isGrounded = true;
	}
}
