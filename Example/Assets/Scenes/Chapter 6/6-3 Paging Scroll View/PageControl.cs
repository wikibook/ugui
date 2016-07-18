using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PageControl : MonoBehaviour
{
	[SerializeField] private Toggle indicatorBase;			// 복사 원본 페이지 인디케이터
	private List<Toggle> indicators = new List<Toggle>();	// 페이지 인디케이터를 저장

	// 인스턴스가 로드될 때 호출된다
	void Awake()
	{
		// 복사 원본 페이지 인디케이터는 비활성화시켜 둔다
		indicatorBase.gameObject.SetActive(false);
	}
	
	// 페이지 수를 설정하는 메서드
	public void SetNumberOfPages(int number)
	{
		if(indicators.Count < number)
		{
			// 페이지 인디케이터 수가 지정된 페이지 수보다 적으면
            //  복사 원본 페이지 인디케이터로부터 새로운 페이지 인디케이터를 작성한다
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
			// 페이지 인디케이터 수가 지정된 페이지 수보다 많으면 삭제한다
			for(int i=indicators.Count-1; i>=number; i--)
			{
				Destroy(indicators[i].gameObject);
				indicators.RemoveAt(i);
			}
		}
	}

	// 현재 페이지를 설정하는 메서드
	public void SetCurrentPage(int index)
	{
		if(index >= 0 && index <= indicators.Count-1)
		{
			// 지정된 페이지에 대응하되는 페이지 인디케이터를 ON으로 지정한다
            // 토글 그룹을 설정해두었으므로 다른 인디케이터는 자동으로 OFF가 된다
			indicators[index].isOn = true;
		}
	}
}
