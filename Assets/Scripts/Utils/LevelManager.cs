using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// 关卡管理器 - 管理关卡的加载、保存和序列化
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    [Header("关卡设置")]
    [SerializeField] private string levelsFolder = "Levels";
    [SerializeField] private bool useBase64Encoding = true;
    
    private int currentLevelId = 0;
    private Dictionary<int, LevelData> loadedLevels = new Dictionary<int, LevelData>();
    
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
    /// 保存关卡
    /// </summary>
    public void SaveLevel(LevelData levelData, int levelId = -1)
    {
        if (levelId == -1)
        {
            levelId = currentLevelId;
        }
        
        string json = JsonUtility.ToJson(levelData, true);
        
        if (useBase64Encoding)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            json = System.Convert.ToBase64String(bytes);
        }
        
        string path = GetLevelPath(levelId);
        File.WriteAllText(path, json);
        
        loadedLevels[levelId] = levelData;
        
        Debug.Log($"关卡 {levelId} 已保存到: {path}");
    }
    
    /// <summary>
    /// 加载关卡
    /// </summary>
    public LevelData LoadLevel(int levelId)
    {
        // 检查缓存
        if (loadedLevels.ContainsKey(levelId))
        {
            return loadedLevels[levelId];
        }
        
        string path = GetLevelPath(levelId);
        
        if (!File.Exists(path))
        {
            Debug.LogWarning($"关卡文件不存在: {path}");
            return null;
        }
        
        string json = File.ReadAllText(path);
        
        if (useBase64Encoding)
        {
            try
            {
                byte[] bytes = System.Convert.FromBase64String(json);
                json = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                // 如果不是Base64，直接使用原文本
            }
        }
        
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        loadedLevels[levelId] = levelData;
        currentLevelId = levelId;
        
        return levelData;
    }
    
    /// <summary>
    /// 获取关卡路径
    /// </summary>
    private string GetLevelPath(int levelId)
    {
        string folder = Path.Combine(Application.persistentDataPath, levelsFolder);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        return Path.Combine(folder, $"level_{levelId}.json");
    }
    
    /// <summary>
    /// 删除关卡
    /// </summary>
    public void DeleteLevel(int levelId)
    {
        string path = GetLevelPath(levelId);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        
        if (loadedLevels.ContainsKey(levelId))
        {
            loadedLevels.Remove(levelId);
        }
    }
    
    /// <summary>
    /// 获取关卡代码（用于分享）
    /// </summary>
    public string GetLevelCode(LevelData levelData)
    {
        string json = JsonUtility.ToJson(levelData);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        return System.Convert.ToBase64String(bytes);
    }
    
    /// <summary>
    /// 从代码加载关卡
    /// </summary>
    public LevelData LoadLevelFromCode(string code)
    {
        try
        {
            byte[] bytes = System.Convert.FromBase64String(code);
            string json = Encoding.UTF8.GetString(bytes);
            return JsonUtility.FromJson<LevelData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"加载关卡代码失败: {e.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 设置当前关卡ID
    /// </summary>
    public void SetCurrentLevelId(int levelId)
    {
        currentLevelId = levelId;
    }
    
    /// <summary>
    /// 获取当前关卡ID
    /// </summary>
    public int GetCurrentLevelId()
    {
        return currentLevelId;
    }
    
    /// <summary>
    /// 获取所有关卡ID列表
    /// </summary>
    public List<int> GetAllLevelIds()
    {
        List<int> levelIds = new List<int>();
        string folder = Path.Combine(Application.persistentDataPath, levelsFolder);
        
        if (Directory.Exists(folder))
        {
            string[] files = Directory.GetFiles(folder, "level_*.json");
            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (int.TryParse(fileName.Replace("level_", ""), out int id))
                {
                    levelIds.Add(id);
                }
            }
        }
        
        return levelIds;
    }
}

