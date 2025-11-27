using UnityEngine;

/// <summary>
/// 性能优化器 - 管理物理和渲染性能
/// </summary>
public class PerformanceOptimizer : MonoBehaviour
{
    [Header("物理优化")]
    [SerializeField] private int maxSolverIterations = 6;
    [SerializeField] private float maxDepenetrationVelocity = 10f;
    [SerializeField] private bool enableSleeping = true;
    
    [Header("渲染优化")]
    [SerializeField] private int targetFrameRate = 60;
    [SerializeField] private bool enableVSync = true;
    
    [Header("LOD设置")]
    [SerializeField] private float lodBias = 1f;
    
    void Start()
    {
        OptimizePhysics();
        OptimizeRendering();
    }
    
    /// <summary>
    /// 优化物理设置
    /// </summary>
    private void OptimizePhysics()
    {
        Physics.defaultSolverIterations = maxSolverIterations;
        
        // 注意：defaultMaxDepenetrationWithDiscreteColliders 在某些Unity版本中不存在
        // 使用 defaultMaxDepenetrationVelocity 替代
        #if UNITY_2020_1_OR_NEWER
        Physics.defaultMaxDepenetrationVelocity = maxDepenetrationVelocity;
        #else
        // 旧版本Unity可能不支持此属性，使用默认值
        #endif
        
        if (enableSleeping)
        {
            // 启用刚体睡眠（默认已启用）
            Physics.sleepThreshold = 0.005f;
        }
    }
    
    /// <summary>
    /// 优化渲染设置
    /// </summary>
    private void OptimizeRendering()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = enableVSync ? 1 : 0;
        QualitySettings.lodBias = lodBias;
        
        // 设置纹理压缩
        #if UNITY_ANDROID
        QualitySettings.masterTextureLimit = 1; // 降低纹理分辨率
        #endif
    }
    
    /// <summary>
    /// 设置物理计算频率（降低以提高性能）
    /// </summary>
    public void SetPhysicsTimeStep(float timeStep)
    {
        Time.fixedDeltaTime = timeStep;
    }
    
    /// <summary>
    /// 启用/禁用物理睡眠
    /// </summary>
    public void SetSleepingEnabled(bool enabled)
    {
        enableSleeping = enabled;
        if (enabled)
        {
            Physics.sleepThreshold = 0.005f;
        }
        else
        {
            Physics.sleepThreshold = 0f;
        }
    }
}

