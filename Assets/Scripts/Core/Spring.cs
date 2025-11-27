using UnityEngine;

/// <summary>
/// 弹簧组件 - 实现弹性机制
/// </summary>
[RequireComponent(typeof(SpringJoint))]
[RequireComponent(typeof(Rigidbody))]
public class Spring : MonoBehaviour
{
    [Header("弹簧参数")]
    [SerializeField] private Transform connectedPoint; // 连接点
    [SerializeField] private float springForce = 100f; // 弹簧力
    [SerializeField] private float damper = 5f; // 阻尼
    [SerializeField] private float minDistance = 1f; // 最小距离
    [SerializeField] private float maxDistance = 3f; // 最大距离
    
    [Header("可视化")]
    [SerializeField] private LineRenderer lineRenderer; // 弹簧可视化线条
    
    private SpringJoint springJoint;
    private Rigidbody rb;
    private Vector3 initialPosition;
    
    void Start()
    {
        InitializeSpring();
    }
    
    /// <summary>
    /// 初始化弹簧
    /// </summary>
    private void InitializeSpring()
    {
        springJoint = GetComponent<SpringJoint>();
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        
        // 如果没有指定连接点，创建一个固定点
        if (connectedPoint == null)
        {
            GameObject anchor = new GameObject("SpringAnchor");
            anchor.transform.position = initialPosition;
            anchor.transform.SetParent(transform.parent);
            
            Rigidbody anchorRb = anchor.AddComponent<Rigidbody>();
            anchorRb.isKinematic = true;
            
            springJoint.connectedBody = anchorRb;
        }
        else
        {
            Rigidbody connectedRb = connectedPoint.GetComponent<Rigidbody>();
            if (connectedRb == null)
            {
                connectedRb = connectedPoint.gameObject.AddComponent<Rigidbody>();
                connectedRb.isKinematic = true;
            }
            springJoint.connectedBody = connectedRb;
        }
        
        // 设置弹簧参数
        springJoint.spring = springForce;
        springJoint.damper = damper;
        springJoint.minDistance = minDistance;
        springJoint.maxDistance = maxDistance;
        springJoint.tolerance = 0.25f;
        
        // 初始化线条渲染器
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
        }
    }
    
    void Update()
    {
        // 更新弹簧可视化
        if (lineRenderer != null && springJoint.connectedBody != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, springJoint.connectedBody.transform.position);
        }
    }
    
    /// <summary>
    /// 设置弹簧力
    /// </summary>
    public void SetSpringForce(float force)
    {
        springForce = force;
        if (springJoint != null)
        {
            springJoint.spring = springForce;
        }
    }
    
    /// <summary>
    /// 设置阻尼
    /// </summary>
    public void SetDamper(float damperValue)
    {
        damper = damperValue;
        if (springJoint != null)
        {
            springJoint.damper = damper;
        }
    }
    
    /// <summary>
    /// 设置距离范围
    /// </summary>
    public void SetDistanceRange(float min, float max)
    {
        minDistance = min;
        maxDistance = max;
        if (springJoint != null)
        {
            springJoint.minDistance = minDistance;
            springJoint.maxDistance = maxDistance;
        }
    }
    
    /// <summary>
    /// 重置弹簧位置
    /// </summary>
    public void ResetSpring()
    {
        transform.position = initialPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    
    /// <summary>
    /// 获取当前拉伸距离
    /// </summary>
    public float GetCurrentDistance()
    {
        if (springJoint.connectedBody != null)
        {
            return Vector3.Distance(transform.position, springJoint.connectedBody.transform.position);
        }
        return 0f;
    }
}

