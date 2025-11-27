using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 部件连接器 - 管理部件之间的连接关系
/// </summary>
public class ComponentConnector : MonoBehaviour
{
    [Header("连接设置")]
    [SerializeField] private LayerMask connectableLayer;
    [SerializeField] private float connectionRange = 2f;
    
    private List<Connection> connections = new List<Connection>();
    private GameObject sourceObject;
    private bool isConnecting = false;
    
    /// <summary>
    /// 开始连接
    /// </summary>
    public void StartConnection(GameObject source)
    {
        sourceObject = source;
        isConnecting = true;
    }
    
    /// <summary>
    /// 完成连接
    /// </summary>
    public void CompleteConnection(GameObject target)
    {
        if (!isConnecting || sourceObject == null) return;
        
        // 检查距离
        float distance = Vector3.Distance(sourceObject.transform.position, target.transform.position);
        if (distance > connectionRange)
        {
            Debug.LogWarning("连接距离过远！");
            return;
        }
        
        // 创建连接
        Connection connection = new Connection
        {
            source = sourceObject,
            target = target,
            connectionType = DetermineConnectionType(sourceObject, target)
        };
        
        connections.Add(connection);
        ApplyConnection(connection);
        
        isConnecting = false;
        sourceObject = null;
    }
    
    /// <summary>
    /// 确定连接类型
    /// </summary>
    private ConnectionType DetermineConnectionType(GameObject source, GameObject target)
    {
        // 根据部件类型确定连接方式
        if (source.GetComponent<Gear>() != null && target.GetComponent<Gear>() != null)
        {
            return ConnectionType.GearToGear;
        }
        else if (source.GetComponent<Lever>() != null && target.GetComponent<Gear>() != null)
        {
            return ConnectionType.LeverToGear;
        }
        else if (source.GetComponent<Spring>() != null)
        {
            return ConnectionType.SpringConnection;
        }
        
        return ConnectionType.Generic;
    }
    
    /// <summary>
    /// 应用连接
    /// </summary>
    private void ApplyConnection(Connection connection)
    {
        switch (connection.connectionType)
        {
            case ConnectionType.GearToGear:
                ConnectGears(connection.source, connection.target);
                break;
            case ConnectionType.LeverToGear:
                ConnectLeverToGear(connection.source, connection.target);
                break;
            case ConnectionType.SpringConnection:
                ConnectSpring(connection.source, connection.target);
                break;
        }
    }
    
    /// <summary>
    /// 连接齿轮
    /// </summary>
    private void ConnectGears(GameObject gear1, GameObject gear2)
    {
        Gear g1 = gear1.GetComponent<Gear>();
        Gear g2 = gear2.GetComponent<Gear>();
        
        if (g1 != null && g2 != null)
        {
            // 通过反射或公共方法设置连接
            // 这里需要Gear类提供连接方法
            System.Reflection.FieldInfo field = typeof(Gear).GetField("connectedGears", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                Gear[] gears1 = field.GetValue(g1) as Gear[];
                Gear[] gears2 = field.GetValue(g2) as Gear[];
                
                if (gears1 != null)
                {
                    System.Array.Resize(ref gears1, gears1.Length + 1);
                    gears1[gears1.Length - 1] = g2;
                    field.SetValue(g1, gears1);
                }
            }
        }
    }
    
    /// <summary>
    /// 连接杠杆到齿轮
    /// </summary>
    private void ConnectLeverToGear(GameObject lever, GameObject gear)
    {
        Lever l = lever.GetComponent<Lever>();
        Gear g = gear.GetComponent<Gear>();
        
        if (l != null && g != null)
        {
            // 通过反射设置连接对象
            System.Reflection.FieldInfo field = typeof(Lever).GetField("connectedObjects", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                MonoBehaviour[] objects = field.GetValue(l) as MonoBehaviour[];
                if (objects != null)
                {
                    System.Array.Resize(ref objects, objects.Length + 1);
                    objects[objects.Length - 1] = g;
                    field.SetValue(l, objects);
                }
            }
        }
    }
    
    /// <summary>
    /// 连接弹簧
    /// </summary>
    private void ConnectSpring(GameObject spring, GameObject target)
    {
        Spring s = spring.GetComponent<Spring>();
        if (s != null)
        {
            // 设置弹簧连接点
            System.Reflection.FieldInfo field = typeof(Spring).GetField("connectedPoint", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                field.SetValue(s, target.transform);
            }
        }
    }
    
    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect(GameObject obj1, GameObject obj2)
    {
        connections.RemoveAll(c => 
            (c.source == obj1 && c.target == obj2) || 
            (c.source == obj2 && c.target == obj1));
    }
    
    /// <summary>
    /// 获取所有连接
    /// </summary>
    public List<Connection> GetConnections()
    {
        return new List<Connection>(connections);
    }
    
    /// <summary>
    /// 连接类型
    /// </summary>
    public enum ConnectionType
    {
        GearToGear,
        LeverToGear,
        SpringConnection,
        Generic
    }
    
    /// <summary>
    /// 连接数据
    /// </summary>
    [System.Serializable]
    public class Connection
    {
        public GameObject source;
        public GameObject target;
        public ConnectionType connectionType;
    }
}

