using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 资源生成器 - 使用Procedural Mesh生成简单几何体
/// 这个工具可以帮助你快速生成基础3D模型，无需外部建模软件
/// </summary>
public class ResourceGenerator : EditorWindow
{
    private string savePath = "Assets/GeneratedMeshes";
    private int gearTeeth = 12;
    private float gearRadius = 1f;
    private float gearThickness = 0.3f;
    
    [MenuItem("Tools/资源生成器")]
    public static void ShowWindow()
    {
        GetWindow<ResourceGenerator>("资源生成器");
    }
    
    void OnGUI()
    {
        GUILayout.Label("基础几何体生成器", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        savePath = EditorGUILayout.TextField("保存路径:", savePath);
        EditorGUILayout.Space();
        
        // 齿轮生成
        EditorGUILayout.LabelField("齿轮生成", EditorStyles.boldLabel);
        gearTeeth = EditorGUILayout.IntField("齿数:", gearTeeth);
        gearRadius = EditorGUILayout.FloatField("半径:", gearRadius);
        gearThickness = EditorGUILayout.FloatField("厚度:", gearThickness);
        
        if (GUILayout.Button("生成齿轮"))
        {
            GenerateGear();
        }
        
        EditorGUILayout.Space();
        
        // 杠杆生成
        EditorGUILayout.LabelField("杠杆生成", EditorStyles.boldLabel);
        if (GUILayout.Button("生成杠杆"))
        {
            GenerateLever();
        }
        
        EditorGUILayout.Space();
        
        // 弹簧生成
        EditorGUILayout.LabelField("弹簧生成", EditorStyles.boldLabel);
        if (GUILayout.Button("生成弹簧"))
        {
            GenerateSpring();
        }
        
        EditorGUILayout.Space();
        
        // 小球生成
        EditorGUILayout.LabelField("小球生成", EditorStyles.boldLabel);
        if (GUILayout.Button("生成小球"))
        {
            GenerateBall();
        }
    }
    
    /// <summary>
    /// 生成齿轮
    /// </summary>
    private void GenerateGear()
    {
        Mesh mesh = CreateGearMesh(gearTeeth, gearRadius, gearThickness);
        
        // 尝试保存mesh，如果失败不影响预制体创建
        try
        {
            SaveMesh(mesh, "Gear");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"保存Gear mesh失败，但将继续创建预制体: {e.Message}");
        }
        
        // 创建GameObject
        GameObject gear = new GameObject("Gear");
        MeshFilter filter = gear.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        
        MeshRenderer renderer = gear.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        
        // 添加物理组件
        Rigidbody rb = gear.AddComponent<Rigidbody>();
        HingeJoint hinge = gear.AddComponent<HingeJoint>();
        
        // 添加Gear脚本
        Gear gearScript = gear.AddComponent<Gear>();
        
        // 保存为预制体
        string prefabPath = savePath.Replace("GeneratedMeshes", "Prefabs") + "/Gear.prefab";
        EnsureDirectoryExists(prefabPath);
        PrefabUtility.SaveAsPrefabAsset(gear, prefabPath);
        
        DestroyImmediate(gear);
        
        Debug.Log($"齿轮已生成: {prefabPath}");
    }
    
    /// <summary>
    /// 创建齿轮网格
    /// </summary>
    private Mesh CreateGearMesh(int teeth, float radius, float thickness)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        
        int segments = teeth * 2;
        float angleStep = 360f / segments;
        
