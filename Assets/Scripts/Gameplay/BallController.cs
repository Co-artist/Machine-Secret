using UnityEngine;

/// <summary>
/// 小球控制器 - 处理小球的物理行为和引导逻辑
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BallController : MonoBehaviour
{
    [Header("物理参数")]
    [SerializeField] private float mass = 1f;
    [SerializeField] private float drag = 0.5f;
    [SerializeField] private float angularDrag = 0.5f;
    
    [Header("重置设置")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float resetDelay = 2f; // 重置延迟时间
    
    [Header("音效")]
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioClip goalSound;
    
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private AudioSource audioSource;
    private bool isActive = true;
    private GameManager gameManager;
    
    void Start()
    {
        InitializeBall();
    }
    
    /// <summary>
    /// 初始化小球
    /// </summary>
    private void InitializeBall()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 设置物理参数
        rb.mass = mass;
        rb.drag = drag;
        rb.angularDrag = angularDrag;
        rb.useGravity = true;
        
        // 设置碰撞器
        sphereCollider.isTrigger = false;
        
        // 记录起始位置
        if (startPosition == Vector3.zero)
        {
            startPosition = transform.position;
        }
        
        // 获取游戏管理器
        gameManager = FindObjectOfType<GameManager>();
    }
    
    /// <summary>
    /// 碰撞检测
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;
        
        // 播放碰撞音效
        if (collisionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }
        
        // 检查是否碰撞到危险区域
        if (collision.gameObject.CompareTag("Hazard"))
        {
            ResetBall();
        }
    }
    
    /// <summary>
    /// 触发器检测 - 到达终点
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        
        if (other.CompareTag("Goal"))
        {
            ReachGoal();
        }
        else if (other.CompareTag("Checkpoint"))
        {
            // 更新检查点位置
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            if (checkpoint != null)
            {
                startPosition = checkpoint.transform.position;
            }
        }
    }
    
    /// <summary>
    /// 到达终点
    /// </summary>
    private void ReachGoal()
    {
        isActive = false;
        
        // 播放胜利音效
        if (goalSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(goalSound);
        }
        
        // 通知游戏管理器
        if (gameManager != null)
        {
            gameManager.OnBallReachGoal();
        }
        
        // 停止物理模拟
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    
    /// <summary>
    /// 重置小球
    /// </summary>
    public void ResetBall()
    {
        if (!isActive) return;
        
        isActive = false;
        
        // 延迟重置
        Invoke(nameof(DoReset), resetDelay);
    }
    
    /// <summary>
    /// 执行重置
    /// </summary>
    private void DoReset()
    {
        // 重置位置和速度
        transform.position = startPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        isActive = true;
    }
    
    /// <summary>
    /// 设置起始位置
    /// </summary>
    public void SetStartPosition(Vector3 position)
    {
        startPosition = position;
    }
    
    /// <summary>
    /// 获取当前速度
    /// </summary>
    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }
    
    /// <summary>
    /// 添加力
    /// </summary>
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }
    
    /// <summary>
    /// 设置是否激活
    /// </summary>
    public void SetActive(bool active)
    {
        isActive = active;
        if (!active)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

