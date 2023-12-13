//
//  NeftaPlugin_iOS.m
//  UnityFramework
//
//  Created by Tomaz Treven on 18/11/2023.
//

#import <Foundation/Foundation.h>
#import <WebKit/WebKit.h>
#import <NeftaSDK/NeftaSDK-Swift.h>

#ifdef __cplusplus
extern "C" {
#endif
    UIViewController* UnityGetGLViewController();
    
    typedef void (*OnReady)(const char *configuration);
    typedef void (*OnBid)(const char *pId, float price);
    typedef void (*OnChange)(const char *pId);
    typedef void (*OnLoadFail)(const char *pId, const char *error);
    typedef void (*OnShow)(const char *pId, int width, int height);
    
    void EnableLogging(bool enable);
    void * NeftaPlugin_Init(const char *appId);
    void NeftaPlugin_RegisterCallbacks(OnReady onReady, OnBid onBid, OnChange onLoadStart, OnLoadFail onLoadFail, OnChange onLoad, OnShow onShow, OnShow onBannerChange, OnChange onClick, OnChange onClose, OnChange onReward);
    const char *NeftaPlugin_GetToolboxUser(void *instance);
    void NeftaPlugin_SetToolboxUser(void *instance, const char *user);
    void NeftaPlugin_SetCustomBatchSize(void *instance, int newBatchSize);
    void NeftaPlugin_Record(void *instance, const char *event);
    void NeftaPlugin_SetPublisherUserId(void *instance, const char *userId);
    void NeftaPlugin_EnableAds(void *instance, Boolean enable);
    void NeftaPlugin_EnableBannerWithType(void *instance, Boolean enable);
    void NeftaPlugin_EnableBannerWithId(void *instance, const char *pId, Boolean enable);
    void NeftaPlugin_SetPlacementModeWithType(void *instance, int type, int mode);
    void NeftaPlugin_SetPlacementModeWithId(void *instance, const char *pId, int mode);
    void NeftaPlugin_BidWithType(void *instance, int type);
    void NeftaPlugin_BidWithId(void *instance, const char *pId);
    void NeftaPlugin_LoadWithType(void *instance, int type);
    void NeftaPlugin_LoadWithId(void *instance, const char *pId);
    void NeftaPlugin_ShowWithType(void *instance, int type);
    void NeftaPlugin_ShowWithId(void *instance, const char *pId);
    void NeftaPlugin_Close(void *instance);
    void NeftaPlugin_CloseWithId(void *instance, const char *pId);
#ifdef __cplusplus
}
#endif

NeftaPlugin_iOS *_plugin;

void NeftaPlugin_EnableLogging(bool enable) {
    [NeftaPlugin_iOS EnableLogging: enable];
}

void * NeftaPlugin_Init(const char *appId)
{
    UIView *view = UnityGetGLViewController().view;
    _plugin = [NeftaPlugin_iOS InitWithAppId: [NSString stringWithUTF8String: appId]];
    [_plugin PrepareRendererWithView: view];
    return (__bridge_retained void *)_plugin;
}

void NeftaPlugin_RegisterCallbacks(OnReady onReady, OnBid onBid, OnChange onLoadStart, OnLoadFail onLoadFail, OnChange onLoad, OnShow onShow, OnShow onBannerChange, OnChange onClick, OnChange onClose, OnChange onReward)
{
    _plugin.IOnReady = ^void(NSString * _Nonnull configuration) {
        const char *cConfiguration = [configuration UTF8String];
        onReady(cConfiguration);
    };
    _plugin.IOnBid = ^void(NSString * _Nonnull pId, float price) {
        const char *cPId = [pId UTF8String];
        onBid(cPId, price);
    };
    _plugin.IOnLoadStart = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onLoadStart(cPId);
    };
    _plugin.IOnLoadFail = ^void(NSString * _Nonnull pId, NSString *error) {
        const char *cPId = [pId UTF8String];
        const char *cError = [error UTF8String];
        onLoadFail(cPId, cError);
    };
    _plugin.IOnLoad = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onLoad(cPId);
    };
    _plugin.IOnShow = ^void(NSString * _Nonnull pId, NSInteger width, NSInteger height) {
        const char *cPId = [pId UTF8String];
        onShow(cPId, (int)width, (int) height);
    };
    _plugin.IOnBannerChange = ^void(NSString * _Nonnull pId, NSInteger width, NSInteger height) {
        const char *cPId = [pId UTF8String];
        onBannerChange(cPId, (int)width, (int) height);
    };
    _plugin.IOnClick = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onClick(cPId);
    };
    _plugin.IOnClose = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onClose(cPId);
    };
    _plugin.IOnReward = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onReward(cPId);
    };
}

const char * NeftaPlugin_GetToolboxUser(void *instance)
{
    NSString *user = [_plugin GetToolboxUser];
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
    [_plugin SetToolboxUserWithJson: [NSString stringWithUTF8String: user]];
}

void NeftaPlugin_Record(void *instance, const char *event)
{
    [_plugin RecordWithEvent: [NSString stringWithUTF8String: event]];
}

void NeftaPlugin_SetCustomBatchSize(void *instance, int newBatchSize)
{
    [_plugin SetCustomBatchSize: newBatchSize];
}

void NeftaPlugin_EnableAds(void *instance, Boolean enable)
{
    [_plugin EnableAds: enable];
}

void NeftaPlugin_SetPublisherUserId(void *instance, const char *userId)
{
    [_plugin SetPublisherUserIdWithId: [NSString stringWithUTF8String: userId]];
}

void NeftaPlugin_EnableBannerWithType(void *instance, Boolean enable)
{
    [_plugin EnableBannerWithEnable: enable];
}

void NeftaPlugin_EnableBannerWithId(void *instance, const char *pId, Boolean enable)
{
    [_plugin EnableBannerWithId: [NSString stringWithUTF8String: pId] enable: enable];
}

void NeftaPlugin_SetPlacementModeWithType(void *instance, int type, int mode)
{
    [_plugin SetPlacementModeWithType: type mode: mode];
}

void NeftaPlugin_SetPlacementModeWithId(void *instance, const char *pId, int mode)
{
    [_plugin SetPlacementModeWithId: [NSString stringWithUTF8String:pId] mode: mode];
}

void NeftaPlugin_BidWithType(void *instance, int type)
{
    [_plugin BidWithType: type];
}

void NeftaPlugin_BidWithId(void *instance, const char *pId)
{
    [_plugin BidWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_LoadWithType(void *instance, int type)
{
    [_plugin LoadWithType: type];
}

void NeftaPlugin_LoadWithId(void *instance, const char *pId)
{
    [_plugin LoadWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_ShowWithType(void *instance, int type)
{
    [_plugin ShowWithType: type];
}

void NeftaPlugin_ShowWithId(void *instance, const char *pId)
{
    [_plugin ShowWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_Close(void *instance)
{
    [_plugin Close];
}

void NeftaPlugin_CloseWithId(void *instance, const char *pId)
{
    [_plugin CloseWithId: [NSString stringWithUTF8String: pId]];
}
