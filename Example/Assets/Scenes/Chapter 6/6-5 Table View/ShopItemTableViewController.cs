using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class ShopItemTableViewController : TableViewController<ShopItemData>
// TableViewController<T>클래스를 상속
{
	// 리스트 항목의 데이터를 읽어 들이는 메서드
	private void LoadData()
	{
		// 일반적인 데이터는 데이터 소스로부터 가져오는데 여기서는 하드 코드를 사용해하여 정의한다
		tableData = new List<ShopItemData>() {
			new ShopItemData { iconName="drink1", name="WATER", 
				price=100, description="Nothing else, just water." }, 
			new ShopItemData { iconName="drink2", name="SODA", 
				price=150, description="Sugar free and low calorie." }, 
			new ShopItemData { iconName="drink3", name="COFFEE", 
				price=200, description="How would you like your coffee?" }, 
			new ShopItemData { iconName="drink4", name="ENERGY DRINK", 
				price=300, description="It will give you wings." }, 
			new ShopItemData { iconName="drink5", name="BEER", 
				price=500, description="It's a drink for grown-ups." }, 
			new ShopItemData { iconName="drink6", name="COCKTAIL", 
				price=1000, description="A cocktail made of tropical fruits." }, 
			new ShopItemData { iconName="fruit1", name="CHERRY", 
				price=100, description="Do you like cherries?" }, 
			new ShopItemData { iconName="fruit2", name="ORANGE", 
				price=150, description="It contains much vitamin C." }, 
			new ShopItemData { iconName="fruit3", name="APPLE", 
				price=300, description="Enjoy the goodness without peeling it." }, 
			new ShopItemData { iconName="fruit4", name="BANANA", 
				price=400, description="Don't slip on its peel." }, 
			new ShopItemData { iconName="fruit5", name="GRAPE", 
				price=600, description="It's not a grapefruit." }, 
			new ShopItemData { iconName="fruit6", name="PINEAPPLE", 
				price=800, description="It's not a hand granade." }, 
			new ShopItemData { iconName="gun1", name="MINI GUN", 
				price=1000, description="A tiny concealed carry gun." }, 
			new ShopItemData { iconName="gun2", name="CLASSIC GUN", 
				price=2000, description="The gun that was used by a pirate." }, 
			new ShopItemData { iconName="gun3", name="STANDARD GUN", 
				price=4000, description="Just a standard weapon." }, 
			new ShopItemData { iconName="gun4", name="REVOLVER", 
				price=5000, description="It can hold a maximum of 6 bullets." }, 
			new ShopItemData { iconName="gun5", name="AUTO RIFLE", 
				price=10000, description="It can fire automatically and rapidly." }, 
			new ShopItemData { iconName="gun6", name="SPACE GUN", 
				price=20000, description="A weapon that comes from the future." }, 
		};

		// 스크롤시킬 내용의 크기를 갱신한다
		UpdateContents();
	}

	// 리스트 항목에 대응하는 셀의 높이를 반환하는 메서드
	protected override float CellHeightAtIndex(int index)
	{
		if(index >= 0 && index <= tableData.Count-1)
		{
			if(tableData[index].price >= 1000)
			{
				// 가격이 1000 이상인 아이템을 표시하는 셀의 높이를 반환한다
				return 240.0f;
			}
			if(tableData[index].price >= 500)
			{
				// 가격이 500 이상인 아이템을 표시하는 셀의 높이를 반환한다
				return 160.0f;
			}
		}
		return 128.0f;
	}

	// 인스턴스를 로드할 때 호출된다
	protected override void Awake()
	{
		// 기반 클래스에 포함된 Awake 메서드를 호출한다
		base.Awake();

		// 아이콘의 스프라이트 시트에 포함된 스프라이트를 캐시해둔다
		SpriteSheetManager.Load("IconAtlas");
	}

	// 인스턴스를 로드할 때 Awake 메서드가 처리된 다음에 호출된다
	protected override void Start()
	{
		// 기반 클래스의 Start 메서드를 호출한다
		base.Start();

		// 리스트 항목의 데이터를 읽어 들인다
		LoadData();

#region 아이템 목록 화면을 내비게이션 뷰에 대응시킨다
		if(navigationView != null)
		{
			// 내비게이션 뷰의 첫 뷰로 설정한다
			navigationView.Push(this);
		}
#endregion
	}

#region 아이템 목록 화면을 내비게이션 뷰에 대응시킨다
	// 내비게이션 뷰
	[SerializeField] private NavigationViewController navigationView;

	// 뷰의 타이틀을 반환한다
	public override string Title { get { return "SHOP"; } }
#endregion

#region 아이템 상세 화면으로 옮기는 처리
	// 아이템 상세 화면의 뷰
	[SerializeField] private ShopDetailViewController detailView;

	// 셀이 선택됐을 때 호출되는 메서드
	public void OnPressCell(ShopItemTableViewCell cell)
	{
		if(navigationView != null)
		{
			// 선택된 셀로부터 아이템 데이터를 가져와서 아이템 상세 화면의 내용을 갱신한다
			detailView.UpdateContent(tableData[cell.DataIndex]);
			// 아이템 상세 화면으로 옮긴다
			navigationView.Push(detailView);
		}
	}
#endregion
}
