# Unity 获取分发渠道号 (iOS 和 Android)

本插件用于在 Unity 项目中获取 iOS 和 Android 平台的分发渠道号。它是 `https://github.com/GameFrameX/GameFrameX` 项目的一个子模块。

## 主要功能

- 支持 iOS 和 Android 双平台。
- 提供简单的 API 来获取预设的渠道信息。
- iOS 平台在构建时自动在 `Info.plist` 中添加默认渠道号 (如果未设置)。

## 如何安装

您可以通过以下三种方式将此插件添加到您的 Unity 项目中：

1.  **通过 `manifest.json` 添加依赖：**
    在项目 `Packages` 目录下的 `manifest.json` 文件的 `dependencies` 节点中添加如下内容：
    ```json
    {
      "dependencies": {
        "com.gameframex.unity.getchannel": "https://github.com/gameframex/com.gameframex.unity.getchannel.git",
        // ... 其他依赖
      }
    }
    ```

2.  **通过 Unity Package Manager 使用 Git URL：**
    在 Unity 编辑器中，打开 `Window -> Package Manager`。
    点击左上角的 `+` 号按钮，选择 `Add package from git URL...`。
    输入以下 URL 并点击 `Add`:
    ```
    https://github.com/gameframex/com.gameframex.unity.getchannel.git
    ```

3.  **直接下载或克隆仓库：**
    将此仓库下载或克隆到您 Unity 项目的 `Packages` 目录下。Unity 会自动识别并加载该插件。

## 如何使用

### 1. 获取渠道号

在您的 C# 脚本中，使用 `BlankGetChannel.GetChannelName(string key)` 方法来获取渠道号。参数 `key` 是您在对应平台设置渠道号时使用的键名。

**示例代码：**

```csharp
using UnityEngine;

public class MyGameScript : MonoBehaviour
{
    void Start()
    {
        // 假设您在 AndroidManifest.xml 或 Info.plist 中使用的键是 "channelName"
        string channel = BlankGetChannel.GetChannelName("channelName"); 
        Debug.Log("当前渠道号: " + channel);

        // 示例中使用的键是 "appchannel"
        // string exampleChannel = BlankGetChannel.GetChannelName("appchannel");
        // Debug.Log("示例渠道号: " + exampleChannel);
    }
}
```

### 2. iOS 平台设置

对于 iOS 平台，插件包含一个构建后处理器 (`PostProcessBuildHandler.cs`)。如果在构建时项目的 `Info.plist` 文件中：
-   **没有** 名为 `channel` 的键，该脚本会自动添加一个键为 `channel`，值为 `default` 的条目。
-   **已经存在** 名为 `channel` 的键，则不会进行任何修改。

您可以在 Xcode 项目的 `Info.plist` 文件中修改 `channel` 的值，或者在调用 `BlankGetChannel.GetChannelName()` 时使用您自定义的键名（确保该键名存在于 `Info.plist` 中）。

### 3. Android 平台设置

对于 Android 平台，您需要在 `AndroidManifest.xml` 文件中定义渠道信息。通常，这是通过在 `<application>` 标签内添加 `<meta-data>` 标签来完成的。

例如，如果您想使用键名 `channelName` 和值为 `myCustomChannel`：

```xml
<application ...>
    <activity ...>
        ...
    </activity>

    <meta-data
        android:name="channelName"
        android:value="myCustomChannel" />

    <!-- 其他 meta-data -->
</application>
```

然后，您可以在 C# 代码中通过 `BlankGetChannel.GetChannelName("channelName")` 来获取这个值 (`myCustomChannel`)。

## 注意事项

-   确保您在调用 `BlankGetChannel.GetChannelName(string key)` 时使用的 `key` 与您在 `Info.plist` (iOS) 或 `AndroidManifest.xml` (Android) 中设置的键名一致。
-   插件包含 `link.xml` 文件以防止代码被 Unity 的代码裁剪功能移除。