        // 顶面
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float r = (i % 2 == 0) ? radius : radius * 0.8f;
            vertices.Add(new Vector3(Mathf.Cos(angle) * r, thickness / 2, Mathf.Sin(angle) * r));
            uvs.Add(new Vector2((float)i / segments, 0));
        }
        
        // 底面
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float r = (i % 2 == 0) ? radius : radius * 0.8f;
            vertices.Add(new Vector3(Mathf.Cos(angle) * r, -thickness / 2, Mathf.Sin(angle) * r));
            uvs.Add(new Vector2((float)i / segments, 1));
        }
        
        // 中心点
        int centerTop = vertices.Count;
        vertices.Add(new Vector3(0, thickness / 2, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));
        
        int centerBottom = vertices.Count;
        vertices.Add(new Vector3(0, -thickness / 2, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));
        
        // 顶面三角形
        for (int i = 0; i < segments; i++)
        {
            triangles.Add(centerTop);
            triangles.Add(i);
            triangles.Add((i + 1) % (segments + 1));
        }
        
        // 底面三角形
        for (int i = 0; i < segments; i++)
        {
            triangles.Add(centerBottom);
            triangles.Add((i + 1) % (segments + 1) + segments + 1);
            triangles.Add(i + segments + 1);
        }
        
        // 侧面
        for (int i = 0; i < segments; i++)
        {
            int top1 = i;
            int top2 = (i + 1) % (segments + 1);
            int bottom1 = i + segments + 1;
            int bottom2 = (i + 1) % (segments + 1) + segments + 1;
            
            triangles.Add(top1);
            triangles.Add(bottom1);
            triangles.Add(top2);
            
            triangles.Add(top2);
            triangles.Add(bottom1);
            triangles.Add(bottom2);
        }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        return mesh;
    }
    
    /// <summary>
    /// 生成杠杆
    /// </summary>
    private void GenerateLever()
    {
        GameObject lever = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lever.name = "Lever";
        lever.transform.localScale = new Vector3(3f, 0.2f, 0.5f);
        
        // 添加组件
        Rigidbody rb = lever.AddComponent<Rigidbody>();
        Lever leverScript = lever.AddComponent<Lever>();
        
        // 保存为预制体
        string prefabPath = savePath.Replace("GeneratedMeshes", "Prefabs") + "/Lever.prefab";
        EnsureDirectoryExists(prefabPath);
        PrefabUtility.SaveAsPrefabAsset(lever, prefabPath);
        
        DestroyImmediate(lever);
        
        Debug.Log($"杠杆已生成: {prefabPath}");
    }
    
    /// <summary>
    /// 生成弹簧
    /// </summary>
    private void GenerateSpring()
    {
        Mesh mesh = CreateSpringMesh(5f, 0.2f, 0.5f, 20);
        
        // 尝试保存mesh，如果失败不影响预制体创建
        try
        {
            SaveMesh(mesh, "Spring");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"保存Spring mesh失败，但将继续创建预制体: {e.Message}");
        }
        
        GameObject spring = new GameObject("Spring");
        MeshFilter filter = spring.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        
        MeshRenderer renderer = spring.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        
        // 添加组件
        Rigidbody rb = spring.AddComponent<Rigidbody>();
        SpringJoint springJoint = spring.AddComponent<SpringJoint>();
        Spring springScript = spring.AddComponent<Spring>();
        
        // 添加LineRenderer
        LineRenderer lineRenderer = spring.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        
        // 保存为预制体
        string prefabPath = savePath.Replace("GeneratedMeshes", "Prefabs") + "/Spring.prefab";
        EnsureDirectoryExists(prefabPath);
        PrefabUtility.SaveAsPrefabAsset(spring, prefabPath);
        
        DestroyImmediate(spring);
        
        Debug.Log($"弹簧已生成: {prefabPath}");
    }
    
    /// <summary>
    /// 创建弹簧网格
    /// </summary>
    private Mesh CreateSpringMesh(float height, float radius, float wireRadius, int turns)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        int segmentsPerTurn = 16;
        int totalSegments = turns * segmentsPerTurn;
        
        for (int i = 0; i <= totalSegments; i++)
        {
            float t = (float)i / totalSegments;
            float angle = t * turns * 2 * Mathf.PI;
            float y = t * height - height / 2;
            float r = radius + Mathf.Sin(angle * 4) * wireRadius;
            
            Vector3 center = new Vector3(Mathf.Cos(angle) * radius, y, Mathf.Sin(angle) * radius);
            
            // 创建圆形截面
            for (int j = 0; j < 8; j++)
            {
                float a = j * 2 * Mathf.PI / 8;
                Vector3 offset = new Vector3(
                    Mathf.Cos(a) * wireRadius,
                    0,
                    Mathf.Sin(a) * wireRadius
                );
                vertices.Add(center + offset);
            }
        }
        
        // 创建三角形
        for (int i = 0; i < totalSegments; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                int current = i * 8 + j;
                int next = i * 8 + (j + 1) % 8;
                int currentNext = (i + 1) * 8 + j;
                int nextNext = (i + 1) * 8 + (j + 1) % 8;
                
                triangles.Add(current);
                triangles.Add(currentNext);
                triangles.Add(next);
                
                triangles.Add(next);
                triangles.Add(currentNext);
                triangles.Add(nextNext);
            }
        }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        return mesh;
    }
    
    /// <summary>
    /// 生成小球
    /// </summary>
    private void GenerateBall()
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Ball";
        ball.transform.localScale = Vector3.one * 0.5f;
        
        // 添加组件
        Rigidbody rb = ball.AddComponent<Rigidbody>();
        rb.mass = 1f;
        rb.drag = 0.5f;
        
        BallController ballController = ball.AddComponent<BallController>();
        
        // 设置标签
        ball.tag = "Ball";
        
        // 保存为预制体
        string prefabPath = savePath.Replace("GeneratedMeshes", "Prefabs") + "/Ball.prefab";
        EnsureDirectoryExists(prefabPath);
        PrefabUtility.SaveAsPrefabAsset(ball, prefabPath);
        
        DestroyImmediate(ball);
        
        Debug.Log($"小球已生成: {prefabPath}");
    }
    
    /// <summary>
    /// 保存网格
    /// </summary>
    private void SaveMesh(Mesh mesh, string name)
    {
        // 确保保存路径存在
        if (!AssetDatabase.IsValidFolder(savePath))
        {
            // 如果文件夹不存在，尝试创建
            string[] folders = savePath.Split('/');
            string parentPath = "Assets";
            
            for (int i = 1; i < folders.Length; i++)
            {
                string currentPath = parentPath + "/" + folders[i];
                if (!AssetDatabase.IsValidFolder(currentPath))
                {
                    AssetDatabase.CreateFolder(parentPath, folders[i]);
                }
                parentPath = currentPath;
            }
        }
        
        string path = $"{savePath}/{name}.asset";
        
        // 检查文件是否已存在
        if (AssetDatabase.LoadAssetAtPath<Mesh>(path) != null)
        {
            // 如果已存在，先删除
            AssetDatabase.DeleteAsset(path);
        }
        
        try
        {
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"网格已保存: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"保存网格失败: {e.Message}");
            // 如果保存失败，不保存mesh文件，直接使用内存中的mesh
        }
    }
    
    /// <summary>
    /// 确保目录存在
    /// </summary>
    private void EnsureDirectoryExists(string filePath)
    {
        // 将文件路径转换为目录路径
        string directory = filePath;
        if (filePath.Contains("."))
        {
            // 如果是文件路径，获取目录
            int lastSlash = filePath.LastIndexOf('/');
            if (lastSlash >= 0)
            {
                directory = filePath.Substring(0, lastSlash);
            }
        }
        
        // 确保目录存在（使用AssetDatabase）
        if (!string.IsNullOrEmpty(directory) && directory.StartsWith("Assets"))
        {
            if (!AssetDatabase.IsValidFolder(directory))
            {
                string[] folders = directory.Split('/');
                string parentPath = "Assets";
                
                for (int i = 1; i < folders.Length; i++)
                {
                    string currentPath = parentPath + "/" + folders[i];
                    if (!AssetDatabase.IsValidFolder(currentPath))
                    {
                        AssetDatabase.CreateFolder(parentPath, folders[i]);
                    }
                    parentPath = currentPath;
                }
            }
        }
    }
}

