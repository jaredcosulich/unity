using UnityEngine;
using System.Collections;

public class botcontroller : MonoBehaviour {

	private float acceleration = 50;
	
	private float topSpeed = 80;
	private float currentSpeed;

	private Vector3 targetLocation;
	private bool targetLocationSet;

	// Use this for initialization
	void Start () {
	
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
			currentSpeed = IncrementTowards (currentSpeed, topSpeed, acceleration);
			Vector3 p = transform.position;

			float distance = currentSpeed * Time.deltaTime;

			if (Mathf.Sqrt(Mathf.Pow(p.x - targetLocation.x, 2) + Mathf.Pow(p.y - targetLocation.y, 2)) < distance) {
				transform.position = targetLocation;
				targetLocationSet = false;
				currentSpeed = 0;
				return;
			}

			float ratio = (p.y - targetLocation.y)/(p.x - targetLocation.x);
			float direction = (p.x - targetLocation.x) < 0 ? 1f : -1f;
			float ratioDistance = Mathf.Sqrt (1f + Mathf.Pow (ratio, 2));
	
			Vector2 amountToMove = new Vector2(direction * distance / ratioDistance, ratio * (distance / ratioDistance));
			Vector2 newPosition = transform.position;
			newPosition.x += amountToMove.x;
			newPosition.y += amountToMove.y;
			transform.position = newPosition;
		}

//		amountToMove.x = currentSpeed;
//		amountToMove.y -= gravity * Time.deltaTime;
//		playerPhysics.Move (amountToMove * Time.deltaTime);		
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
