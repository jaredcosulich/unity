using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics))]

public class PlayerController : MonoBehaviour {

	// Player Handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
  public float jumpHeight = 12;

	private float currentSpeed;
	private float targetSpeed;

	private Vector2 amountToMove;
	private PlayerPhysics playerPhysics;

  private Animator animator;
  private SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics> ();
    animator = GetComponent<Animator>();
  }


  // Update is called once per frame
  void Update () {
    if (playerPhysics.movementStopped) {
      targetSpeed = 0;
      currentSpeed = 0;
    }
    
    float direction = Input.GetAxisRaw ("Horizontal");
    
    if (Mathf.Abs(direction) == 1) {
      setDirection((int) direction);
      animator.SetInteger ("Direction", 1);
    } else {
      animator.SetInteger ("Direction", 0);
    }
    
    targetSpeed = direction * speed;
    currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);
    
    if (playerPhysics.grounded) {
      animator.SetInteger ("Jumping", 0);
      amountToMove.y = 0;
      if (Input.GetButtonDown ("Jump")) {
        animator.SetInteger ("Jumping", 1);
        amountToMove.y = jumpHeight;
      }
    }
    
    amountToMove.x = currentSpeed;
    amountToMove.y -= gravity * Time.deltaTime;

    if (animator.GetInteger ("Jumping") == 1 && amountToMove.y < 0) {
      animator.SetInteger ("Jumping", -1);
    }

    playerPhysics.Move (amountToMove * Time.deltaTime);   
  }
  
  private void setDirection (int direction) {
    Vector3 localScale = transform.localScale;
    if (localScale.x * direction > 0) {
      localScale.x *= -1;
      transform.localScale = localScale;
    }
  }

  private float IncrementTowards (float current, float target, float acceleration) {
		if (current == target) {
			return current;
		} else {
			float direction = Mathf.Sign (target - current);
			current += acceleration * Time.deltaTime * direction;
			return (direction == Mathf.Sign (target - current) ? current : target);
		}
	}
}
