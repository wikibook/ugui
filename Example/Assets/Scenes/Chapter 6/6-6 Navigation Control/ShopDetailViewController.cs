using UnityEngine;
using UnityEngine.UI;

public class ShopDetailViewController : ViewController	// ViewController 클래스를 상속
{
	// 내비게이션 뷰 저장
	[SerializeField] private NavigationViewController navigationView;
	
	[SerializeField] private Image iconImage;		// 아이템의 아이콘을 표시하는 이미지
	[SerializeField] private Text nameLabel;		// 아이템 이름을 표시하는 텍스트
	[SerializeField] private Text descriptionLabel;	// 설명을 표시할 텍스트
	[SerializeField] private Text priceLabel;		// 가격을 표시할 텍스트

	private ShopItemData itemData;					// 아이템 데이터를 저장

	// 뷰의 타이틀을 반환한다
	public override string Title {
		get { return (itemData != null)? itemData.name: ""; } }

	// 아이템 상세 화면의 내용을 갱신하는 메서드
	public void UpdateContent(ShopItemData itemData)
	{
		// 아이템의 데이터를 저장해둔다
		this.itemData = itemData;
		
		iconImage.sprite = 
			SpriteSheetManager.GetSpriteByName("IconAtlas", itemData.iconName);
		nameLabel.text = itemData.name;
		priceLabel.text = itemData.price.ToString();
		descriptionLabel.text = itemData.description;
	}

#region 확인화면으로 옮겨지는 처리를 구현
	// 확인 화면의 뷰
	[SerializeField] private ShopConfirmationViewController confirmationView;

	// BUY 버튼이 눌렸을 때 호출되는 메서드
	public void OnPressBuyButton()
	{
		// 확인 화면의 내용을 갱신한다
		confirmationView.UpdateContent(itemData);
		// 確認画面に遷移する
		navigationView.Push(confirmationView);
	}
#endregion
}
