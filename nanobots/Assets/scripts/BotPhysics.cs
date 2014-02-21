using UnityEngine;
using System.Collections;

public class BotPhysics : MonoBehaviour {


	[HideInInspector]
	public bool hasMomentum = false;

	private Vector3 startPosition;

	public void StartMovement () {
		if (hasMomentum) {
				return;
		}

		startPosition = transform.position;
	}

	public void Move (Vector2 amount) {

	}

}
