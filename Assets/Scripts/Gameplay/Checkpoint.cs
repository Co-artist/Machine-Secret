using UnityEngine;

/// <summary>
/// 检查点 - 记录小球进度
/// </summary>
public class Checkpoint : MonoBehaviour
{
    [Header("检查点设置")]
    [SerializeField] private int checkpointIndex = 0;
    [SerializeField] private bool isActive = true;
    
    [Header("视觉效果")]
    [SerializeField] private GameObject activatedEffect;
    [SerializeField] private AudioClip activateSound;
    
    private bool isActivated = false;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 确保是触发器
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!isActive || isActivated) return;
        
        if (other.CompareTag("Ball"))
        {
            ActivateCheckpoint(other);
        }
    }
    
    /// <summary>
    /// 激活检查点
    /// </summary>
    private void ActivateCheckpoint(Collider other)
    {
        isActivated = true;
        
        // 播放音效
        if (activateSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(activateSound);
        }
        
        // 显示激活效果
        if (activatedEffect != null)
        {
            activatedEffect.SetActive(true);
        }
        
        // 通知游戏管理器
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            BallController ball = other.GetComponent<BallController>();
            if (ball != null)
            {
                ball.SetStartPosition(transform.position);
            }
        }
    }
    
    /// <summary>
    /// 重置检查点
    /// </summary>
    public void ResetCheckpoint()
    {
        isActivated = false;
        if (activatedEffect != null)
        {
            activatedEffect.SetActive(false);
        }
    }
    
    /// <summary>
    /// 设置激活状态
    /// </summary>
    public void SetActive(bool active)
    {
        isActive = active;
    }
}

