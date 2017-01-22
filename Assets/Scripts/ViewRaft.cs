using UnityEngine;
using System.Collections;

public class ViewRaft : MonoBehaviour {

	public Canvas victoryScreen;
	public Canvas raftReadyScreen;
	public GameObject player;

//	Animator anim;

	// Use this for initialization
	void Start () {
//		anim = player.transform.FindChild ("pug").GetComponent<Animator> ();
	}

	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject.name == "Player") {
			victoryScreen.gameObject.SetActive (true);
			raftReadyScreen.gameObject.SetActive(false);
			player.GetComponent<PlayerController> ().controlEnabled = false;
//			anim.SetBool ("isRunning", false);
//			anim.SetBool ("isJumping", false);
//			Time.timeScale = 0f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
