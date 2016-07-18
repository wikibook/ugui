using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
	[SerializeField] private GameObject cubePrefab;	// 상자 프리팹
	
	public void Generate()
	{
		// 상자 프리팹을 인스턴스로 만든다
		GameObject obj = Instantiate(cubePrefab) as GameObject;
		// 인스턴스를 「CubeGenerator」오브젝트의 자식 형태로 추가
		obj.transform.SetParent(transform);
		// 인스턴스의 스케일을 프리팹에 맞춘다
		obj.transform.localScale = cubePrefab.transform.localScale;
		// 인스턴스의 위치를 CubeGenerator 오브젝트에 맞춘다
		obj.transform.position = transform.position;
		// 떨어질 때마다 변화하도록 회전 각도를 무작위로 지정
		obj.transform.rotation = Random.rotation;
	}
}
