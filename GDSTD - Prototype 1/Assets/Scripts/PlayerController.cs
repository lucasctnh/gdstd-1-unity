using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[Header("Player Movement")]
	public float defaultSpeed = 200f;
	public float runningSpeed = 400f;
	public float upSpeed = 200f;

	[Header("Camera Movement")]
	public float mouseSensitivity = 100f;

	[Header("Aditional Settings")]
	public ParticleSystem fireBreath;
	public float fireBreathAnimationDelay = 0.5f;
	public float fireBreathDuration = 1.5f;

	[Header("In-Script Variables")]
	private Rigidbody _dragonRb;
	private Animator _anim;
	private InputReader _inputReader;

	private int _numberOfAttackClicks = 0;
	private float _currentSpeed;
	private float _rotationOnAxisX, _rotationOnAxisY, _rotationOnAxisZ;
	[SerializeField] private const float _rotationOnAxisZForce = 50f;
	private const float lerpFactor = 0.5f;

	private void Awake() {
		_dragonRb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
		_inputReader = GetComponent<InputReader>();
	}

	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void FixedUpdate() {
		bool isRunning = _inputReader.ReadIfRunning();
		_currentSpeed = isRunning ? runningSpeed : defaultSpeed;
		_anim.SetFloat("Speed Multiplier", isRunning ? 2f : 1f);

		MovePlayer(_inputReader.ReadVerticalInput(transform), _currentSpeed);
		_anim.SetFloat("Speed", _inputReader.VerticalInput * _currentSpeed);

		MovePlayer(_inputReader.ReadUpDownInput(), upSpeed);

		// Z axis rotation is driven by A and D keys
		float horizontalInput = _inputReader.ReadHorizontalInput(_rotationOnAxisZForce);
		if (horizontalInput == 0)
			LerpRotationZ();

		_rotationOnAxisZ -= horizontalInput;
		_rotationOnAxisZ = Mathf.Clamp(_rotationOnAxisZ, -60f, 60f);

		_rotationOnAxisX -= _inputReader.MouseY; // += is inverted
		_rotationOnAxisY += _inputReader.MouseX;

		Vector3 rotation = new Vector3(_rotationOnAxisX, _rotationOnAxisY, _rotationOnAxisZ);
		LerpRotation(rotation); // Lerping to 0 so it wont be a continuous rotation

		RotatePlayer(rotation);

		if (_inputReader.ReadIfReadyToAttack()) {
			if (!fireBreath.isPlaying)
				_numberOfAttackClicks++;

			Attack();
		}
	}

	private void MovePlayer(Vector3 force, float speed) {
		_dragonRb.AddForce(force * speed, ForceMode.VelocityChange);
	}

	private void LerpRotation(Vector3 rotation) {
		_rotationOnAxisX = Mathf.Lerp(rotation.x, 0f, lerpFactor);
		_rotationOnAxisY = Mathf.Lerp(rotation.y, 0f, lerpFactor);
		_rotationOnAxisZ = Mathf.Lerp(rotation.z, 0f, lerpFactor);
	}

	private void LerpRotationZ() {
		Quaternion zRotationToZero = Quaternion.identity;
		zRotationToZero.eulerAngles = new Vector3(transform.rotation.eulerAngles.x,
			transform.rotation.eulerAngles.y, 0f);

		transform.rotation = Quaternion.Lerp(transform.rotation, zRotationToZero, lerpFactor * Time.deltaTime);
	}

	private void RotatePlayer(Vector3 rotation) {
		transform.Rotate(rotation, Space.Self);
	}

	private void Attack() {
		if (_numberOfAttackClicks == 1)
			StartCoroutine(PlayFireBreath());
	}

	private IEnumerator PlayFireBreath() {
		_anim.SetTrigger("Attack");
		yield return new WaitForSeconds(fireBreathAnimationDelay);

		fireBreath.Play();
		yield return new WaitForSeconds(fireBreathDuration);
		fireBreath.Stop();

		_numberOfAttackClicks = 0;
	}
}
