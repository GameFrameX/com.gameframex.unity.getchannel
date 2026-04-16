<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# Unity 多平台渠道號獲取

[![Version](https://img.shields.io/badge/version-1.3.0-blue.svg)](https://github.com/gameframex/com.gameframex.unity.getchannel)
[![Unity](https://img.shields.io/badge/Unity-2017.1+-green.svg)](https://unity3d.com/get-unity/download)
[![License](https://img.shields.io/badge/license-MIT+Apache%202.0-orange.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex.doc.alianblank.com-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使**

[📖 文檔](https://gameframex.doc.alianblank.com) • [🚀 快速開始](#安裝) • [💬 QQ群: 467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)

---

🌐 **語言**: [English](./README.md) | [简体中文](./README.zh-CN.md) | [**繁體中文**](./README.zh-TW.md) | [日本語](./README.ja.md) | [한국어](./README.ko.md)

---

</div>

本插件用於在 Unity 專案中獲取多平台的分發渠道號（支援 iOS、tvOS、visionOS、Android、Editor、PC、WebGL、UWP 和主機平台）。它是 `https://github.com/GameFrameX/GameFrameX` 專案的一個子模組。

## 主要功能

- **多平台支援**：iOS、tvOS、visionOS、Android、Editor、PC（Windows/Mac/Linux）、WebGL、UWP、PS4、PS5、Xbox One、Nintendo Switch。
- 提供簡單的 API 來獲取預設的渠道資訊。
- iOS 平台在構建時自動在 `Info.plist` 中添加預設渠道號（如果未設定）。

## 安裝

您可以透過以下三種方式將此插件添加到您的 Unity 專案中：

1. **透過 `manifest.json` 添加依賴：**
    在專案 `Packages` 目錄下的 `manifest.json` 檔案的 `dependencies` 節點中添加如下內容：
    ```json
    {
      "dependencies": {
        "com.gameframex.unity.getchannel": "https://github.com/gameframex/com.gameframex.unity.getchannel.git",
        // ... 其他依賴
      }
    }
    ```

2. **透過 Unity Package Manager 使用 Git URL：**
    在 Unity 編輯器中，開啟 `Window -> Package Manager`。
    點擊左上角的 `+` 號按鈕，選擇 `Add package from git URL...`。
    輸入以下 URL 並點擊 `Add`：
    ```
    https://github.com/gameframex/com.gameframex.unity.getchannel.git
    ```

3. **直接下載或複製儲存庫：**
    將此儲存庫下載或複製到您 Unity 專案的 `Packages` 目錄下。Unity 會自動識別並載入該插件。

## 使用方法

### 1. 獲取渠道號

在您的 C# 腳本中，使用 `BlankGetChannel.GetChannelName(string key)` 方法來獲取渠道號。參數 `key` 是您在對應平台設定渠道號時使用的鍵名。

**範例程式碼：**

```csharp
using UnityEngine;

public class MyGameScript : MonoBehaviour
{
    void Start()
    {
        // 獲取預設渠道號（鍵名為 "channel"）
        string channel = BlankGetChannel.GetChannelName();
        Debug.Log("當前渠道號: " + channel);

        // 獲取指定鍵的渠道號
        string customChannel = BlankGetChannel.GetChannelName("channelName");
        Debug.Log("自訂渠道號: " + customChannel);

        // 獲取渠道號，並指定預設值
        string subChannel = BlankGetChannel.GetChannelName("sub_channel", "unknown");
        Debug.Log("子渠道號: " + subChannel);
    }
}
```

### 2. iOS / tvOS / visionOS 平台設定

對於 iOS、tvOS 和 visionOS 平台，插件包含一個構建後處理器 (`PostProcessBuildHandler.cs`)。如果在構建時專案的 `Info.plist` 檔案中：
- **沒有** 名為 `channel` 的鍵，該腳本會自動添加一個鍵為 `channel`，值為 `default` 的條目。
- **已經存在** 名為 `channel` 的鍵，則不會進行任何修改。

您可以在 Xcode 專案的 `Info.plist` 檔案中修改 `channel` 的值，或者在呼叫 `BlankGetChannel.GetChannelName()` 時使用您自訂的鍵名（確保該鍵名存在於 `Info.plist` 中）。

**Info.plist 設定範例：**

```xml
<key>channel</key>
<string>ios_cn_taptap</string>

<key>sub_channel</key>
<string>beta</string>
```

### 3. Android 平台設定

對於 Android 平台，您需要在 `AndroidManifest.xml` 檔案中定義渠道資訊。通常，這是透過在 `<application>` 標籤內添加 `<meta-data>` 標籤來完成的。

例如，如果您想使用鍵名 `channel` 和值為 `android_cn_taptap`：

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

然後，您可以在 C# 程式碼中透過 `BlankGetChannel.GetChannelName("channel")` 來獲取這個值。

### 4. Editor / PC / WebGL / UWP / 主機平台設定

對於 Editor、PC（Windows/Mac/Linux）、WebGL、UWP、PS4、PS5、Xbox One、Nintendo Switch 等平台，您需要在 Unity 專案的 `Resources` 資料夾下建立一個名為 `app_info.txt` 的文字檔案。

**app_info.txt 檔案格式範例：**

```
channel=editor_cn_test
sub_channel=beta
other_key=other_value
```

每行格式為：`鍵名=值`

插件會自動讀取該檔案中的鍵值對，並快取起來供後續使用。

## 注意事項

- 確保您在呼叫 `BlankGetChannel.GetChannelName(string key)` 時使用的 `key` 與您在對應平台的設定檔案中設定的鍵名一致：
    - **iOS / tvOS / visionOS**：`Info.plist` 檔案
    - **Android**：`AndroidManifest.xml` 檔案中的 `<meta-data>` 標籤
    - **Editor / PC / WebGL / UWP / 主機平台**：`Resources/app_info.txt` 檔案
- 插件包含 `link.xml` 檔案以防止程式碼被 Unity 的程式碼裁剪功能移除。
- `GetChannelName()` 方法會快取渠道資訊，避免重複讀取設定檔案，提高效能。
