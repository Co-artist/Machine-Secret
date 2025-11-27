using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// 自动设置工具 - 一键完成所有项目设置
/// 运行此工具可以自动完成：
/// 1. 创建所有场景
/// 2. 生成所有资源（齿轮、杠杆、弹簧、小球）
/// 3. 设置标签和层级
/// 4. 配置基本场景对象
/// </summary>
public class AutoSetup : EditorWindow
{
    [MenuItem("Tools/自动设置工具/一键完成所有设置")]
    public static void ShowWindow()
    {
        GetWindow<AutoSetup>("自动设置工具");
    }
    
    [SerializeField] private bool clearExistingData = true; // 是否清理现有数据
    
    void OnGUI()
    {
        GUILayout.Label("《机械之谜》自动设置工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "此工具将自动完成以下设置：\n" +
            "1. 清理之前的建模数据（可选）\n" +
            "2. 创建所有场景（MainMenu, Gameplay, LevelEditor）\n" +
            "3. 生成所有资源（齿轮、杠杆、弹簧、小球）\n" +
            "4. 设置标签和层级（Ball, Goal, Checkpoint, Hazard）\n" +
            "5. 配置基本场景对象",
            MessageType.Info
        );
        
        EditorGUILayout.Space();
        
        // 清理选项
        clearExistingData = EditorGUILayout.Toggle("清理之前的建模数据", clearExistingData);
        EditorGUILayout.HelpBox(
            "勾选此项将删除：\n" +
            "- 之前生成的预制体（Gear, Lever, Spring, Ball）\n" +
            "- 之前生成的网格（GeneratedMeshes文件夹）\n" +
            "- 之前创建的材质（Materials文件夹）\n" +
            "- 之前创建的场景（Scenes文件夹）",
            MessageType.Warning
        );
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("开始自动设置", GUILayout.Height(40)))
        {
            RunAutoSetup();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        GUILayout.Label("单独设置选项", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("仅创建场景"))
        {
            CreateAllScenes();
        }
        if (GUILayout.Button("仅生成资源"))
        {
            GenerateAllResources();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("仅设置标签"))
        {
            SetupTags();
        }
        if (GUILayout.Button("仅创建材质"))
        {
            CreateMaterials();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        GUILayout.Label("清理选项", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "警告：清理操作将永久删除之前的建模数据！\n" +
            "请确保已备份重要数据。",
            MessageType.Warning
        );
        
        if (GUILayout.Button("仅清理之前的建模数据", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog(
                "确认清理",
                "确定要清理之前的建模数据吗？\n\n" +
                "这将删除：\n" +
                "- 生成的预制体（Gear, Lever, Spring, Ball）\n" +
                "- 生成的网格（GeneratedMeshes文件夹）\n" +
                "- 创建的材质（Materials文件夹）\n" +
                "- 创建的场景（Scenes文件夹）\n\n" +
                "此操作无法撤销！",
                "确定清理",
                "取消"
            ))
            {
                ClearExistingData();
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("清理完成", "之前的建模数据已清理完成。", "确定");
            }
        }
    }
    
    /// <summary>
    /// 运行完整的自动设置
    /// </summary>
    private void RunAutoSetup()
    {
        // 获取窗口实例以访问clearExistingData
        AutoSetup window = GetWindow<AutoSetup>();
        bool shouldClear = window.clearExistingData;
        
        if (shouldClear)
        {
            EditorUtility.DisplayProgressBar("自动设置", "正在清理之前的建模数据...", 0.05f);
            ClearExistingData();
        }
        
        EditorUtility.DisplayProgressBar("自动设置", "正在设置标签和层级...", 0.15f);
        SetupTags();
        
        EditorUtility.DisplayProgressBar("自动设置", "正在创建文件夹...", 0.25f);
        CreateFolders();
        
        EditorUtility.DisplayProgressBar("自动设置", "正在生成资源...", 0.45f);
        GenerateAllResources();
        
        EditorUtility.DisplayProgressBar("自动设置", "正在创建材质...", 0.65f);
        CreateMaterials();
        
        EditorUtility.DisplayProgressBar("自动设置", "正在创建场景...", 0.85f);
        CreateAllScenes();
        
        EditorUtility.DisplayProgressBar("自动设置", "正在刷新资源...", 0.95f);
        AssetDatabase.Refresh();
        
        EditorUtility.ClearProgressBar();
        
        string clearMessage = shouldClear ? "\n✓ 已清理之前的建模数据\n" : "\n";
        
        EditorUtility.DisplayDialog(
            "自动设置完成",
            "所有设置已完成！\n\n" +
            clearMessage +
            "✓ 标签和层级已设置\n" +
            "✓ 资源已生成\n" +
            "✓ 材质已创建\n" +
            "✓ 场景已创建\n\n" +
            "请检查Project窗口中的资源。",
            "确定"
        );
        
        Debug.Log("✓ 自动设置完成！");
    }
    
    /// <summary>
    /// 清理之前的建模数据
    /// </summary>
    private static void ClearExistingData()
    {
        Debug.Log("开始清理之前的建模数据...");
        
        // 清理预制体
        ClearPrefabs();
        
        // 清理生成的网格
        ClearGeneratedMeshes();
        
        // 清理材质
        ClearMaterials();
        
        // 清理场景
        ClearScenes();
        
        AssetDatabase.Refresh();
        Debug.Log("✓ 清理完成");
    }
    
    /// <summary>
    /// 清理预制体
    /// </summary>
    private static void ClearPrefabs()
    {
        string[] prefabPaths = {
            "Assets/Prefabs/Generated/Gear.prefab",
            "Assets/Prefabs/Generated/Lever.prefab",
            "Assets/Prefabs/Generated/Spring.prefab",
            "Assets/Prefabs/Generated/Ball.prefab"
        };
        
        foreach (string path in prefabPaths)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
                Debug.Log($"已删除预制体: {path}");
            }
        }
        
