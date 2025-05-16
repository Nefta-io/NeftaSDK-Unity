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
    typedef void (*OnBid)(const char *pId, float price, int expirationTime);
    typedef void (*OnFail)(const char *pId, int code, const char *error);
    typedef void (*OnLoad)(const char *pId, int width, int height);
    typedef void (*OnChange)(const char *pId);
    typedef void (*OnBehaviourInsight)(int requestId, const char *behaviourInsight);
    
    void NeftaPlugin_EnableLogging(bool enable);
    void UnityWrapper_Init(const char *appId);
    void UnityWrapper_RegisterCallbacks(OnReady onReady, OnBid onBid, OnChange onLoadStart, OnFail onLoadFail, OnLoad onLoad, OnFail onShowFail, OnChange onShow, OnChange onClick, OnChange onClose, OnChange onReward, OnBehaviourInsight OnBehaviourInsight);
    void UnityWrapper_Record(int type, int category, int subCategory, const char *name, long value, const char *customPayload);
    void UnityWrapper_SetPublisherUserId(const char *userId);
    void UnityWrapper_SetContentRating(const char *rating);
    void UnityWrapper_CreateBannerWithId(const char *pId, int position, bool autoRefresh);
    void UnityWrapper_SetFloorPrice(const char *pId, float floorPrice);
    const char * UnityWrapper_GetPartialBidRequest(const char *pId);
    void UnityWrapper_BidWithId(const char *pId);
    void UnityWrapper_LoadWithId(const char *pId);
    void UnityWrapper_LoadWithBidResponse(const char *pId, const char *bidResponse);
    int UnityWrapper_CanShowWithId(const char *pId);
    void UnityWrapper_ShowWithId(const char *pId);
    void UnityWrapper_HideWithId(const char *pId);
    void UnityWrapper_CloseWithId(const char *pId);
    void UnityWrapper_Mute(const char *pId, bool mute);
    void UnityWrapper_SetOverride(const char *root);
    const char * UnityWrapper_GetNuid(bool present);
    void UnityWrapper_GetBehaviourInsight(int requestId, const char *insights);
#ifdef __cplusplus
}
#endif

UnityWrapper *_wrapper;

void NeftaPlugin_EnableLogging(bool enable) {
    [NeftaPlugin EnableLogging: enable];
}

void UnityWrapper_Init(const char *appId) {
    UIViewController *viewController = UnityGetGLViewController();
    _wrapper = [[UnityWrapper alloc] initWithViewController: viewController appId: [NSString stringWithUTF8String: appId]];
}

void UnityWrapper_RegisterCallbacks(OnReady onReady, OnBid onBid, OnChange onLoadStart, OnFail onLoadFail, OnLoad onLoad, OnFail onShowFail, OnChange onShow, OnChange onClick, OnChange onClose, OnChange onReward, OnBehaviourInsight onBehaviourInsight) {
    _wrapper.IOnReady = ^void(NSString * _Nonnull configuration) {
        const char *cConfiguration = [configuration UTF8String];
        onReady(cConfiguration);
    };
    _wrapper.IOnBid = ^void(NSString * _Nonnull pId, float price, NSInteger expirationTime) {
        const char *cPId = [pId UTF8String];
        onBid(cPId, price, (int)expirationTime);
    };
    _wrapper.IOnLoadStart = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onLoadStart(cPId);
    };
    _wrapper.IOnLoadFail = ^void(NSString * _Nonnull pId, NSInteger code, NSString *error) {
        const char *cPId = [pId UTF8String];
        const char *cError = [error UTF8String];
        onLoadFail(cPId, (int)code, cError);
    };
    _wrapper.IOnLoad = ^void(NSString * _Nonnull pId, NSInteger width, NSInteger height) {
        const char *cPId = [pId UTF8String];
        onLoad(cPId, (int)width, (int)height);
    };
    _wrapper.IOnShowFail = ^void(NSString * _Nonnull pId, NSInteger code, NSString *error) {
        const char *cPId = [pId UTF8String];
        const char *cError = [error UTF8String];
        onShowFail(cPId, (int)code, cError);
    };
    _wrapper.IOnShow = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onShow(cPId);
    };
    _wrapper.IOnClick = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onClick(cPId);
    };
    _wrapper.IOnClose = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onClose(cPId);
    };
    _wrapper.IOnReward = ^void(NSString * _Nonnull pId) {
        const char *cPId = [pId UTF8String];
        onReward(cPId);
    };
    _wrapper._plugin.OnBehaviourInsightAsString = ^void(NSInteger requestId, NSString * _Nonnull behaviourInsight) {
        const char *cBI = [behaviourInsight UTF8String];
        onBehaviourInsight((int)requestId, cBI);
    };
}

