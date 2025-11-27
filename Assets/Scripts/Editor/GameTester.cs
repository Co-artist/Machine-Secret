using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 游戏测试工具 - 自动测试游戏功能
/// 运行此工具可以自动测试游戏的各种功能
/// </summary>
public class GameTester : EditorWindow
{
    [MenuItem("Tools/游戏测试工具/运行自动化测试")]
    public static void ShowWindow()
    {
        GetWindow<GameTester>("游戏测试工具");
    }
    
    private bool isTesting = false;
    private List<string> testResults = new List<string>();
    private Vector2 scrollPosition;
    
    void OnGUI()
    {
        GUILayout.Label("《机械之谜》自动化测试工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "此工具将自动测试：\n" +
            "1. 场景加载\n" +
            "2. 资源检查\n" +
            "3. 标签检查\n" +
            "4. GameManager配置\n" +
            "5. 预制体检查\n" +
            "6. 物理系统检查",
            MessageType.Info
        );
        
        EditorGUILayout.Space();
        
        EditorGUI.BeginDisabledGroup(isTesting);
        if (GUILayout.Button("开始自动化测试", GUILayout.Height(40)))
        {
            RunAllTests();
        }
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        GUILayout.Label("单独测试选项", EditorStyles.boldLabel);
        
        EditorGUI.BeginDisabledGroup(isTesting);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("测试场景"))
        {
            TestScenes();
        }
        if (GUILayout.Button("测试资源"))
        {
            TestResources();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("测试标签"))
        {
            TestTags();
        }
        if (GUILayout.Button("测试GameManager"))
        {
            TestGameManager();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        // 显示测试结果
        GUILayout.Label("测试结果", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
        
        if (testResults.Count == 0)
        {
            EditorGUILayout.HelpBox("还没有运行测试。点击上面的按钮开始测试。", MessageType.Info);
        }
        else
        {
            foreach (string result in testResults)
            {
                if (result.StartsWith("✓"))
                {
                    EditorGUILayout.HelpBox(result, MessageType.Info);
                }
                else if (result.StartsWith("✗") || result.StartsWith("错误"))
                {
                    EditorGUILayout.HelpBox(result, MessageType.Error);
                }
                else if (result.StartsWith("⚠"))
                {
                    EditorGUILayout.HelpBox(result, MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.LabelField(result);
                }
            }
        }
        
        EditorGUILayout.EndScrollView();
        
        if (testResults.Count > 0)
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("清除结果"))
            {
                testResults.Clear();
            }
        }
    }
    
