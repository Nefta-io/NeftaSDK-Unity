## Introduction

Welcome to the Nefta Toolbox Unity Package!
This package is designed to provide a set of tools and assets to help you quickly and easily create your own games and projects within the Unity game engine.

## Requirements

Unity 2020.3 or higher

## Supported platforms

Standalone, IOS, Android

## NeftaEditorWindow

The Nefta Toolbox Unity Package comes with an EditorWindow. To access the window, you should click _window_ in Unity menu bar and then _Nefta SDK_:
![Opening Nefta SDK editor window](../Documentation/NeftaSDKmenu.png)

## Usage

You can explore the example DemoScene and examine the source code of the scripts and components.

To get started using the package, configure the SDK in Unity menu: Window > Nefta SDK; Configuration > Marketplace Id with 'm-' from Nefta platform as parameter.
In your code initialize toolbox by calling Nefta.ToolboxSdk.Toolbox.Init.

### Logging

By default all logging is enabled. In order to disable them, change the define symbol from NEFTA_SDK_DBG to NEFTA_SDK_REL. You can also change this in the editor window:
![Toggle logging through editor window](../Documentation/ToggleLogging.png)

### oAuth sign-in options

In order to use oAuth sign-in options, you have to set your oAuth provider client id and secret in OAuthPanel.cs.

- A note about _Asset Traits_. You have to add your custom traits into the _Traits_ class. This allows you to access all metadata relating to assets that are created through the dashboard.

- A note about testing the authorisation methods. In order to test the different authorization methods, after each attempt, you will need to click _Clear Logged In PlayerPrefs_ in the NeftaEditorWindow.

- In order for the MetaMask integration to work, you have to install official MetaMask package and add NEFTA_INTEGRATION_METAMASK symbol.

## Documentation

You can find online documentation for the NeftaToolboxSDK here: https://docs.nefta.io/docs/neftaunity-sdk

The BE API reference can be found here: https://docs.nefta.io/reference/

While the Toolbox SDK API reference be be found here: https://nefta-io.github.io/suite-for-unity/ToolboxSdk/Nefta.ToolboxSdk.Toolbox.html

## Support

If you need help or have any questions about the Unity Package, please feel free to reach out to the development team by email at support@Nefta.io. We will do our best to assist you and resolve any issues as quickly as possible.
