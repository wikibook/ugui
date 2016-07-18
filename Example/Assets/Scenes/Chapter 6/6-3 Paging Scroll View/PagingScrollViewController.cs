using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class PagingScrollViewController : 
	ViewController, IBeginDragHandler, IEndDragHandler
// ViewController 클래스를 상속하고 IBeginDragHandler 인터페이스와
// IEndDragHandler인터페이스를 상속한다
{
	[SerializeField] private float animationDuration = 0.3f;
	[SerializeField] private float key1InTangent = 0.0f;
	[SerializeField] private float key1OutTangent = 0.1f;
	[SerializeField] private float key2InTangent = 0.0f;
	[SerializeField] private float key2OutTangent = 0.0f;

#region 페이지 컨트롤을 제어하는 처리를 추가
	[SerializeField] private PageControl pageControl;	// 관련지을 페이지 컨트롤
#endregion

#region OnBeginDrag関数、OnEndDrag関数の実装
	// ScrollRect컴포넌트를 캐시한다
	private ScrollRect cachedScrollRect;
	public ScrollRect CachedScrollRect
	{
		get {
			if(cachedScrollRect == null)
				{ cachedScrollRect = GetComponent<ScrollRect>(); }
			return cachedScrollRect;
		}
	}

	private bool isAnimating = false;		// 애니메이션 재생 중임을 나타내는 플래그
	private Vector2 destPosition;			// 최종적인 스크롤 위치
	private Vector2 initialPosition;		// 자동 스크롤을 시작할 때의 스크롤 위치
	private AnimationCurve animationCurve;	// 자동 스크롤에 관련된 애니메이션 커브
	private int prevPageIndex = 0;			// 이전 페이지의 인덱스

	// 드래그가 시작될 때 호출된다
	public void OnBeginDrag(PointerEventData eventData)
	{
		// 애니메이션 도중에 플래그를 리셋한다
		isAnimating = false;
	}

	// 드래그가 끝날 때 호출된다
	public void OnEndDrag(PointerEventData eventData)
	{
		GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();

		// 현재 동작 중인 스크롤 뷰를 멈춘다
		CachedScrollRect.StopMovement();

		// GridLayoutGroup의 cellSize와 spacing을 이용하여 한 페이지의 폭을 계산한다
		float pageWidth = -(grid.cellSize.x + grid.spacing.x);

		// 스크롤의 현재 위치로부터 맞출 페이지의 인덱스를 계산한다
		int pageIndex = 
			Mathf.RoundToInt((CachedScrollRect.content.anchoredPosition.x) / pageWidth);

		if(pageIndex == prevPageIndex && Mathf.Abs(eventData.delta.x) >= 4)
		{
			// 일정 속도 이상으로 드래그할 경우 해당 방향으로 한 페이지 진행시킨다
			CachedScrollRect.content.anchoredPosition += 
				new Vector2(eventData.delta.x, 0.0f);
			pageIndex += (int)Mathf.Sign(-eventData.delta.x);
		}

		// 첫 페이지 또는 끝 페이지일 경우에는 그 이상 스크롤하지 않도록 한다
		if(pageIndex < 0)
		{
			pageIndex = 0;
		}
		else if(pageIndex > grid.transform.childCount-1)
		{
			pageIndex = grid.transform.childCount-1;
		}

		prevPageIndex = pageIndex;  // 현재 페이지의 인덱스를 유지한다

		// 최종적인 스크롤 위치를 계산한다
		float destX = pageIndex * pageWidth;
		destPosition = new Vector2(destX, CachedScrollRect.content.anchoredPosition.y);

		// 시작할 때의 스크롤 위치를 저장해둔다
		initialPosition = CachedScrollRect.content.anchoredPosition;

		// 애니메이션 커브를 작성한다
		Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);
		Keyframe keyFrame2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
		animationCurve = new AnimationCurve(keyFrame1, keyFrame2);

		// 애니메이션 재생 중임을 나타내는 플래그를 설정한다
		isAnimating = true;

#region ページコントロールを制御する処理の追加
		// 페이지 컨트롤 표시를 갱신한다
		pageControl.SetCurrentPage(pageIndex);
#endregion
	}
#endregion

#region 自動スクロールアニメーションの実装
	// 매 프레임마다 Update 메서드가 처리된 다음에 호출된다
	void LateUpdate()
	{
		if(isAnimating)
		{
			if(Time.time >= animationCurve.keys[animationCurve.length-1].time)
			{
				// 애니메이션 커브의 마지막 키프레임을 지나가면 애니메이션을 끝낸다
				CachedScrollRect.content.anchoredPosition = destPosition;
				isAnimating = false;
				return;
			}

			// 애니메이션 커브를 사용하여 현재 스크롤 위치를 계산해서 스크롤 뷰를 이동시킨다
			Vector2 newPosition = initialPosition + 
				(destPosition - initialPosition) * animationCurve.Evaluate(Time.time);
			CachedScrollRect.content.anchoredPosition = newPosition;
		}
	}
#endregion

#region スクロール位置の調整
	private Rect currentViewRect;	// 스크롤 뷰의 사각형 크기

	// 인스턴스를 로드할 때 Awake 메서드가 처리된 다음에 호출된다
	void Start()
	{
		// 「Scroll Content」のPaddingを初期化する
		UpdateView();

#region ページコントロールを制御する処理の追加
		pageControl.SetNumberOfPages(5);	// 페이지 수를 5로 설정한다
		pageControl.SetCurrentPage(0);		// 페이지 컨트롤 표시를 초기화한다
#endregion
	}

	// 매 프레임마다 호출된다
	void Update()
	{
		if(CachedRectTransform.rect.width != currentViewRect.width || 
		   CachedRectTransform.rect.height != currentViewRect.height)
		{
			// 스크롤 뷰의 폭이나 높이가 변화하면 Scroll Content의 Padding을 갱신한다
			UpdateView();
		}
	}

	// Scroll Content의 Padding을 갱신한는 메서드
	private void UpdateView()
	{
		// 스크롤 뷰의 사각형 크기를 보존해둔다
		currentViewRect = CachedRectTransform.rect;

		// GridLayoutGroup의 cellSize를 사용하여 Scroll Content의 Padding을 계산하여 설정한다
		GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
		int paddingH = Mathf.RoundToInt((currentViewRect.width - grid.cellSize.x) / 2.0f);
		int paddingV = Mathf.RoundToInt((currentViewRect.height - grid.cellSize.y) / 2.0f);
		grid.padding = new RectOffset(paddingH, paddingH, paddingV, paddingV);
	}
#endregion
}
