<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Unity Get Channel for Multiple Platforms

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.getchannel)](https://github.com/GameFrameX/com.gameframex.unity.getchannel/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.getchannel)](https://github.com/GameFrameX/com.gameframex.unity.getchannel/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## Features

- **Multi-platform support**: iOS, tvOS, visionOS, Android, Editor, PC (Windows/Mac/Linux), WebGL, UWP, PS4, PS5, Xbox One, Nintendo Switch.
- Provides a simple API to retrieve predefined channel information.
- Automatically adds a default channel to `Info.plist` during iOS builds (if not already set).

## Installation

You can add this plugin to your Unity project in one of the following three ways:

1. **Add via `manifest.json`:**
    Add the following to the `dependencies` node in the `manifest.json` file in your project's `Packages` directory:
    ```json
    {
      "dependencies": {
        "com.gameframex.unity.getchannel": "https://github.com/gameframex/com.gameframex.unity.getchannel.git",
        // ... other dependencies
      }
    }
    ```

2. **Via Unity Package Manager using Git URL:**
    In the Unity Editor, open `Window -> Package Manager`.
    Click the `+` button in the top-left corner and select `Add package from git URL...`.
    Enter the following URL and click `Add`:
    ```
    https://github.com/gameframex/com.gameframex.unity.getchannel.git
    ```

3. **Download or Clone Repository:**
    Download or clone this repository into the `Packages` directory of your Unity project. Unity will automatically recognize and load the plugin.

## Usage

### 1. Getting Channel Information

In your C# scripts, use the `BlankGetChannel.GetChannelName(string key)` method to retrieve channel information. The `key` parameter is the key name you used when setting up the channel information on the corresponding platform.

**Example Code:**

```csharp
using UnityEngine;

public class MyGameScript : MonoBehaviour
{
    void Start()
    {
        // Get default channel (key name is "channel")
        string channel = BlankGetChannel.GetChannelName();
        Debug.Log("Current channel: " + channel);

        // Get channel with a specific key
        string customChannel = BlankGetChannel.GetChannelName("channelName");
        Debug.Log("Custom channel: " + customChannel);

        // Get channel with a default fallback value
        string subChannel = BlankGetChannel.GetChannelName("sub_channel", "unknown");
        Debug.Log("Sub channel: " + subChannel);
    }
}
```

### 2. iOS / tvOS / visionOS Platform Setup

For iOS, tvOS, and visionOS platforms, the plugin includes a build post-processor (`PostProcessBuildHandler.cs`). When building, if the project's `Info.plist` file:
-   **Does not** have a key named `channel`, the script will automatically add an entry with key `channel` and value `default`.
-   **Already has** a key named `channel`, no modifications will be made.

You can modify the `channel` value in the Xcode project's `Info.plist` file, or use your custom key name when calling `BlankGetChannel.GetChannelName()` (ensure that key name exists in `Info.plist`).

**Info.plist Configuration Example:**

```xml
<key>channel</key>
<string>ios_cn_taptap</string>

<key>sub_channel</key>
<string>beta</string>
```

### 3. Android Platform Setup

For the Android platform, you need to define channel information in the `AndroidManifest.xml` file. This is typically done by adding `<meta-data>` tags within the `<application>` tag.

For example, if you want to use the key name `channel` and value `android_cn_taptap`:

```xml
<application ...>
    <activity ...>
        ...
    </activity>

    <meta-data
        android:name="channel"
        android:value="android_cn_taptap" />

    <meta-data
        android:name="sub_channel"
        android:value="beta" />

    <!-- other meta-data -->
</application>
```

Then, you can retrieve this value in C# code using `BlankGetChannel.GetChannelName("channel")`.

### 4. Editor / PC / WebGL / UWP / Console Platforms Setup

For Editor, PC (Windows/Mac/Linux), WebGL, UWP, PS4, PS5, Xbox One, Nintendo Switch, and other platforms, you need to create a text file named `application_config.txt` in the `Resources` folder of your Unity project.

**application_config.txt File Format Example:**

```
channel=editor_cn_test
sub_channel=beta
other_key=other_value
```

Each line format is: `key=value`

The plugin will automatically read the key-value pairs from this file and cache them for subsequent use.

## Notes

-   Ensure that the `key` you use when calling `BlankGetChannel.GetChannelName(string key)` matches the key name you set in the corresponding platform's configuration file:
    -   **iOS / tvOS / visionOS**: `Info.plist` file
    -   **Android**: `<meta-data>` tags in `AndroidManifest.xml` file
    -   **Editor / PC / WebGL / UWP / Console Platforms**: `Resources/application_config.txt` file
-   The plugin includes a `link.xml` file to prevent code from being removed by Unity's code stripping feature.
-   The `GetChannelName()` method caches channel information to avoid repeated reading of configuration files, improving performance.
