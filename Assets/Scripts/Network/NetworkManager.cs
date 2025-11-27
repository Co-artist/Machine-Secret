using UnityEngine;
using System.Collections;

/// <summary>
/// 网络管理器 - 处理微信小程序API和网络请求
/// </summary>
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }
    
    [Header("服务器设置")]
    [SerializeField] private string serverURL = "https://api.example.com";
    
    private bool isLoggedIn = false;
    private string userId;
    private string sessionKey;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        InitializeWeChat();
    }
    
    /// <summary>
    /// 初始化微信
    /// </summary>
    private void InitializeWeChat()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        // 显示分享菜单
        WeChatBridge.ShowShareMenu();
        
        // 设置分享内容
        WeChatBridge.SetShareAppMessage("挑战我的机械谜题", "/pages/play/play", "");
        #endif
    }
    
    private System.Action<bool> loginCallback;
    
    /// <summary>
    /// 微信登录
    /// </summary>
    public void WeChatLogin(System.Action<bool> callback = null)
    {
        loginCallback = callback;
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        // 使用WeChatBridge调用微信登录
        WeChatBridge.Login();
        #else
        // 编辑器模式模拟登录
        SimulateLogin(callback);
        #endif
    }
    
    /// <summary>
    /// 微信登录成功回调（由WeChatBridge调用）
    /// </summary>
    public void OnWeChatLoginSuccess(string code)
    {
        LoginWithCode(code, loginCallback);
    }
    
    /// <summary>
    /// 微信登录失败回调（由WeChatBridge调用）
    /// </summary>
    public void OnWeChatLoginFailed(string error)
    {
        Debug.LogError($"微信登录失败: {error}");
        loginCallback?.Invoke(false);
    }
    
    /// <summary>
    /// 获取用户信息成功回调（由WeChatBridge调用）
    /// </summary>
    public void OnGetUserInfoSuccess(string userInfoJson)
    {
        Debug.Log($"用户信息: {userInfoJson}");
        // TODO: 解析用户信息
    }
    
    /// <summary>
    /// 分享成功回调（由WeChatBridge调用）
    /// </summary>
    public void OnShareSuccess()
    {
        Debug.Log("分享成功");
    }
    
    /// <summary>
    /// 分享失败回调（由WeChatBridge调用）
    /// </summary>
    public void OnShareFailed(string error)
    {
        Debug.LogError($"分享失败: {error}");
    }
    
    /// <summary>
    /// 获取好友排行榜成功回调（由WeChatBridge调用）
    /// </summary>
    public void OnGetFriendCloudStorageSuccess(string dataJson)
    {
        Debug.Log($"好友排行榜数据: {dataJson}");
        // TODO: 解析排行榜数据
    }
    
    /// <summary>
    /// 获取好友排行榜失败回调（由WeChatBridge调用）
    /// </summary>
    public void OnGetFriendCloudStorageFailed(string error)
    {
        Debug.LogError($"获取好友排行榜失败: {error}");
    }
    
    /// <summary>
    /// 使用code登录
    /// </summary>
    private void LoginWithCode(string code, System.Action<bool> callback)
    {
        StartCoroutine(LoginCoroutine(code, callback));
    }
    
    /// <summary>
    /// 登录协程
    /// </summary>
    private IEnumerator LoginCoroutine(string code, System.Action<bool> callback)
    {
        // TODO: 实现实际的登录请求
        yield return new WaitForSeconds(1f);
        
        isLoggedIn = true;
        userId = "test_user_" + System.Guid.NewGuid().ToString();
        sessionKey = "test_session";
        
        callback?.Invoke(true);
    }
    
    /// <summary>
    /// 模拟登录（编辑器模式）
    /// </summary>
    private void SimulateLogin(System.Action<bool> callback)
    {
        isLoggedIn = true;
        userId = "editor_user";
        sessionKey = "editor_session";
        callback?.Invoke(true);
    }
    
    /// <summary>
    /// 上传成绩
    /// </summary>
    public void UploadScore(int levelId, float time)
    {
        if (!isLoggedIn)
        {
            Debug.LogWarning("未登录，无法上传成绩");
            return;
        }
        
        StartCoroutine(UploadScoreCoroutine(levelId, time));
    }
    
    /// <summary>
    /// 上传成绩协程
    /// </summary>
    private IEnumerator UploadScoreCoroutine(int levelId, float time)
    {
        // 构建请求数据
        var scoreData = new
        {
            userId = userId,
            levelId = levelId,
            time = time,
            timestamp = System.DateTime.UtcNow.ToString("o")
        };
        
        string json = JsonUtility.ToJson(scoreData);
        
        // TODO: 实现实际的HTTP请求
        // using (UnityEngine.Networking.UnityWebRequest request = ...)
        
        yield return null;
        
        Debug.Log($"成绩已上传: 关卡 {levelId}, 时间 {time}秒");
    }
    
    /// <summary>
    /// 获取排行榜
    /// </summary>
    public void GetLeaderboard(int levelId, System.Action<LeaderboardData> callback)
    {
        StartCoroutine(GetLeaderboardCoroutine(levelId, callback));
    }
    
    /// <summary>
    /// 获取排行榜协程
    /// </summary>
    private IEnumerator GetLeaderboardCoroutine(int levelId, System.Action<LeaderboardData> callback)
    {
        // TODO: 实现实际的HTTP请求
        
        // 模拟数据
        LeaderboardData data = new LeaderboardData
        {
            levelId = levelId,
            entries = new System.Collections.Generic.List<LeaderboardEntry>()
        };
        
        // 添加模拟条目
        for (int i = 0; i < 10; i++)
        {
            data.entries.Add(new LeaderboardEntry
            {
                rank = i + 1,
                userId = $"user_{i}",
                userName = $"玩家{i + 1}",
                time = 10f + i * 0.5f,
                isFriend = i < 3
            });
        }
        
        yield return new WaitForSeconds(0.5f);
        
        callback?.Invoke(data);
    }
    
    /// <summary>
    /// 分享关卡
    /// </summary>
    public void ShareLevel(string levelCode, System.Action<bool> callback = null)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        // 使用WeChatBridge分享
        string title = "挑战我的机械谜题";
        string path = $"/pages/play/play?code={levelCode}";
        WeChatBridge.ShareAppMessage(title, path, "");
        callback?.Invoke(true);
        #else
        // 编辑器模式：复制到剪贴板
        GUIUtility.systemCopyBuffer = levelCode;
        Debug.Log($"关卡代码已复制到剪贴板: {levelCode}");
        callback?.Invoke(true);
        #endif
    }
    
    /// <summary>
    /// 发送挑战
    /// </summary>
    public void SendChallenge(string friendId, int levelId, System.Action<bool> callback = null)
    {
        StartCoroutine(SendChallengeCoroutine(friendId, levelId, callback));
    }
    
    /// <summary>
    /// 发送挑战协程
    /// </summary>
    private IEnumerator SendChallengeCoroutine(string friendId, int levelId, System.Action<bool> callback)
    {
        // TODO: 实现实际的HTTP请求
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log($"挑战已发送给好友: {friendId}, 关卡: {levelId}");
        callback?.Invoke(true);
    }
    
    /// <summary>
    /// 检查是否已登录
    /// </summary>
    public bool IsLoggedIn()
    {
        return isLoggedIn;
    }
    
    /// <summary>
    /// 获取用户ID
    /// </summary>
    public string GetUserId()
    {
        return userId;
    }
}

/// <summary>
/// 排行榜数据
/// </summary>
[System.Serializable]
public class LeaderboardData
{
    public int levelId;
    public System.Collections.Generic.List<LeaderboardEntry> entries;
}

/// <summary>
/// 排行榜条目
/// </summary>
[System.Serializable]
public class LeaderboardEntry
{
    public int rank;
    public string userId;
    public string userName;
    public float time;
    public bool isFriend;
}

