using UnityEngine;
using System.Collections;

public class DemoRotator : MonoBehaviour {

	[SerializeField] private float rotationSpeed;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
		float yAngle = rotationSpeed * Time.deltaTime;
		transform.Rotate(0.0f, yAngle, 0.0f);
	}
}
