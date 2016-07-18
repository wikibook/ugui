package com.example.locationplugin;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.location.Address;
import android.location.Criteria;
import android.location.Geocoder;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Looper;
import android.provider.Settings;

import com.unity3d.player.UnityPlayer;

import java.io.IOException;
import java.util.List;
import java.util.Locale;

// LocationPluginクラスの定義
public class LocationPlugin implements LocationListener
// LocationListenerインターフェイスを実装する
{
	// シングルトンインスタンスの初期化と取得
	private static LocationPlugin instance = null;
	public static synchronized LocationPlugin getInstance()
	{
		if(instance == null)
		{
			instance = new LocationPlugin();
		}
		return instance;
	}

	private LocationManager locationManager = null;	// LocationManagerのインスタンスを保持
	private String callbackTarget = null;			// コールバックの送信先を保持
	private boolean isReverseGeocoding = false;		// 逆ジオコーディングの処理中かどうかを保持

	// 位置情報の取得を開始するメソッド
	public boolean startUpdatingLocation(String newCallbackTarget)
	{
		// Unity側にコールバックを送信するときの送信先ゲームオブジェクト名を保持しておく
		callbackTarget = newCallbackTarget;

		Activity activity = UnityPlayer.currentActivity;
		if(locationManager == null)
		{
			locationManager = 
				(LocationManager)activity.getSystemService(Context.LOCATION_SERVICE);
		}

		// 有効な位置情報プロバイダーを取得する
		Criteria criteria = new Criteria();
		criteria.setBearingRequired(false);
		List<String> providers = locationManager.getProviders(criteria, true);
		if(providers.size() < 1)
		{
			// 有効な位置情報プロバイダーがない場合、ユーザーに設定を促す
			Intent intent = new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS);
			activity.startActivity(intent);
			return false;
		}

		// 各位置情報プロバイダーを使って位置情報の取得を開始する
		Looper looper = Looper.getMainLooper();
		for(String provider : providers)
		{
			locationManager.requestLocationUpdates(provider, 1000, 0, this, looper);
		}

		return true;
	}

	// 位置情報の取得を停止するメソッド
	public void stopUpdatingLocation()
	{
		if(locationManager != null)
		{
			locationManager.removeUpdates(this);
		}
	}

	// 逆ジオコーディングが完了したときに呼ぶコールバックの定義
	public interface ReverseGeocodingTaskCallback
	{
		void onFinishReverceGeocoding(List<Address> addresses);
	}

	// 逆ジオコーディングを非同期でおこなうためのAsyncTaskサブクラスの定義
	private class ReverseGeocodingTask extends AsyncTask<Location, Void, Void>
	{
		private Context context = null;
		private ReverseGeocodingTaskCallback callback = null;

		public ReverseGeocodingTask(Context newContext, 
			ReverseGeocodingTaskCallback newCallback)
		{
			super();
			context = newContext;
			callback = newCallback;
		}

		@Override
		protected Void doInBackground(Location... params)
		{
			Location location = params[0];
			Geocoder geocoder = new Geocoder(context);
			List<Address> addresses = null;
			try
			{
				// 渡された位置情報を元に逆ジオコーディングをおこなう
				addresses = geocoder.getFromLocation(
					location.getLatitude(), location.getLongitude(), 1);
			}
			catch(IOException e)
			{
				e.printStackTrace();
			}

			if(callback != null)
			{
				// 逆ジオコーディングの結果をコールバックに渡す
				callback.onFinishReverceGeocoding(addresses);
			}

			return null;
		}
	}

	// 位置情報が更新されたときに呼ばれる
	@Override
	public void onLocationChanged(final Location location)
	{
		if(isReverseGeocoding)
		{
			return;
		}

		// 住所を取得するため、取得した位置情報を元に逆ジオコーディングをおこなう
		isReverseGeocoding = true;
		Activity activity = UnityPlayer.currentActivity;
		(new ReverseGeocodingTask(activity, new ReverseGeocodingTaskCallback() {
			@Override
			public void onFinishReverceGeocoding(List<Address> addresses) {
				isReverseGeocoding = false;

				// Addressオブジェクトから住所の文字列を取得する
				String addressString = "";
				if(addresses != null && addresses.size() >= 1)
				{
					Address address = addresses.get(0);
					for(int i=0; i<=address.getMaxAddressLineIndex(); i++)
					{
						if(i >= 1) addressString += " ";
						addressString += address.getAddressLine(i);
					}
				}

				// パラメーターとして渡す文字列を作成する
				String parameter = String.format(Locale.getDefault(), 
					"%f\t%f\t%f\t%f\t%s", location.getLatitude(), location.getLongitude(),
					location.getSpeed(), location.getAccuracy(),
					addressString);

				// UnitySendMessageメソッドを使ってUnity側のOnUpdateLocationメソッドを呼ぶ
				UnityPlayer.UnitySendMessage(callbackTarget, "OnUpdateLocation", 
					parameter);
			}
		})).execute(location);
	}

	// そのほかの必須メソッド
	@Override
	public void onStatusChanged(String provider, int status, Bundle extras) {}

	@Override
	public void onProviderDisabled(String provider) {}

	@Override
	public void onProviderEnabled(String provider) {}
}
