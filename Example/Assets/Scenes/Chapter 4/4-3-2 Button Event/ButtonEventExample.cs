using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonEventExample : MonoBehaviour {

	[SerializeField] private Button button;

	// Use this for initialization
	void Start () {
		button.onClick.AddListener(OnClickButton);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickButton()
	{
		Debug.Log("Button is clicked!");
	}
}
