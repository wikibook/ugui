using UnityEngine;

public class InputController : MonoBehaviour
{
	void Update()
	{
		if(!Application.isMobilePlatform)
		{
			// 모바일 플랫폼이 아닐 때 실시할 처리
			if(Input.GetMouseButtonUp(0))
			{
				// 마우스 왼쪽 버튼을 눌렀다 뗀 상태라면
                // CubeGenerator 컴포넌트에 포함된 Generate 메서드 호출
				GetComponent<CubeGenerator>().Generate();
			}
		}
		else
		{
			// 모바일 플랫폼일 때 실시할 처리
			if(Input.touchCount >= 1)
			{
				Touch touch = Input.GetTouch(0);
				if(touch.phase == TouchPhase.Began)
				{
					// 터치가 시작된 상태라면 CubeGenerator 컴포넌트에 포함된
                    // Generate 메서드를 호출
					GetComponent<CubeGenerator>().Generate();
				}
			}
		}
	}
}
