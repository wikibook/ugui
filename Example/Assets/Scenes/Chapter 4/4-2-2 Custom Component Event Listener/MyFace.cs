using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MyFace : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeFace(Sprite sprite)
	{
		Image image = GetComponent<Image>();
		image.sprite = sprite;
	}
}
