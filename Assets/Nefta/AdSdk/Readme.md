# Introduction

Welcome to the Nefta Ad Unity Package! The purpose of this package is to simplify ad integration into your app to monetize it.

## Requirements

- Unity 2020.3 or higher
- Android: API level 22 or higher
- iOS: version 11 or higher

## Supported platforms

Standalone, IOS, Android

### Getting started

+ The first step is to initialize Nefta Ad Manager. If you don't, the SDK won't do anything on its own and it won't waste any of the device resources.
    ```
    AdManager.Init();
    ```
+ In case you want to have your own identifier for each user you can set it through:
    ```
    AdManager.Instance._adManager.UserId = "user1";
    ```
+ Then you also need to call Update on each frame. With this, you have absolute control over the order of various ad-related events:
    ```
    AdManager.Instance.Update();
    ```
  All ad showing/closing and callbacks will happen here. So all calls to show/hide ads after the Update will be scheduled for the next frame.

### Loading Ad Placement

+ You request an ad for a specific placement, defined in the dashboard, as follows:
    ```
    AdManager.Instance.LoadAd("placementId");
    ```

  + After which either success or failure callback is called:
      ```
      AdManager.Instance._onLoad(adPlacement);
      AdManager.Instance._onLoadFail(adPlacement, AdPlacement.LoadFailReason);
      ```
    You also have two methods, to check if an ad opportunity is available for your placement and the amount you will earn for showing it:
      ```
      AdManager.Instance.IsAdAvailable("placementId");
  
      AdManager.Instance.GetAvailableAdPrice(_bannerPlacement1);
      ```

### Showing of Ad Placement

+ When you want to show specific loaded ad placement you call:
    ```
    AdManager.Instance.ShowAd("placementId");
    ```
  + When the ad is shown the following callback is invoked:
    ```
    AdManager.Instance._onShow(adPlacement);
    ```

### Closing placements

+ You can close an ad at any time by calling:
  ```
  AdManager.Instance.Close(adPlacement);
  ```
  The player can also close interstitial or rewarded videos by pressing either the x button or back button on Android or the Escape in keyboard input is present.

    + When the ad is closed, the following callback is fired:
      ```
      AdManager.Instance._onClose(adPlacement);
      ```

  + In the case of rewarded video, if the video was watched till the end the rewarded callback is also fired:
    ```
    AdManager.Instance._onUserRewarded(adPlacement);
    ```

## Demo sample

Feel free to check out AdDemo for an integration example.

## Documentation

You can find online documentation for the NeftaToolboxSDK here: https://dash.readme.com/project/nefta2/v1.0/docs/events

The API reference can be found here: https://nefta-io.github.io/suite-for-unity/AdSdk/index.html


