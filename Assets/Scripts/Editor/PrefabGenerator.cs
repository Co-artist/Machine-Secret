using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 预制体生成器 - 一键生成所有游戏预制体
/// 运行此工具可以自动生成：Gear, Lever, Spring, Ball
/// </summary>
public class PrefabGenerator : EditorWindow
{
    [MenuItem("Tools/预制体生成器/一键生成所有预制体")]
    public static void ShowWindow()
    {
        GetWindow<PrefabGenerator>("预制体生成器");
    }
    
    void OnGUI()
    {
        GUILayout.Label("预制体自动生成工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "此工具将自动生成以下预制体：\n" +
            "1. Gear（齿轮）\n" +
            "2. Lever（杠杆）\n" +
            "3. Spring（弹簧）\n" +
            "4. Ball（小球）\n\n" +
            "所有预制体将保存到：Assets/Prefabs/Generated/",
            MessageType.Info
        );
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("生成所有预制体", GUILayout.Height(40)))
        {
            GenerateAllPrefabs();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        GUILayout.Label("单独生成选项", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成齿轮"))
        {
            GenerateGear();
        }
        if (GUILayout.Button("生成杠杆"))
        {
            GenerateLever();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成弹簧"))
        {
            GenerateSpring();
        }
        if (GUILayout.Button("生成小球"))
        {
            GenerateBall();
        }
        EditorGUILayout.EndHorizontal();
    }
    
    /// <summary>
    /// 生成所有预制体
    /// </summary>
    private static void GenerateAllPrefabs()
    {
        EditorUtility.DisplayProgressBar("生成预制体", "正在生成齿轮...", 0.1f);
        GenerateGear();
        
        EditorUtility.DisplayProgressBar("生成预制体", "正在生成杠杆...", 0.3f);
        GenerateLever();
        
        EditorUtility.DisplayProgressBar("生成预制体", "正在生成弹簧...", 0.6f);
        GenerateSpring();
        
        EditorUtility.DisplayProgressBar("生成预制体", "正在生成小球...", 0.8f);
        GenerateBall();
        
        EditorUtility.DisplayProgressBar("生成预制体", "正在刷新资源...", 0.9f);
        AssetDatabase.Refresh();
        
        EditorUtility.ClearProgressBar();
        
        EditorUtility.DisplayDialog(
            "生成完成",
            "所有预制体已生成完成！\n\n" +
            "✓ Gear.prefab\n" +
            "✓ Lever.prefab\n" +
            "✓ Spring.prefab\n" +
            "✓ Ball.prefab\n\n" +
            "位置：Assets/Prefabs/Generated/",
            "确定"
        );
        
        Debug.Log("✓ 所有预制体已生成完成！");
    }
    
    /// <summary>
    /// 生成齿轮预制体
    /// </summary>
    private static void GenerateGear()
    {
        // 确保文件夹存在
        string prefabFolder = "Assets/Prefabs/Generated";
        EnsureDirectoryExists(prefabFolder);
        
        // 创建齿轮网格
        Mesh gearMesh = CreateGearMesh(12, 1f, 0.3f);
        
        // 创建GameObject
        GameObject gear = new GameObject("Gear");
        MeshFilter filter = gear.AddComponent<MeshFilter>();
        filter.mesh = gearMesh;
        
        MeshRenderer renderer = gear.AddComponent<MeshRenderer>();
        Material gearMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/GearMaterial.mat");
        if (gearMat != null)
        {
            renderer.material = gearMat;
        }
        else
        {
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(0.6f, 0.6f, 0.65f);
        }
        
        // 添加物理组件
        Rigidbody rb = gear.AddComponent<Rigidbody>();
        rb.mass = 1f;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
        
        // 创建固定锚点用于HingeJoint
        GameObject anchor = new GameObject("Anchor");
        anchor.transform.parent = gear.transform;
        anchor.transform.localPosition = Vector3.zero;
        Rigidbody anchorRb = anchor.AddComponent<Rigidbody>();
        anchorRb.isKinematic = true;
        anchorRb.useGravity = false;
        
        HingeJoint hinge = gear.AddComponent<HingeJoint>();
        hinge.axis = Vector3.forward; // Z轴旋转
        hinge.anchor = Vector3.zero;
        hinge.connectedBody = anchorRb;
        
        // 添加Collider
        MeshCollider collider = gear.AddComponent<MeshCollider>();
        collider.convex = true;
        
        // 添加Gear脚本
        Gear gearScript = gear.AddComponent<Gear>();
        
        // 确保文件夹存在
        EnsureDirectoryExists(prefabFolder);
        
        // 保存为预制体
        string prefabPath = $"{prefabFolder}/Gear.prefab";
        EnsureDirectoryExists(prefabPath);
        
        GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (existingPrefab != null)
        {
            PrefabUtility.SaveAsPrefabAsset(gear, prefabPath);
            Debug.Log($"✓ 已更新预制体: {prefabPath}");
        }
        else
        {
            PrefabUtility.SaveAsPrefabAsset(gear, prefabPath);
            Debug.Log($"✓ 已创建预制体: {prefabPath}");
        }
        
        DestroyImmediate(gear);
    }
    
    /// <summary>
    /// 生成杠杆预制体
    /// </summary>
    private static void GenerateLever()
    {
        // 确保文件夹存在
        string prefabFolder = "Assets/Prefabs/Generated";
        EnsureDirectoryExists(prefabFolder);
        
        // 创建杠杆
        GameObject lever = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lever.name = "Lever";
        lever.transform.localScale = new Vector3(3f, 0.2f, 0.5f);
        
        // 应用材质
        Material leverMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/LeverMaterial.mat");
        if (leverMat != null)
        {
            lever.GetComponent<MeshRenderer>().material = leverMat;
        }
        
        // 添加组件
        Rigidbody rb = lever.AddComponent<Rigidbody>();
        Lever leverScript = lever.AddComponent<Lever>();
        
        // 设置Collider（不是Trigger，以便产生物理碰撞）
        BoxCollider collider = lever.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.isTrigger = false; // 改为false，让小球可以碰撞杠杆
        }
        
        // 确保文件夹存在
        EnsureDirectoryExists(prefabFolder);
        
        // 保存为预制体
        string prefabPath = $"{prefabFolder}/Lever.prefab";
        EnsureDirectoryExists(prefabPath);
        
        GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (existingPrefab != null)
        {
            PrefabUtility.SaveAsPrefabAsset(lever, prefabPath);
            Debug.Log($"✓ 已更新预制体: {prefabPath}");
        }
        else
        {
            PrefabUtility.SaveAsPrefabAsset(lever, prefabPath);
            Debug.Log($"✓ 已创建预制体: {prefabPath}");
        }
        
        DestroyImmediate(lever);
    }
    
    /// <summary>
    /// 生成弹簧预制体
    /// </summary>
    private static void GenerateSpring()
    {
        // 确保文件夹存在
        string prefabFolder = "Assets/Prefabs/Generated";
        EnsureDirectoryExists(prefabFolder);
        
        // 创建弹簧网格
        Mesh springMesh = CreateSpringMesh(5f, 0.2f, 0.5f, 20);
        
        GameObject spring = new GameObject("Spring");
        MeshFilter filter = spring.AddComponent<MeshFilter>();
        filter.mesh = springMesh;
        
        MeshRenderer renderer = spring.AddComponent<MeshRenderer>();
        Material springMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/SpringMaterial.mat");
        if (springMat != null)
        {
            renderer.material = springMat;
        }
        else
        {
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(0.78f, 0.78f, 0.82f);
        }
        
        // 添加组件
        Rigidbody rb = spring.AddComponent<Rigidbody>();
        SpringJoint springJoint = spring.AddComponent<SpringJoint>();
        Spring springScript = spring.AddComponent<Spring>();
        
        // 添加Collider
        MeshCollider collider = spring.AddComponent<MeshCollider>();
        collider.convex = true;
        
        // 添加LineRenderer（可选）
        LineRenderer lineRenderer = spring.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;
        
        // 保存为预制体
        string prefabPath = $"{prefabFolder}/Spring.prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
        {
            PrefabUtility.SaveAsPrefabAsset(spring, prefabPath);
            Debug.Log($"✓ 已更新预制体: {prefabPath}");
        }
        else
        {
            PrefabUtility.SaveAsPrefabAsset(spring, prefabPath);
            Debug.Log($"✓ 已创建预制体: {prefabPath}");
        }
        
        DestroyImmediate(spring);
    }
    
    /// <summary>
    /// 生成小球预制体
    /// </summary>
    private static void GenerateBall()
    {
        // 确保文件夹存在
        string prefabFolder = "Assets/Prefabs/Generated";
        EnsureDirectoryExists(prefabFolder);
        
        // 创建小球
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Ball";
        ball.transform.localScale = Vector3.one * 0.5f;
        
        // 应用材质
        Material ballMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/BallMaterial.mat");
        if (ballMat != null)
        {
            ball.GetComponent<MeshRenderer>().material = ballMat;
        }
        
        // 添加组件
        Rigidbody rb = ball.AddComponent<Rigidbody>();
        rb.mass = 1f;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
        rb.useGravity = true;
        
        BallController ballController = ball.AddComponent<BallController>();
        
        // 设置标签
        try
        {
            ball.tag = "Ball";
        }
        catch
        {
            Debug.LogWarning("Ball标签未设置，请先运行标签设置工具");
        }
        
        // 保存为预制体
        string prefabPath = $"{prefabFolder}/Ball.prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
        {
            PrefabUtility.SaveAsPrefabAsset(ball, prefabPath);
            Debug.Log($"✓ 已更新预制体: {prefabPath}");
        }
        else
        {
            PrefabUtility.SaveAsPrefabAsset(ball, prefabPath);
            Debug.Log($"✓ 已创建预制体: {prefabPath}");
        }
        
        DestroyImmediate(ball);
    }
    
    /// <summary>
    /// 创建齿轮网格
    /// </summary>
    private static Mesh CreateGearMesh(int teeth, float radius, float thickness)
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
    /// 创建弹簧网格
    /// </summary>
    private static Mesh CreateSpringMesh(float length, float radius, float thickness, int segments)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        int ringSegments = 8;
        float angleStep = 360f / ringSegments;
        
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float y = t * length;
            float currentRadius = radius + Mathf.Sin(t * Mathf.PI * 4f) * 0.1f;
            
            for (int j = 0; j < ringSegments; j++)
            {
                float angle = j * angleStep * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * currentRadius;
                float z = Mathf.Sin(angle) * currentRadius;
                vertices.Add(new Vector3(x, y, z));
            }
        }
        
