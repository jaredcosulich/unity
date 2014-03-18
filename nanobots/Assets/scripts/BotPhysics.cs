using UnityEngine;
using System.Collections;

public class BotPhysics : MonoBehaviour {

	public float currentXSpeed = 0;
  	public float currentYSpeed = 0;
	public Vector3 targetLocation;

	public LayerMask collisionMask;

  	private Vector3 s;
	private Vector3 c;
	private Vector3 p;

	private float buffer = 0.04f;
	private float speedIncrement = 0.02f;
	private float bufferSpeed = 1f;

  	private float windY = 0;

	RaycastHit hit;

  
  	void Start () {
		BoxCollider boxCollider = GetComponent<BoxCollider> ();
		s = boxCollider.size;
		c = boxCollider.center;
		targetLocation = transform.position;
  	}

    void Update () {
		p = transform.position;

    	for (float i=-1; i<2; ++i) {
			for (float direction=-1; direction<=1; direction+=2) {
				Debug.DrawRay(new Vector3(p.x, p.y), new Vector3((s.x+buffer)*i,(s.y+buffer)*direction));				
				if (TestRay(new Ray(new Vector3(p.x, p.y), new Vector3((s.x+buffer)*i,(s.y+buffer)*direction)))) {
					Debug.Log ("HIT1");
					currentXSpeed += (speedIncrement * i);
					Debug.Log (currentXSpeed);
					currentYSpeed += (speedIncrement * direction * -1);
					break;
				}				
			}
    	}

		for (float direction=-1; direction<=1; direction+=2) {
			Debug.DrawRay(new Vector3(p.x, p.y), new Vector3((s.x+buffer)*direction,0));
			if (TestRay(new Ray(new Vector3(p.x, p.y), new Vector3((s.x+buffer)*direction,0)))) {
				Debug.Log ("HIT2");
				currentYSpeed += (speedIncrement * direction * -1);
				break;
      		}			
      	}

		if (currentXSpeed < bufferSpeed && currentYSpeed < bufferSpeed) {
			currentYSpeed -= speedIncrement;
    	}
	}

	private bool TestRay (Ray ray) {
		return Physics.Raycast (ray, buffer);
	}
}
