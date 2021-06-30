using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[Header("Camera settings")]
	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	[Header("In-script Variables")]
	private Transform focus;

	private void Awake() {
		focus = GameObject.Find("Camera Focus").transform;
	}

	private void FixedUpdate() {
		Vector3 desiredPosition = focus.TransformPoint(offset);
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;

		transform.LookAt(focus);
	}
}
