using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Player Movement")]
	public float defaultSpeed = 200f;
	public float runningSpeed = 400f;
	public float upSpeed = 200f;

	[Header("Camera Movement")]
	public float mouseSensitivity = 100f;

	[Header("In-Script Variables")]
	private Rigidbody dragonRb;
	private Animator animator;

	private float currentSpeed;
	private float rotationOnAxisX = 0f;
	private float rotationOnAxisY = 0f;
	private float rotationOnAxisZ = 0f;
	[SerializeField] private const float rotationOnAxisZForce = 50f;
	private const float lerpFactor = 0.5f;

	private void Start() {
		dragonRb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate() {
		// -------------- Player movement ----------------

		if(Input.GetKey(KeyCode.LeftShift)) {
			currentSpeed = runningSpeed;
			animator.SetFloat("Run Multiplier", 2f);
		}
		else {
			currentSpeed = defaultSpeed;
			animator.SetFloat("Run Multiplier", 1f);
		}

		float zInput = Input.GetAxis("Vertical") * Time.deltaTime;

		dragonRb.AddForce(transform.forward * zInput * currentSpeed, ForceMode.VelocityChange);
		animator.SetFloat("Speed", zInput * currentSpeed);

		if(Input.GetKey(KeyCode.Space))
			dragonRb.AddForce(Vector3.up * upSpeed, ForceMode.VelocityChange);
		if(Input.GetKey(KeyCode.LeftControl))
			dragonRb.AddForce(Vector3.up * -1 * upSpeed, ForceMode.VelocityChange);

		float xInput = Input.GetAxis("Horizontal") * Time.deltaTime;

 		rotationOnAxisZ -= xInput * rotationOnAxisZForce; // Z axis rotation is driven by A and D keys
		rotationOnAxisZ = Mathf.Clamp(rotationOnAxisZ, -60f, 60f);

		// -------------- Camera movement ----------------
		Cursor.lockState = CursorLockMode.Locked;

		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		rotationOnAxisX -= mouseY; // += is inverted
		rotationOnAxisY += mouseX;

		// Lerping to 0 so it wont be a continuous rotation
		rotationOnAxisX = Mathf.Lerp(rotationOnAxisX, 0f, lerpFactor);
		rotationOnAxisY = Mathf.Lerp(rotationOnAxisY, 0f, lerpFactor);
		rotationOnAxisZ = Mathf.Lerp(rotationOnAxisZ, 0f, lerpFactor);

		transform.Rotate(new Vector3(rotationOnAxisX, rotationOnAxisY, rotationOnAxisZ), Space.Self);
	}
}
