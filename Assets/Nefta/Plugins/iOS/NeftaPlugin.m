//
//  NeftaPlugin.m
//  UnityFramework
//
//  Created by Tomaz Treven on 18/11/2023.
//

#import <Foundation/Foundation.h>
#import <WebKit/WebKit.h>
#import <NeftaSDK/NeftaSDK-Swift.h>
#import <UIKit/UIKit.h>

#ifdef __cplusplus
extern "C" {
#endif
    UIViewController* UnityGetGLViewController();
    
    typedef void (*OnReady)(const char *configuration);
    typedef void (*OnBid)(const char *pId, float price);
    typedef void (*OnLoadFail)(const char *pId, const char *error);
    typedef void (*OnLoad)(const char *pId, int width, int height);
    typedef void (*OnChange)(const char *pId);
    
    void EnableLogging(bool enable);
    void * NeftaPlugin_Init(const char *appId);
    void NeftaPlugin_RegisterCallbacks(OnReady onReady, OnBid onBid, OnChange onLoadStart, OnLoadFail onLoadFail, OnLoad onLoad, OnChange onShow, OnChange onClick, OnChange onClose, OnChange onReward);
    void NeftaPlugin_Record(void *instance, const char *event);
    void NeftaPlugin_SetPublisherUserId(void *instance, const char *userId);
    void NeftaPlugin_EnableAds(void *instance, Boolean enable);
    void NeftaPlugin_EnableBannerWithId(void *instance, const char *pId, Boolean enable);
    void NeftaPlugin_SetPlacementPositionWithId(void *instance, const char *pId, int position);
    void NeftaPlugin_SetPlacementModeWithId(void *instance, const char *pId, int mode);
    void NeftaPlugin_SetFloorPrice(void *instance, const char *pId, float floorPrice);
    const char * NeftaPlugin_GetPartialBidRequest(void *instance, const char *pId);
    void NeftaPlugin_BidWithId(void *instance, const char *pId);
    void NeftaPlugin_LoadWithId(void *instance, const char *pId);
    void NeftaPlugin_LoadWithBidResponse(void *instance, const char *pId, const char *bidResponse);
    void NeftaPlugin_ShowWithId(void *instance, const char *pId);
    void NeftaPlugin_Close(void *instance);
    void NeftaPlugin_CloseWithId(void *instance, const char *pId);
    void NeftaPlugin_Mute(void *instance, bool mute);
    void NeftaPlugin_SetOverride(void *instance, const char *root);
    const char * NeftaPlugin_GetNuid(void *instance, bool present);
#ifdef __cplusplus
}
#endif

NeftaPlugin *_plugin;

void NeftaPlugin_EnableLogging(bool enable) {
    [NeftaPlugin EnableLogging: enable];
}

void * NeftaPlugin_Init(const char *appId) {
    _plugin = [NeftaPlugin InitWithAppId: [NSString stringWithUTF8String: appId]];
    [_plugin PrepareRendererWithViewController: UnityGetGLViewController()];
    return (__bridge_retained void *)_plugin;
}

void NeftaPlugin_RegisterCallbacks(OnReady onReady, OnBid onBid, OnChange onLoadStart, OnLoadFail onLoadFail, OnLoad onLoad, OnChange onShow, OnChange onClick, OnChange onClose, OnChange onReward) {
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
    _plugin.IOnLoad = ^void(NSString * _Nonnull pId, NSInteger width, NSInteger height) {
        const char *cPId = [pId UTF8String];
        onLoad(cPId, (int)width, (int)height);
    };
    _plugin.IOnShow = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onShow(cPId);
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

void NeftaPlugin_Record(void *instance, const char *event) {
    [_plugin RecordWithEvent: [NSString stringWithUTF8String: event]];
}

void NeftaPlugin_EnableAds(void *instance, Boolean enable) {
    [_plugin EnableAds: enable];
}

void NeftaPlugin_SetPublisherUserId(void *instance, const char *userId) {
    [_plugin SetPublisherUserIdWithId: [NSString stringWithUTF8String: userId]];
}

void NeftaPlugin_EnableBannerWithId(void *instance, const char *pId, Boolean enable) {
    [_plugin EnableBannerWithId: [NSString stringWithUTF8String: pId] enable: enable];
}

void NeftaPlugin_SetPlacementPositionWithId(void *instance, const char *pId, int position) {
    [_plugin SetPlacementPositionWithId: [NSString stringWithUTF8String:pId] position: position];
}

void NeftaPlugin_SetFloorPrice(void *instance, const char *pId, float floorPrice) {
    [_plugin SetFloorPriceWithId: [NSString stringWithUTF8String:pId] floorPrice: floorPrice];
}

void NeftaPlugin_SetPlacementModeWithId(void *instance, const char *pId, int mode) {
    [_plugin SetPlacementModeWithId: [NSString stringWithUTF8String:pId] mode: mode];
}

const char * NeftaPlugin_GetPartialBidRequest(void *instance, const char *pId) {
    if (pId == NULL) {
        return NULL;
    }
    const char *string = [[_plugin GetPartialBidRequestAsString: [NSString stringWithUTF8String: pId]] UTF8String];
    char *returnString = (char *)malloc(strlen(string) + 1);
    strcpy(returnString, string);
    return returnString;
}

void NeftaPlugin_BidWithId(void *instance, const char *pId) {
    [_plugin BidWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_LoadWithId(void *instance, const char *pId) {
    [_plugin LoadWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_LoadWithBidResponse(void *instance, const char *pId, const char *bidResponse) {
    [_plugin LoadWithBidResponseWithId: [NSString stringWithUTF8String: pId] bidResponse: [NSData dataWithBytes: bidResponse length: strlen(bidResponse)]];
}

void NeftaPlugin_ShowWithId(void *instance, const char *pId) {
    [_plugin ShowWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_Close(void *instance) {
    [_plugin Close];
}

void NeftaPlugin_CloseWithId(void *instance, const char *pId) {
    [_plugin CloseWithId: [NSString stringWithUTF8String: pId]];
}

void NeftaPlugin_Mute(void *instance, bool mute) {
    [_plugin Mute: mute];
}

void NeftaPlugin_SetOverride(void *instance, const char *root) {
    [_plugin SetOverrideWithUrl: [NSString stringWithUTF8String: root]];
}

const char * NeftaPlugin_GetNuid(void *instance, bool present) {
    const char *string = [[_plugin GetNuidWithPresent: present] UTF8String];
    char *returnString = (char *)malloc(strlen(string) + 1);
    strcpy(returnString, string);
    return returnString;
}
