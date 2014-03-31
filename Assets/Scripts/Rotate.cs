using UnityEngine;
using System.Collections;

public class Rotate0 : MonoBehaviour {

	float rotate = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		rotate -= Time.deltaTime * 0.01f;
		transform.Translate(new Vector3(rotate, 0,0));
	}
}
