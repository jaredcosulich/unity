using UnityEngine;
using System.Collections;

public class BotPhysics : MonoBehaviour {

	public float currentXSpeed = 0;
  	public float currentYSpeed = 0;

	public LayerMask collisionMask;

  	private Vector3 s;
	private Vector3 c;
	private Vector3 p;

	private float buffer = 0.04f;

	RaycastHit hit;

  
  	void Start () {
		BoxCollider boxCollider = GetComponent<BoxCollider> ();
		s = boxCollider.size;
		c = boxCollider.center;
  	}

    void Update () {
		p = transform.position;

    	for (float i=-1; i<2; ++i) {
			for (float direction=-1; direction<=1; direction+=2) {
				float x = p.x + ((s.x / 2) * (i));
				float y = p.y + ((s.y / 2) * direction);

				Debug.DrawRay(new Vector3(x, y), new Vector3(i*buffer,direction*buffer));				
        		if (TestRay(new Ray(new Vector3(x, y), new Vector3(i*buffer,direction*buffer)))) {
				}				
			}
    	}

		for (float direction=-1; direction<=1; direction+=2) {
			float x = p.x + ((s.x / 2) *  direction);
			float y = p.y;
			
			Debug.DrawRay(new Vector3(x, y), new Vector3(direction*buffer,0));
      		if (TestRay(new Ray(new Vector3(x, y), new Vector3(direction*buffer,0)))) {
			}
			
      	}

	}

	private bool TestRay (Ray ray) {
		return Physics.Raycast (ray, buffer);
	}
}
