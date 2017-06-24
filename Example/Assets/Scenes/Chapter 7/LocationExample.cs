using UnityEngine;
using UnityEngine.UI;

public class LocationExample : MonoBehaviour
{
	[SerializeField] private Text addressLabel;		// 주소를 표시할 텍스트
	[SerializeField] private Text latitudeLabel;	// 위도를 표시할 텍스트
	[SerializeField] private Text longitudeLabel;	// 경도를 표시할 텍스트
	[SerializeField] private Text speedLabel;		// 이동 속도를 표시할 텍스트
	[SerializeField] private Text accuracyLabel;	// 정확도를 표시할 텍스트
	
	// 위치 정보를 가져오는 중인지 나타내는 속성
	private bool IsUpdatingLocation { get; set; }
	
	void Awake()
	{
		IsUpdatingLocation = false;
		UpdateUpdateLocationButton();
	}

	private void UpdateUpdateLocationButton()
	{
		Animator animator = GetComponent<Animator>();
		if(animator != null)
		{
			animator.SetTrigger(IsUpdatingLocation? "Started": "Stopped");
		}
	}
	
	// 위치 정보를 가져오는 작업을 시작/중지하는 버튼을 눌렀을 때 호출되는 메서드
	public void OnPressUpdateLocationButton()
	{
		// 버튼이 눌릴 때마다 위치 정보를 가져오는 작업 시작/중지를 전환한다
		IsUpdatingLocation = !IsUpdatingLocation;
		if(IsUpdatingLocation)
		{
			// 위치 정보를 가져오는 작업을 시작한다
			if(!LocationPluginInterface.StartUpdatingLocation(this.name))
			{
				IsUpdatingLocation = false;
				return;
			}
		}
		else
		{
			// 위치 정보를 가져오는 작업을 중지한다
			LocationPluginInterface.StopUpdatingLocation();
		}
		UpdateUpdateLocationButton();
	}
	
	// 위치 정보가 갱신됐을 때 네이티브 플러그인으로부터 호출되는 콜백 메서드
	public void OnUpdateLocation(string parameter)
	{
		// 파라미터로 받은 문자열에서 필요한 정보를 가져와 표시한다
		string[] components = parameter.Split(new char[] {'\t'}, 5);
		float latitude = float.Parse(components[0]);
		float longitude = float.Parse(components[1]);
		float speed = float.Parse(components[2]);
		float accuracy = float.Parse(components[3]);
		string address = components[4];

		addressLabel.text = address;
		latitudeLabel.text = string.Format("{0:0}° {1:0}′", 
			Mathf.Floor(latitude), (latitude - Mathf.Floor(latitude)) * 60);
		longitudeLabel.text = string.Format("{0:0}° {1:0}′", 
			Mathf.Floor(longitude), (longitude - Mathf.Floor(longitude)) * 60);
		speedLabel.text = string.Format("{0:0.0} m/s", (speed >= 0.0f)? speed: 0.0f);
		accuracyLabel.text = string.Format("{0} m", accuracy);
	}
}
