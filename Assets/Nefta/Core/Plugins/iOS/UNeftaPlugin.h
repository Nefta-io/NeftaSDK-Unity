#import <Foundation/Foundation.h>
#import <WebKit/WebKit.h>
#import <NeftaPlugin_iOS/NeftaPlugin_iOS-Swift.h>

typedef void (^OnReadyBlock)(NSDictionary<NSString*, Placement*> *);
typedef void (^OnChangeBlock)(int type, Placement *);
typedef void (^OnBidBlock)(int type, Placement*, BidResponse*);
typedef void (^OnLoadFailBlock)(int type, Placement*, NSString*);
typedef void (^OnShowBlock)(int type, Placement*, NSInteger, NSInteger);

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
- (void)SetToolboxUser:(NSString*)user;
- (NSString*)GetToolboxUser;
- (void)Record:(NSString*)event;
- (void)SetPublisherUserId:(NSString *)userId;
- (void)EnableAds:(Boolean)enable;
- (void)SetPlacementModeWithType:(int)type mode:(int)mode;
- (void)SetPlacementModeWithId:(NSString *)pId mode:(int)mode;
- (void)BidWithType:(int)type;
- (void)BidWithId:(NSString *)pId;
- (void)LoadWithType:(int)type;
- (void)LoadWithId:(NSString *)pId;
- (void)ShowWithType:(int)type;
- (void)ShowWithId:(NSString *)pId;
- (void)Close;
- (void)CloseWithId:(NSString*)pId;
- (NSString*)GetMessage;

@end
