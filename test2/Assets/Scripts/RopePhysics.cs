using UnityEngine;
using System.Collections;

public class RopePhysics : MonoBehaviour {

  public LayerMask collisionMask;

  private Vector2 lockPosition;
  private float rayDistance = 0.25f;
  private float skin = .005f;

  [HideInInspector]
  public bool attached=false;
  
  Ray ray;
  RaycastHit hit;
  
  void Start() {
    lockPosition = transform.position;
  }

  void Update() {
    if (attached) {
      transform.position = lockPosition;
      transform.rigidbody.useGravity = false;
      transform.rigidbody.isKinematic = true;
    }
  }

  public void Attach () {
    ray = new Ray (transform.position, transform.transform.forward);
    if (Physics.Raycast (ray, out hit, rayDistance, collisionMask)) {
      float distance = Vector3.Distance (ray.origin, hit.point);
      
      if (distance > skin) {
        attached = true;
      } else {
        attached = false;
      }
    }
  }

  public void Detach () {
    attached = false;
  }

  public void Remove() {
    GameObject ropeSection = transform.gameObject;
    while (ropeSection.name.Contains("Rope")) {
      Destroy (ropeSection, 0.5f);
      ropeSection = ropeSection.hingeJoint.connectedBody.gameObject;
    }
  }
}