void UnityWrapper_Record(int type, int category, int subCategory, const char *name, long value, const char *customPayload) {
    NSString *n = name ? [NSString stringWithUTF8String: name] : nil;
    NSString *cp = customPayload ? [NSString stringWithUTF8String: customPayload] : nil;
    [_wrapper._plugin RecordWithType: type category: category subCategory: subCategory name: n value: value customPayload: cp];
}

void UnityWrapper_SetPublisherUserId(const char *userId) {
    [_wrapper._plugin SetPublisherUserIdWithId: [NSString stringWithUTF8String: userId]];
}

void UnityWrapper_SetContentRating(const char *rating) {
    [_wrapper._plugin SetContentRatingWithRating: [NSString stringWithUTF8String: rating]];
}

void UnityWrapper_CreateBannerWithId(const char *pId, int position, bool autoRefresh) {
    [_wrapper CreateBannerWithId: [NSString stringWithUTF8String: pId] position: position autoRefresh: autoRefresh];
}

void UnityWrapper_SetFloorPriceWithId(const char *pId, float floorPrice) {
    [_wrapper SetFloorPriceWithId: [NSString stringWithUTF8String:pId] floorPrice: floorPrice];
}

const char * UnityWrapper_GetPartialBidRequest(const char *pId) {
    if (pId == NULL) {
        return NULL;
    }
    const char *string = [[_wrapper GetPartialBidRequestAsString: [NSString stringWithUTF8String: pId]] UTF8String];
    char *returnString = (char *)malloc(strlen(string) + 1);
    strcpy(returnString, string);
    return returnString;
}

void UnityWrapper_BidWithId(const char *pId) {
    [_wrapper BidWithId: [NSString stringWithUTF8String: pId]];
}

void UnityWrapper_LoadWithId(const char *pId) {
    [_wrapper LoadWithId: [NSString stringWithUTF8String: pId]];
}

void UnityWrapper_LoadWithBidResponse(const char *pId, const char *bidResponse) {
    [_wrapper LoadWithBidResponseWithId: [NSString stringWithUTF8String: pId] bidResponse: [NSData dataWithBytes: bidResponse length: strlen(bidResponse)]];
}

int UnityWrapper_CanShowWithId(const char *pId) {
    return (int) [_wrapper CanShowWithId: [NSString stringWithUTF8String: pId]];
}

void UnityWrapper_ShowWithId(const char *pId) {
    [_wrapper ShowWithId: [NSString stringWithUTF8String: pId]];
}

void UnityWrapper_HideWithId(const char *pId) {
    [_wrapper HideWithId: [NSString stringWithUTF8String: pId]];
}

void UnityWrapper_CloseWithId(const char *pId) {
    [_wrapper CloseWithId: [NSString stringWithUTF8String: pId]];
}

void UnityWrapper_MuteWithId(const char *pId, bool mute) {
    [_wrapper MuteWithId: [NSString stringWithUTF8String: pId] mute: mute];
}

void UnityWrapper_SetOverride(const char *root) {
    [NeftaPlugin SetOverrideWithUrl: [NSString stringWithUTF8String: root]];
}

const char * UnityWrapper_GetNuid(bool present) {
    const char *string = [[_wrapper._plugin GetNuidWithPresent: present] UTF8String];
    char *returnString = (char *)malloc(strlen(string) + 1);
    strcpy(returnString, string);
    return returnString;
}

void UnityWrapper_GetBehaviourInsight(int requestId, const char *insights) {
    [_wrapper._plugin GetBehaviourInsightBridge: requestId string: [NSString stringWithUTF8String: insights]];
}
