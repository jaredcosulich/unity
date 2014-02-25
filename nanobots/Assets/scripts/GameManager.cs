using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject bot;
	public Transform[] bots = new Transform[5];

	// Use this for initialization
	void Start () {
		for (int i=0; i<5; ++i) {
			SpawnBot (i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void SpawnBot(int index) {
		bots[index] = (Instantiate (bot, new Vector3(index/1.5f, 0), Quaternion.identity) as GameObject).transform;
	}
}
