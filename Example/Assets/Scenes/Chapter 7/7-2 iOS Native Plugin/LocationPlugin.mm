#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

// LocationPlugin 클래스 정의
@interface LocationPlugin : NSObject<CLLocationManagerDelegate>
// CLLocationManagerDelegate 프로토콜을 채용한다
{
	CLLocationManager *locationManager;	// CLLocationManager 인스턴스를 저장
	NSString *callbackTarget;			// 콜백을 송신할 곳을 저장
	bool isReverseGeocoding;			// 역 지오코딩 처리 중인지 여부를 저장
}

+ (LocationPlugin *)sharedInstance;		// 싱글톤 인스턴스

@end

// LocationPlugin 클래스 구현
@implementation LocationPlugin

// 싱글톤 인스턴스를 초기화하고 가져온다
static LocationPlugin *sharedInstance = nil;
+ (id)sharedInstance
{
	@synchronized(self)
	{
		if(sharedInstance == nil)
		{
			sharedInstance = [[self alloc] init];
		}
	}
	return sharedInstance;
}

// 위치 정보를 가져오는 작업을 시작하는 메서드
- (BOOL)startUpdatingLocation:(NSString *)newCallbackTarget
{
	// 유니티 쪽으로 콜백을 송신할 때 그 송신할 게임 오브젝트 이름을 저장해둔다
	callbackTarget = newCallbackTarget;
	
	if(locationManager == nil)
	{
		locationManager = [[CLLocationManager alloc] init];
	}

	// 위치 정보 서비스가 유효하고 또한 허가돼되어 있는지 확인한다
	BOOL isEnabledAndAuthorized = NO;
	if([CLLocationManager locationServicesEnabled])
	{
		CLAuthorizationStatus status = [CLLocationManager authorizationStatus];
		if(status == kCLAuthorizationStatusAuthorizedAlways || 
		   status == kCLAuthorizationStatusAuthorizedWhenInUse)
		{
			isEnabledAndAuthorized = YES;
		}
	}
	if(!isEnabledAndAuthorized)
	{
		// 위치 정보 서비스가 무효하거나 허가돼되어 있지 않으면않을 경우 사용자에게 이를 허가할 것을 요구한다
		[locationManager requestWhenInUseAuthorization];
		return NO;
	}
	
	// 위치 정보를 가져오는 작업을 시작한다
	locationManager.delegate = self;
	locationManager.desiredAccuracy = kCLLocationAccuracyBest;
	[locationManager startUpdatingLocation];

	return YES;
}

// 위치 정보를 가져오는 작업을 중지정지하는 메서드
- (void)stopUpdatingLocation
{
	if(locationManager != nil)
	{
		[locationManager stopUpdatingLocation];
	}
}

#pragma mark - CLLocationManagerDelegate 프로토콜 메서드 구현

// 위치 정보가 갱신됐을 때 호출된다
- (void)locationManager:(CLLocationManager *)manager didUpdateLocations:(NSArray *)locations
{
	if(isReverseGeocoding)
	{
		return;
	}
	
	// 주소를 구하기 위해 가져온 위치 정보를 토대로 역 지오코딩을 수행한다
	isReverseGeocoding = YES;
	
	CLLocation *location = [locations lastObject];
	CLGeocoder *geocoder = [[CLGeocoder alloc] init];
	[geocoder reverseGeocodeLocation:location
				   completionHandler:^(NSArray *placemarks, NSError *error) {
		isReverseGeocoding = NO;

		// CLPlacemark 오브젝트의 FormattedAddressLines로부터 주소 문자열을 구한다
		NSString *addressString = @"";
		if(placemarks.count >= 1)
		{
			CLPlacemark *placemark = [placemarks firstObject];
			NSArray *addressLines = 
				[placemark addressDictionary][@"FormattedAddressLines"];
			addressString = [addressLines componentsJoinedByString:@" "];
		}

		// 파라미터로 넘겨줄 문자열을 작성한다
		NSString *parameter = [NSString stringWithFormat:@"%f\t%f\t%f\t%f\t%@",
							   location.coordinate.latitude, 
							   location.coordinate.longitude,
							   location.speed, location.horizontalAccuracy,
							   addressString];

		//  UnitySendMessage 메서드를 사용해 유니티 쪽 OnUpdateLocation 메서드를 호출한다
		UnitySendMessage([callbackTarget cStringUsingEncoding:NSUTF8StringEncoding],
						 "OnUpdateLocation",
						 [parameter cStringUsingEncoding:NSUTF8StringEncoding]);
	}];
}

// 그 밖의 필수 메서드
- (void)locationManager:(CLLocationManager *)manager
	  didDetermineState:(CLRegionState)state forRegion:(CLRegion *)region {}

- (void)locationManagerDidPauseLocationUpdates:(CLLocationManager *)manager {}

- (void)locationManagerDidResumeLocationUpdates:(CLLocationManager *)manager {}

@end

#pragma mark -  네이티브 플러그인 쪽의 호출 인터페이스 추가

// C++ 컴파일 시에 발생하는 네임 맹글링을 피하기 위해 C 링케이지에서 선언한다
extern "C" {
	// 위치 정보를 가져오는 작업을 시작하는 메서드를 호출하는 인터페이스
	BOOL _startUpdatingLocation(const char *callbackTarget)
	{
		LocationPlugin *instance = [LocationPlugin sharedInstance];
		@synchronized(instance)
		{
			return [instance startUpdatingLocation:
				[NSString stringWithUTF8String:callbackTarget]];
		}
	}

	// 위치 정보를 가져오는 작업을 멈추는 메서드를 호출하는 인터페이스
	void _stopUpdatingLocation()
	{
		LocationPlugin *instance = [LocationPlugin sharedInstance];
		@synchronized(instance)
		{
			[instance stopUpdatingLocation];
		}
	}
}
