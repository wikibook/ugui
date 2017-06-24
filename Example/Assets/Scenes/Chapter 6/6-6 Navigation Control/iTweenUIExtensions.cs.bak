using UnityEngine;

// iTweenからのイベントを制御するクラス
public class iTweenEventHandler : MonoBehaviour
{
	// 移動アニメーションのステップごとに呼ぶコールバックメソッド
	public System.Action<Vector2> OnUpdateMoveDelegate { get; set; }

	// 移動アニメーションのステップごとに呼ばれるメソッド
	public void OnUpdateMove(Vector2 value)
	{
		if(OnUpdateMoveDelegate != null)
		{
			OnUpdateMoveDelegate.Invoke(value);
		}
	}

	// アニメーションの終了時に呼ぶコールバックメソッド
	public System.Action OnCompleteDelegate { get; set; }

	// アニメーションが終了したときに呼ばれるメソッド
	public void OnComplete()
	{
		if(OnCompleteDelegate != null)
		{
			OnCompleteDelegate.Invoke();
		}
	}
}

// 拡張メソッドのための静的クラス
public static class iTweenUIExtensions
{
	// iTweenからのイベントを制御するハンドラーを設定するメソッド
	private static iTweenEventHandler SetUpEventHandler(GameObject targetObj)
	{
		iTweenEventHandler eventHandler = 
			targetObj.GetComponent<iTweenEventHandler>();
		if(eventHandler == null)
		{
			eventHandler = targetObj.AddComponent<iTweenEventHandler>();
		}
		return eventHandler;
	}

	// RectTransform.MoveTo
	// Rect Transformを現在の位置から指定された位置に移動するアニメーション
	public static void MoveTo(this RectTransform target, Vector2 pos, 
		float time, float delay, iTween.EaseType easeType, 
		System.Action onCompleteDelegate=null)
	{
		// iTweenからのイベントを制御するハンドラーを設定する
		iTweenEventHandler eventHandler = SetUpEventHandler(target.gameObject);

		// 移動アニメーションのステップごとに呼ぶコールバックメソッドを設定する
		eventHandler.OnUpdateMoveDelegate = (Vector2 value)=>{
			// Rect Transformの位置を更新する
			target.anchoredPosition = value;
		};

		// アニメーションの終了時に呼ぶコールバックメソッドを設定する
		eventHandler.OnCompleteDelegate = onCompleteDelegate;

		// iTweenのValueToメソッドを呼んでアニメーションを開始する
		iTween.ValueTo(target.gameObject, iTween.Hash(
			"from", target.anchoredPosition, 
			"to", pos, 
			"time", time, 
			"delay", delay, 
			"easetype", easeType, 
			"onupdate", "OnUpdateMove", 
			"onupdatetarget", eventHandler.gameObject, 
			"oncomplete", "OnComplete", 
			"oncompletetarget", eventHandler.gameObject
		));
	}
}
