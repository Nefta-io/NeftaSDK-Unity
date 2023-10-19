#import <CoreGraphics/CGContext.h>
#import <unistd.h>
#import "UNeftaPlugin.h"

@implementation UNeftaPlugin

- (void)Init:(UIView *)view appId:(NSString*)appId useMessages:(Boolean)useMessages {
    self._plugin = [[NeftaPlugin_iOS alloc] init];
    __unsafe_unretained __typeof(self) weakSelf = self;
    self._plugin.OnReady = ^(NSDictionary<NSString *, Placement *> * placements) {
        if (weakSelf.OnReady != nil) {
            [weakSelf OnReady](placements);
        }
    };
    self._plugin.OnBid = ^(Placement *placement, BidResponse *bidResponse) {
        if (weakSelf.OnBid != nil) {
            [weakSelf OnBid](placement, bidResponse);
        }
    };
    self._plugin.OnLoadStart = ^(Placement *placement) {
        if (weakSelf.OnLoadStart != nil) {
            [weakSelf OnLoadStart](placement);
        }
    };
    self._plugin.OnLoadFail= ^(Placement *placement, NSString *error) {
        if (weakSelf.OnLoadFail != nil) {
            [weakSelf OnLoadFail](placement, error);
        }
    };
    self._plugin.OnLoad = ^(Placement *placement) {
        if (weakSelf.OnLoad != nil) {
            [weakSelf OnLoad](placement);
        }
    };
    self._plugin.OnShow = ^(Placement *placement, NSInteger width, NSInteger height) {
        if (weakSelf.OnShow != nil) {
            [weakSelf OnShow](placement, width, height);
        }
    };
    self._plugin.OnClose = ^(Placement *placement) {
        if (weakSelf.OnClose != nil) {
            [weakSelf OnClose](placement);
        }
    };
    [self._plugin InitWithUiView: view appId: appId useMessages: useMessages];
}

- (void)EnableAds:(Boolean)enable {
    [self._plugin EnableAdsWithEnable: enable];
}

- (NSString *)GetUser {
    return [self._plugin GetUser];
}

- (void)SetUser:(NSString*)user {
    [self._plugin SetUserWithJson: user];
}

- (void)SetPublisherUserId:(NSString *)userId {
    [self._plugin SetPublisherUserIdWithId: userId];
}

- (void)Record:(NSString *)event {
    [self._plugin RecordWithEvent: event];
}

- (void)Bid:(NSString*)placementId {
    [self._plugin BidWithId: placementId];
}

- (void)BidWithAutoLoad:(NSString*)placementId {
    [self._plugin BidWithAutoLoadWithId: placementId];
}

- (void)Load:(NSString*)placementId {
    [self._plugin LoadWithId: placementId];
}

- (void)Show:(NSString*)placementId {
    [self._plugin ShowWithId: placementId];
}

- (void)Close:(NSString*)placementId {
    [self._plugin CloseWithId: placementId];
}

- (void)OnResume {
    [self._plugin OnResume];
}

- (void)OnPause {
    [self._plugin OnPause];
}

- (NSString *)GetMessage {
    return [self._plugin GetMessage];
}

@end

typedef void (*UnityRenderEventFunc)(int eventId);
#ifdef __cplusplus
extern "C" {
#endif
    UIViewController* UnityGetGLViewController();
    void * NeftaPlugin_Init(const char *appId);
    void NeftaPlugin_EnableAds(void *instance, Boolean enable);
    const char *NeftaPlugin_GetUser(void *instance);
    void NeftaPlugin_SetUser(void *instance, const char *user);
    void NeftaPlugin_Record(void *instance, const char *event);
    void NeftaPlugin_SetPublisherUserId(void *instance, const char *userId);
    void NeftaPlugin_Bid(void *instance, const char *placementId);
    void NeftaPlugin_Load(void *instance, const char *placementId);
    void NeftaPlugin_Show(void *instance, const char *placementId);
    void NeftaPlugin_Close(void *instance, const char *placementId);
    void NeftaPlugin_OnResume(void *instance);
    void NeftaPlugin_OnPause(void *instance);
    const char * NeftaPlugin_GetMessage(void *instance);
#ifdef __cplusplus
}
#endif

void * NeftaPlugin_Init(const char *appId)
{
    UNeftaPlugin *plugin = [UNeftaPlugin alloc];
    UIView *view = UnityGetGLViewController().view;
    [plugin Init: view appId: [NSString stringWithUTF8String: appId] useMessages: true];
    return (__bridge_retained void *)plugin;
}

void NeftaPlugin_EnableAds(void *instance, Boolean enable)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin EnableAds: enable];
}

const char * NeftaPlugin_GetUser(void *instance)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    NSString *user = [plugin GetUser];
    if (user == nil) {
        return nil;
    }
    const char *string = [user UTF8String];
    char *returnString = (char *)malloc(strlen(string) + 1);
    strcpy(returnString, string);
    return returnString;
}

void NeftaPlugin_SetUser(void *instance, const char *user)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin SetUser: [NSString stringWithUTF8String: user]];
}

void NeftaPlugin_Record(void *instance, const char *event)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin Record: [NSString stringWithUTF8String: event]];
}

void NeftaPlugin_SetPublisherUserId(void *instance, const char *userId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin SetPublisherUserId: [NSString stringWithUTF8String: userId]];
}

void NeftaPlugin_Bid(void *instance, const char *placementId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin Bid: [NSString stringWithUTF8String: placementId]];
}

void NeftaPlugin_BidWithAutoLoad(void *instance, const char *placementId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin BidWithAutoLoad: [NSString stringWithUTF8String: placementId]];
}

void NeftaPlugin_Load(void *instance, const char *placementId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin Load: [NSString stringWithUTF8String: placementId]];
}

void NeftaPlugin_Show(void *instance, const char *placementId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin Show: [NSString stringWithUTF8String: placementId]];
}

void NeftaPlugin_Close(void *instance, const char *placementId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin Close: [NSString stringWithUTF8String: placementId]];
}

void NeftaPlugin_OnResume(void *instance)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin OnResume];
}

void NeftaPlugin_OnPause(void *instance)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin OnPause];
}

const char * NeftaPlugin_GetMessage(void *instance)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    NSString *message = [plugin GetMessage];
    if (message == nil) {
        return nil;
    }
    const char *string = [message UTF8String];
    char *returnString = (char *)malloc(strlen(string) + 1);
    strcpy(returnString, string);
    return returnString;
}
