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
      ThrowRope (Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
	}

  private void ThrowRope (Vector3 screenPoint) {
    screenPoint.z = 0;
    
    BoxCollider boxCollider = GetComponent<BoxCollider> ();
    
    Vector3 topOfPlayerPosition = transform.position;
    
    topOfPlayerPosition.x -= boxCollider.size.x/2;
    topOfPlayerPosition.y += boxCollider.size.y/2;
    
    float ropeLength = Mathf.Sqrt(
      Mathf.Pow(topOfPlayerPosition.x - screenPoint.x, 2f) + 
      Mathf.Pow(topOfPlayerPosition.y - screenPoint.y, 2f)
      );
    
    float segmentLength = 0.25f;
    Vector3 segmentScale = new Vector3(0.05f, 0.05f, segmentLength);
    rope.hingeJoint.anchor = new Vector3(0f, 0f, -segmentLength/2f);
    
    Rigidbody connection = transform.rigidbody;
    float segmentsCount = ropeLength/segmentLength;
    
    float xDistance = (topOfPlayerPosition.x - screenPoint.x) / segmentsCount;
    float yDistance = (topOfPlayerPosition.y - screenPoint.y) / segmentsCount;

    for (float i=0f; i<segmentsCount; ++i) {
      rope.hingeJoint.connectedBody = connection;
      
      Vector3 ropePosition = new Vector3(
        topOfPlayerPosition.x - (xDistance * (i + 0.5f)),
        topOfPlayerPosition.y - (yDistance * (i + 0.5f))
      );
      
      Vector3 relativePos = screenPoint - ropePosition;
      Quaternion ropeRotation = Quaternion.LookRotation(relativePos);
      
      Transform ropeTransform = (Instantiate (rope, ropePosition, ropeRotation) as GameObject).transform;
      ropeTransform.localScale = segmentScale;
      connection = ropeTransform.rigidbody;
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
