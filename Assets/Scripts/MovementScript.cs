using System.Collections;
using UnityEngine;
/// <summary>
/// @alex-memo 2023
/// This class is responsible for 
/// </summary>
public class MovementScript : MonoBehaviour
{
	private CharacterController controller;
	private float speed = 4f;

	private const float turnSmoothTime = .2f;
	private float turnSmoothVelocity;

	private float jumpHeight = 1.5f;

	private Animator anim;

	private Transform cam;

	private Vector3 moveVelocity;
	private float idleTimer = 0f;	

	private const float groundDistance = .2f;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private Transform groundCheck;
	private bool isGrounded => Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
	[field: SerializeField]public bool CanMove { get; set; } = true;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
		cam = Camera.main.transform;
		anim = GetComponent<Animator>();

		Cursor.lockState = CursorLockMode.Locked;
	}
	private void Update()
	{

		if (isGrounded && moveVelocity.y < 0)
		{
			moveVelocity.y = -2f;
		}
		movePlayer();
	}
	private void movePlayer()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		Vector3 _dir = new Vector3(horizontal, 0f, vertical).normalized;

		if (_dir.magnitude >= 0.01f)
		{
			if (isGrounded)
			{
				walk();
			}
			if (!CanMove)
			{
				CanMove = true;
				triggerAnim("Run");
			}

			float targetAngle = Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
			Vector3 movedir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

			controller.Move(speed * Time.deltaTime * movedir.normalized);

		}
		else
		{
			if (isGrounded)
			{
				idle();
			}
			if (!CanMove)
			{
				return;
			}
		}

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			moveVelocity.y = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
			triggerAnim("Jump");
			idleTimer = 0f;
		}

		moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		controller.Move(moveVelocity * Time.deltaTime);
	}
	private void walk()
	{

		animate(1f);

	}
	private void idle()
	{
		idleTimer += Time.deltaTime;
		if (idleTimer > 15f)
		{
			triggerAnim("Stretch");
			CanMove = false;
			idleTimer = 0f;
		}
		animate(0);

	}

	private void animate(float _value, float _dampTime = .1f)
	{
		anim.SetFloat("Speed", _value, _dampTime, Time.deltaTime);
	}
	private void triggerAnim(string _trigger)
	{
		anim.SetTrigger(_trigger);
	}
	public void resetIdleTimer()
	{
		idleTimer = 0f;
	}
}