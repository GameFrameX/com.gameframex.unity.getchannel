<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Unity 多平台渠道号获取

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.getchannel)](https://github.com/GameFrameX/com.gameframex.unity.getchannel/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.getchannel)](https://github.com/GameFrameX/com.gameframex.unity.getchannel/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#quick-start) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 主要功能

- **多平台支持**：iOS、tvOS、visionOS、Android、Editor、PC（Windows/Mac/Linux）、WebGL、UWP、PS4、PS5、Xbox One、Nintendo Switch。
- 提供简单的 API 来获取预设的渠道信息。
- iOS 平台在构建时自动在 `Info.plist` 中添加默认渠道号（如果未设置）。

## 快速开始

### 安装

选择以下任一方式：

1. 编辑 Unity 项目的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.getchannel": "1.3.2"
     }
   }
   ```

   `scopes` 控制哪些包通过此注册表解析。只有以 `com.gameframex` 开头的包才会从这个注册表获取。

2. 直接在 `manifest.json` 的 `dependencies` 节点下添加以下内容：
   ```json
   {
      "com.gameframex.unity.getchannel": "https://github.com/gameframex/com.gameframex.unity.getchannel.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加库，地址为：`https://github.com/gameframex/com.gameframex.unity.getchannel.git`
4. 直接下载仓库放置到 Unity 项目的 `Packages` 目录下，会自动加载识别。
### 安装

编辑 Unity 项目的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：

```json
{
  "scopedRegistries": [
    {
      "name": "GameFrameX",
      "url": "https://gameframex.upm.alianblank.uk",
      "scopes": [
        "com.gameframex"
      ]
    }
  ]
}
```

`scopes` 控制哪些包通过此注册表解析。只有以 `com.gameframex` 开头的包才会从这个注册表获取。

Then add the package to `dependencies`:

```json
{
  "dependencies": {
    "com.gameframex.unity.getchannel": "1.3.2"
  }
}
```


## 使用方法

### 1. 获取渠道号

在您的 C# 脚本中，使用 `BlankGetChannel.GetChannelName(string key)` 方法来获取渠道号。参数 `key` 是您在对应平台设置渠道号时使用的键名。

**示例代码：**

```csharp
using UnityEngine;

public class MyGameScript : MonoBehaviour
{
    void Start()
    {
        // 获取默认渠道号（键名为 "channel"）
        string channel = BlankGetChannel.GetChannelName();
        Debug.Log("当前渠道号: " + channel);

        // 获取指定键的渠道号
        string customChannel = BlankGetChannel.GetChannelName("channelName");
        Debug.Log("自定义渠道号: " + customChannel);

        // 获取渠道号，并指定默认值
        string subChannel = BlankGetChannel.GetChannelName("sub_channel", "unknown");
        Debug.Log("子渠道号: " + subChannel);
    }
}
```

### 2. iOS / tvOS / visionOS 平台设置

对于 iOS、tvOS 和 visionOS 平台，插件包含一个构建后处理器 (`PostProcessBuildHandler.cs`)。如果在构建时项目的 `Info.plist` 文件中：
- **没有** 名为 `channel` 的键，该脚本会自动添加一个键为 `channel`，值为 `default` 的条目。
- **已经存在** 名为 `channel` 的键，则不会进行任何修改。

您可以在 Xcode 项目的 `Info.plist` 文件中修改 `channel` 的值，或者在调用 `BlankGetChannel.GetChannelName()` 时使用您自定义的键名（确保该键名存在于 `Info.plist` 中）。

**Info.plist 配置示例：**

```xml
<key>channel</key>
<string>ios_cn_taptap</string>

<key>sub_channel</key>
<string>beta</string>
```

### 3. Android 平台设置

对于 Android 平台，您需要在 `AndroidManifest.xml` 文件中定义渠道信息。通常，这是通过在 `<application>` 标签内添加 `<meta-data>` 标签来完成的。

例如，如果您想使用键名 `channel` 和值为 `android_cn_taptap`：

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

    <!-- 其他 meta-data -->
</application>
```

然后，您可以在 C# 代码中通过 `BlankGetChannel.GetChannelName("channel")` 来获取这个值。

### 4. Editor / PC / WebGL / UWP / 主机平台设置

对于 Editor、PC（Windows/Mac/Linux）、WebGL、UWP、PS4、PS5、Xbox One、Nintendo Switch 等平台，您需要在 Unity 项目的 `Resources` 文件夹下创建一个名为 `application_config.txt` 的文本文件。

**application_config.txt 文件格式示例：**

```
channel=editor_cn_test
sub_channel=beta
other_key=other_value
```

每行格式为：`键名=值`

插件会自动读取该文件中的键值对，并缓存起来供后续使用。

## 注意事项

- 确保您在调用 `BlankGetChannel.GetChannelName(string key)` 时使用的 `key` 与您在对应平台的配置文件中设置的键名一致：
    - **iOS / tvOS / visionOS**：`Info.plist` 文件
    - **Android**：`AndroidManifest.xml` 文件中的 `<meta-data>` 标签
    - **Editor / PC / WebGL / UWP / 主机平台**：`Resources/application_config.txt` 文件
- 插件包含 `link.xml` 文件以防止代码被 Unity 的代码裁剪功能移除。
- `GetChannelName()` 方法会缓存渠道信息，避免重复读取配置文件，提高性能。
