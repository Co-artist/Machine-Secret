using UnityEngine;

/// <summary>
/// 旋转轴枚举
/// </summary>
public enum RotationAxis
{
    X,  // 绕X轴旋转（前后旋转）
    Y,  // 绕Y轴旋转（上下旋转）
    Z   // 绕Z轴旋转（左右旋转，默认）
}

/// <summary>
/// 齿轮组件 - 实现齿轮传动机制
/// </summary>
[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(Rigidbody))]
public class Gear : MonoBehaviour
{
    [Header("齿轮参数")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float motorForce = 1000f;
    [SerializeField] private bool autoRotate = false;
    [SerializeField] private bool reverseDirection = false; // 反转旋转方向
    
    [Header("旋转轴设置")]
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Z; // 旋转轴方向
    
    [Header("连接设置")]
    [SerializeField] private Gear[] connectedGears;
    
    private HingeJoint hinge;
    private Rigidbody rb;
    private bool isActive = false;
    
    void Start()
    {
        InitializeGear();
    }
    
    /// <summary>
    /// 初始化齿轮
    /// </summary>
    private void InitializeGear()
    {
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
        
        // 设置旋转轴
        ApplyRotationAxis(rotationAxis);
        
        // 应用旋转方向反转
        float actualSpeed = reverseDirection ? -rotationSpeed : rotationSpeed;
        
        // 设置铰链关节参数
        hinge.useMotor = true;
        JointMotor motor = new JointMotor
        {
            targetVelocity = actualSpeed,
            force = motorForce,
            freeSpin = false
        };
        hinge.motor = motor;
        
        // 如果设置为自动旋转，启动电机
        if (autoRotate)
        {
            Activate();
        }
    }
    
    /// <summary>
    /// 应用旋转轴设置（内部方法）
    /// </summary>
    private void ApplyRotationAxis(RotationAxis axis)
    {
        Vector3 axisVector = Vector3.zero;
        
        switch (axis)
        {
            case RotationAxis.X:
                axisVector = Vector3.right; // (1, 0, 0)
                break;
            case RotationAxis.Y:
                axisVector = Vector3.up; // (0, 1, 0)
                break;
            case RotationAxis.Z:
                axisVector = Vector3.forward; // (0, 0, 1)
                break;
        }
        
        if (hinge != null)
        {
            hinge.axis = axisVector;
        }
    }
    
    /// <summary>
    /// 激活齿轮旋转
    /// </summary>
    public void Activate()
    {
        if (isActive) return;
        
        isActive = true;
        JointMotor motor = hinge.motor;
        // 应用旋转方向反转
        float actualSpeed = reverseDirection ? -rotationSpeed : rotationSpeed;
        motor.targetVelocity = actualSpeed;
        hinge.motor = motor;
        
        // 驱动连接的齿轮
        foreach (var gear in connectedGears)
        {
            if (gear != null)
            {
                gear.DriveFrom(this);
            }
        }
    }
    
    /// <summary>
    /// 停止齿轮旋转
    /// </summary>
    public void Deactivate()
    {
        if (!isActive) return;
        
        isActive = false;
        JointMotor motor = hinge.motor;
        motor.targetVelocity = 0f;
        hinge.motor = motor;
    }
    
    /// <summary>
    /// 被其他齿轮驱动
    /// </summary>
    public void DriveFrom(Gear sourceGear)
    {
        if (isActive) return;
        
        // 反向旋转（齿轮啮合）
        rotationSpeed = -sourceGear.rotationSpeed;
        Activate();
    }
    
    /// <summary>
    /// 碰撞检测 - 与其他齿轮接触时传递动力
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        Gear otherGear = collision.gameObject.GetComponent<Gear>();
        if (otherGear != null && isActive)
        {
            otherGear.DriveFrom(this);
        }
    }
    
    /// <summary>
    /// 设置旋转速度
    /// </summary>
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
        if (isActive)
        {
            JointMotor motor = hinge.motor;
            // 应用旋转方向反转
            float actualSpeed = reverseDirection ? -rotationSpeed : rotationSpeed;
            motor.targetVelocity = actualSpeed;
            hinge.motor = motor;
        }
    }
    
    /// <summary>
    /// 反转旋转方向
    /// </summary>
    public void ReverseDirection()
    {
        reverseDirection = !reverseDirection;
        if (isActive)
        {
            // 重新激活以应用新方向
            Deactivate();
            Activate();
        }
    }
    
    /// <summary>
    /// 获取当前旋转速度
    /// </summary>
    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }
    
    /// <summary>
    /// 设置旋转轴（运行时）
    /// </summary>
    public void SetRotationAxis(RotationAxis axis)
    {
        rotationAxis = axis;
        if (hinge != null)
        {
            ApplyRotationAxis(axis);
        }
    }
}

