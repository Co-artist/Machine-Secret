using UnityEngine;

/// <summary>
/// 杠杆组件 - 实现杠杆原理
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Lever : MonoBehaviour
{
    [Header("杠杆参数")]
    [SerializeField] private Transform pivotPoint; // 支点位置
    [SerializeField] private float leverArmLength = 2f; // 杠杆臂长度
    [SerializeField] private float triggerForce = 10f; // 触发所需力
    [SerializeField] private float maxAngle = 45f; // 最大旋转角度
    
    [Header("触发设置")]
    [SerializeField] private LayerMask triggerLayer; // 触发层（小球）
    [SerializeField] private bool triggerOnEnter = true;
    
    [Header("连接对象")]
    [SerializeField] private MonoBehaviour[] connectedObjects; // 触发后激活的对象
    
    private Rigidbody rb;
    private FixedJoint fixedJoint;
    private bool isTriggered = false;
    private Vector3 initialRotation;
    
    void Start()
    {
        InitializeLever();
    }
    
    /// <summary>
    /// 初始化杠杆
    /// </summary>
    private void InitializeLever()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.eulerAngles;
        
        // 如果没有指定支点，使用自身位置
        if (pivotPoint == null)
        {
            pivotPoint = transform;
        }
        
        // 创建固定关节连接支点
        GameObject pivotObj = new GameObject("PivotPoint");
        pivotObj.transform.position = pivotPoint.position;
        pivotObj.transform.SetParent(transform.parent);
        
        Rigidbody pivotRb = pivotObj.AddComponent<Rigidbody>();
        pivotRb.isKinematic = true;
        
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = pivotRb;
        
        // 设置约束，只允许绕Z轴旋转
        rb.constraints = RigidbodyConstraints.FreezePosition | 
                        RigidbodyConstraints.FreezeRotationX | 
                        RigidbodyConstraints.FreezeRotationY;
    }
    
    /// <summary>
    /// 触发器检测 - 小球接触杠杆
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (triggerOnEnter && !isTriggered)
        {
            CheckTrigger(other);
        }
    }
    
    /// <summary>
    /// 碰撞检测 - 小球撞击杠杆
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (!triggerOnEnter && !isTriggered)
        {
            CheckTrigger(collision.collider);
        }
    }
    
    /// <summary>
    /// 检查是否触发杠杆
    /// </summary>
    private void CheckTrigger(Collider other)
    {
        // 检查是否在触发层
        if ((triggerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;
        
        Rigidbody otherRb = other.GetComponent<Rigidbody>();
        if (otherRb != null)
        {
            // 计算力矩
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Vector3 leverArm = contactPoint - pivotPoint.position;
            float force = otherRb.mass * otherRb.velocity.magnitude;
            
            if (force >= triggerForce)
            {
                TriggerLever(leverArm, force);
            }
        }
    }
    
    /// <summary>
    /// 触发杠杆
    /// </summary>
    private void TriggerLever(Vector3 leverArm, float force)
    {
        if (isTriggered) return;
        
        isTriggered = true;
        
        // 计算旋转方向和角度
        Vector3 torque = Vector3.Cross(leverArm, Vector3.up) * force;
        rb.AddTorque(torque);
        
        // 限制旋转角度
        StartCoroutine(LimitRotation());
        
        // 激活连接的对象
        ActivateConnectedObjects();
    }
    
    /// <summary>
    /// 限制旋转角度
    /// </summary>
    private System.Collections.IEnumerator LimitRotation()
    {
        while (true)
        {
            float currentAngle = Mathf.Abs(transform.eulerAngles.z - initialRotation.z);
            if (currentAngle > 180f) currentAngle = 360f - currentAngle;
            
            if (currentAngle >= maxAngle)
            {
                rb.angularVelocity = Vector3.zero;
                break;
            }
            
            yield return null;
        }
    }
    
    /// <summary>
    /// 激活连接的对象
    /// </summary>
    private void ActivateConnectedObjects()
    {
        foreach (var obj in connectedObjects)
        {
            if (obj != null)
            {
                // 尝试调用激活方法
                var gear = obj as Gear;
                if (gear != null)
                {
                    gear.Activate();
                }
                else
                {
                    // 使用反射调用通用激活方法
                    var method = obj.GetType().GetMethod("Activate");
                    if (method != null)
                    {
                        method.Invoke(obj, null);
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 重置杠杆
    /// </summary>
    public void ResetLever()
    {
        isTriggered = false;
        transform.eulerAngles = initialRotation;
        rb.angularVelocity = Vector3.zero;
    }
}

