#if UNITY_5
#pragma warning disable 0618
#endif

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class BitmapFontCreater : MonoBehaviour {

	// XML 형식의 .fnt파일을 읽어 들이려면 트리 구조를 매핑하기 위한 클래스를 선언해야 한다
    // (여기서는 최소한의 데이터 구조를 가지는 클래스를 선언한다)
	[XmlRoot("font")]
	public class FontData
	{
		[XmlElementAttribute("common")]
		public FontCommon common;
		[XmlArray("chars")]
		[XmlArrayItem("char")]
		public List<FontChar> chars;
	}

	public class FontCommon
	{
		[XmlAttribute("lineHeight")] public float lineHeight;
		[XmlAttribute("scaleW")] public float scaleW;
		[XmlAttribute("scaleH")] public float scaleH;
	}

	public class FontChar
	{
		[XmlAttribute("id")] public int id;
		[XmlAttribute("x")] public float x;
		[XmlAttribute("y")] public float y;
		[XmlAttribute("width")] public float width;
		[XmlAttribute("height")] public float height;
		[XmlAttribute("xoffset")] public float xoffset;
		[XmlAttribute("yoffset")] public float yoffset;
		[XmlAttribute("xadvance")] public float xadvance;
	}

	// Assets 메뉴→Create를 이용해 Bitmap Font 항목을 추가할 수 있게 한다
	[MenuItem("Assets/Create/Bitmap Font")]
	public static void Create()
	{
		// 프로젝트 뷰에서 선택된 텍스트 파일과 텍스처를 가져온다
		Object[] selectedTextAssets = 
			Selection.GetFiltered(typeof(TextAsset), SelectionMode.DeepAssets);
		Object[] selectedTextures = 
			Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
		
		// 텍스트 파일이 선택돼 있지 않다면 오류가 발생한다
		if(selectedTextAssets.Length < 1)
		{
			Debug.LogWarning("No text asset selected.");
			return;
		}
		
		// 텍스처가 선택돼 있지 않으면 오류가 발생한다
		if(selectedTextures.Length < 1)
		{
			Debug.LogWarning("No texture selected.");
			return;
		}
		
		// 텍스트 파일이 들어 있는 폴더에 나중에 에셋을 저장한다
		string baseDir = 
			Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedTextAssets[0]));
		// 폰트 이름은 텍스트 파일 이름과 같은 이름을 사용한다
		string fontName = selectedTextAssets[0].name;
		// 텍스트 파일의 내용을 가져온다
		string xml = ((TextAsset)selectedTextAssets[0]).text;
		
		// XML을 읽어 들이고 트리 구조를 FontData 클래스에 매핑한다
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(FontData));
		FontData fontData = null;
		using(StringReader reader = new StringReader(xml))
		{
			fontData = (FontData)xmlSerializer.Deserialize(reader);
		}
		
		// 데이터가 적합하지 않으면 오류가 발생한다
		if(fontData == null || fontData.chars.Count < 1)
		{
			Debug.LogWarning("Invalid data.");
			return;
		}
		
		// 사용자 지정 폰트를 위한 머티리얼을 생성해서 현재 선택된 텍스처에 할당한다
		Material fontMaterial = new Material(Shader.Find("UI/Default"));
		fontMaterial.mainTexture = (Texture2D)selectedTextures[0];
		
		// 사용자 지정 폰트를 생성해서 머티리얼을 할당한다
		Font font = new Font(fontName);
		font.material = fontMaterial;
		
		// 사용자 지정 폰트에 문자를 추가한다
		float textureWidth = fontData.common.scaleW;
		float textureHeight = fontData.common.scaleH;
		CharacterInfo[] characterInfos = new CharacterInfo[fontData.chars.Count];
		for(int i=0; i<fontData.chars.Count; i++)
		{
			FontChar fontChar = fontData.chars[i];
			float charX = fontChar.x;
			float charY = fontChar.y;
			float charWidth = fontChar.width;
			float charHeight = fontChar.height;
			
			// 문자 정보를 설정한다[^5]
			characterInfos[i] = new CharacterInfo();
			characterInfos[i].index = fontChar.id;
			characterInfos[i].uv = new Rect(
				charX/textureWidth, (textureHeight-charY-charHeight)/textureHeight, 
				charWidth/textureWidth, charHeight/textureHeight);
			characterInfos[i].vert = new Rect(
				fontChar.xoffset, -fontChar.yoffset, 
				charWidth, -charHeight);
			characterInfos[i].width = fontChar.xadvance;
		}
		font.characterInfo = characterInfos;
		
		// Line Spacing 속성은 스크립트로 직접 설정할 수 없으므로
        // SerializedProperty를 사용해 설정한다
        // (이 방법은 유니티의 이후 버전에서 사용하지 못할 가능성이 있습니다)
		SerializedObject serializedFont = new SerializedObject(font);
		SerializedProperty serializedLineSpacing = 
			serializedFont.FindProperty("m_LineSpacing");
		serializedLineSpacing.floatValue = fontData.common.lineHeight;
		serializedFont.ApplyModifiedProperties();
		
		// 생성한 머티리얼과 폰트를 에셋 형태로 저장한다
		SaveAsset(fontMaterial, baseDir + "/" + fontName + ".mat");
		SaveAsset(font, baseDir + "/" + fontName + ".fontsettings");
	}
	
	// 에셋 형태로 저장하기 위한 함수. 기존의 것이 있다면 오버라이트한다	
	private static void SaveAsset(Object obj, string path)
	{
		Object existingAsset = AssetDatabase.LoadMainAssetAtPath(path);
		if(existingAsset != null)
		{
			EditorUtility.CopySerialized(obj, existingAsset);
			AssetDatabase.SaveAssets();
		}
		else
		{
			AssetDatabase.CreateAsset(obj, path);
		}
	}
}

#if UNITY_5
#pragma warning restore 0618
#endif
