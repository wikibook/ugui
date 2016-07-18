using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
#if UNITY_4_6
// 유니티 4.6.x에서는 AnimatorController 클래스가 UnityEditorInternal 네임 스페이스에 정의돼 있다
using UnityEditorInternal;
#else
using UnityEditor.Animations;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class SpriteAnimationCreator : MonoBehaviour {
    // 기본적인 프레임 간격을 정의한다
    private static float defaultInterval = 0.1f;
    
    // Assets 메뉴→Create에 Sprite Animation 항목을 추가한다
    [MenuItem("Assets/Create/Sprite Animation")]
    public static void Create()
    {
        // 프로젝트 뷰에서 선택된 스프라이트를 가져온다
        List<Sprite> selectedSprites = new List<Sprite>(
        	Selection.GetFiltered(typeof(Sprite), SelectionMode.DeepAssets)
        	.OfType<Sprite>());
        
        // 텍스처가 선택돼 있을 때는 그 안에 있는 스프라이트를 가져온다
        Object[] selectedTextures = 
        	Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
        foreach(Object texture in selectedTextures)
        {
            selectedSprites.AddRange(AssetDatabase.LoadAllAssetsAtPath(
            	AssetDatabase.GetAssetPath(texture)).OfType<Sprite>());
        }
        
        // 스프라이트가 선택돼 있지 않으면 오류가 발생한다
        if(selectedSprites.Count < 1)
        {
            Debug.LogWarning("No sprite selected.");
            return;
        }
        
        // 스프라이트에 붙은 번호를 기준으로 하여 순서대로  정렬한다
        string suffixPattern = "_?([0-9]+)$";
        selectedSprites.Sort((Sprite _1, Sprite _2)=>{
            Match match1 = Regex.Match(_1.name, suffixPattern);
            Match match2 = Regex.Match(_2.name, suffixPattern);
            if(match1.Success && match2.Success)
            {
                return (int.Parse(match1.Groups[1].Value) - 
                	int.Parse(match2.Groups[1].Value));
            }
            else
            {
                return _1.name.CompareTo(_2.name);
            }
        });
        
        // 첫 스프라이트가 있는 폴더에 나중에 에셋을 저장한다
        string baseDir = 
        	Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedSprites[0]));
        // 애니메이션 이름은 첫 스프라이트의 번호 없는 이름으로 정한다
        string baseName = Regex.Replace(selectedSprites[0].name, suffixPattern, "");
        if(string.IsNullOrEmpty(baseName))
        {
            baseName = selectedSprites[0].name;
        }
        
        // 캔버스가 없으면 생성한다
        Canvas canvas = FindObjectOfType<Canvas>();
        if(canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvasObj.layer = LayerMask.NameToLayer("UI");
        }
        
        // 이미지를 생성한다
        GameObject obj = new GameObject(baseName);
        obj.transform.parent = canvas.transform;
        obj.transform.localPosition = Vector3.zero;
        
        Image image = obj.AddComponent<Image>();
        image.sprite = (Sprite)selectedSprites[0];
        image.SetNativeSize();
        
        // Animator 컴포넌트를 추가한다
        Animator animator = obj.AddComponent<Animator>();
        
        // 애니메이션 클립을 생성한다
        AnimationClip animationClip = 
        	AnimatorController.AllocateAnimatorClip(baseName);
        
#if UNITY_4_6
        // 유니티 4.6.x에서는 애니메이션 타입을 ModelImporterAnimationType.Generic으로 지정한다
        AnimationUtility.SetAnimationType(
        	animationClip, ModelImporterAnimationType.Generic);
#endif
        
        // EditorCurveBinding을 사용해 키프레임과 이미지의 Sprite 속성을 관련짓는다
        EditorCurveBinding editorCurveBinding = new EditorCurveBinding();
        editorCurveBinding.type = typeof(Image);
        editorCurveBinding.path = "";
        editorCurveBinding.propertyName = "m_Sprite";
        
        // 선택된 스프라이트의 개수만큼 키프레임을 생성해 각 키프레임에 스프라이트를 할당한다
        ObjectReferenceKeyframe[] keyFrames = 
        	new ObjectReferenceKeyframe[selectedSprites.Count];
        for(int i=0; i<selectedSprites.Count; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = i * defaultInterval;
            keyFrames[i].value = selectedSprites[i];
        }
        
        AnimationUtility.SetObjectReferenceCurve(
        	animationClip, editorCurveBinding, keyFrames);
        
        // Loop Time 속성은 스크립트에서 직접 설정할 수 없으므로
        // SerializedProperty를 사용해 설정한다
        // (이 방법은 유니티의 이후 버전에서는 사용하지 못하게 될 수도 있다)
        SerializedObject serializedAnimationClip = 
        	new SerializedObject(animationClip);
        SerializedProperty serializedAnimationClipSettings = 
        	serializedAnimationClip.FindProperty("m_AnimationClipSettings");
        serializedAnimationClipSettings
        	.FindPropertyRelative("m_LoopTime").boolValue = true;
        serializedAnimationClip.ApplyModifiedProperties();
        
        // 작성한 애니메이션 클립을 에셋 형태로 저장한다
        SaveAsset(animationClip, baseDir + "/" + baseName + ".anim");
        
        // 애니메이터 컨트롤러를 생성한다
        AnimatorController animatorController = 
        	AnimatorController.CreateAnimatorControllerAtPathWithClip(
        	baseDir + "/" + baseName + ".controller", animationClip);
        animator.runtimeAnimatorController = 
        	(RuntimeAnimatorController)animatorController;
    }
    
    // 에셋 형태로 저장하기 위한 함수. 기존의 것이 있을 때는 오버라이트한다
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
