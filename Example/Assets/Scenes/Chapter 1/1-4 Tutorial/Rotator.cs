using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
	[SerializeField] private float rotationSpeed;	// 회전 속도를 설정하는 필드

	// Use this for initialization
	void Start()
	{
//		rotationSpeed = 10.0f;		// 회전 속도를 초기화
		Application.targetFrameRate = 60;	// 프레임률을 60fps로 설정
	}
	
	// Update is called once per frame
	void Update()
	{
		// 회전시킬 각도를 계산
		float yAngle = rotationSpeed * Time.deltaTime;
		// 게임 오브젝트를 Y축을 중심으로 회전
		transform.Rotate(0.0f, yAngle, 0.0f);
	}
}
