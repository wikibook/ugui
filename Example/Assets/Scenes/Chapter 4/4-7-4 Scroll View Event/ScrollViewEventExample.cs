using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollViewEventExample : MonoBehaviour {

	[SerializeField] private ScrollRect scrollRect;

	// Use this for initialization
	void Start () {
		scrollRect.onValueChanged.AddListener(OnScroll);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnScroll(Vector2 position)
	{
		Debug.Log("Scroll potision is " + position.ToString());
	}
}
