#if UNITY_IOS
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFrameX.GetChannel.Editor
{
    internal static class PostProcessBuildHandler
    {
        [PostProcessBuild(99)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }

            try
            {
                Run(path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static void Run(string path)
        {
            const string channelKey = "channel";
            const string channelValue = "default";

            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            if (!plist.root.values.TryGetValue(channelKey, out var value))
            {
                plist.root.SetString(channelKey, channelValue);
                plist.WriteToFile(plistPath);
                Debug.Log("配置渠道成功=>" + channelValue);
            }
            else
            {
                Debug.Log("已有渠道,跳过设置=>" + value);
            }
        }
    }
}
#endif