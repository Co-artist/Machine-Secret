using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 关卡选择器 - 显示关卡列表
/// </summary>
public class LevelSelector : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private Transform levelContainer;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Button backButton;
    
    [Header("关卡设置")]
    [SerializeField] private int levelsPerPage = 12;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button prevPageButton;
    [SerializeField] private Text pageText;
    
    private int currentPage = 0;
    private List<int> allLevelIds = new List<int>();
    private List<GameObject> levelButtons = new List<GameObject>();
    
    void Start()
    {
        InitializeSelector();
    }
    
    /// <summary>
    /// 初始化选择器
    /// </summary>
    private void InitializeSelector()
    {
        LoadLevelList();
        CreateLevelButtons();
        
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackClicked);
        }
        
        if (nextPageButton != null)
        {
            nextPageButton.onClick.AddListener(NextPage);
        }
        
        if (prevPageButton != null)
        {
            prevPageButton.onClick.AddListener(PrevPage);
        }
        
        UpdatePageUI();
    }
    
    /// <summary>
    /// 加载关卡列表
    /// </summary>
    private void LoadLevelList()
    {
        if (LevelManager.Instance != null)
        {
            allLevelIds = LevelManager.Instance.GetAllLevelIds();
        }
        
        // 如果没有关卡，添加默认关卡
        if (allLevelIds.Count == 0)
        {
            for (int i = 1; i <= 10; i++)
            {
                allLevelIds.Add(i);
            }
        }
    }
    
    /// <summary>
    /// 创建关卡按钮
    /// </summary>
    private void CreateLevelButtons()
    {
        ClearButtons();
        
        int startIndex = currentPage * levelsPerPage;
        int endIndex = Mathf.Min(startIndex + levelsPerPage, allLevelIds.Count);
        
        for (int i = startIndex; i < endIndex; i++)
        {
            int levelId = allLevelIds[i];
            CreateLevelButton(levelId, i - startIndex);
        }
    }
    
    /// <summary>
    /// 创建单个关卡按钮
    /// </summary>
    private void CreateLevelButton(int levelId, int index)
    {
        if (levelButtonPrefab == null || levelContainer == null) return;
        
        GameObject button = Instantiate(levelButtonPrefab, levelContainer);
        levelButtons.Add(button);
        
        Button btn = button.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => LoadLevel(levelId));
        }
        
        // 设置按钮文本
        Text[] texts = button.GetComponentsInChildren<Text>();
        if (texts.Length > 0)
        {
            texts[0].text = $"关卡 {levelId}";
        }
        
        // 显示最佳时间
        float bestTime = PlayerPrefs.GetFloat($"Level_{levelId}_BestTime", float.MaxValue);
        if (texts.Length > 1 && bestTime < float.MaxValue)
        {
            texts[1].text = FormatTime(bestTime);
        }
        
        // 显示完成状态
        bool isCompleted = PlayerPrefs.GetInt($"Level_{levelId}_Completed", 0) == 1;
        Image[] images = button.GetComponentsInChildren<Image>();
        if (images.Length > 1 && isCompleted)
        {
            images[1].color = Color.green; // 完成标记
        }
    }
    
    /// <summary>
    /// 清除按钮
    /// </summary>
    private void ClearButtons()
    {
        foreach (var button in levelButtons)
        {
            Destroy(button);
        }
        levelButtons.Clear();
    }
    
    /// <summary>
    /// 加载关卡
    /// </summary>
    private void LoadLevel(int levelId)
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SetCurrentLevelId(levelId);
        }
        
        // 加载游戏场景
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadScene("Gameplay");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }
    }
    
    /// <summary>
    /// 下一页
    /// </summary>
    private void NextPage()
    {
        int maxPage = Mathf.CeilToInt((float)allLevelIds.Count / levelsPerPage) - 1;
        if (currentPage < maxPage)
        {
            currentPage++;
            CreateLevelButtons();
            UpdatePageUI();
        }
    }
    
    /// <summary>
    /// 上一页
    /// </summary>
    private void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            CreateLevelButtons();
            UpdatePageUI();
        }
    }
    
    /// <summary>
    /// 更新页面UI
    /// </summary>
    private void UpdatePageUI()
    {
        int maxPage = Mathf.CeilToInt((float)allLevelIds.Count / levelsPerPage) - 1;
        
        if (pageText != null)
        {
            pageText.text = $"{currentPage + 1} / {maxPage + 1}";
        }
        
        if (prevPageButton != null)
        {
            prevPageButton.interactable = currentPage > 0;
        }
        
        if (nextPageButton != null)
        {
            nextPageButton.interactable = currentPage < maxPage;
        }
    }
    
    /// <summary>
    /// 格式化时间
    /// </summary>
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
    
    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void OnBackClicked()
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadScene("MainMenu");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}

