using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class ShopItemTableViewController : TableViewController<ShopItemData>
// TableViewController<T>クラスを継承
{
	// リスト項目のデータを読み込むメソッド
	private void LoadData()
	{
		// 通常データはデータソースから取得しますが、ここではハードコードで定義する
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

		// スクロールさせる内容のサイズを更新する
		UpdateContents();
	}

	// リスト項目に対応するセルの高さを返すメソッド
	protected override float CellHeightAtIndex(int index)
	{
		if(index >= 0 && index <= tableData.Count-1)
		{
			if(tableData[index].price >= 1000)
			{
				// 価格が1000以上のアイテムを表示するセルの高さを返す
				return 240.0f;
			}
			if(tableData[index].price >= 500)
			{
				// 価格が500以上のアイテムを表示するセルの高さを返す
				return 160.0f;
			}
		}
		return 128.0f;
	}

	// インスタンスのロード時に呼ばれる
	protected override void Awake()
	{
		// ベースクラスのAwakeメソッドを呼ぶ
		base.Awake();

		// アイコンのスプライトシートに含まれるスプライトをキャッシュしておく
		SpriteSheetManager.Load("IconAtlas");
	}

	// インスタンスのロード時Awakeメソッドの後に呼ばれる
	protected override void Start()
	{
		// ベースクラスのStartメソッドを呼ぶ
		base.Start();

		// リスト項目のデータを読み込む
		LoadData();

#region アイテム一覧画面をナビゲーションビューに対応させる
		if(navigationView != null)
		{
			// ナビゲーションビューの最初のビューとして設定する
			navigationView.Push(this);
		}
#endregion
	}

#region アイテム一覧画面をナビゲーションビューに対応させる
	// ナビゲーションビューを保持
	[SerializeField] private NavigationViewController navigationView;

	// ビューのタイトルを返す
	public override string Title { get { return "SHOP"; } }
#endregion

#region アイテム詳細画面に遷移させる処理の実装
	// アイテム詳細画面のビューを保持
	[SerializeField] private ShopDetailViewController detailView;

	// セルが選択されたときに呼ばれるメソッド
	public void OnPressCell(ShopItemTableViewCell cell)
	{
		if(navigationView != null)
		{
			// 選択されたセルからアイテムのデータを取得して、アイテム詳細画面の内容を更新する
			detailView.UpdateContent(tableData[cell.DataIndex]);
			// アイテム詳細画面に遷移する
			navigationView.Push(detailView);
		}
	}
#endregion
}
