using UnityEngine;
using System.Runtime.InteropServices;

public class LocationPluginInterface
{
#if UNITY_IOS
	// 네이티브 플러그인의 인터페이스에 접근하기 위한 선언
	[DllImport ("__Internal")]
	private static extern bool _startUpdatingLocation(string callbackTarget);

	[DllImport ("__Internal")]
	private static extern void _stopUpdatingLocation();
#endif
	
#if UNITY_ANDROID
	// Java 쪽 LocationPlugin 클래스의 인스턴스를 가져오는 속성
	private static AndroidJavaObject locationPluginInstance;
	public static AndroidJavaObject LocationPluginInstance {
		get {
			if(locationPluginInstance == null)
			{
				// 패키지 이름을 사용하여 JavaのLocationPlugin 클래스를 가져와서 메서드를 호출한다
				AndroidJavaClass locationPluginClass = 
					new AndroidJavaClass("com.example.locationplugin.LocationPlugin");
				locationPluginInstance = 
					locationPluginClass.CallStatic<AndroidJavaObject>("getInstance");
			}
			return locationPluginInstance;
		}
	}

	// Java 쪽 메서드를 호출하는 인터페이스 메서드
	private static bool _startUpdatingLocation(string callbackTarget)
	{
		// LocationPlugin 클래스의 인스턴스인 startUpdatingLocation 메서드를 호출한다
		return LocationPluginInstance.Call<bool>("startUpdatingLocation", 
			callbackTarget);
	}

	private static void _stopUpdatingLocation()
	{
		// LocationPlugin 클래스의 인스턴스인 stopUpdatingLocation 메서드를 호출한다
		LocationPluginInstance.Call("stopUpdatingLocation");
	}
#endif
	
	// Unity 쪽에서 네이티브 플러그인 인터페이스를 호출하는 메서드 정의
	// 장치에서 실행했을 때에만 호출되도록 한다

	// 위치 정보를 가져오는 일을 시작하는 메서드
	public static bool StartUpdatingLocation(string callbackTarget)
	{
#if !UNITY_EDITOR
		return _startUpdatingLocation(callbackTarget);
#else
		return false;
#endif
	}
	
	// 위치 정보를 가져오는 일을 중지하는 메서드
	public static void StopUpdatingLocation()
	{
#if !UNITY_EDITOR
		_stopUpdatingLocation();
#endif
	}
}
