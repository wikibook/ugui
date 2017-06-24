using UnityEngine;

[RequireComponent(typeof(RectTransform))]	// RectTransform 컴포넌트가 필수이다
public class ViewController : MonoBehaviour
{
	// Rect Transform 컴포넌트를 캐시한다
	private RectTransform cachedRectTransform;
	public RectTransform CachedRectTransform
	{
		get {
			if(cachedRectTransform == null)
				{ cachedRectTransform = GetComponent<RectTransform>(); }
			return cachedRectTransform;
		}
	}

	// 뷰의 타이틀
	public virtual string Title { get { return ""; } set {} }
}
