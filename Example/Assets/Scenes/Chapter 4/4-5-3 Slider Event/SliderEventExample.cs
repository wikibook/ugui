using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderEventExample : MonoBehaviour {

	[SerializeField] private Slider slider;

	// Use this for initialization
	void Start () {
		slider.onValueChanged.AddListener(OnSlide);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnSlide(float value)
	{
		Debug.Log("Slider value is " + value.ToString());
	}
}
