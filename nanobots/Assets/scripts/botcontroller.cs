using UnityEngine;
using System.Collections;

public class BotController : MonoBehaviour {

	private float acceleration = 50;
	private float breakAhead = 8;

	private float topSpeed = 20;
	private float gravity = 10;

	private Vector3 basePosition;

	private Vector3 targetLocation;
	private bool targetLocationSet;

	private BotPhysics botPhysics;

	private float windY;
  
	// Use this for initialization
	void Start () {
		botPhysics = GetComponent<BotPhysics> ();
		basePosition = transform.position;
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
			float remainingYDistance = p.y - targetLocation.y;

			float topXSpeed = topSpeed;
			float topYSpeed = topSpeed;
			if (Mathf.Abs (remainingXDistance) > Mathf.Abs (remainingYDistance)) {
				topYSpeed = topSpeed * Mathf.Abs (remainingYDistance / remainingXDistance);
			} else {
				topXSpeed = topSpeed * Mathf.Abs (remainingXDistance / remainingYDistance);
			}

			botPhysics.currentXSpeed = CalculateSpeed (botPhysics.currentXSpeed, remainingXDistance, topXSpeed);
			float xDistance = botPhysics.currentXSpeed * Time.deltaTime;

			botPhysics.currentYSpeed = CalculateSpeed (botPhysics.currentYSpeed, remainingYDistance, topYSpeed);
			float yDistance = botPhysics.currentYSpeed * Time.deltaTime;

			Vector2 newPosition = transform.position;
			newPosition.x += xDistance;
			newPosition.y += yDistance;
			transform.position = newPosition;

			if (Mathf.Abs (xDistance) <= 0.01f && Mathf.Abs (yDistance) <= 0.01f && Mathf.Abs (botPhysics.currentXSpeed) <= 0.085f && Mathf.Abs (botPhysics.currentYSpeed) <= 0.08f) {
				transform.position = targetLocation;
				basePosition = targetLocation;
				targetLocationSet = false;
				Hover ();
			}
		} else {
			Hover ();
		}
	}

	private void Hover() {
		Vector2 position = transform.position;
		if (basePosition.y == position.y && windY == 0) {
			botPhysics.currentYSpeed = 0;
			windY += 6;
		}

		if (windY > 0) {
			botPhysics.currentYSpeed -= 0.06f;
			windY -= 1;
		} else if (position.y < basePosition.y) {
			botPhysics.currentYSpeed += 0.06f;		
		} else if (position.y > basePosition.y) {
			botPhysics.currentYSpeed -= 0.06f;		
		}

		float yDistance = botPhysics.currentYSpeed * Time.deltaTime;
		position.y += yDistance;
		transform.position = position;
	}

	private float CalculateSpeed (float currentSpeed, float distance, float targetSpeed) {
		float direction = (distance > 0 ? -1 : 1);
		
		if (Mathf.Abs (distance) < Mathf.Abs (breakAhead * (currentSpeed * Time.deltaTime))) {
			currentSpeed = IncrementTowards (currentSpeed, 0, acceleration * 2);
		} else {
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed * direction, acceleration);
		}
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
