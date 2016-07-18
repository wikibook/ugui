using UnityEngine;
using UnityEngine.UI;

// 리스트 항목 데이터 클래스를 정의
public class ShopItemData
{
	public string iconName;		// 아이콘 이름
	public string name;			// 아이템 이름
	public int price;			// 가격
	public string description;	// 설명
}

// TableViewCell<T>클래스를 상속한다
public class ShopItemTableViewCell : TableViewCell<ShopItemData>
{
	[SerializeField] private Image iconImage;	// 아이콘을 표시할 이미지
	[SerializeField] private Text nameLabel;	// 아이템 이름을 표시할 텍스트
	[SerializeField] private Text priceLabel;	// 가격을 표시할 텍스트

	// 셀의 내용을 갱신하는 메서드를 오버라이트한다
	public override void UpdateContent(ShopItemData itemData)
	{
		nameLabel.text = itemData.name;
		priceLabel.text = itemData.price.ToString();

#region 아이콘의 스프라이트를 변경하는 코드를 추가한다
		// 스프라이트 시트 이름과 스프라이트 이름을 지정해서 아이콘의 스프라이트를 변경한다
		iconImage.sprite = 
			SpriteSheetManager.GetSpriteByName("IconAtlas", itemData.iconName);
#endregion
	}
}
