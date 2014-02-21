using UnityEngine;
using System.Collections;

public class BotController : MonoBehaviour {

	private float acceleration = 50;
	private float breakAhead = 5;

	private float topSpeed = 15;
	private float gravity = 10;
	private float currentXSpeed;
	private float currentYSpeed;

	private Vector3 targetLocation;
	private bool targetLocationSet;

	private BotPhysics botPhysics;

  
	// Use this for initialization
	void Start () {
		botPhysics = GetComponent<BotPhysics> ();
  }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 clickLocation = Input.mousePosition;
			clickLocation.z = 10;
			targetLocation = Camera.main.ScreenToWorldPoint(clickLocation);
			targetLocationSet = true;
		}

		if (targetLocationSet) {
			Vector3 p = transform.position;

			float remainingXDistance = p.x - targetLocation.x;
			currentXSpeed = CalculateSpeed (currentXSpeed, remainingXDistance);
			float xDistance = currentXSpeed * Time.deltaTime;

			float remainingYDistance = p.y - targetLocation.y;
			currentYSpeed = CalculateSpeed (currentYSpeed, remainingYDistance);
			float yDistance = currentYSpeed * Time.deltaTime;

			Vector2 newPosition = transform.position;
			newPosition.x += xDistance;
			newPosition.y += yDistance;
			transform.position = newPosition;
		}

//		amountToMove.x = currentSpeed;
//		amountToMove.y -= gravity * Time.deltaTime;
//		playerPhysics.Move (amountToMove * Time.deltaTime);		
	}

	private float CalculateSpeed (float currentSpeed, float remainingDistance) {
		float direction = (remainingDistance > 0 ? -1 : 1);
		
		if (Mathf.Abs (remainingDistance) < Mathf.Abs (breakAhead * (currentSpeed * Time.deltaTime))) {
			currentSpeed = IncrementTowards (currentSpeed, 0, acceleration * 2);
		} else {
			currentSpeed = IncrementTowards (currentSpeed, topSpeed * direction, acceleration);
		}
		return IncrementTowards (currentSpeed, topSpeed * direction, acceleration);		
	}


	private float IncrementTowards (float current, float target, float acceleration) {
		if (current == target) {
			return current;
		} else {
			float gravitizedAcceleration = acceleration;
			if (Mathf.Abs (current) < 1) {
				gravitizedAcceleration = acceleration * (Mathf.Abs (current) + 0.1f);
			} 

			float direction = Mathf.Sign (target - current);
			current += gravitizedAcceleration * Time.deltaTime * direction;
			return (direction == Mathf.Sign (target - current) ? current : target);
		}
	}

}
