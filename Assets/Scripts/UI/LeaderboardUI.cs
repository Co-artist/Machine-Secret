using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 排行榜UI - 显示排行榜界面
/// </summary>
public class LeaderboardUI : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Text levelNameText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button refreshButton;
    
    [Header("样式设置")]
    [SerializeField] private Color friendColor = Color.yellow;
    [SerializeField] private Color myColor = Color.green;
    [SerializeField] private Color normalColor = Color.white;
    
    private int currentLevelId = 0;
    private List<GameObject> entryObjects = new List<GameObject>();
    
    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseLeaderboard);
        }
        
        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(RefreshLeaderboard);
        }
    }
    
    /// <summary>
    /// 显示排行榜
    /// </summary>
    public void ShowLeaderboard(int levelId)
    {
        currentLevelId = levelId;
        
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(true);
        }
        
        if (levelNameText != null)
        {
            levelNameText.text = $"关卡 {levelId} 排行榜";
        }
        
        LoadLeaderboard();
    }
    
    /// <summary>
    /// 关闭排行榜
    /// </summary>
    public void CloseLeaderboard()
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// 刷新排行榜
    /// </summary>
    public void RefreshLeaderboard()
    {
        LoadLeaderboard();
    }
    
    /// <summary>
    /// 加载排行榜
    /// </summary>
    private void LoadLeaderboard()
    {
        if (NetworkManager.Instance == null) return;
        
        NetworkManager.Instance.GetLeaderboard(currentLevelId, OnLeaderboardLoaded);
    }
    
    /// <summary>
    /// 排行榜加载完成回调
    /// </summary>
    private void OnLeaderboardLoaded(LeaderboardData data)
    {
        // 清除旧条目
        foreach (var obj in entryObjects)
        {
            Destroy(obj);
        }
        entryObjects.Clear();
        
        if (data == null || data.entries == null) return;
        
        // 创建新条目
        foreach (var entry in data.entries)
        {
            CreateLeaderboardEntry(entry);
        }
    }
    
    /// <summary>
    /// 创建排行榜条目
    /// </summary>
    private void CreateLeaderboardEntry(LeaderboardEntry entry)
    {
        if (entryPrefab == null || entryContainer == null) return;
        
        GameObject entryObj = Instantiate(entryPrefab, entryContainer);
        entryObjects.Add(entryObj);
        
        // 设置文本
        Text[] texts = entryObj.GetComponentsInChildren<Text>();
        if (texts.Length >= 3)
        {
            texts[0].text = entry.rank.ToString(); // 排名
            texts[1].text = entry.userName; // 用户名
            texts[2].text = FormatTime(entry.time); // 时间
        }
        
        // 设置颜色
        Image bgImage = entryObj.GetComponent<Image>();
        if (bgImage != null)
        {
            if (entry.isFriend)
            {
                bgImage.color = friendColor;
            }
            else if (entry.userId == NetworkManager.Instance?.GetUserId())
            {
                bgImage.color = myColor;
            }
            else
            {
                bgImage.color = normalColor;
            }
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
}

