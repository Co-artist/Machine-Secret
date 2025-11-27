using UnityEngine;

/// <summary>
/// 游戏配置 - 存储游戏全局配置
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("物理设置")]
    public float gravity = -9.81f;
    public int solverIterations = 6;
    public float maxDepenetrationVelocity = 10f;
    
    [Header("游戏设置")]
    public float defaultTimeLimit = 300f;
    public float ballResetDelay = 2f;
    public float ballMass = 1f;
    public float ballDrag = 0.5f;
    
    [Header("编辑器设置")]
    public float defaultGridSize = 1f;
    public bool defaultSnapToGrid = true;
    public int maxUndoHistory = 50;
    
    [Header("网络设置")]
    public string serverURL = "https://api.example.com";
    public float networkTimeout = 10f;
    
    [Header("性能设置")]
    public int targetFrameRate = 60;
    public bool enableVSync = true;
    public float lodBias = 1f;
    
    private static GameConfig instance;
    
    public static GameConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameConfig>("GameConfig");
                if (instance == null)
                {
                    Debug.LogWarning("GameConfig资源未找到，使用默认值");
                    instance = CreateInstance<GameConfig>();
                }
            }
            return instance;
        }
    }
}

