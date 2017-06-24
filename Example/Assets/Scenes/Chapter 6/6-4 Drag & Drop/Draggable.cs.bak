using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
	
[RequireComponent(typeof(Image))]
public class Draggable : 
	MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// ドラッグ操作を制御するためのインターフェイスを実装する
{
#region ドラッグ中のアイコンの位置を設定するメソッドの実装
	[SerializeField]
	private Vector2 draggingOffset = new Vector2(0.0f, 40.0f);	// ドラッグ中のアイコンのオフセット
	private GameObject draggingObject;							// ドラッグ中のアイコンのゲームオブジェクトを保持
	private RectTransform canvasRectTransform;					// カンバスのRect Transformを保持

	private void UpdateDraggingObjectPos(PointerEventData pointerEventData)
	{
		if(draggingObject != null)
		{
			// ドラッグ中のアイコンのスクリーン座標を算出する
			Vector3 screenPos = pointerEventData.position + draggingOffset;

			// スクリーン座標をワールド座標に変換する
			Camera camera = pointerEventData.pressEventCamera;
			Vector3 newPos;
			if(RectTransformUtility.ScreenPointToWorldPointInRectangle(
				canvasRectTransform, screenPos, camera, out newPos))
			{
				// ドラッグ中のアイコンの位置を設定する
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

		// 元のアイコンのImageコンポーネントを取得する
		Image sourceImage = GetComponent<Image>();

		// ドラッグ中のアイコンのゲームオブジェクトを作成する
		draggingObject = new GameObject("Dragging Object");
		// 元のアイコンのカンバスの子要素にして、最前面に表示する
		draggingObject.transform.SetParent(sourceImage.canvas.transform);
		draggingObject.transform.SetAsLastSibling();
		draggingObject.transform.localScale = Vector3.one;

		// Canvas GroupコンポーネントのBlock Raycastsプロパティを使って
		// レイキャストがブロックされないようにする
		CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = false;

		// ドラッグ中のアイコンのゲームオブジェクトにImageコンポーネントをアタッチする
		Image draggingImage = draggingObject.AddComponent<Image>();
		// 元のアイコンと同じアピアランスを設定する
		draggingImage.sprite = sourceImage.sprite;
		draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
		draggingImage.color = sourceImage.color;
		draggingImage.material = sourceImage.material;

		// カンバスのRect Transformを保持しておく
		canvasRectTransform = draggingImage.canvas.transform as RectTransform;

		// ドラッグ中のアイコンの位置を更新する
		UpdateDraggingObjectPos(pointerEventData);
	}
#endregion

#region OnDragメソッドの実装
	public void OnDrag(PointerEventData pointerEventData)
	{
		UpdateDraggingObjectPos(pointerEventData);
	}
#endregion

#region OnEndDragメソッドの実装
	public void OnEndDrag(PointerEventData pointerEventData)
	{
		Destroy(draggingObject);
	}
#endregion
}
