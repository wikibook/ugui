using UnityEngine;

public class LegacyGUI : MonoBehaviour
{
	[SerializeField] private Texture2D homeIcon;
	[SerializeField] private Texture2D levelsIcon;
	[SerializeField] private Texture2D equipmentsIcon;
	[SerializeField] private Texture2D shopIcon;
	[SerializeField] private Texture2D optionsIcon;
	
	void OnGUI()
	{
		GUI.Box(new Rect(0.0f, 0.0f, Screen.width, 88.0f), "");
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.fontSize = 36;
		style.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(0.0f, 0.0f, Screen.width, 88.0f), "This is Legacy GUI");

		GUI.Box(new Rect(0.0f, Screen.height - 112.0f, Screen.width, 112.0f), "");
		GUILayout.BeginArea(new Rect(2.0f, Screen.height - 110.0f, Screen.width - 4.0f, 108.0f));
			GUILayout.BeginHorizontal();

				if(GUILayout.Button(homeIcon, GUILayout.Height(108.0f)))
				{
					Debug.Log("HOME button is clicked.");
				}
				if(GUILayout.Button(levelsIcon, GUILayout.Height(108.0f)))
				{
					Debug.Log("LEVELS button is clicked.");
				}
				if(GUILayout.Button(equipmentsIcon, GUILayout.Height(108.0f)))
				{
					Debug.Log("EQUIPMENTS button is clicked.");
				}
				if(GUILayout.Button(shopIcon, GUILayout.Height(108.0f)))
				{
					Debug.Log("SHOP button is clicked.");
				}
				if(GUILayout.Button(optionsIcon, GUILayout.Height(108.0f)))
				{
					Debug.Log("OPTIONS button is clicked.");
				}

			GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}
