using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
	
[RequireComponent(typeof(Image))]
public class Draggable : 
	MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// 드래그 조작을 제어하기 위한 인터페이스를 상속
{
#region ドラッグ中のアイコンの位置を設定するメソッドの実装
	[SerializeField]
	private Vector2 draggingOffset = new Vector2(0.0f, 40.0f);	// 드래그 중인 아이콘의 오프셋
	private GameObject draggingObject;							// 드래그 중인 아이콘의 게임 오브젝트
	private RectTransform canvasRectTransform;					// 캔버스의 Rect Transform

	private void UpdateDraggingObjectPos(PointerEventData pointerEventData)
	{
		if(draggingObject != null)
		{
			// 드래그 중인 아이콘의 스크린 좌표를 계산한다
			Vector3 screenPos = pointerEventData.position + draggingOffset;

			// 스크린 좌표를 월드 좌표로 변환한다
			Camera camera = pointerEventData.pressEventCamera;
			Vector3 newPos;
			if(RectTransformUtility.ScreenPointToWorldPointInRectangle(
				canvasRectTransform, screenPos, camera, out newPos))
			{
				// 드래그 중인 아이콘의 위치를 설정한다
				draggingObject.transform.position = newPos;
				draggingObject.transform.rotation = canvasRectTransform.rotation;
			}
		}
	}
#endregion

#region OnBeginDragメソッドの実装
	public void OnBeginDrag(PointerEventData pointerEventData)
	{
		if(draggingObject != null)
		{
			Destroy(draggingObject);
		}

		// 본래 아이콘의 Image 컴포넌트를 가져온다
		Image sourceImage = GetComponent<Image>();

		// 드래그 중인 아이콘의 게임 오브젝트를 생성한다
		draggingObject = new GameObject("Dragging Object");
		// 본래 아이콘의 캔버스의 자식요소로 종속시키고 맨 앞쪽에 표시한다
		draggingObject.transform.SetParent(sourceImage.canvas.transform);
		draggingObject.transform.SetAsLastSibling();
		draggingObject.transform.localScale = Vector3.one;

		// Canvas Group 컴포넌트의 Block Raycasts 속성을 사용하여
		// 레이캐스트가 막히지 않게 한다
		CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = false;

		// 드래그 중인 아이콘의 게임 오브젝트에 Image 컴포넌트를 어태치한다
		Image draggingImage = draggingObject.AddComponent<Image>();
		// 본래 아이콘과 동일한 외양을 설정한다
		draggingImage.sprite = sourceImage.sprite;
		draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
		draggingImage.color = sourceImage.color;
		draggingImage.material = sourceImage.material;

		// 캔버스의 Rect Transform을 보존해둔다
		canvasRectTransform = draggingImage.canvas.transform as RectTransform;

		// 드래그 중인 아이콘의 위치를 갱신한다
		UpdateDraggingObjectPos(pointerEventData);
	}
#endregion

#region OnDrag 메서드 본체
	public void OnDrag(PointerEventData pointerEventData)
	{
		UpdateDraggingObjectPos(pointerEventData);
	}
#endregion

#region OnEndDrag 메서드 본체
	public void OnEndDrag(PointerEventData pointerEventData)
	{
		Destroy(draggingObject);
	}
#endregion
}
