using UnityEngine;
using UnityEditor;

#if UNITY_4_6
// Unity 유니티5 이전에는 AnimatorController 클래스가 유니티 EditorInternal 이름 공간에 정의돼되어 있다.
using UnityEditorInternal;
#else
using UnityEditor.Animations;
#endif

// 생성할 애니메이션 클립의 이름을 입력하기 위한 대화상자의 클래스
public class RenameWindow : EditorWindow
{
	public string CaptionText { get; set; }		// 대화상자의 캡션
	public string ButtonText { get; set; }		// 버튼의 레이블
	public string NewName { get; set; }			// 입력된 이름
	public System.Action<string> 
		OnClickButtonDelegate { get; set; }		// 버튼을 눌렀을 때이 눌러졌을 때 실행되는 델리게이트
	
	void OnGUI()
	{
		NewName = EditorGUILayout.TextField(CaptionText, NewName);
		if(GUILayout.Button(ButtonText))
		{
			if(OnClickButtonDelegate != null)
			{
				// 버튼을 누르면이 눌러지면 미리 설정된 델리게이트에 입력된되어 있는 이름을 넘겨준다
				OnClickButtonDelegate.Invoke(NewName.Trim());
			}
			
			Close();
			GUIUtility.ExitGUI();
		}
	}
}

public class NestedAnimationCreator : MonoBehaviour
{
	// Assets 메뉴→Create에 Nested Animation 항목을 추가한다
	[MenuItem("Assets/Create/Nested Animation")]
	public static void Create()
	{
		// 프로젝트 Project 뷰에서 선택된선택되어 있는 애니메이터 컨트롤러를 가져온다
		AnimatorController selectedAnimatorController = 
			Selection.activeObject as AnimatorController;

		// 애니메이터 컨트롤러가 선택돼선택되어 있지 않으면 오류
		if(selectedAnimatorController == null)
		{
			Debug.LogWarning("No animator controller selected.");
			return;
		}

		// 생성할 애니메이션 클립의 이름을 입력할 대화상자를 연다
		RenameWindow renameWindow = 
			EditorWindow.GetWindow<RenameWindow>("Create Nested Animation");
		renameWindow.CaptionText = "New Animation Name";
		renameWindow.NewName = "New Clip";
		renameWindow.ButtonText ="Create";
		// 대화상자에 있는 버튼을 누르면이 눌러지면 호출되는 메서드의 델리게이트
		renameWindow.OnClickButtonDelegate = (string newName)=>{
			if(string.IsNullOrEmpty(newName))
			{
				Debug.LogWarning("Invalid name.");
				return;
			}

			// 대화상자에 입력된 이름으로 애니메이션 클립을 생성한다
			AnimationClip animationClip = 
				AnimatorController.AllocateAnimatorClip(newName);

			// 선택된 애니메이터 컨트롤러의 서브트 에셋의 형태로  
            // 생성된 애니메이션 클립을 추가한다
			AssetDatabase.AddObjectToAsset(animationClip, selectedAnimatorController);

			// 애니메이터 컨트롤러를 다시 임포트해서 변경 사항을 반영한시킨다
			AssetDatabase.ImportAsset(
				AssetDatabase.GetAssetPath(selectedAnimatorController));
		};
	}

	// 서브 에셋의 형태로 작성한 애니메이션 클립은 Assets 메뉴→Delete로는
    // 삭제할 수 없으므로 Assets 메뉴에 Delete Sub Asset 항목을 추가한다
	[MenuItem("Assets/Delete Sub Asset")]
	public static void Delete()
	{
		Object[] selectedAssets = Selection.objects;
		if(selectedAssets.Length < 1)
		{
			Debug.LogWarning("No sub asset selected.");
			return;
		}

		foreach(Object asset in selectedAssets)
		{
			// 선택된 오브젝트가 서브 에셋이면 삭제한다
			if(AssetDatabase.IsSubAsset(asset))
			{
				string path = AssetDatabase.GetAssetPath(asset);
				DestroyImmediate(asset, true);
				AssetDatabase.ImportAsset(path);
			}
		}
	}
}
