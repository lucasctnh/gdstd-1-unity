using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Player Movement")]
	public float speed = 10f;

	[Header("Camera Movement")]
	public float mouseSensitivity = 100f;

	[Header("In-Script Variables")]
	private Rigidbody dragonRb;
	private Animator animator;

	private float rotationOnAxisX = 0f;
	private float rotationOnAxisY = 0f;
	private float rotationOnAxisZ = 0f;
	[SerializeField] private float rotationOnAxisZForce = 50f;
	private float lerpFactor = 0.5f;

	private void Start()
	{
		dragonRb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		// Player movement
		float zInput = Input.GetAxis("Vertical") * Time.deltaTime;

		dragonRb.AddForce(transform.forward * zInput * speed, ForceMode.VelocityChange);
		animator.SetFloat("Speed", zInput * speed);

		float xInput = Input.GetAxis("Horizontal") * Time.deltaTime;

 		rotationOnAxisZ -= xInput * rotationOnAxisZForce; // Z Rotation driven by A and D keys
		rotationOnAxisZ = Mathf.Clamp(rotationOnAxisZ, -60f, 60f);

		// Camera movement
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
