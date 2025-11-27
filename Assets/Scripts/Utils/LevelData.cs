using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡数据类 - 存储关卡信息
/// </summary>
[Serializable]
public class LevelData
{
    public string version;
    public string levelName;
    public List<ComponentData> components;
    public float[] startPosition;
    public float[] endPosition;
    public Dictionary<string, object> parameters; // 额外参数
    
    public LevelData()
    {
        components = new List<ComponentData>();
        parameters = new Dictionary<string, object>();
    }
}

/// <summary>
/// 部件数据类
/// </summary>
[Serializable]
public class ComponentData
{
    public string type; // 部件类型
    public float[] position; // 位置 [x, y, z]
    public float[] rotation; // 旋转 [x, y, z]
    public float[] scale; // 缩放 [x, y, z]
    public Dictionary<string, object> parameters; // 部件参数
    public List<int> connections; // 连接的部件索引
    
    public ComponentData()
    {
        parameters = new Dictionary<string, object>();
        connections = new List<int>();
    }
}

