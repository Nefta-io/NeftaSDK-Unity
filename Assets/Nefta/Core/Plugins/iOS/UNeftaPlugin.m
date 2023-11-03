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
    self._plugin.OnBid = ^(Types type, Placement *placement, BidResponse *bidResponse) {
        if (weakSelf.OnBid != nil) {
            [weakSelf OnBid]((int)type, placement, bidResponse);
        }
    };
    self._plugin.OnLoadStart = ^(Types type, Placement *placement) {
        if (weakSelf.OnLoadStart != nil) {
            [weakSelf OnLoadStart]((int)type, placement);
        }
    };
    self._plugin.OnLoadFail= ^(Types type, Placement *placement, NSString *error) {
        if (weakSelf.OnLoadFail != nil) {
            [weakSelf OnLoadFail]((int)type, placement, error);
        }
    };
    self._plugin.OnLoad = ^(Types type, Placement *placement) {
        if (weakSelf.OnLoad != nil) {
            [weakSelf OnLoad]((int)type, placement);
        }
    };
    self._plugin.OnShow = ^(Types type, Placement *placement, NSInteger width, NSInteger height) {
        if (weakSelf.OnShow != nil) {
            [weakSelf OnShow]((int)type, placement, (int)width, (int)height);
        }
    };
    self._plugin.OnClose = ^(Types type, Placement *placement) {
        if (weakSelf.OnClose != nil) {
            [weakSelf OnClose]((int)type, placement);
        }
    };
    [self._plugin InitWithUiView: view appId: appId useMessages: useMessages];
}

- (NSString *)GetToolboxUser {
    return [self._plugin GetToolboxUser];
}

- (void)SetToolboxUser:(NSString*)user {
    [self._plugin SetToolboxUserWithJson: user];
}

- (void)SetPublisherUserId:(NSString *)userId {
    [self._plugin SetPublisherUserIdWithId: userId];
}

- (void)Record:(NSString *)event {
    [self._plugin RecordWithEvent: event];
}

- (void)EnableAds:(Boolean)enable {
    [self._plugin EnableAdsWithEnable: enable];
}

- (void)SetPlacementModeWithType:(int)type mode:(int)mode {
    [self._plugin SetPlacementModeWithType: type mode: mode];
}

- (void)SetPlacementModeWithId:(NSString *)pId mode:(int)mode {
    [self._plugin SetPlacementModeWithId: pId mode: mode];
}

- (void)BidWithType:(int)type {
    [self._plugin BidWithType: type];
}

- (void)BidWithId:(NSString*)pId {
    [self._plugin BidWithId: pId];
}

- (void)LoadWithType:(int)type {
    [self._plugin LoadWithType: type];
}

- (void)LoadWithId:(NSString *)pId {
    [self._plugin LoadWithId: pId];
}

- (void)ShowWithType:(int)type {
    [self._plugin ShowWithType: type];
}

- (void)ShowWithId:(NSString *)pId {
    [self._plugin ShowWithId: pId];
}

- (void)Close {
    [self._plugin Close];
}

- (void)CloseWithId:(NSString*)pId {
    [self._plugin CloseWithId: pId];
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
    const char *NeftaPlugin_GetToolboxUser(void *instance);
    void NeftaPlugin_SetToolboxUser(void *instance, const char *user);
    void NeftaPlugin_Record(void *instance, const char *event);
    void NeftaPlugin_SetPublisherUserId(void *instance, const char *userId);
    void NeftaPlugin_EnableAds(void *instance, Boolean enable);
    void NeftaPlugin_SetPlacementModeWithType(void *instance, int type, int mode);
    void NeftaPlugin_SetPlacementModeWithId(void *instance, const char *placementId, int mode);
    void NeftaPlugin_BidWithType(void *instance, int type);
    void NeftaPlugin_BidWithId(void *instance, const char *placementId);
    void NeftaPlugin_LoadWithType(void *instance, int type);
    void NeftaPlugin_LoadWithId(void *instance, const char *placementId);
    void NeftaPlugin_ShowWithType(void *instance, int type);
    void NeftaPlugin_ShowWithId(void *instance, const char *placementId);
    void NeftaPlugin_Close(void *instance);
    void NeftaPlugin_CloseWithId(void *instance, const char *placementId);
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

const char * NeftaPlugin_GetToolboxUser(void *instance)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    NSString *user = [plugin GetToolboxUser];
    if (user == nil) {
        return nil;
    }
    const char *string = [user UTF8String];
    char *returnString = (char *)malloc(strlen(string) + 1);
    strcpy(returnString, string);
    return returnString;
}

void NeftaPlugin_SetToolboxUser(void *instance, const char *user)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin SetToolboxUser: [NSString stringWithUTF8String: user]];
}

void NeftaPlugin_Record(void *instance, const char *event)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin Record: [NSString stringWithUTF8String: event]];
}

void NeftaPlugin_EnableAds(void *instance, Boolean enable)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin EnableAds: enable];
}

void NeftaPlugin_SetPublisherUserId(void *instance, const char *userId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin SetPublisherUserId: [NSString stringWithUTF8String: userId]];
}

void NeftaPlugin_SetPlacementModeWithType(void *instance, int type, int mode)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin SetPlacementModeWithType: type mode: mode];
}

void NeftaPlugin_SetPlacementModeWithId(void *instance, const char *pId, int mode)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin SetPlacementModeWithId: [NSString stringWithUTF8String:pId] mode: mode];
}

void NeftaPlugin_BidWithType(void *instance, int type)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin BidWithType: type];
}

void NeftaPlugin_BidWithId(void *instance, const char *pId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin BidWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_LoadWithType(void *instance, int type)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin LoadWithType: type];
}

void NeftaPlugin_LoadWithId(void *instance, const char *pId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin LoadWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_ShowWithType(void *instance, int type)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin ShowWithType: type];
}

void NeftaPlugin_ShowWithId(void *instance, const char *pId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin ShowWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_Close(void *instance)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin Close];
}

void NeftaPlugin_CloseWithId(void *instance, const char *pId)
{
    UNeftaPlugin *plugin = (__bridge UNeftaPlugin *)instance;
    [plugin CloseWithId: [NSString stringWithUTF8String: pId]];
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
