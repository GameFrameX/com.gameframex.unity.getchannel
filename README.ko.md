<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# Unity 멀티 플랫폼 채널 가져오기

[![Version](https://img.shields.io/badge/version-1.3.0-blue.svg)](https://github.com/gameframex/com.gameframex.unity.getchannel)
[![Unity](https://img.shields.io/badge/Unity-2017.1+-green.svg)](https://unity3d.com/get-unity/download)
[![License](https://img.shields.io/badge/license-MIT+Apache%202.0-orange.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex.doc.alianblank.com-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현**

[📖 문서](https://gameframex.doc.alianblank.com) • [🚀 빠른 시작](#설치) • [💬 QQ 그룹: 467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)

---

🌐 **언어**: [English](./README.md) | [简体中文](./README.zh-CN.md) | [繁體中文](./README.zh-TW.md) | [日本語](./README.ja.md) | [**한국어**](./README.ko.md)

---

</div>

이 플러그인은 Unity 프로젝트에서 멀티 플랫폼의 배포 채널 식별자를 가져오는 데 사용됩니다（iOS, tvOS, visionOS, Android, Editor, PC, WebGL, UWP 및 콘솔 플랫폼 지원）. `https://github.com/GameFrameX/GameFrameX` 프로젝트의 서브모듈입니다.

## 기능

- **멀티 플랫폼 지원**: iOS, tvOS, visionOS, Android, Editor, PC（Windows/Mac/Linux）, WebGL, UWP, PS4, PS5, Xbox One, Nintendo Switch.
- 사전 설정된 채널 정보를 가져오는 간단한 API 제공.
- iOS 플랫폼에서 빌드 시 `Info.plist`에 기본 채널을 자동 추가（미설정 시）.

## 설치

다음 세 가지 방법 중 하나로 이 플러그인을 Unity 프로젝트에 추가할 수 있습니다：

1. **`manifest.json`을 통한 추가：**
    프로젝트 `Packages` 디렉토리의 `manifest.json` 파일에 있는 `dependencies` 노드에 다음을 추가하세요：
    ```json
    {
      "dependencies": {
        "com.gameframex.unity.getchannel": "https://github.com/gameframex/com.gameframex.unity.getchannel.git",
        // ... 기타 종속성
      }
    }
    ```

2. **Unity Package Manager에서 Git URL 사용：**
    Unity 에디터에서 `Window -> Package Manager`를 엽니다.
    좌측 상단의 `+` 버튼을 클릭하고 `Add package from git URL...`을 선택합니다.
    다음 URL을 입력하고 `Add`를 클릭하세요：
    ```
    https://github.com/gameframex/com.gameframex.unity.getchannel.git
    ```

3. **저장소 다운로드 또는 복제：**
    이 저장소를 Unity 프로젝트의 `Packages` 디렉토리에 다운로드하거나 복제하세요. Unity가 자동으로 인식하여 플러그인을 로드합니다.

## 사용 방법

### 1. 채널 정보 가져오기

C# 스크립트에서 `BlankGetChannel.GetChannelName(string key)` 메서드를 사용하여 채널 정보를 가져옵니다. `key` 매개변수는 해당 플랫폼에서 채널 정보를 설정할 때 사용한 키 이름입니다.

**예제 코드：**

```csharp
using UnityEngine;

public class MyGameScript : MonoBehaviour
{
    void Start()
    {
        // 기본 채널 가져오기（키 이름은 "channel"）
        string channel = BlankGetChannel.GetChannelName();
        Debug.Log("현재 채널: " + channel);

        // 특정 키로 채널 가져오기
        string customChannel = BlankGetChannel.GetChannelName("channelName");
        Debug.Log("커스텀 채널: " + customChannel);

        // 기본값을 지정하여 채널 가져오기
        string subChannel = BlankGetChannel.GetChannelName("sub_channel", "unknown");
        Debug.Log("서브 채널: " + subChannel);
    }
}
```

### 2. iOS / tvOS / visionOS 플랫폼 설정

iOS, tvOS, visionOS 플랫폼의 경우 플러그인에 빌드 후 처리기 (`PostProcessBuildHandler.cs`)가 포함되어 있습니다. 빌드 시 프로젝트의 `Info.plist` 파일에：
- `channel`이라는 이름의 키가 **없는** 경우, 스크립트가 자동으로 키 `channel`, 값 `default` 항목을 추가합니다.
- `channel`이라는 이름의 키가 **이미 존재하는** 경우, 수정이 이루어지지 않습니다.

Xcode 프로젝트의 `Info.plist` 파일에서 `channel` 값을 수정하거나, `BlankGetChannel.GetChannelName()`을 호출할 때 커스텀 키 이름을 사용하세요（해당 키 이름이 `Info.plist`에 존재하는지 확인하세요）.

**Info.plist 설정 예시：**

```xml
<key>channel</key>
<string>ios_cn_taptap</string>

<key>sub_channel</key>
<string>beta</string>
```

### 3. Android 플랫폼 설정

Android 플랫폼에서는 `AndroidManifest.xml` 파일에 채널 정보를 정의해야 합니다. 일반적으로 `<application>` 태그 내에 `<meta-data>` 태그를 추가하여 수행합니다.

예를 들어, 키 이름 `channel`과 값 `android_cn_taptap`을 사용하려는 경우：

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

    <!-- 기타 meta-data -->
</application>
```

그런 다음 C# 코드에서 `BlankGetChannel.GetChannelName("channel")`을 사용하여 이 값을 가져올 수 있습니다.

### 4. Editor / PC / WebGL / UWP / 콘솔 플랫폼 설정

Editor, PC（Windows/Mac/Linux）, WebGL, UWP, PS4, PS5, Xbox One, Nintendo Switch 등의 플랫폼에서는 Unity 프로젝트의 `Resources` 폴더에 `app_info.txt`라는 이름의 텍스트 파일을 만들어야 합니다.

**app_info.txt 파일 형식 예시：**

```
channel=editor_cn_test
sub_channel=beta
other_key=other_value
```

각 행의 형식：`키이름=값`

플러그인은 이 파일에서 키-값 쌍을 자동으로 읽고 후속 사용을 위해 캐시합니다.

## 주의 사항

- `BlankGetChannel.GetChannelName(string key)`를 호출할 때 사용하는 `key`가 해당 플랫폼의 설정 파일에 설정한 키 이름과 일치하는지 확인하세요：
    - **iOS / tvOS / visionOS**：`Info.plist` 파일
    - **Android**：`AndroidManifest.xml` 파일의 `<meta-data>` 태그
    - **Editor / PC / WebGL / UWP / 콘솔 플랫폼**：`Resources/app_info.txt` 파일
- 플러그인에는 Unity의 코드 스트리핑 기능으로 인한 코드 제거를 방지하기 위해 `link.xml` 파일이 포함되어 있습니다.
- `GetChannelName()` 메서드는 채널 정보를 캐시하여 설정 파일의 반복 읽기를 방지하고 성능을 향상시킵니다.
