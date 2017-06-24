using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PageControl : MonoBehaviour
{
	[SerializeField] private Toggle indicatorBase;			// コピー元のページインジケーター
	private List<Toggle> indicators = new List<Toggle>();	// ページインジケーターを保持

	// インスタンスのロード時に呼ばれる
	void Awake()
	{
		// コピー元のページインジケーターは非アクティブにしておく
		indicatorBase.gameObject.SetActive(false);
	}
	
	// ページ数を設定するメソッド
	public void SetNumberOfPages(int number)
	{
		if(indicators.Count < number)
		{
			// ページインジケーターの数が指定されたページ数より少なかったら、
			// コピー元のページインジケーターから新しいページインジケーターを作成する
			for(int i=indicators.Count; i<number; i++)
			{
				Toggle indicator = Instantiate(indicatorBase) as Toggle;
				indicator.gameObject.SetActive(true);
				indicator.transform.SetParent(indicatorBase.transform.parent);
				indicator.transform.localScale = indicatorBase.transform.localScale;
				indicator.isOn = false;
				indicators.Add(indicator);
			}
		}
		else if(indicators.Count > number)
		{
			// ページインジケーターの数が指定されたページ数より多かったら削除する
			for(int i=indicators.Count-1; i>=number; i--)
			{
				Destroy(indicators[i].gameObject);
				indicators.RemoveAt(i);
			}
		}
	}

	// 現在のページを設定するメソッド
	public void SetCurrentPage(int index)
	{
		if(index >= 0 && index <= indicators.Count-1)
		{
			// 指定されたページに対応するページインジケーターをオンにする
			// トグルグループを設定してあるので、ほかのインジケーターは自動的にオフになる
			indicators[index].isOn = true;
		}
	}
}
