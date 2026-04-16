#if UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
using System.Runtime.InteropServices;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 渠道信息获取工具类。
/// </summary>
/// <remarks>
/// Utility class for retrieving channel information in Unity applications.
///
/// <para><b>Android Platform Configuration:</b></para>
/// Add the following meta-data tag to the main launch Activity:
/// <code><![CDATA[
/// <meta-data android:name="channel" android:value="android_cn_taptap" />
/// ]]></code>
///
/// <para><b>iOS/tvOS/visionOS Platform Configuration:</b></para>
/// Add the following key-value pair (String type) to Info.plist:
/// <code><![CDATA[
/// <key>channel</key>
/// <string>ios_cn_xxx</string>
/// ]]></code>
///
/// <para><b>Editor/PC/WebGL/UWP/Console Platform Configuration:</b></para>
/// Create an application_config.txt file in the Resources folder with the following format:
/// <code><![CDATA[
/// channel=editor_cn_test
/// sub_channel=test
/// ]]></code>
/// </remarks>
[UnityEngine.Scripting.Preserve]
public sealed class BlankGetChannel
{
#if UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
	/// <summary>
	/// iOS/tvOS/visionOS 平台原生方法：从 Info.plist 中获取渠道名称。
	/// </summary>
	/// <remarks>
	/// iOS/tvOS/visionOS platform native method: retrieves channel name from Info.plist.
	/// </remarks>
	/// <param name="channelKey">渠道键名，默认为 "channel" / Channel key name, defaults to "channel"</param>
	/// <returns>渠道名称 / Channel name</returns>
	[DllImport("__Internal")]
	private static extern string getChannelName(string channelKey);

#endif
    /// <summary>
    /// 渠道信息缓存字典，用于避免重复读取配置。
    /// </summary>
    /// <remarks>
    /// Channel information cache dictionary, used to avoid repeated configuration reads.
    /// </remarks>
    private static readonly Dictionary<string, string> ChannelCache = new Dictionary<string, string>(32);

    /// <summary>
    /// 获取指定渠道键对应的渠道名称。
    /// </summary>
    /// <remarks>
    /// Retrieves the channel name for the specified channel key based on the current
    /// platform's configuration source, using cache to avoid repeated reads.
    /// <list type="table">
    /// <listheader>
    /// <term>Platform</term>
    /// <description>Configuration Source</description>
    /// </listheader>
    /// <item>
    /// <term>Android</term>
    /// <description>meta-data in AndroidManifest.xml</description>
    /// </item>
    /// <item>
    /// <term>iOS/tvOS/visionOS</term>
    /// <description>Key-value pairs in Info.plist</description>
    /// </item>
    /// <item>
    /// <term>Editor/PC/WebGL/UWP/Console</term>
    /// <description>Resources/application_config.txt file</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="channelKey">渠道键名，默认为 "channel" / Channel key name, defaults to "channel"</param>
    /// <param name="defaultValue">当未找到渠道配置时返回的默认值，默认为 "default" / Default value returned when channel configuration is not found, defaults to "default"</param>
    /// <returns>渠道名称，如果未配置则返回 <paramref name="defaultValue"/> / Channel name, or <paramref name="defaultValue"/> if not configured</returns>
    /// <example>
    /// 以下示例展示如何获取渠道名称：
    /// <code><![CDATA[
    /// // 使用默认参数获取主渠道
    /// string channel = BlankGetChannel.GetChannelName();
    ///
    /// // 获取指定键的渠道
    /// string subChannel = BlankGetChannel.GetChannelName("sub_channel", "unknown");
    /// ]]></code>
    /// </example>
    [UnityEngine.Scripting.Preserve]
    public static string GetChannelName(string channelKey = "channel", string defaultValue = "default")
    {
        if (ChannelCache.TryGetValue(channelKey, out var value))
        {
            return value;
        }

        string channelName = defaultValue;
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_PS5 || UNITY_XBOXONE || UNITY_SWITCH
        var textAsset = Resources.Load<TextAsset>("application_config");
        if (textAsset != null)
        {
            var lines = textAsset.text.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var split = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length > 1)
                {
                    var key = split[0].Trim();
                    var channelValue = split[1].Trim();
                    ChannelCache[key] = channelValue;
                }
            }
        }
        else
        {
            ChannelCache[channelKey] = defaultValue;
        }

        if (ChannelCache.TryGetValue(channelKey, out value))
        {
            return value;
        }
#elif UNITY_ANDROID
        using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.alianhome.getchannel.MainActivity"))
        {
            channelName = androidJavaClass.CallStatic<string>("GetChannel", channelKey);
        }
#elif UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
        channelName = getChannelName(channelKey);
#endif
        ChannelCache[channelKey] = channelName;
        return channelName;
    }
}