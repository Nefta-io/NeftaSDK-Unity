#import <Foundation/Foundation.h>
#import <WebKit/WebKit.h>
#import <NeftaPlugin_iOS/NeftaPlugin_iOS-Swift.h>

typedef void (^OnReadyBlock)(NSDictionary<NSString*, Placement*> *);
typedef void (^OnChangeBlock)(Placement *);
typedef void (^OnBidBlock)(Placement*, BidResponse*);
typedef void (^OnLoadFailBlock)(Placement*, NSString*);
typedef void (^OnShowBlock)(Placement*, NSInteger, NSInteger);

@interface UNeftaPlugin : NSObject

@property (nonatomic) NeftaPlugin_iOS *_plugin;
@property (nonatomic) OnReadyBlock OnReady;
@property (nonatomic) OnBidBlock OnBid;
@property (nonatomic) OnChangeBlock OnLoadStart;
@property (nonatomic) OnLoadFailBlock OnLoadFail;
@property (nonatomic) OnChangeBlock OnLoad;
@property (nonatomic) OnShowBlock OnShow;
@property (nonatomic) OnChangeBlock OnClose;
@property (nonatomic) OnChangeBlock OnReward;

- (void)Init:(UIView*)view appId:(NSString*)appId useMessages:(Boolean)useMessages;
- (void)EnableAds:(Boolean)enable;
- (NSString*)GetUser;
- (void)SetUser:(NSString*)user;
- (void)SetPublisherUserId:(NSString *)userId;
- (void)Record:(NSString*)event;
- (void)Bid:(NSString*)placementId;
- (void)BidWithAutoLoad:(NSString*)placementId;
- (void)Load:(NSString*)placementId;
- (void)Show:(NSString*)placementId;
- (void)Close:(NSString*)placementId;
- (void)OnResume;
- (void)OnPause;
- (NSString*)GetMessage;

@end
