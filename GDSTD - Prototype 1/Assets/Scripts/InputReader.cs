using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour {
	public float VerticalInput { get { return Input.GetAxis("Vertical"); } }
	public float HorizontalInput { get { return Input.GetAxis("Horizontal"); } }
	public float MouseX { get { return Input.GetAxis("Mouse X"); } }
	public float MouseY { get { return Input.GetAxis("Mouse Y"); } }

	internal Vector3 ReadVerticalInput(Transform dragonTransform) {
		return dragonTransform.forward * VerticalInput * Time.deltaTime;
	}

	internal float ReadHorizontalInput(float force) {
		return force * HorizontalInput * Time.deltaTime;
	}

	internal bool ReadIfRunning() {
		bool isRunning;
		if (Input.GetKey(KeyCode.LeftShift))
			isRunning = true;
		else
			isRunning = false;

		return isRunning;
	}

	internal Vector3 ReadUpDownInput() {
		float force = 0;

		if (Input.GetKey(KeyCode.Space))
			force = 1 * Time.deltaTime;
		else if (Input.GetKey(KeyCode.LeftControl))
			force = -1 * Time.deltaTime;

		return Vector3.up * force;
	}

	internal bool ReadIfReadyToAttack() {
		bool isReadyToAttack;

		if (Input.GetMouseButtonDown(0))
			isReadyToAttack = true;
		else
			isReadyToAttack = false;

		return isReadyToAttack;
	}
}