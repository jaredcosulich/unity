using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics))]

public class PlayerController : MonoBehaviour
{

	// Player Handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
  public float jumpHeight = 12;

	private float currentSpeed;
	private float targetSpeed;

	private Vector2 amountToMove;
	private PlayerPhysics playerPhysics;

  public GameObject rope;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics> ();
	}

	// Update is called once per frame
	void Update () {
    if (playerPhysics.movementStopped) {
      targetSpeed = 0;
      currentSpeed = 0;
    }
    
    targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);

    if (playerPhysics.grounded) {
      amountToMove.y = 0;
      if (Input.GetButtonDown("Jump")) {
        amountToMove.y = jumpHeight;
      }
    }

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove * Time.deltaTime);

    if (Input.GetMouseButtonDown (0)) {
      BoxCollider boxCollider = GetComponent<BoxCollider> ();

      Vector3 topOfPlayerPosition = transform.position;

      topOfPlayerPosition.x -= boxCollider.size.x/2;
      topOfPlayerPosition.y += boxCollider.size.y/2;

      Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      screenPoint.z = 0;

      Vector3 ropePosition = new Vector3(
        topOfPlayerPosition.x - ((topOfPlayerPosition.x - screenPoint.x) / 2),
        topOfPlayerPosition.y - ((topOfPlayerPosition.y - screenPoint.y) / 2)
      );

      Vector3 relativePos = screenPoint - ropePosition;
      Quaternion ropeRotation = Quaternion.LookRotation(relativePos);

      Vector3 ropeScale = new Vector3(0.05f, 0.05f, Mathf.Sqrt(
        Mathf.Pow(topOfPlayerPosition.x - screenPoint.x, 2) + 
        Mathf.Pow(topOfPlayerPosition.y - screenPoint.y, 2)
      ));

      Transform ropeTransform = (Instantiate (rope, ropePosition, ropeRotation) as GameObject).transform;
      ropeTransform.localScale = ropeScale;
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
