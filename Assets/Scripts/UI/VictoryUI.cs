using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 胜利UI - 显示胜利界面
/// </summary>
public class VictoryUI : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Text timeText;
    [SerializeField] private Text bestTimeText;
    [SerializeField] private Text rankText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button shareButton;
    [SerializeField] private Button leaderboardButton;
    
    private float completionTime;
    private int currentLevelId;
    
    void Start()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(LoadNextLevel);
        }
        
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RetryLevel);
        }
        
        if (shareButton != null)
        {
            shareButton.onClick.AddListener(ShareLevel);
        }
        
        if (leaderboardButton != null)
        {
            leaderboardButton.onClick.AddListener(ShowLeaderboard);
        }
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// 显示胜利界面
    /// </summary>
    public void ShowVictory(float time, int levelId)
    {
        completionTime = time;
        currentLevelId = levelId;
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        UpdateUI();
        LoadRank();
    }
    
    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
        if (timeText != null)
        {
            timeText.text = $"完成时间: {FormatTime(completionTime)}";
        }
        
        // 加载最佳时间
        float bestTime = PlayerPrefs.GetFloat($"Level_{currentLevelId}_BestTime", float.MaxValue);
        if (bestTimeText != null)
        {
            if (bestTime < float.MaxValue)
            {
                bestTimeText.text = $"最佳时间: {FormatTime(bestTime)}";
                
                if (completionTime < bestTime)
                {
                    bestTimeText.text += " (新记录！)";
                }
            }
            else
            {
                bestTimeText.text = "最佳时间: --:--";
            }
        }
    }
    
    /// <summary>
    /// 加载排名
    /// </summary>
    private void LoadRank()
    {
        if (NetworkManager.Instance != null && rankText != null)
        {
            NetworkManager.Instance.GetLeaderboard(currentLevelId, (data) =>
            {
                if (data != null && data.entries != null)
                {
                    string userId = NetworkManager.Instance.GetUserId();
                    int rank = data.entries.FindIndex(e => e.userId == userId) + 1;
                    
                    if (rank > 0)
                    {
                        rankText.text = $"排名: 第 {rank} 名";
                    }
                    else
                    {
                        rankText.text = "排名: 未上榜";
                    }
                }
            });
        }
    }
    
    /// <summary>
    /// 格式化时间
    /// </summary>
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time % 1f) * 100f);
        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
    
    /// <summary>
    /// 加载下一关
    /// </summary>
    private void LoadNextLevel()
    {
        int nextLevelId = currentLevelId + 1;
        LevelManager.Instance?.SetCurrentLevelId(nextLevelId);
        // TODO: 加载下一关场景
    }
    
    /// <summary>
    /// 重试关卡
    /// </summary>
    private void RetryLevel()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
        
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager?.ResetGame();
    }
    
    /// <summary>
    /// 分享关卡
    /// </summary>
    private void ShareLevel()
    {
        if (LevelManager.Instance != null && NetworkManager.Instance != null)
        {
            LevelData levelData = LevelManager.Instance.LoadLevel(currentLevelId);
            if (levelData != null)
            {
                string levelCode = LevelManager.Instance.GetLevelCode(levelData);
                NetworkManager.Instance.ShareLevel(levelCode);
            }
        }
    }
    
    /// <summary>
    /// 显示排行榜
    /// </summary>
    private void ShowLeaderboard()
    {
        LeaderboardUI leaderboardUI = FindObjectOfType<LeaderboardUI>();
        if (leaderboardUI != null)
        {
            leaderboardUI.ShowLeaderboard(currentLevelId);
        }
    }
}

