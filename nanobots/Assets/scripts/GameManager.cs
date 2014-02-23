using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject bot;
	public ArrayList bots;

	// Use this for initialization
	void Start () {
		SpawnBot ();
		SpawnBot ();
		SpawnBot ();
		SpawnBot ();
		SpawnBot ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void SpawnBot() {
		bots.Add ((Instantiate (bot, Vector3.zero, Quaternion.identity) as GameObject).transform);
	}
}
