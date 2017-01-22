using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {
	public Canvas canvas;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other) {
//		Destroy(other.transform.FindChild("pug").gameObject);
		Time.timeScale = 0;
		canvas.gameObject.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
