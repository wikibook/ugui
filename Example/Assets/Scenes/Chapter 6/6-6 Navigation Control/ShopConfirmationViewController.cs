using UnityEngine;
using UnityEngine.UI;

public class ShopConfirmationViewController : ViewController
{
    [SerializeField]
    private Text messageLabel;	// 메시지를 표시하는 텍스트

    // 뷰의 타이틀을 반환한다
	public override string Title { get { return "CONFIRMATION"; } }

    // 확인 화면의 내용을 갱신하는 메서드
	public void UpdateContent(ShopItemData itemData)
	{
		messageLabel.text = string.Format("Buy {0} for {1} coins?", 
			itemData.name, itemData.price.ToString());
	}

#region アラートビューを表示する処理の追加
	// CONFIRM 버튼이 눌렸을 때 호출되는 메서드
	public void OnPressConfirmButton()
	{
		string title = "ARE YOU SURE?";
		string message = messageLabel.text;
		// 알림 뷰를 표시한다
		AlertViewController.Show(title, message, new AlertViewOptions {
			// 취소 버튼의 타이틀과 눌렸을 때 실행되는 델리게이트를 설정
			cancelButtonTitle = "CANCEL", cancelButtonDelegate = ()=>{
				Debug.Log("Cancelled.");
			}, 
			// OK 버튼의 타이틀과 눌렸을 때 실행되는 델리게이트를 설정
			okButtonTitle = "BUY", okButtonDelegate = ()=>{
				Debug.Log("Bought.");
			}, 
		});
	}
#endregion
}
