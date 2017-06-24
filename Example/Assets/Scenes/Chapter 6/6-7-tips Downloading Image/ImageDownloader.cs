using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDownloader : MonoBehaviour
{
	// 비동기로 처리를 수행하기 위해 Start 메서드를 코루틴 형태로 실행한다
	IEnumerator Start()
	{
		// WWW 클래스의 생성자에 그림의 URL을 지정하고 내려받는다
		string url = "http://www.mybdesign.com/unityui/images/cat.jpg";
		WWW www = new WWW(url);
		
		// 그림을 내려받는 작업이 끝나기를 기다린다
		yield return www;
		
		// 웹서버에서 가져온 그림을 로우 이미지로 표시한다
		RawImage rawImage = GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		// 로우 이미지의 크기를 픽셀과 같은 비율로 맞춘다
		rawImage.SetNativeSize();
	}
}
