using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 编辑器UI - 关卡编辑器的用户界面
/// </summary>
public class EditorUI : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private GameObject editorPanel;
    [SerializeField] private Transform componentLibraryContainer;
    [SerializeField] private GameObject componentButtonPrefab;
    [SerializeField] private InputField levelNameInput;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    [SerializeField] private Toggle gridToggle;
    [SerializeField] private Slider gridSizeSlider;
    [SerializeField] private Text gridSizeText;
    
    [Header("编辑器引用")]
    [SerializeField] private LevelEditor levelEditor;
    
    private List<GameObject> componentButtons = new List<GameObject>();
    
    void Start()
    {
        InitializeUI();
    }
    
    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitializeUI()
    {
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveLevel);
        }
        
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(LoadLevel);
        }
        
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayLevel);
        }
        
        if (undoButton != null)
        {
            undoButton.onClick.AddListener(() => levelEditor?.Undo());
        }
        
        if (redoButton != null)
        {
            redoButton.onClick.AddListener(() => levelEditor?.Redo());
        }
        
        if (gridToggle != null)
        {
            gridToggle.onValueChanged.AddListener(OnGridToggleChanged);
        }
        
        if (gridSizeSlider != null)
        {
            gridSizeSlider.onValueChanged.AddListener(OnGridSizeChanged);
            if (gridSizeText != null)
            {
                gridSizeText.text = $"网格大小: {gridSizeSlider.value:F1}";
            }
        }
        
        CreateComponentButtons();
    }
    
    /// <summary>
    /// 创建部件按钮
    /// </summary>
    private void CreateComponentButtons()
    {
        if (levelEditor == null || componentButtonPrefab == null || componentLibraryContainer == null)
            return;
        
        // 通过反射获取部件预制体数组
        System.Reflection.FieldInfo field = typeof(LevelEditor).GetField("componentPrefabs", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            GameObject[] prefabs = field.GetValue(levelEditor) as GameObject[];
            
            if (prefabs != null)
            {
                for (int i = 0; i < prefabs.Length; i++)
                {
                    int index = i; // 闭包变量
                    GameObject button = Instantiate(componentButtonPrefab, componentLibraryContainer);
                    componentButtons.Add(button);
                    
                    Button btn = button.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.onClick.AddListener(() => levelEditor.SelectComponent(index));
                    }
                    
                    // 设置按钮文本
                    Text text = button.GetComponentInChildren<Text>();
                    if (text != null && prefabs[i] != null)
                    {
                        text.text = prefabs[i].name;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 保存关卡
    /// </summary>
    private void SaveLevel()
    {
        if (levelEditor == null) return;
        
        string levelName = levelNameInput != null ? levelNameInput.text : "未命名关卡";
        if (string.IsNullOrEmpty(levelName))
        {
            levelName = "未命名关卡";
        }
        
        levelEditor.SaveLevel(levelName);
        
        Debug.Log($"关卡已保存: {levelName}");
    }
    
    /// <summary>
    /// 加载关卡
    /// </summary>
    private void LoadLevel()
    {
        // TODO: 实现关卡选择界面
        Debug.Log("加载关卡功能待实现");
    }
    
    /// <summary>
    /// 播放关卡
    /// </summary>
    private void PlayLevel()
    {
        if (editorPanel != null)
        {
            editorPanel.SetActive(false);
        }
        
        // 切换到游戏模式
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.StartGame();
        }
    }
    
    /// <summary>
    /// 网格对齐切换
    /// </summary>
    private void OnGridToggleChanged(bool value)
    {
        // 通过反射设置网格对齐
        if (levelEditor != null)
        {
            System.Reflection.FieldInfo field = typeof(LevelEditor).GetField("snapToGrid", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(levelEditor, value);
            }
        }
    }
    
    /// <summary>
    /// 网格大小改变
    /// </summary>
    private void OnGridSizeChanged(float value)
    {
        if (gridSizeText != null)
        {
            gridSizeText.text = $"网格大小: {value:F1}";
        }
        
        // 通过反射设置网格大小
        if (levelEditor != null)
        {
            System.Reflection.FieldInfo field = typeof(LevelEditor).GetField("gridSize", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(levelEditor, value);
            }
        }
    }
    
    /// <summary>
    /// 显示/隐藏编辑器
    /// </summary>
    public void SetEditorVisible(bool visible)
    {
        if (editorPanel != null)
        {
            editorPanel.SetActive(visible);
        }
    }
}

