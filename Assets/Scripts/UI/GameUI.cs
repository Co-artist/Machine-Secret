using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏UI - 显示游戏界面信息
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private Text timeText;
    [SerializeField] private Text bestTimeText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button menuButton;
    
    [Header("提示信息")]
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private Text hintText;
    
    private GameManager gameManager;
    private float bestTime = float.MaxValue;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        if (gameManager != null)
        {
            gameManager.OnTimeUpdate += UpdateTime;
            gameManager.OnGameComplete += OnGameComplete;
        }
        
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(() => gameManager?.PauseGame());
        }
        
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(() => gameManager?.ResetGame());
        }
        
        LoadBestTime();
    }
    
    void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnTimeUpdate -= UpdateTime;
            gameManager.OnGameComplete -= OnGameComplete;
        }
    }
    
    /// <summary>
    /// 更新时间显示
    /// </summary>
    private void UpdateTime(float time)
    {
        if (timeText != null)
        {
            timeText.text = FormatTime(time);
        }
    }
    
    /// <summary>
    /// 游戏完成
    /// </summary>
    private void OnGameComplete(float time)
    {
        if (time < bestTime)
        {
            bestTime = time;
            SaveBestTime();
        }
        
        if (bestTimeText != null)
        {
            bestTimeText.text = $"最佳: {FormatTime(bestTime)}";
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
    /// 加载最佳时间
    /// </summary>
    private void LoadBestTime()
    {
        int levelId = LevelManager.Instance?.GetCurrentLevelId() ?? 0;
        bestTime = PlayerPrefs.GetFloat($"Level_{levelId}_BestTime", float.MaxValue);
        
        if (bestTimeText != null)
        {
            if (bestTime < float.MaxValue)
            {
                bestTimeText.text = $"最佳: {FormatTime(bestTime)}";
            }
            else
            {
                bestTimeText.text = "最佳: --:--";
            }
        }
    }
    
    /// <summary>
    /// 保存最佳时间
    /// </summary>
    private void SaveBestTime()
    {
        int levelId = LevelManager.Instance?.GetCurrentLevelId() ?? 0;
        PlayerPrefs.SetFloat($"Level_{levelId}_BestTime", bestTime);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 显示提示
    /// </summary>
    public void ShowHint(string message)
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(true);
        }
        
        if (hintText != null)
        {
            hintText.text = message;
        }
    }
    
    /// <summary>
    /// 隐藏提示
    /// </summary>
    public void HideHint()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }
    }
}

