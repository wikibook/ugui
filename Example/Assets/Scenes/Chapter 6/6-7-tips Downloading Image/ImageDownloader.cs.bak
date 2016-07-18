using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDownloader : MonoBehaviour
{
	// 非同期で処理をおこなうため、Startメソッドをコルーチンとして実行する
	IEnumerator Start()
	{
		// WWWクラスのコンストラクタに画像のURLを指定してダウンロードを開始する
		string url = "http://www.mybdesign.com/unityui/images/cat.jpg";
		WWW www = new WWW(url);
		
		// 画像のダウンロードが完了するのを待つ
		yield return www;
		
		// ウェブサーバーから取得した画像をローイメージで表示する
		RawImage rawImage = GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		// ローイメージのサイズをピクセル等倍にする
		rawImage.SetNativeSize();
	}
}
