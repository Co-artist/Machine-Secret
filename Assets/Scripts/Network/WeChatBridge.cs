using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// 微信桥接 - Unity C# 端接口
/// 用于调用 JavaScript 桥接代码
/// </summary>
public class WeChatBridge : MonoBehaviour
{
    #if UNITY_WEBGL && !UNITY_EDITOR
    // JavaScript 函数声明
    [DllImport("__Internal")]
    private static extern void WeChatLogin();
    
    [DllImport("__Internal")]
    private static extern void GetUserInfo();
    
    [DllImport("__Internal")]
    private static extern void ShowShareMenu();
    
    [DllImport("__Internal")]
    private static extern void SetShareAppMessage(string title, string path, string imageUrl);
    
    [DllImport("__Internal")]
    private static extern void ShareAppMessage(string title, string path, string imageUrl);
    
    [DllImport("__Internal")]
    private static extern void GetFriendCloudStorage(string keyList);
    
    [DllImport("__Internal")]
    private static extern void SetUserCloudStorage(string keyValueList);
    
    [DllImport("__Internal")]
    private static extern void ShowToast(string title, string icon, int duration);
    
    [DllImport("__Internal")]
    private static extern void ShowLoading(string title);
    
    [DllImport("__Internal")]
    private static extern void HideLoading();
    
    [DllImport("__Internal")]
    private static extern void ShowModal(string title, string content);
    #endif
    
    /// <summary>
    /// 微信登录
    /// </summary>
    public static void Login()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        WeChatLogin();
        #else
        Debug.Log("[模拟] 微信登录");
        #endif
    }
    
    /// <summary>
    /// 获取用户信息
    /// </summary>
    public static void GetUserInfo()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        GetUserInfo();
        #else
        Debug.Log("[模拟] 获取用户信息");
        #endif
    }
    
    /// <summary>
    /// 显示分享菜单
    /// </summary>
    public static void ShowShareMenu()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        ShowShareMenu();
        #else
        Debug.Log("[模拟] 显示分享菜单");
        #endif
    }
    
    /// <summary>
    /// 设置分享内容
    /// </summary>
    public static void SetShareAppMessage(string title, string path, string imageUrl = "")
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        SetShareAppMessage(title, path, imageUrl);
        #else
        Debug.Log($"[模拟] 设置分享: {title}, {path}");
        #endif
    }
    
    /// <summary>
    /// 分享到好友
    /// </summary>
    public static void ShareAppMessage(string title, string path, string imageUrl = "")
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        ShareAppMessage(title, path, imageUrl);
        #else
        Debug.Log($"[模拟] 分享: {title}, {path}");
        #endif
    }
    
    /// <summary>
    /// 获取好友排行榜
    /// </summary>
    public static void GetFriendCloudStorage(string[] keyList)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        string keys = string.Join(",", keyList);
        GetFriendCloudStorage(keys);
        #else
        Debug.Log($"[模拟] 获取好友排行榜: {string.Join(",", keyList)}");
        #endif
    }
    
    /// <summary>
    /// 设置用户排行榜数据
    /// </summary>
    public static void SetUserCloudStorage(string key, string value)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        string json = $"[{{\"key\":\"{key}\",\"value\":\"{value}\"}}]";
        SetUserCloudStorage(json);
        #else
        Debug.Log($"[模拟] 设置用户数据: {key} = {value}");
        #endif
    }
    
    /// <summary>
    /// 显示提示
    /// </summary>
    public static void ShowToast(string title, string icon = "none", int duration = 2000)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        ShowToast(title, icon, duration);
        #else
        Debug.Log($"[提示] {title}");
        #endif
    }
    
    /// <summary>
    /// 显示加载提示
    /// </summary>
    public static void ShowLoading(string title = "加载中...")
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        ShowLoading(title);
        #else
        Debug.Log($"[加载] {title}");
        #endif
    }
    
    /// <summary>
    /// 隐藏加载提示
    /// </summary>
    public static void HideLoading()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        HideLoading();
        #else
        Debug.Log("[加载] 完成");
        #endif
    }
    
    /// <summary>
    /// 显示模态对话框
    /// </summary>
    public static void ShowModal(string title, string content)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        ShowModal(title, content);
        #else
        Debug.Log($"[对话框] {title}: {content}");
        #endif
    }
    
    // Unity 回调函数（由 JavaScript 调用）
    
    /// <summary>
    /// 微信登录成功回调
    /// </summary>
    public void OnWeChatLogin(string code)
    {
        NetworkManager.Instance?.OnWeChatLoginSuccess(code);
    }
    
    /// <summary>
    /// 微信登录失败回调
    /// </summary>
    public void OnWeChatLoginFailed(string error)
    {
        NetworkManager.Instance?.OnWeChatLoginFailed(error);
    }
    
    /// <summary>
    /// 获取用户信息成功回调
    /// </summary>
    public void OnGetUserInfo(string userInfoJson)
    {
        NetworkManager.Instance?.OnGetUserInfoSuccess(userInfoJson);
    }
    
    /// <summary>
    /// 分享成功回调
    /// </summary>
    public void OnShareSuccess(string result)
    {
        NetworkManager.Instance?.OnShareSuccess();
    }
    
    /// <summary>
    /// 分享失败回调
    /// </summary>
    public void OnShareFailed(string error)
    {
        NetworkManager.Instance?.OnShareFailed(error);
    }
    
    /// <summary>
    /// 获取好友排行榜成功回调
    /// </summary>
    public void OnGetFriendCloudStorage(string dataJson)
    {
        NetworkManager.Instance?.OnGetFriendCloudStorageSuccess(dataJson);
    }
    
    /// <summary>
    /// 获取好友排行榜失败回调
    /// </summary>
    public void OnGetFriendCloudStorageFailed(string error)
    {
        NetworkManager.Instance?.OnGetFriendCloudStorageFailed(error);
    }
    
    /// <summary>
    /// 模态对话框结果回调
    /// </summary>
    public void OnModalResult(string result)
    {
        // 处理对话框结果
        Debug.Log($"对话框结果: {result}");
    }
}

