#if UNITY_IOS
using System.Runtime.InteropServices;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 获取渠道名称
/// Android:
///         需要在主启动的Activity 中添加
/// <meta-data android:name="channel" android:value="android_cn_taptap" />
///
/// iOS :
///         需要在Info.plist中添加
///         Key     ==> value    String 类型
///         channel ==> ios_cn_xxx    
/// 
/// </summary>
public sealed class BlankGetChannel
{
#if UNITY_IOS
	[DllImport("__Internal")]
	private static extern string getChannelName(string channelKey);

#endif
    private static readonly Dictionary<string, string> ChannelCache = new Dictionary<string, string>();

    /// <summary>
    /// 获取渠道值
    /// </summary>
    public static string GetChannelName(string channelKey = "channel")
    {
        if (ChannelCache.TryGetValue(channelKey, out var value))
        {
            return value;
        }

        string channelName = "default";
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