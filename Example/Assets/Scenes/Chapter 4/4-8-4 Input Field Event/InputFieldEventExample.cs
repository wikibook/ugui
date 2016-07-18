using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputFieldEventExample : MonoBehaviour {

	[SerializeField] private InputField inputField;

	// Use this for initialization
	void Start () {
		inputField.onValueChange.AddListener(OnValueChange);
		inputField.onEndEdit.AddListener(OnSubmit);

		inputField.onValidateInput = (string text, int charIndex, char addedChar)=>{
			// 알파벳 소문자가 입력되면 대문자로 바꾼다
			char ret = addedChar;
			if(addedChar >= 'a' && addedChar <= 'z')
			{
				ret = (char)(addedChar + ('A' - 'a'));
			}
			return ret;
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnValueChange(string value)
	{
		Debug.Log("Input Field value is " + value);
	}

	public void OnSubmit(string value)
	{
		Debug.Log("Submit value is " + value);
	}
}