        // 创建三角形
        for (int i = 0; i < segments; i++)
        {
            for (int j = 0; j < ringSegments; j++)
            {
                int current = i * ringSegments + j;
                int next = i * ringSegments + (j + 1) % ringSegments;
                int currentNext = (i + 1) * ringSegments + j;
                int nextNext = (i + 1) * ringSegments + (j + 1) % ringSegments;
                
                triangles.Add(current);
                triangles.Add(next);
                triangles.Add(currentNext);
                
                triangles.Add(next);
                triangles.Add(nextNext);
                triangles.Add(currentNext);
            }
        }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        return mesh;
    }
    
    /// <summary>
    /// 确保目录存在
    /// </summary>
    private static void EnsureDirectoryExists(string filePath)
    {
        string directory = Path.GetDirectoryName(filePath).Replace("\\", "/");
        
        // 确保路径以Assets开头
        if (!directory.StartsWith("Assets/"))
        {
            directory = "Assets/" + directory.TrimStart('A', 's', 's', 'e', 't', 's', '/');
        }
        
        if (!AssetDatabase.IsValidFolder(directory))
        {
            string[] folders = directory.Split('/');
            string currentPath = "Assets";
            
            for (int i = 1; i < folders.Length; i++)
            {
                if (string.IsNullOrEmpty(folders[i])) continue;
                
                string nextPath = $"{currentPath}/{folders[i]}";
                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    AssetDatabase.CreateFolder(currentPath, folders[i]);
                    AssetDatabase.Refresh();
                }
                currentPath = nextPath;
            }
        }
    }
}

