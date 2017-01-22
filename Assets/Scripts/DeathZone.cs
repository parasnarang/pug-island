using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {
	public Canvas canvas;
	public Canvas raftReadyScreen;
	public GameObject player;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other) {
//		Destroy(other.transform.FindChild("pug").gameObject);
		Time.timeScale = 0;
		canvas.gameObject.SetActive (true);
		raftReadyScreen.gameObject.SetActive (false);

		player.GetComponent<PlayerController> ().showAndroidControls (false);

	}

	// Update is called once per frame
	void Update () {
	
	}
}
