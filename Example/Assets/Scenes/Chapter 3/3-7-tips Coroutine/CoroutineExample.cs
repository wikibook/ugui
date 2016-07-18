using UnityEngine;
using System.Collections;

public class CoroutineExample : MonoBehaviour
{
	void Start()
	{
		// 코루틴의 실행을 시작한다
		StartCoroutine(WaitAndGo(1.0f));
		Debug.Log("Waiting...");	// ②
	}
	
	IEnumerator WaitAndGo(float duration)
	{
		Debug.Log("Ready");			// ①
		// duration에 지정된 시간(초)만큼 기다린다
		yield return new WaitForSeconds(duration);
		Debug.Log("Go!");			// ③
	}
}
