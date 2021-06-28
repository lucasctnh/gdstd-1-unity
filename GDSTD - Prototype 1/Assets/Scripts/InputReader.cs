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
		int direction = 0;

		if (Input.GetKey(KeyCode.Space))
			direction = 1;
		else if (Input.GetKey(KeyCode.LeftControl))
			direction = -1;

		return Vector3.up * direction;
	}
}