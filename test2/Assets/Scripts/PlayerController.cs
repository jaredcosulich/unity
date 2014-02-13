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
  public float ropeVelocity = 20f;
  private bool throwingRope = false;
  private Vector3 ropeTarget;
  private Rigidbody ropeConnection;
  private Vector3 ropePosition;
  private float totalRopeLength = 0;
  private float currentRopeLength = 0;
  private float instantiatedRopeLength = 0;
  private float segmentCount = 0;
  private float segmentLength = 0.25f;

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
      ropePosition = transform.position;   
      ropePosition.x -= boxCollider.size.x/2;
      ropePosition.y += boxCollider.size.y/2;

      ropeTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      ropeTarget.z = 0;

      totalRopeLength = Mathf.Sqrt(
        Mathf.Pow(ropePosition.x - ropeTarget.x, 2f) + 
        Mathf.Pow(ropePosition.y - ropeTarget.y, 2f)
      );

      ropeConnection = transform.rigidbody;

      throwingRope = true;
    }

    if (throwingRope) {
      ThrowRope ();
    }

    if (ropeConnection && !throwingRope) {
      FinishRope ();
    }
	}

  private void ThrowRope () {
    currentRopeLength += ropeVelocity * Time.deltaTime;
    if (currentRopeLength > totalRopeLength) {
      currentRopeLength = totalRopeLength;
    }

    float ropeLength = currentRopeLength - instantiatedRopeLength;
    
    if (currentRopeLength + ropeLength >= totalRopeLength) {
      BuildSegments (totalRopeLength - instantiatedRopeLength);
      FinishRope ();
    } else {
      BuildSegments (ropeLength);
    }
  }

  private void BuildSegments (float ropeLength) {
    Vector3 segmentScale = new Vector3(0.025f, 0.025f, segmentLength);
    rope.hingeJoint.anchor = new Vector3(0f, 0f, -segmentLength/2f);
    
    float segmentsCount = Mathf.Floor (ropeLength/segmentLength);
    
    float totalSegments = Mathf.Ceil (totalRopeLength / segmentLength);
    float xDistance = (ropePosition.x - ropeTarget.x) / totalSegments;
    float yDistance = (ropePosition.y - ropeTarget.y) / totalSegments;
    
    for (float i=0f; i<segmentsCount; ++i) {
      Vector3 segmentPosition = new Vector3(
        ropePosition.x - (xDistance * (segmentCount + 0.5f)),
        ropePosition.y - (yDistance * (segmentCount + 0.5f))
      );
      
      Vector3 relativePos = ropeTarget - ropePosition;
      Quaternion ropeRotation = Quaternion.LookRotation(relativePos);
      
      Transform ropeTransform = (Instantiate (rope, segmentPosition, ropeRotation) as GameObject).transform;
      ropeTransform.localScale = segmentScale;

      ropeTransform.hingeJoint.connectedBody = ropeConnection;

      instantiatedRopeLength += segmentLength;
      ropeConnection = ropeTransform.rigidbody;
      segmentCount++;
    }
  }

  public void FinishRope() {
    throwingRope = false;
   
    Ray ray = new Ray (ropeConnection.position, ropeConnection.transform.forward);
    Debug.DrawRay (ropeConnection.position, ropeConnection.transform.forward);

//    if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask)) {
//      float distance = Vector3.Distance(ray.origin, hit.point);
//      
//      if (distance > skin) {
//        deltaY = (distance * direction) - (skin * direction);
//      } else {
//        deltaY = 0;
//      }
//      grounded = true;
//      break;
//    }


    Rigidbody connection = ropeConnection;
    while (!connection.useGravity) {
//      connection.useGravity = true;
      if (!connection.hingeJoint) {
        break;
      }
      connection = connection.hingeJoint.connectedBody;
    }

    currentRopeLength = 0;
    instantiatedRopeLength = 0;
    segmentCount = 0;
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
