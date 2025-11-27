using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏管理器 - 管理游戏流程和状态
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("游戏设置")]
    [SerializeField] private BallController ballPrefab;
    [SerializeField] private Transform ballStartPosition;
    [SerializeField] private float gameTimeLimit = 300f; // 游戏时间限制（秒）
    
    [Header("UI引用")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject pauseUI;
    
    private BallController currentBall;
    private float gameStartTime;
    private float gameTime;
    private bool isGameActive = false;
    private bool isPaused = false;
    
    // 事件
    public System.Action<float> OnTimeUpdate;
    public System.Action OnGameStart;
    public System.Action OnGamePause;
    public System.Action OnGameResume;
    public System.Action<float> OnGameComplete; // 参数：完成时间
    
    void Start()
    {
        InitializeGame();
    }
    
    void Update()
    {
        if (isGameActive && !isPaused)
        {
            UpdateGameTime();
        }
        
        // 暂停控制
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    /// <summary>
    /// 初始化游戏
    /// </summary>
    private void InitializeGame()
    {
        SpawnBall();
    }
    
    /// <summary>
    /// 生成小球
    /// </summary>
    private void SpawnBall()
    {
        if (ballPrefab == null || ballStartPosition == null)
        {
            Debug.LogError("小球预制体或起始位置未设置！");
            return;
        }
        
        // 销毁旧小球
        if (currentBall != null)
        {
            Destroy(currentBall.gameObject);
        }
        
        // 生成新小球
        currentBall = Instantiate(ballPrefab, ballStartPosition.position, Quaternion.identity);
        currentBall.SetStartPosition(ballStartPosition.position);
    }
    
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (isGameActive) return;
        
        isGameActive = true;
        isPaused = false;
        gameStartTime = Time.time;
        gameTime = 0f;
        
        if (gameUI != null)
        {
            gameUI.SetActive(true);
        }
        
        OnGameStart?.Invoke();
    }
    
    /// <summary>
    /// 更新游戏时间
    /// </summary>
    private void UpdateGameTime()
    {
        gameTime = Time.time - gameStartTime;
        OnTimeUpdate?.Invoke(gameTime);
        
        // 检查时间限制
        if (gameTime >= gameTimeLimit)
        {
            GameOver();
        }
    }
    
    /// <summary>
    /// 小球到达终点
    /// </summary>
    public void OnBallReachGoal()
    {
        if (!isGameActive) return;
        
        CompleteGame();
    }
    
    /// <summary>
    /// 完成游戏
    /// </summary>
    private void CompleteGame()
    {
        isGameActive = false;
        
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }
        
        OnGameComplete?.Invoke(gameTime);
        
        // 保存成绩
        SaveScore(gameTime);
    }
    
    /// <summary>
    /// 游戏结束
    /// </summary>
    private void GameOver()
    {
        isGameActive = false;
        Debug.Log("游戏时间到！");
    }
    
    /// <summary>
    /// 切换暂停状态
    /// </summary>
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        if (!isGameActive || isPaused) return;
        
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseUI != null)
        {
            pauseUI.SetActive(true);
        }
        
        OnGamePause?.Invoke();
    }
    
    /// <summary>
    /// 恢复游戏
    /// </summary>
    public void ResumeGame()
    {
        if (!isPaused) return;
        
        isPaused = false;
        Time.timeScale = 1f;
        
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        
        OnGameResume?.Invoke();
    }
    
    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        isGameActive = false;
        isPaused = false;
        Time.timeScale = 1f;
        gameTime = 0f;
        
        if (victoryUI != null)
        {
            victoryUI.SetActive(false);
        }
        
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        
        SpawnBall();
    }
    
    /// <summary>
    /// 保存成绩
    /// </summary>
    private void SaveScore(float time)
    {
        // 获取当前关卡ID
        int levelId = LevelManager.Instance?.GetCurrentLevelId() ?? 0;
        
        // 保存本地记录
        float bestTime = PlayerPrefs.GetFloat($"Level_{levelId}_BestTime", float.MaxValue);
        if (time < bestTime)
        {
            PlayerPrefs.SetFloat($"Level_{levelId}_BestTime", time);
            PlayerPrefs.Save();
        }
        
        // 上传到服务器（如果已登录）
        if (NetworkManager.Instance != null && NetworkManager.Instance.IsLoggedIn())
        {
            NetworkManager.Instance.UploadScore(levelId, time);
        }
    }
    
    /// <summary>
    /// 获取当前游戏时间
    /// </summary>
    public float GetGameTime()
    {
        return gameTime;
    }
    
    /// <summary>
    /// 获取当前小球
    /// </summary>
    public BallController GetCurrentBall()
    {
        return currentBall;
    }
}