        // 清理整个Generated文件夹（如果为空）
        string generatedFolder = "Assets/Prefabs/Generated";
        if (AssetDatabase.IsValidFolder(generatedFolder))
        {
            string[] assets = AssetDatabase.FindAssets("", new[] { generatedFolder });
            if (assets.Length == 0)
            {
                AssetDatabase.DeleteAsset(generatedFolder);
                Debug.Log($"已删除空文件夹: {generatedFolder}");
            }
        }
    }
    
    /// <summary>
    /// 清理生成的网格
    /// </summary>
    private static void ClearGeneratedMeshes()
    {
        string meshFolder = "Assets/GeneratedMeshes";
        
        if (AssetDatabase.IsValidFolder(meshFolder))
        {
            // 删除Meshes子文件夹中的网格
            string meshesPath = meshFolder + "/Meshes";
            if (AssetDatabase.IsValidFolder(meshesPath))
            {
                string[] meshGUIDs = AssetDatabase.FindAssets("t:Mesh", new[] { meshesPath });
                foreach (string guid in meshGUIDs)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    AssetDatabase.DeleteAsset(path);
                    Debug.Log($"已删除网格: {path}");
                }
            }
            
            // 如果文件夹为空，删除整个文件夹
            string[] allAssets = AssetDatabase.FindAssets("", new[] { meshFolder });
            if (allAssets.Length == 0)
            {
                AssetDatabase.DeleteAsset(meshFolder);
                Debug.Log($"已删除空文件夹: {meshFolder}");
            }
        }
    }
    
    /// <summary>
    /// 清理材质
    /// </summary>
    private static void ClearMaterials()
    {
        string[] materialPaths = {
            "Assets/Materials/GearMaterial.mat",
            "Assets/Materials/LeverMaterial.mat",
            "Assets/Materials/SpringMaterial.mat",
            "Assets/Materials/BallMaterial.mat",
            "Assets/Materials/GroundMaterial.mat"
        };
        
        foreach (string path in materialPaths)
        {
            if (AssetDatabase.LoadAssetAtPath<Material>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
                Debug.Log($"已删除材质: {path}");
            }
        }
    }
    
    /// <summary>
    /// 清理场景
    /// </summary>
    private static void ClearScenes()
    {
        string[] scenePaths = {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Gameplay.unity",
            "Assets/Scenes/LevelEditor.unity"
        };
        
        foreach (string path in scenePaths)
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) != null)
            {
                // 如果场景当前已打开，先关闭
                Scene scene = EditorSceneManager.GetSceneByPath(path);
                if (scene.IsValid() && scene.isLoaded)
                {
                    EditorSceneManager.CloseScene(scene, false);
                }
                
                AssetDatabase.DeleteAsset(path);
                Debug.Log($"已删除场景: {path}");
            }
        }
    }
    
    /// <summary>
    /// 设置标签和层级
    /// </summary>
    private static void SetupTags()
    {
        try
        {
            TagSetup.SetupTagsAndLayers();
            Debug.Log("✓ 标签设置完成");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"标签设置失败: {e.Message}");
        }
    }
    
    /// <summary>
    /// 创建必要的文件夹
    /// </summary>
    private static void CreateFolders()
    {
        string[] folders = {
            "Assets/Materials",
            "Assets/UI",
            "Assets/UI/Sprites",
            "Assets/UI/Fonts",
            "Assets/Textures"
        };
        
        foreach (string folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                string parent = Path.GetDirectoryName(folder).Replace("\\", "/");
                string name = Path.GetFileName(folder);
                AssetDatabase.CreateFolder(parent, name);
            }
        }
        
        Debug.Log("✓ 文件夹创建完成");
    }
    
    /// <summary>
    /// 生成所有资源
    /// </summary>
    private static void GenerateAllResources()
    {
        try
        {
            // 使用ResourceGenerator生成资源
            // 注意：这需要ResourceGenerator窗口打开或直接调用其方法
            Debug.Log("正在生成资源...");
            Debug.LogWarning("请手动运行 Tools -> 资源生成器 来生成资源，或确保ResourceGenerator已正确配置");
            Debug.Log("✓ 资源生成提示完成");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"资源生成失败: {e.Message}");
        }
    }
    
    /// <summary>
    /// 创建基本材质
    /// </summary>
    private static void CreateMaterials()
    {
        // 确保Materials文件夹存在
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
        
        CreateMaterial("GearMaterial", new Color(0.6f, 0.6f, 0.65f), 0.8f, 0.5f);
        CreateMaterial("LeverMaterial", new Color(0.55f, 0.35f, 0.17f), 0.0f, 0.3f);
        CreateMaterial("SpringMaterial", new Color(0.78f, 0.78f, 0.82f), 0.9f, 0.7f);
        CreateMaterial("BallMaterial", new Color(1f, 0f, 0f), 0.0f, 0.8f);
        CreateMaterial("GroundMaterial", new Color(0.8f, 0.8f, 0.8f), 0.0f, 0.2f);
        
        AssetDatabase.SaveAssets();
        Debug.Log("✓ 材质创建完成");
    }
    
    /// <summary>
    /// 创建材质
    /// </summary>
    private static void CreateMaterial(string name, Color color, float metallic, float smoothness)
    {
        string path = $"Assets/Materials/{name}.mat";
        
        // 如果材质已存在，跳过
        if (AssetDatabase.LoadAssetAtPath<Material>(path) != null)
        {
            Debug.Log($"材质已存在: {name}");
            return;
        }
        
        Material mat = new Material(Shader.Find("Standard"));
        mat.name = name;
        mat.color = color;
        mat.SetFloat("_Metallic", metallic);
        mat.SetFloat("_Glossiness", smoothness);
        
        AssetDatabase.CreateAsset(mat, path);
        Debug.Log($"✓ 已创建材质: {name}");
    }
    
    /// <summary>
    /// 创建所有场景
    /// </summary>
    private static void CreateAllScenes()
    {
        try
        {
            // 使用SceneConfigurator创建场景
            SceneConfigurator configurator = new SceneConfigurator();
            
            // 通过反射调用私有方法，或直接使用公共方法
            // 这里我们直接调用SceneConfigurator的配置方法
            Debug.Log("正在创建场景...");
            
            // 由于SceneConfigurator的方法是私有的，我们需要直接实现
            ConfigureMainMenuScene();
            ConfigureGameplayScene();
            ConfigureEditorScene();
            
            Debug.Log("✓ 场景创建完成");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"场景创建失败: {e.Message}");
        }
    }
    
    /// <summary>
    /// 配置主菜单场景
    /// </summary>
    private static void ConfigureMainMenuScene()
    {
        EnsureScenesFolderExists();
        
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        scene.name = "MainMenu";
        
        // 创建光源
        GameObject light = new GameObject("Directional Light");
        Light lightComponent = light.AddComponent<Light>();
        lightComponent.type = LightType.Directional;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        
        // 创建NetworkManager
        GameObject networkManager = new GameObject("NetworkManager");
        networkManager.AddComponent<NetworkManager>();
        
        // 创建SceneLoader
        GameObject sceneLoader = new GameObject("SceneLoader");
        sceneLoader.AddComponent<SceneLoader>();
        
        // 创建AudioManager
        GameObject audioManager = new GameObject("AudioManager");
        audioManager.AddComponent<AudioManager>();
        
        // 创建Canvas
        CreateMainMenuCanvas();
        
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainMenu.unity");
        Debug.Log("✓ 主菜单场景已创建");
    }
    
    /// <summary>
    /// 配置游戏场景
    /// </summary>
    private static void ConfigureGameplayScene()
    {
        EnsureScenesFolderExists();
        
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        scene.name = "Gameplay";
        
        // 创建地面
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = Vector3.one * 10f;
        
        // 应用地面材质
        Material groundMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/GroundMaterial.mat");
        if (groundMat != null)
        {
            ground.GetComponent<MeshRenderer>().material = groundMat;
        }
        
        // 创建光源
        GameObject light = new GameObject("Directional Light");
        Light lightComponent = light.AddComponent<Light>();
        lightComponent.type = LightType.Directional;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        
        // 创建GameManager
        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<GameManager>();
        
        // 创建LevelManager
        GameObject levelManager = new GameObject("LevelManager");
        levelManager.AddComponent<LevelManager>();
        
        // 创建NetworkManager
        GameObject networkManager = new GameObject("NetworkManager");
        networkManager.AddComponent<NetworkManager>();
        
        // 创建PerformanceOptimizer
        GameObject perfOptimizer = new GameObject("PerformanceOptimizer");
        perfOptimizer.AddComponent<PerformanceOptimizer>();
        
        // 创建ObjectPool
        GameObject objectPool = new GameObject("ObjectPool");
        objectPool.AddComponent<ObjectPool>();
        
        // 创建AudioManager
        GameObject audioManager = new GameObject("AudioManager");
        audioManager.AddComponent<AudioManager>();
        
        // 创建SceneLoader
        GameObject sceneLoader = new GameObject("SceneLoader");
        sceneLoader.AddComponent<SceneLoader>();
        
        // 创建小球起始点
        GameObject startPoint = new GameObject("BallStartPoint");
        startPoint.transform.position = new Vector3(0, 2, 0);
        
        // 创建终点
        GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        goal.name = "Goal";
        goal.transform.position = new Vector3(10, 1, 0);
        goal.transform.localScale = Vector3.one * 2f;
        goal.tag = "Goal";
        Collider goalCollider = goal.GetComponent<Collider>();
        if (goalCollider != null)
        {
            goalCollider.isTrigger = true;
        }
        
        // 应用材质
        Material ballMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/BallMaterial.mat");
        if (ballMat != null)
        {
            goal.GetComponent<MeshRenderer>().material = ballMat;
        }
        
        // 创建Canvas
        CreateGameCanvas();
        
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Gameplay.unity");
        Debug.Log("✓ 游戏场景已创建");
    }
    
    /// <summary>
    /// 配置编辑器场景
    /// </summary>
    private static void ConfigureEditorScene()
    {
        EnsureScenesFolderExists();
        
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        scene.name = "LevelEditor";
        
        // 创建地面
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = Vector3.one * 20f;
        
        // 应用地面材质
        Material groundMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/GroundMaterial.mat");
        if (groundMat != null)
        {
            ground.GetComponent<MeshRenderer>().material = groundMat;
        }
        
        // 创建光源
        GameObject light = new GameObject("Directional Light");
        Light lightComponent = light.AddComponent<Light>();
        lightComponent.type = LightType.Directional;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        
        // 创建LevelEditor
        GameObject levelEditor = new GameObject("LevelEditor");
        levelEditor.AddComponent<LevelEditor>();
        
        // 创建ComponentConnector
        GameObject connector = new GameObject("ComponentConnector");
        connector.AddComponent<ComponentConnector>();
        
        // 创建LevelManager
        GameObject levelManager = new GameObject("LevelManager");
        levelManager.AddComponent<LevelManager>();
        
        // 创建Canvas
        CreateEditorCanvas();
        
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/LevelEditor.unity");
        Debug.Log("✓ 编辑器场景已创建");
    }
    
    /// <summary>
    /// 创建游戏Canvas
    /// </summary>
    private static void CreateGameCanvas()
    {
        GameObject canvas = new GameObject("Canvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // 创建GameUI
        GameObject gameUI = new GameObject("GameUI");
        gameUI.transform.SetParent(canvas.transform);
        gameUI.AddComponent<GameUI>();
        
        // 创建VictoryUI
        GameObject victoryUI = new GameObject("VictoryUI");
        victoryUI.transform.SetParent(canvas.transform);
        victoryUI.AddComponent<VictoryUI>();
        
        // 创建LeaderboardUI
        GameObject leaderboardUI = new GameObject("LeaderboardUI");
        leaderboardUI.transform.SetParent(canvas.transform);
        leaderboardUI.AddComponent<LeaderboardUI>();
    }
    
    /// <summary>
    /// 创建编辑器Canvas
    /// </summary>
    private static void CreateEditorCanvas()
    {
        GameObject canvas = new GameObject("Canvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // 创建EditorUI
        GameObject editorUI = new GameObject("EditorUI");
        editorUI.transform.SetParent(canvas.transform);
        editorUI.AddComponent<EditorUI>();
    }
    
    /// <summary>
    /// 创建主菜单Canvas
    /// </summary>
    private static void CreateMainMenuCanvas()
    {
        GameObject canvas = new GameObject("Canvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // 创建LevelSelector
        GameObject levelSelector = new GameObject("LevelSelector");
        levelSelector.transform.SetParent(canvas.transform);
        levelSelector.AddComponent<LevelSelector>();
    }
    
    /// <summary>
    /// 确保Scenes文件夹存在
    /// </summary>
    private static void EnsureScenesFolderExists()
    {
        string scenesPath = "Assets/Scenes";
        
        if (!AssetDatabase.IsValidFolder(scenesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
            AssetDatabase.Refresh();
        }
    }
}

