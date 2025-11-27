using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 挑战管理器 - 管理好友挑战功能
/// </summary>
public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance { get; private set; }
    
    [Header("挑战设置")]
    [SerializeField] private int maxActiveChallenges = 10;
    
    private List<ChallengeData> activeChallenges = new List<ChallengeData>();
    private List<ChallengeData> receivedChallenges = new List<ChallengeData>();
    
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
    
    /// <summary>
    /// 发送挑战
    /// </summary>
    public void SendChallenge(string friendId, int levelId, System.Action<bool> callback = null)
    {
        // 检查挑战数量限制
        if (activeChallenges.Count >= maxActiveChallenges)
        {
            Debug.LogWarning($"已达到最大活跃挑战数限制: {maxActiveChallenges}");
            callback?.Invoke(false);
            return;
        }
        
        ChallengeData challenge = new ChallengeData
        {
            challengeId = System.Guid.NewGuid().ToString(),
            senderId = NetworkManager.Instance?.GetUserId() ?? "unknown",
            receiverId = friendId,
            levelId = levelId,
            sendTime = System.DateTime.UtcNow,
            status = ChallengeStatus.Pending
        };
        
        activeChallenges.Add(challenge);
        
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.SendChallenge(friendId, levelId, (success) =>
            {
                if (success)
                {
                    Debug.Log($"挑战已发送给好友: {friendId}");
                }
                callback?.Invoke(success);
            });
        }
        else
        {
            callback?.Invoke(false);
        }
    }
    
    /// <summary>
    /// 接收挑战
    /// </summary>
    public void ReceiveChallenge(ChallengeData challenge)
    {
        receivedChallenges.Add(challenge);
        
        // 显示挑战通知
        ShowChallengeNotification(challenge);
    }
    
    /// <summary>
    /// 接受挑战
    /// </summary>
    public void AcceptChallenge(string challengeId)
    {
        ChallengeData challenge = receivedChallenges.Find(c => c.challengeId == challengeId);
        if (challenge != null)
        {
            challenge.status = ChallengeStatus.Accepted;
            
            // 加载挑战关卡
            LoadChallengeLevel(challenge);
        }
    }
    
    /// <summary>
    /// 拒绝挑战
    /// </summary>
    public void RejectChallenge(string challengeId)
    {
        ChallengeData challenge = receivedChallenges.Find(c => c.challengeId == challengeId);
        if (challenge != null)
        {
            challenge.status = ChallengeStatus.Rejected;
            receivedChallenges.Remove(challenge);
        }
    }
    
    /// <summary>
    /// 提交挑战结果
    /// </summary>
    public void SubmitChallengeResult(string challengeId, float completionTime)
    {
        ChallengeData challenge = activeChallenges.Find(c => c.challengeId == challengeId);
        if (challenge != null)
        {
            challenge.completionTime = completionTime;
            challenge.status = ChallengeStatus.Completed;
            
            // 上传结果到服务器
            // TODO: 实现结果上传
        }
    }
    
    /// <summary>
    /// 显示挑战通知
    /// </summary>
    private void ShowChallengeNotification(ChallengeData challenge)
    {
        // TODO: 实现UI通知
        Debug.Log($"收到来自 {challenge.senderId} 的挑战，关卡: {challenge.levelId}");
    }
    
    /// <summary>
    /// 加载挑战关卡
    /// </summary>
    private void LoadChallengeLevel(ChallengeData challenge)
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SetCurrentLevelId(challenge.levelId);
            // TODO: 加载关卡场景
        }
    }
    
    /// <summary>
    /// 获取活跃挑战列表
    /// </summary>
    public List<ChallengeData> GetActiveChallenges()
    {
        return new List<ChallengeData>(activeChallenges);
    }
    
    /// <summary>
    /// 获取收到的挑战列表
    /// </summary>
    public List<ChallengeData> GetReceivedChallenges()
    {
        return new List<ChallengeData>(receivedChallenges);
    }
}

/// <summary>
/// 挑战数据
/// </summary>
[System.Serializable]
public class ChallengeData
{
    public string challengeId;
    public string senderId;
    public string receiverId;
    public int levelId;
    public System.DateTime sendTime;
    public ChallengeStatus status;
    public float completionTime;
    public float bestTime; // 对方的最佳时间
}

/// <summary>
/// 挑战状态
/// </summary>
public enum ChallengeStatus
{
    Pending,    // 待接受
    Accepted,   // 已接受
    Completed,  // 已完成
    Rejected    // 已拒绝
}

