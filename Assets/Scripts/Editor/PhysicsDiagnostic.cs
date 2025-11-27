using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// 物理系统诊断工具 - 检查并修复物理交互问题
/// </summary>
public class PhysicsDiagnostic : EditorWindow
{
    private Vector2 scrollPosition;
    
    [MenuItem("Tools/物理系统诊断工具")]
    public static void ShowWindow()
    {
        GetWindow<PhysicsDiagnostic>("物理系统诊断工具");
    }
    
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("物理系统诊断工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "此工具将检查场景中的物理组件配置，\n" +
            "并自动修复常见问题。",
            MessageType.Info
        );
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("诊断场景物理系统", GUILayout.Height(40)))
        {
            DiagnoseScene();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("修复所有物理问题", GUILayout.Height(40)))
        {
            FixAllPhysicsIssues();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("快速修复", EditorStyles.boldLabel);
        
        if (GUILayout.Button("修复杠杆Collider（移除Trigger）", GUILayout.Height(30)))
        {
            FixLeverColliders();
        }
        
        if (GUILayout.Button("修复齿轮HingeJoint配置", GUILayout.Height(30)))
        {
            FixGearHingeJoints();
        }
        
        if (GUILayout.Button("确保Ground有Collider", GUILayout.Height(30)))
        {
            EnsureGroundCollider();
        }
        
        if (GUILayout.Button("修复所有预制体", GUILayout.Height(30)))
        {
            FixAllPrefabs();
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    /// <summary>
    /// 诊断场景
    /// </summary>
    private void DiagnoseScene()
    {
        Debug.Log("=== 开始诊断场景物理系统 ===");
        
        int issuesFound = 0;
        
        // 检查Ground
        GameObject ground = GameObject.Find("Ground");
        if (ground == null)
        {
            Debug.LogWarning("✗ Ground对象不存在");
            issuesFound++;
        }
        else
        {
            Collider groundCollider = ground.GetComponent<Collider>();
            if (groundCollider == null)
            {
                Debug.LogWarning("✗ Ground没有Collider组件");
                issuesFound++;
            }
            else
            {
                Debug.Log("✓ Ground有Collider组件");
            }
        }
        
        // 检查所有杠杆
        Lever[] levers = FindObjectsOfType<Lever>();
        Debug.Log($"找到 {levers.Length} 个杠杆");
        foreach (Lever lever in levers)
        {
            Collider collider = lever.GetComponent<Collider>();
            if (collider != null && collider.isTrigger)
            {
                Debug.LogWarning($"✗ 杠杆 '{lever.name}' 的Collider是Trigger，小球会穿过");
                issuesFound++;
            }
            else if (collider == null)
            {
                Debug.LogWarning($"✗ 杠杆 '{lever.name}' 没有Collider组件");
                issuesFound++;
            }
            else
            {
                Debug.Log($"✓ 杠杆 '{lever.name}' Collider配置正确");
            }
        }
        
        // 检查所有齿轮
        Gear[] gears = FindObjectsOfType<Gear>();
        Debug.Log($"找到 {gears.Length} 个齿轮");
        foreach (Gear gear in gears)
        {
            HingeJoint hinge = gear.GetComponent<HingeJoint>();
            if (hinge == null)
            {
                Debug.LogWarning($"✗ 齿轮 '{gear.name}' 没有HingeJoint组件");
                issuesFound++;
            }
            else
            {
                // 检查锚点配置
                if (hinge.anchor == Vector3.zero && hinge.connectedBody == null)
                {
                    Debug.LogWarning($"✗ 齿轮 '{gear.name}' HingeJoint未配置锚点或连接体");
                    issuesFound++;
                }
                else
                {
                    Debug.Log($"✓ 齿轮 '{gear.name}' HingeJoint配置正确");
                }
            }
            
            Rigidbody rb = gear.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogWarning($"✗ 齿轮 '{gear.name}' 没有Rigidbody组件");
                issuesFound++;
            }
        }
        
        // 检查所有弹簧
        Spring[] springs = FindObjectsOfType<Spring>();
        Debug.Log($"找到 {springs.Length} 个弹簧");
        foreach (Spring spring in springs)
        {
            SpringJoint springJoint = spring.GetComponent<SpringJoint>();
            if (springJoint == null)
            {
                Debug.LogWarning($"✗ 弹簧 '{spring.name}' 没有SpringJoint组件");
                issuesFound++;
            }
            
            Collider collider = spring.GetComponent<Collider>();
            if (collider == null)
            {
                Debug.LogWarning($"✗ 弹簧 '{spring.name}' 没有Collider组件");
                issuesFound++;
            }
        }
        
        // 检查小球
        BallController[] balls = FindObjectsOfType<BallController>();
        Debug.Log($"找到 {balls.Length} 个小球");
        foreach (BallController ball in balls)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogWarning($"✗ 小球 '{ball.name}' 没有Rigidbody组件");
                issuesFound++;
            }
            else if (!rb.useGravity)
            {
                Debug.LogWarning($"✗ 小球 '{ball.name}' 未启用重力");
                issuesFound++;
            }
            else
            {
                Debug.Log($"✓ 小球 '{ball.name}' 配置正确");
            }
        }
        
        Debug.Log($"=== 诊断完成，发现 {issuesFound} 个问题 ===");
        
        if (issuesFound > 0)
        {
            EditorUtility.DisplayDialog(
                "诊断完成",
                $"发现 {issuesFound} 个物理配置问题。\n\n" +
                "请查看Console窗口了解详细信息，\n" +
                "或点击'修复所有物理问题'按钮自动修复。",
                "确定"
            );
        }
        else
        {
            EditorUtility.DisplayDialog(
                "诊断完成",
                "✓ 未发现物理配置问题！",
                "确定"
            );
        }
    }
    
    /// <summary>
    /// 修复所有物理问题
    /// </summary>
    private void FixAllPhysicsIssues()
    {
        if (!EditorUtility.DisplayDialog(
            "确认修复",
            "将修复场景中所有物理配置问题。\n\n" +
            "此操作会修改场景中的对象，请确保已保存场景。",
            "确定", "取消"))
        {
            return;
        }
        
        FixLeverColliders();
        FixGearHingeJoints();
        EnsureGroundCollider();
        
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        
        EditorUtility.DisplayDialog(
            "修复完成",
            "✓ 所有物理问题已修复！\n\n" +
            "请保存场景并重新运行游戏测试。",
            "确定"
        );
        
        Debug.Log("✓ 所有物理问题已修复");
    }
    
    /// <summary>
    /// 修复杠杆Collider
    /// </summary>
    private void FixLeverColliders()
    {
        Lever[] levers = FindObjectsOfType<Lever>();
        int fixedCount = 0;
        
        foreach (Lever lever in levers)
        {
            Collider collider = lever.GetComponent<Collider>();
            if (collider != null && collider.isTrigger)
            {
                collider.isTrigger = false;
                Debug.Log($"✓ 已修复杠杆 '{lever.name}' 的Collider（移除了Trigger）");
                fixedCount++;
            }
            else if (collider == null)
            {
                BoxCollider boxCollider = lever.gameObject.AddComponent<BoxCollider>();
                boxCollider.isTrigger = false;
                Debug.Log($"✓ 已为杠杆 '{lever.name}' 添加Collider");
                fixedCount++;
            }
        }
        
        Debug.Log($"✓ 修复了 {fixedCount} 个杠杆的Collider");
    }
    
    /// <summary>
    /// 修复齿轮HingeJoint
    /// </summary>
    private void FixGearHingeJoints()
    {
        Gear[] gears = FindObjectsOfType<Gear>();
        int fixedCount = 0;
        
        foreach (Gear gear in gears)
        {
            HingeJoint hinge = gear.GetComponent<HingeJoint>();
            if (hinge == null)
            {
                hinge = gear.gameObject.AddComponent<HingeJoint>();
                Debug.Log($"✓ 已为齿轮 '{gear.name}' 添加HingeJoint");
                fixedCount++;
            }
            
            // 配置HingeJoint
            Rigidbody rb = gear.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 设置锚点在中心
                hinge.anchor = Vector3.zero;
                
                // 如果没有连接体，创建一个固定锚点
                if (hinge.connectedBody == null)
                {
                    // 创建一个不可见的固定锚点
                    GameObject anchor = new GameObject($"{gear.name}_Anchor");
                    anchor.transform.position = gear.transform.position;
                    anchor.transform.parent = gear.transform;
                    
                    Rigidbody anchorRb = anchor.AddComponent<Rigidbody>();
                    anchorRb.isKinematic = true;
                    anchorRb.useGravity = false;
                    
                    hinge.connectedBody = anchorRb;
                    
                    Debug.Log($"✓ 已为齿轮 '{gear.name}' 创建固定锚点");
                    fixedCount++;
                }
                
                // 启用电机（如果需要）
                JointMotor motor = hinge.motor;
                motor.force = 1000f;
                motor.freeSpin = false;
                hinge.motor = motor;
                hinge.useMotor = false; // 默认不启用，由Gear脚本控制
            }
        }
        
        Debug.Log($"✓ 修复了 {fixedCount} 个齿轮的HingeJoint");
    }
    
    /// <summary>
    /// 确保Ground有Collider
    /// </summary>
    private void EnsureGroundCollider()
    {
        GameObject ground = GameObject.Find("Ground");
        if (ground == null)
        {
            // 创建Ground
            ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = Vector3.one * 10f;
            Debug.Log("✓ 已创建Ground对象");
        }
        
        Collider collider = ground.GetComponent<Collider>();
        if (collider == null)
        {
            ground.AddComponent<MeshCollider>();
            Debug.Log("✓ 已为Ground添加Collider");
        }
        else
        {
            Debug.Log("✓ Ground已有Collider");
        }
    }
    
    /// <summary>
    /// 修复所有预制体
    /// </summary>
    private void FixAllPrefabs()
    {
        if (!EditorUtility.DisplayDialog(
            "确认修复预制体",
            "将修复所有预制体的物理配置。\n\n" +
            "此操作会修改预制体文件，建议先备份。",
            "确定", "取消"))
        {
            return;
        }
        
        // 修复杠杆预制体
        string leverPath = "Assets/Prefabs/Generated/Lever.prefab";
        GameObject leverPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(leverPath);
        if (leverPrefab != null)
        {
            GameObject leverInstance = PrefabUtility.LoadPrefabContents(leverPath);
            BoxCollider collider = leverInstance.GetComponent<BoxCollider>();
            if (collider != null && collider.isTrigger)
            {
                collider.isTrigger = false;
                PrefabUtility.SaveAsPrefabAsset(leverInstance, leverPath);
                Debug.Log("✓ 已修复杠杆预制体的Collider");
            }
            PrefabUtility.UnloadPrefabContents(leverInstance);
        }
        
        // 修复齿轮预制体
        string gearPath = "Assets/Prefabs/Generated/Gear.prefab";
        GameObject gearPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(gearPath);
        if (gearPrefab != null)
        {
            GameObject gearInstance = PrefabUtility.LoadPrefabContents(gearPath);
            HingeJoint hinge = gearInstance.GetComponent<HingeJoint>();
            if (hinge != null && hinge.connectedBody == null)
            {
                // 创建锚点
                GameObject anchor = new GameObject("Anchor");
                anchor.transform.parent = gearInstance.transform;
                anchor.transform.localPosition = Vector3.zero;
                
                Rigidbody anchorRb = anchor.AddComponent<Rigidbody>();
                anchorRb.isKinematic = true;
                anchorRb.useGravity = false;
                
                hinge.connectedBody = anchorRb;
                hinge.anchor = Vector3.zero;
                
                PrefabUtility.SaveAsPrefabAsset(gearInstance, gearPath);
                Debug.Log("✓ 已修复齿轮预制体的HingeJoint");
            }
            PrefabUtility.UnloadPrefabContents(gearInstance);
        }
        
        AssetDatabase.Refresh();
        
        EditorUtility.DisplayDialog(
            "修复完成",
            "✓ 所有预制体已修复！\n\n" +
            "请重新生成场景中的对象以应用更改。",
            "确定"
        );
    }
}

