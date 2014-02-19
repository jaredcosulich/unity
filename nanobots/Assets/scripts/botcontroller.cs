using UnityEngine;
using System.Collections;

public class botcontroller : MonoBehaviour {

	private float acceleration = 3;
	
	private float topSpeed = 10;
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
			Debug.Log (Input.mousePosition);
			Debug.Log (targetLocation.ToString("G3"));
		}

		if (targetLocationSet) {
			currentSpeed = IncrementTowards (currentSpeed, topSpeed, acceleration);
			Vector3 p = transform.position;
			float totalDistance = Mathf.Sqrt (Mathf.Pow (p.x - targetLocation.x, 2) + Mathf.Pow (p.y - targetLocation.y, 2));
			float distance = currentSpeed * Time.deltaTime;
			Debug.Log (totalDistance + " - " + distance);
			transform.position = targetLocation;
			targetLocationSet = false;
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
