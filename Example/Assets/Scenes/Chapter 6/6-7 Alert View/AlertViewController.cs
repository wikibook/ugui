using UnityEngine;
using UnityEngine.UI;

// 알림 뷰의 표시 옵션을 지정하기 위한 클래스를 정의한다
public class AlertViewOptions
{
	public string cancelButtonTitle;			// 취소 버튼의 타이틀
	public System.Action cancelButtonDelegate;	// "을 눌렀을 때 실행되는 델리게이트
	public string okButtonTitle;				// OK 버튼의 타이틀
	public System.Action okButtonDelegate;		// "을 눌렀을 때 실행되는 델리게이트
}

public class AlertViewController : ViewController	// ViewController 클래스를 상속한다
{
	[SerializeField] Text titleLabel;			// 타이틀을 표시하는 텍스트
	[SerializeField] Text messageLabel;			// 메시지를 표시하는 텍스트
	[SerializeField] Button cancelButton;		// 취소 버튼
	[SerializeField] Text cancelButtonLabel;	// "의 타이틀을 표시할 텍스트
	[SerializeField] Button okButton;			// OK 버튼
	[SerializeField] Text okButtonLabel;		// "의 타이틀을 표시할 텍스트

	private static GameObject prefab = null;	// 알림 뷰의 프리팹을 저장
	private System.Action cancelButtonDelegate;	// 취소 버튼을 눌렀을 때
												// 실행되는 델리게이트를 저장
	private System.Action okButtonDelegate;		// OK 버튼을 눌렀을 때
												// 실행되는 델리게이트를 저장

	// 알림 뷰를 표시하는 static 메서드
	public static AlertViewController Show(
		string title, string message, AlertViewOptions options=null)
	{
		if(prefab == null)
		{
			// 프리팹을 읽어 들인다
			prefab = Resources.Load("Alert View") as GameObject;
		}
		
		// 프리팹을 인스턴스화하여 알림 뷰를 표시한다
		GameObject obj = Instantiate(prefab) as GameObject;
		AlertViewController alertView = obj.GetComponent<AlertViewController>();
		alertView.UpdateContent(title, message, options);

		return alertView;
	}

	// 알림 뷰의 내용을 갱신하는 메서드
	public void UpdateContent(
		string title, string message, AlertViewOptions options=null)
	{
		// 타이틀과 메시지를 설정한다
		titleLabel.text = title;
		messageLabel.text = message;
		
		if(options != null)
		{
			// 표시 옵션이 지정돼 있을 때 옵션의 내용에 맞춰 버튼을 표시하거나 표시하지 않는다
			cancelButton.transform.parent.gameObject.SetActive(
				options.cancelButtonTitle != null || options.okButtonTitle != null
			);

			cancelButton.gameObject.SetActive(options.cancelButtonTitle != null);
			cancelButtonLabel.text = options.cancelButtonTitle ?? "";
			cancelButtonDelegate = options.cancelButtonDelegate;

			okButton.gameObject.SetActive(options.okButtonTitle != null);
			okButtonLabel.text = options.okButtonTitle ?? "";
			okButtonDelegate = options.okButtonDelegate;
		}
		else
		{
			// 표시 옵션이 지정돼 있지 않을 때 기본 버튼을 표시한다
			cancelButton.gameObject.SetActive(false);
			okButton.gameObject.SetActive(true);
			okButtonLabel.text = "OK";
		}
	}

	// 알림 뷰를 닫는 메서드
	public void Dismiss()
	{
		Destroy(gameObject);
	}

	// 취소 버튼을 눌렀을 때 호출되는 메서드
	public void OnPressCancelButton()
	{
		if(cancelButtonDelegate != null)
		{
			cancelButtonDelegate.Invoke();
		}
		Dismiss();
	}
	
	// OK 버튼을 눌렸을 때 호출되는 메서드
	public void OnPressOKButton()
	{
		if(okButtonDelegate != null)
		{
			okButtonDelegate.Invoke();
		}
		Dismiss();
	}
}
