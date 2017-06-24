using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollbarEventExample : MonoBehaviour {

	[SerializeField] private Scrollbar scrollbar;

	// Use this for initialization
	void Start () {
		scrollbar.onValueChanged.AddListener(OnScroll);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnScroll(float value)
	{
		Debug.Log("Scrollbar value is " + value.ToString());
	}
}
