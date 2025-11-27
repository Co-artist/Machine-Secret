using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景配置器 - 自动配置游戏场景
/// 运行此工具可以自动创建和配置场景中的对象
/// </summary>
public class SceneConfigurator : EditorWindow
{
    [MenuItem("Tools/场景配置器")]
    public static void ShowWindow()
    {
        GetWindow<SceneConfigurator>("场景配置器");
    }
    
    void OnGUI()
    {
        GUILayout.Label("场景自动配置工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        if (GUILayout.Button("配置游戏场景", GUILayout.Height(30)))
        {
            ConfigureGameplayScene();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("配置编辑器场景", GUILayout.Height(30)))
        {
            ConfigureEditorScene();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("配置主菜单场景", GUILayout.Height(30)))
        {
            ConfigureMainMenuScene();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        if (GUILayout.Button("创建所有场景", GUILayout.Height(30)))
        {
            CreateAllScenes();
        }
    }
    
    /// <summary>
    /// 配置游戏场景
    /// </summary>
    private void ConfigureGameplayScene()
    {
        // 确保Scenes文件夹存在
        EnsureScenesFolderExists();
        
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        scene.name = "Gameplay";
        
        // 创建地面
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = Vector3.one * 10f;
        
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
        
        // 创建Canvas
        CreateGameCanvas();
        
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Gameplay.unity");
        Debug.Log("游戏场景已配置并保存！");
    }
    
    /// <summary>
    /// 配置编辑器场景
    /// </summary>
    private void ConfigureEditorScene()
    {
        // 确保Scenes文件夹存在
        EnsureScenesFolderExists();
        
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        scene.name = "LevelEditor";
        
        // 创建地面
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = Vector3.one * 20f;
        
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
        Debug.Log("编辑器场景已配置并保存！");
    }
    
    /// <summary>
    /// 配置主菜单场景
    /// </summary>
    private void ConfigureMainMenuScene()
    {
        // 确保Scenes文件夹存在
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
        Debug.Log("主菜单场景已配置并保存！");
    }
    
    /// <summary>
    /// 创建所有场景
    /// </summary>
    private void CreateAllScenes()
    {
        ConfigureMainMenuScene();
        ConfigureGameplayScene();
        ConfigureEditorScene();
        Debug.Log("所有场景已创建完成！");
    }
    
    /// <summary>
    /// 创建游戏Canvas
    /// </summary>
    private void CreateGameCanvas()
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
    private void CreateEditorCanvas()
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
    private void CreateMainMenuCanvas()
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
    private void EnsureScenesFolderExists()
    {
        string scenesPath = "Assets/Scenes";
        
        // 检查文件夹是否存在
        if (!AssetDatabase.IsValidFolder(scenesPath))
        {
            // 创建Scenes文件夹
            AssetDatabase.CreateFolder("Assets", "Scenes");
            AssetDatabase.Refresh();
            Debug.Log("已创建Scenes文件夹");
        }
    }
}

