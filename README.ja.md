<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Unity マルチプラットフォームチャンネル取得

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.getchannel)](https://github.com/GameFrameX/com.gameframex.unity.getchannel/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.getchannel)](https://github.com/GameFrameX/com.gameframex.unity.getchannel/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#quick-start) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>
## 機能

- **マルチプラットフォーム対応**：iOS、tvOS、visionOS、Android、Editor、PC（Windows/Mac/Linux）、WebGL、UWP、PS4、PS5、Xbox One、Nintendo Switch。
- プリセットされたチャンネル情報を取得するためのシンプルな API を提供。
- iOS プラットフォームでは、ビルド時に `Info.plist` にデフォルトチャンネルを自動追加（未設定の場合）。

## インストール

以下の3つの方法で、このプラグインを Unity プロジェクトに追加できます：

1. **`manifest.json` 経由で追加：**
    プロジェクトの `Packages` ディレクトリにある `manifest.json` ファイルの `dependencies` ノードに以下を追加してください：
    ```json
    {
      "dependencies": {
        "com.gameframex.unity.getchannel": "https://github.com/gameframex/com.gameframex.unity.getchannel.git",
        // ... その他の依存関係
      }
    }
    ```

2. **Unity Package Manager で Git URL を使用：**
    Unity エディタで `Window -> Package Manager` を開きます。
    左上の `+` ボタンをクリックし、`Add package from git URL...` を選択します。
    以下の URL を入力して `Add` をクリックしてください：
    ```
    https://github.com/gameframex/com.gameframex.unity.getchannel.git
    ```

3. **リポジトリのダウンロードまたはクローン：**
    このリポジトリを Unity プロジェクトの `Packages` ディレクトリにダウンロードまたはクローンしてください。Unity が自動的に認識してプラグインを読み込みます。

## 使用方法

### 1. チャンネル情報の取得

C# スクリプトで、`BlankGetChannel.GetChannelName(string key)` メソッドを使用してチャンネル情報を取得します。`key` パラメータは、対応プラットフォームでチャンネル情報を設定した際のキー名です。

**サンプルコード：**

```csharp
using UnityEngine;

public class MyGameScript : MonoBehaviour
{
    void Start()
    {
        // デフォルトチャンネルを取得（キー名は "channel"）
        string channel = BlankGetChannel.GetChannelName();
        Debug.Log("現在のチャンネル: " + channel);

        // 特定のキーでチャンネルを取得
        string customChannel = BlankGetChannel.GetChannelName("channelName");
        Debug.Log("カスタムチャンネル: " + customChannel);

        // デフォルト値を指定してチャンネルを取得
        string subChannel = BlankGetChannel.GetChannelName("sub_channel", "unknown");
        Debug.Log("サブチャンネル: " + subChannel);
    }
}
```

### 2. iOS / tvOS / visionOS プラットフォーム設定

iOS、tvOS、visionOS プラットフォームでは、プラグインにビルド後処理 (`PostProcessBuildHandler.cs`) が含まれています。ビルド時、プロジェクトの `Info.plist` ファイルに：
- `channel` という名前のキーが **ない** 場合、スクリプトは自動的にキー `channel`、値 `default` のエントリを追加します。
- `channel` という名前のキーが **すでに存在する** 場合、変更は行われません。

Xcode プロジェクトの `Info.plist` ファイルで `channel` の値を変更するか、`BlankGetChannel.GetChannelName()` を呼び出す際にカスタムキー名を使用してください（そのキー名が `Info.plist` に存在することを確認してください）。

**Info.plist 設定例：**

```xml
<key>channel</key>
<string>ios_cn_taptap</string>

<key>sub_channel</key>
<string>beta</string>
```

### 3. Android プラットフォーム設定

Android プラットフォームでは、`AndroidManifest.xml` ファイルでチャンネル情報を定義する必要があります。通常、`<application>` タグ内に `<meta-data>` タグを追加して行います。

例えば、キー名 `channel`、値 `android_cn_taptap` を使用する場合：

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

    <!-- その他の meta-data -->
</application>
```

その後、C# コードで `BlankGetChannel.GetChannelName("channel")` を使用してこの値を取得できます。

### 4. Editor / PC / WebGL / UWP / コンソールプラットフォーム設定

Editor、PC（Windows/Mac/Linux）、WebGL、UWP、PS4、PS5、Xbox One、Nintendo Switch などのプラットフォームでは、Unity プロジェクトの `Resources` フォルダに `application_config.txt` という名前のテキストファイルを作成する必要があります。

**application_config.txt ファイル形式の例：**

```
channel=editor_cn_test
sub_channel=beta
other_key=other_value
```

各行の形式：`キー名=値`

プラグインはこのファイルからキーと値のペアを自動的に読み取り、後続の使用のためにキャッシュします。

## 注意事項

- `BlankGetChannel.GetChannelName(string key)` を呼び出す際、使用する `key` が対応プラットフォームの設定ファイルで設定したキー名と一致していることを確認してください：
    - **iOS / tvOS / visionOS**：`Info.plist` ファイル
    - **Android**：`AndroidManifest.xml` ファイルの `<meta-data>` タグ
    - **Editor / PC / WebGL / UWP / コンソールプラットフォーム**：`Resources/application_config.txt` ファイル
- プラグインには `link.xml` ファイルが含まれており、Unity のコードストリッピング機能によるコード削除を防ぎます。
- `GetChannelName()` メソッドはチャンネル情報をキャッシュし、設定ファイルの繰り返し読み込みを回避してパフォーマンスを向上させます。
