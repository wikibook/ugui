using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Droppable : 
	MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
// 드롭 조작을 제어하기 위한 인터페이스를 상속한다
{
#region OnPointerEnterメソッドの実装
	// 드롭 영역에 표시되어 있는 아이콘
	[SerializeField] private Image iconImage;
	// 드롭 영역에 표시되어 있는 아이콘의 하이라이트 색
	[SerializeField] private Color highlightedColor;
	// 드롭 영역에 표시하고 있는 아이콘의 본래 색
	private Color normalColor;

	// 인스턴스를 로드할 때 Awake 메서드가 처리된 다음에 호출된다
	void Start()
	{
		// 드롭 영역에 표시되어 있는 아이콘의 본래 색을 보존해둔다
		normalColor = iconImage.color;
	}

	// 마우스 커서가 영역에 들어왔을 때 호출된다
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		if(pointerEventData.dragging)
		{
			// 드래그 중이라며 드롭 영역에 표시되어 있는 아이콘 색을 하이라이트 색으로 변경한다
			iconImage.color = highlightedColor;
		}
	}
#endregion

#region OnPointerExitメソッドの実装    
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		if(pointerEventData.dragging)
		{
			// 드래그 도중이라면 드롭 영역에 표시되어 있는 아이콘 색을 본래 색으로 되돌린다
			iconImage.color = normalColor;
		}
	}
#endregion

#region OnDropメソッドの実装
	public void OnDrop(PointerEventData pointerEventData)
	{
		// 드래그하고 있었던 아이콘의 Image 컴포넌트를 가져온다
		Image droppedImage = pointerEventData.pointerDrag.GetComponent<Image>();
		// 드롭 영역에 표시되어 있는 아이콘의 스프라이트를
		// 드롭된 아이콘과 동일한 스프라이트로 변경하고 색을 본래 색으로 되돌린다
		iconImage.sprite = droppedImage.sprite;
		iconImage.color = normalColor;
	}
#endregion
}
