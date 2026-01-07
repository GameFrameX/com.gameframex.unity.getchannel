#if UNITY_IOS
using System.Runtime.InteropServices;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 渠道信息获取工具类
/// </summary>
/// <remarks>
/// 用于在Unity应用中获取当前运行平台的渠道信息。
///
/// <para><b>Android 平台配置：</b></para>
/// 需要在主启动的 Activity 中添加以下 meta-data 标签：
/// <code><![CDATA[
/// <meta-data android:name="channel" android:value="android_cn_taptap" />
/// ]]></code>
///
/// <para><b>iOS 平台配置：</b></para>
/// 需要在 Info.plist 中添加以下键值对（String 类型）：
/// <code><![CDATA[
/// <key>channel</key>
/// <string>ios_cn_xxx</string>
/// ]]></code>
///
/// <para><b>Editor/PC 平台配置：</b></para>
/// 在 StreamingAssets 文件夹下创建 channel.txt 文件，格式如下：
/// <code><![CDATA[
/// channel=editor_cn_test
/// ]]></code>
/// </remarks>
public sealed class BlankGetChannel
{
#if UNITY_IOS
	/// <summary>
	/// iOS 平台原生方法：从 Info.plist 中获取渠道名称
	/// </summary>
	/// <param name="channelKey">渠道键名，默认为 "channel"</param>
	/// <returns>渠道名称</returns>
	[DllImport("__Internal")]
	private static extern string getChannelName(string channelKey);

#endif
    /// <summary>
    /// 渠道信息缓存字典，用于避免重复读取配置
    /// </summary>
    private static readonly Dictionary<string, string> ChannelCache = new Dictionary<string, string>();

    /// <summary>
    /// 获取指定渠道键对应的渠道名称
    /// </summary>
    /// <remarks>
    /// 该方法会根据当前运行平台从相应的配置源读取渠道信息，并使用缓存避免重复读取。
    /// <list type="table">
    /// <listheader>
    /// <term>平台</term>
    /// <description>配置源</description>
    /// </listheader>
    /// <item>
    /// <term>Android</term>
    /// <description>AndroidManifest.xml 中的 meta-data</description>
    /// </item>
    /// <item>
    /// <term>iOS</term>
    /// <description>Info.plist 中的键值对</description>
    /// </item>
    /// <item>
    /// <term>Editor/PC</term>
    /// <description>StreamingAssets/channel.txt 文件</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="channelKey">渠道键名，默认为 "channel"</param>
    /// <param name="defaultValue">当未找到渠道配置时返回的默认值，默认为 "default"</param>
    /// <returns>渠道名称，如果未配置则返回 <paramref name="defaultValue"/></returns>
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
    public static string GetChannelName(string channelKey = "channel", string defaultValue = "default")
    {
        if (ChannelCache.TryGetValue(channelKey, out var value))
        {
            return value;
        }

        string channelName = defaultValue;
#if UNITY_STANDALONE || UNITY_EDITOR
        string path = Application.streamingAssetsPath + "/channel.txt";
        if (File.Exists(path))
        {
            var channelReadAllLines = File.ReadAllLines(Application.streamingAssetsPath + "/channel.txt");
            if (channelReadAllLines.Length > 0)
            {
                foreach (var line in channelReadAllLines)
                {
                    var split = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length > 1 && split[0] == channelKey)
                    {
                        channelName = split[1].Trim();
                        break;
                    }
                }
            }
        }

#elif UNITY_ANDROID
        using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.alianhome.getchannel.MainActivity"))
        {
            channelName = androidJavaClass.CallStatic<string>("GetChannel", channelKey);
        }
#elif UNITY_IOS
        channelName = getChannelName(channelKey);
#endif
        ChannelCache[channelKey] = channelName;
        return channelName;
    }
}