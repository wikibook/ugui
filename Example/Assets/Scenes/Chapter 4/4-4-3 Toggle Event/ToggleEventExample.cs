using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleEventExample : MonoBehaviour {

	[SerializeField] private Toggle toggle;

	// Use this for initialization
	void Start () {
		toggle.onValueChanged.AddListener(OnToggle);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnToggle(bool value)
	{
		Debug.Log("Toggle value is " + value.ToString());
	}
}