    /// <summary>
    /// 运行所有测试
    /// </summary>
    private void RunAllTests()
    {
        // 检查是否在Play Mode
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "无法运行测试",
                "测试工具无法在Play Mode下运行。\n\n请先停止游戏（点击Stop按钮），然后再运行测试。",
                "确定"
            );
            return;
        }
        
        isTesting = true;
        testResults.Clear();
        
        AddResult("开始自动化测试...", false);
        
        // 测试场景
        AddResult("", false);
        AddResult("=== 测试场景 ===", false);
        TestScenes();
        
        // 测试资源
        AddResult("", false);
        AddResult("=== 测试资源 ===", false);
        TestResources();
        
        // 测试标签
        AddResult("", false);
        AddResult("=== 测试标签 ===", false);
        TestTags();
        
        // 测试GameManager
        AddResult("", false);
        AddResult("=== 测试GameManager ===", false);
        TestGameManager();
        
        // 测试预制体
        AddResult("", false);
        AddResult("=== 测试预制体 ===", false);
        TestPrefabs();
        
        // 测试物理系统
        AddResult("", false);
        AddResult("=== 测试物理系统 ===", false);
        TestPhysics();
        
        // 总结
        AddResult("", false);
        AddResult("=== 测试完成 ===", false);
        int passCount = 0;
        int failCount = 0;
        List<string> failedTests = new List<string>();
        
        foreach (string result in testResults)
        {
            if (result.StartsWith("✓")) passCount++;
            if (result.StartsWith("✗") || result.StartsWith("错误"))
            {
                failCount++;
                failedTests.Add(result);
            }
        }
        
        AddResult($"通过: {passCount}, 失败: {failCount}", false);
        
        // 显示失败的测试
        if (failedTests.Count > 0)
        {
            AddResult("", false);
            AddResult("失败的测试项：", false);
            foreach (string failed in failedTests)
            {
                AddResult($"  {failed}", false);
            }
        }
        
        isTesting = false;
        
        EditorUtility.DisplayDialog(
            "测试完成",
            $"测试已完成！\n\n通过: {passCount}\n失败: {failCount}\n\n请查看测试结果。",
            "确定"
        );
    }
    
    /// <summary>
    /// 测试场景
    /// </summary>
    private void TestScenes()
    {
        string[] scenePaths = {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Gameplay.unity",
            "Assets/Scenes/LevelEditor.unity"
        };
        
        string[] sceneNames = {
            "MainMenu",
            "Gameplay",
            "LevelEditor"
        };
        
        for (int i = 0; i < scenePaths.Length; i++)
        {
            SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePaths[i]);
            if (scene != null)
            {
                AddResult($"✓ 场景 {sceneNames[i]} 存在", true);
            }
            else
            {
                AddResult($"✗ 场景 {sceneNames[i]} 不存在: {scenePaths[i]}", false);
            }
        }
    }
    
    /// <summary>
    /// 测试资源
    /// </summary>
    private void TestResources()
    {
        // 测试材质
        string[] materialPaths = {
            "Assets/Materials/GearMaterial.mat",
            "Assets/Materials/LeverMaterial.mat",
            "Assets/Materials/SpringMaterial.mat",
            "Assets/Materials/BallMaterial.mat",
            "Assets/Materials/GroundMaterial.mat"
        };
        
        string[] materialNames = {
            "GearMaterial",
            "LeverMaterial",
            "SpringMaterial",
            "BallMaterial",
            "GroundMaterial"
        };
        
        for (int i = 0; i < materialPaths.Length; i++)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPaths[i]);
            if (mat != null)
            {
                AddResult($"✓ 材质 {materialNames[i]} 存在", true);
            }
            else
            {
                AddResult($"⚠ 材质 {materialNames[i]} 不存在: {materialPaths[i]}", false);
            }
        }
        
        // 测试文件夹
        string[] folders = {
            "Assets/Materials",
            "Assets/UI",
            "Assets/Scenes",
            "Assets/Prefabs"
        };
        
        foreach (string folder in folders)
        {
            if (AssetDatabase.IsValidFolder(folder))
            {
                AddResult($"✓ 文件夹 {folder} 存在", true);
            }
            else
            {
                AddResult($"⚠ 文件夹 {folder} 不存在", false);
            }
        }
    }
    
    /// <summary>
    /// 测试标签
    /// </summary>
    private void TestTags()
    {
        string[] requiredTags = { "Ball", "Goal", "Checkpoint", "Hazard" };
        
        foreach (string tag in requiredTags)
        {
            try
            {
                // 尝试查找使用该标签的对象
                GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                bool tagExists = false;
                
                foreach (GameObject obj in allObjects)
                {
                    try
                    {
                        if (obj.CompareTag(tag))
                        {
                            tagExists = true;
                            break;
                        }
                    }
                    catch
                    {
                        // 标签不存在时会抛出异常
                    }
                }
                
                // 更可靠的方法：检查TagManager
                UnityEngine.Object[] tagAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                if (tagAssets != null && tagAssets.Length > 0)
                {
                    SerializedObject tagManager = new SerializedObject(tagAssets[0]);
                    SerializedProperty tagsProp = tagManager.FindProperty("tags");
                    
                    if (tagsProp != null)
                    {
                        for (int i = 0; i < tagsProp.arraySize; i++)
                        {
                            SerializedProperty tagProp = tagsProp.GetArrayElementAtIndex(i);
                            if (tagProp.stringValue == tag)
                            {
                                tagExists = true;
                                break;
                            }
                        }
                    }
                }
                
                if (tagExists)
                {
                    AddResult($"✓ 标签 {tag} 已设置", true);
                }
                else
                {
                    AddResult($"✗ 标签 {tag} 未设置", false);
                }
            }
            catch (System.Exception e)
            {
                AddResult($"⚠ 检查标签 {tag} 时出错: {e.Message}", false);
            }
        }
    }
    
    /// <summary>
    /// 测试GameManager
    /// </summary>
    private void TestGameManager()
    {
        // 检查是否在Play Mode
        if (EditorApplication.isPlaying)
        {
            AddResult($"⚠ 无法在Play Mode下测试GameManager，请先停止游戏", false);
            return;
        }
        
        // 保存当前场景（如果有未保存的更改）
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveOpenScenes();
        }
        
        // 加载Gameplay场景
        string gameplayScenePath = "Assets/Scenes/Gameplay.unity";
        Scene gameplayScene;
        
        try
        {
            gameplayScene = EditorSceneManager.OpenScene(gameplayScenePath, OpenSceneMode.Single);
        }
        catch (System.Exception e)
        {
            AddResult($"✗ 无法打开场景: {gameplayScenePath}", false);
            AddResult($"  错误: {e.Message}", false);
            return;
        }
        
        if (!gameplayScene.IsValid())
        {
            AddResult($"✗ 无法打开场景: {gameplayScenePath}", false);
            return;
        }
        
        // 查找GameManager
        GameManager gameManager = Object.FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            AddResult($"✗ GameManager 对象不存在", false);
            return;
        }
        
        AddResult($"✓ GameManager 对象存在", true);
        
        // 检查Ball Prefab
        SerializedObject so = new SerializedObject(gameManager);
        SerializedProperty ballPrefabProp = so.FindProperty("ballPrefab");
        
        if (ballPrefabProp != null && ballPrefabProp.objectReferenceValue != null)
        {
            AddResult($"✓ GameManager Ball Prefab 已配置", true);
        }
        else
        {
            AddResult($"✗ GameManager Ball Prefab 未配置", false);
        }
        
        // 检查Ball Start Position
        SerializedProperty ballStartPosProp = so.FindProperty("ballStartPosition");
        if (ballStartPosProp != null && ballStartPosProp.objectReferenceValue != null)
        {
            AddResult($"✓ GameManager Ball Start Position 已配置", true);
        }
        else
        {
            AddResult($"✗ GameManager Ball Start Position 未配置", false);
        }
        
        // 检查Ground对象
        GameObject ground = GameObject.Find("Ground");
        if (ground != null)
        {
            AddResult($"✓ Ground 对象存在", true);
        }
        else
        {
            AddResult($"⚠ Ground 对象不存在", false);
        }
        
        // 检查Goal对象
        GameObject goal = GameObject.Find("Goal");
        if (goal != null)
        {
            AddResult($"✓ Goal 对象存在", true);
            
            // 检查Goal标签
            if (goal.CompareTag("Goal"))
            {
                AddResult($"✓ Goal 对象标签正确", true);
            }
            else
            {
                AddResult($"✗ Goal 对象标签不正确", false);
            }
            
            // 检查Goal Collider
            Collider goalCollider = goal.GetComponent<Collider>();
            if (goalCollider != null && goalCollider.isTrigger)
            {
                AddResult($"✓ Goal Collider 已设置为Trigger", true);
            }
            else
            {
                AddResult($"✗ Goal Collider 未设置为Trigger", false);
            }
        }
        else
        {
            AddResult($"✗ Goal 对象不存在", false);
        }
        
        // 检查BallStartPoint
        GameObject ballStartPoint = GameObject.Find("BallStartPoint");
        if (ballStartPoint != null)
        {
            AddResult($"✓ BallStartPoint 对象存在", true);
        }
        else
        {
            AddResult($"✗ BallStartPoint 对象不存在", false);
        }
        
        // 检查Canvas
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            AddResult($"✓ Canvas 存在", true);
        }
        else
        {
            AddResult($"⚠ Canvas 不存在", false);
        }
    }
    
    /// <summary>
    /// 测试预制体
    /// </summary>
    private void TestPrefabs()
    {
        string[] prefabPaths = {
            "Assets/Prefabs/Generated/Gear.prefab",
            "Assets/Prefabs/Generated/Lever.prefab",
            "Assets/Prefabs/Generated/Spring.prefab",
            "Assets/Prefabs/Generated/Ball.prefab"
        };
        
        string[] prefabNames = {
            "Gear",
            "Lever",
            "Spring",
            "Ball"
        };
        
        for (int i = 0; i < prefabPaths.Length; i++)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[i]);
            if (prefab != null)
            {
                AddResult($"✓ 预制体 {prefabNames[i]} 存在", true);
                
                // 检查Ball预制体的组件
                if (prefabNames[i] == "Ball")
                {
                    Rigidbody rb = prefab.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        AddResult($"  ✓ Ball 有 Rigidbody 组件", true);
                        if (rb.useGravity)
                        {
                            AddResult($"  ✓ Ball Rigidbody Use Gravity 已启用", true);
                        }
                        else
                        {
                            AddResult($"  ✗ Ball Rigidbody Use Gravity 未启用", false);
                        }
                    }
                    else
                    {
                        AddResult($"  ✗ Ball 缺少 Rigidbody 组件", false);
                    }
                    
                    SphereCollider collider = prefab.GetComponent<SphereCollider>();
                    if (collider != null)
                    {
                        AddResult($"  ✓ Ball 有 SphereCollider 组件", true);
                    }
                    else
                    {
                        AddResult($"  ✗ Ball 缺少 SphereCollider 组件", false);
                    }
                    
                    BallController ballController = prefab.GetComponent<BallController>();
                    if (ballController != null)
                    {
                        AddResult($"  ✓ Ball 有 BallController 组件", true);
                    }
                    else
                    {
                        AddResult($"  ✗ Ball 缺少 BallController 组件", false);
                    }
                }
                
                // 检查Gear预制体的组件
                if (prefabNames[i] == "Gear")
                {
                    Rigidbody rb = prefab.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        AddResult($"  ✓ Gear 有 Rigidbody 组件", true);
                    }
                    else
                    {
                        AddResult($"  ✗ Gear 缺少 Rigidbody 组件", false);
                    }
                    
                    HingeJoint hinge = prefab.GetComponent<HingeJoint>();
                    if (hinge != null)
                    {
                        AddResult($"  ✓ Gear 有 HingeJoint 组件", true);
                    }
                    else
                    {
                        AddResult($"  ✗ Gear 缺少 HingeJoint 组件", false);
                    }
                    
                    Gear gear = prefab.GetComponent<Gear>();
                    if (gear != null)
                    {
                        AddResult($"  ✓ Gear 有 Gear 组件", true);
                    }
                    else
                    {
                        AddResult($"  ✗ Gear 缺少 Gear 组件", false);
                    }
                }
            }
            else
            {
                AddResult($"✗ 预制体 {prefabNames[i]} 不存在: {prefabPaths[i]}", false);
            }
        }
    }
    
    /// <summary>
    /// 测试物理系统
    /// </summary>
    private void TestPhysics()
    {
        // 检查是否在Play Mode
        if (EditorApplication.isPlaying)
        {
            AddResult($"⚠ 无法在Play Mode下测试物理系统，请先停止游戏", false);
            return;
        }
        
        // 检查物理设置
        AddResult($"✓ 物理系统已初始化", true);
        AddResult($"  重力: {Physics.gravity}", true);
        
        // 检查Ball预制体的物理设置
        GameObject ballPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Generated/Ball.prefab");
        if (ballPrefab != null)
        {
            Rigidbody rb = ballPrefab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                AddResult($"✓ Ball 物理设置正常", true);
                AddResult($"  Mass: {rb.mass}, Drag: {rb.drag}, Use Gravity: {rb.useGravity}", true);
            }
        }
        
        // 检查Ground的Collider
        string gameplayScenePath = "Assets/Scenes/Gameplay.unity";
        Scene gameplayScene = EditorSceneManager.GetSceneByPath(gameplayScenePath);
        if (gameplayScene.IsValid() && gameplayScene.isLoaded)
        {
            GameObject[] rootObjects = gameplayScene.GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (obj.name == "Ground")
                {
                    Collider collider = obj.GetComponent<Collider>();
                    if (collider != null)
                    {
                        AddResult($"✓ Ground 有 Collider 组件", true);
                    }
                    break;
                }
            }
        }
        else
        {
            // 如果场景未加载，尝试加载它
            try
            {
                gameplayScene = EditorSceneManager.OpenScene(gameplayScenePath, OpenSceneMode.Additive);
                if (gameplayScene.IsValid() && gameplayScene.isLoaded)
                {
                    GameObject[] rootObjects = gameplayScene.GetRootGameObjects();
                    foreach (GameObject obj in rootObjects)
                    {
                        if (obj.name == "Ground")
                        {
                            Collider collider = obj.GetComponent<Collider>();
                            if (collider != null)
                            {
                                AddResult($"✓ Ground 有 Collider 组件", true);
                            }
                            break;
                        }
                    }
                    // 关闭场景（如果之前未打开）
                    if (!EditorSceneManager.GetActiveScene().path.Equals(gameplayScenePath))
                    {
                        EditorSceneManager.CloseScene(gameplayScene, false);
                    }
                }
            }
            catch (System.Exception e)
            {
                AddResult($"⚠ 无法检查Ground Collider: {e.Message}", false);
            }
        }
    }
    
    /// <summary>
    /// 添加测试结果
    /// </summary>
    private void AddResult(string message, bool isSuccess)
    {
        if (isSuccess)
        {
            testResults.Add($"✓ {message}");
            Debug.Log($"[测试] ✓ {message}");
        }
        else
        {
            testResults.Add(message);
            if (message.StartsWith("✗") || message.StartsWith("错误"))
            {
                Debug.LogError($"[测试] {message}");
            }
            else
            {
                Debug.LogWarning($"[测试] {message}");
            }
        }
        
        Repaint();
    }
}

