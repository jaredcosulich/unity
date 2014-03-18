using UnityEngine;
using System.Collections;

public class BotController : MonoBehaviour {

	private float acceleration = 50;
	private float breakAhead = 8;

	private float topSpeed = 20;
	private float gravity = 10;

	private BotPhysics botPhysics;

	private float windY;
  
	// Use this for initialization
	void Start () {
		botPhysics = GetComponent<BotPhysics> ();
  }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 clickLocation = Input.mousePosition;
			clickLocation.z = 10;
			botPhysics.targetLocation = Camera.main.ScreenToWorldPoint(clickLocation);
		}

		Vector3 p = transform.position;

		float remainingXDistance = p.x - botPhysics.targetLocation.x;
		float remainingYDistance = p.y - botPhysics.targetLocation.y;

		if (Mathf.Abs (remainingXDistance) > Random.value/4 || Mathf.Abs (remainingYDistance) > Random.value/4) {
			float topXSpeed = topSpeed;
			float topYSpeed = topSpeed;
			if (Mathf.Abs (remainingXDistance) > Mathf.Abs (remainingYDistance)) {
				topYSpeed = topSpeed * Mathf.Abs (remainingYDistance / remainingXDistance);
			} else {
				topXSpeed = topSpeed * Mathf.Abs (remainingXDistance / remainingYDistance);
			}
			
			botPhysics.currentXSpeed = CalculateSpeed (botPhysics.currentXSpeed, remainingXDistance, topXSpeed);
			botPhysics.currentYSpeed = CalculateSpeed (botPhysics.currentYSpeed, remainingYDistance, topYSpeed);
		}

		float xDistance = botPhysics.currentXSpeed * Time.deltaTime;
    	float yDistance = botPhysics.currentYSpeed * Time.deltaTime;

		Vector2 newPosition = transform.position;
		newPosition.x += xDistance;
		newPosition.y += yDistance;
		transform.position = newPosition;
	}
	
	private float CalculateSpeed (float currentSpeed, float distance, float targetSpeed) {
		float direction = (distance > 0 ? -1 : 1);

		if (Mathf.Abs (distance) < Mathf.Abs (breakAhead * (currentSpeed * Time.deltaTime))) {
			currentSpeed = IncrementTowards (currentSpeed, 0, acceleration * 2);
		} else {
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed * direction, acceleration);
		}
		return currentSpeed;
		return IncrementTowards (currentSpeed, targetSpeed * direction, acceleration);		
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
