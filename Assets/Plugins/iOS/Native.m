#import <Foundation/Foundation.h>
#import <WebKit/WebKit.h>
#import <NeftaSDK/NeftaSDK-Swift.h>
#import <AdSupport/ASIdentifierManager.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>

#ifdef __cplusplus
extern "C" {
#endif
    void CheckTrackingPermission();
#ifdef __cplusplus
}
#endif

void CheckTrackingPermission() {
    if (@available(iOS 14.5, *)) {
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
            [NeftaPlugin._instance SetTrackingWithIsAuthorized: status == ATTrackingManagerAuthorizationStatusAuthorized];
        }];
    } else {
        [NeftaPlugin._instance SetTrackingWithIsAuthorized: [[ASIdentifierManager sharedManager] isAdvertisingTrackingEnabled]];
    }
}