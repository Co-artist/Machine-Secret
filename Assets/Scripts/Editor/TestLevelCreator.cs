using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 测试关卡创建器 - 自动创建测试关卡
/// </summary>
public class TestLevelCreator : EditorWindow
{
    private Vector2 scrollPosition;
    private bool createInScene = true;
    private bool saveAsJson = true;
    private bool autoSetCamera = true;
    private string levelName = "测试关卡";
    private int levelId = 1;
    
    // 摄像机设置
    private float cameraHeight = 10f;      // 摄像机高度
    private float cameraDistance = 15f;      // 摄像机距离
    private float cameraAngle = 45f;         // 摄像机俯视角度
    
    // 预制体路径
    private string gearPrefabPath = "Assets/Prefabs/Generated/Gear.prefab";
    private string leverPrefabPath = "Assets/Prefabs/Generated/Lever.prefab";
    private string springPrefabPath = "Assets/Prefabs/Generated/Spring.prefab";
    
    [MenuItem("Tools/测试关卡创建器")]
    public static void ShowWindow()
    {
        GetWindow<TestLevelCreator>("测试关卡创建器");
    }
    
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("测试关卡创建器", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // 设置选项
        EditorGUILayout.LabelField("创建选项", EditorStyles.boldLabel);
        createInScene = EditorGUILayout.Toggle("在场景中创建", createInScene);
        saveAsJson = EditorGUILayout.Toggle("保存为JSON文件", saveAsJson);
        autoSetCamera = EditorGUILayout.Toggle("自动设置摄像机位置", autoSetCamera);
        
        // 摄像机设置（仅在自动设置时显示）
        if (autoSetCamera)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("摄像机设置", EditorStyles.boldLabel);
            cameraHeight = EditorGUILayout.Slider("摄像机高度", cameraHeight, 5f, 30f);
            cameraDistance = EditorGUILayout.Slider("摄像机距离", cameraDistance, 10f, 50f);
            cameraAngle = EditorGUILayout.Slider("俯视角度", cameraAngle, 20f, 80f);
        }
        
        EditorGUILayout.Space();
        
        // 关卡信息
        EditorGUILayout.LabelField("关卡信息", EditorStyles.boldLabel);
        levelName = EditorGUILayout.TextField("关卡名称", levelName);
        levelId = EditorGUILayout.IntField("关卡ID", levelId);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        // 预设关卡
        EditorGUILayout.LabelField("预设测试关卡", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("创建简单关卡", GUILayout.Height(30)))
        {
            CreateSimpleLevel();
        }
        if (GUILayout.Button("创建中等关卡", GUILayout.Height(30)))
        {
            CreateMediumLevel();
        }
        if (GUILayout.Button("创建困难关卡", GUILayout.Height(30)))
        {
            CreateHardLevel();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("创建齿轮测试关卡", GUILayout.Height(30)))
        {
            CreateGearTestLevel();
        }
        if (GUILayout.Button("创建杠杆测试关卡", GUILayout.Height(30)))
        {
            CreateLeverTestLevel();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("创建弹簧测试关卡", GUILayout.Height(30)))
        {
            CreateSpringTestLevel();
        }
        if (GUILayout.Button("创建综合测试关卡", GUILayout.Height(30)))
        {
            CreateComprehensiveTestLevel();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        // 批量创建
        EditorGUILayout.LabelField("批量创建", EditorStyles.boldLabel);
        if (GUILayout.Button("创建所有预设关卡", GUILayout.Height(40)))
        {
            CreateAllPresetLevels();
        }
        
        EditorGUILayout.Space();
        
        // 清理选项
        EditorGUILayout.LabelField("清理", EditorStyles.boldLabel);
        if (GUILayout.Button("清理场景中的测试关卡", GUILayout.Height(30)))
        {
            CleanupTestLevelsInScene();
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    /// <summary>
    /// 创建简单关卡
    /// </summary>
    private void CreateSimpleLevel()
    {
        LevelData levelData = new LevelData
        {
            version = "1.0",
            levelName = levelName + " - 简单",
            startPosition = new float[] { 0f, 2f, 0f },
            endPosition = new float[] { 10f, 0.5f, 0f },
            components = new List<ComponentData>()
        };
        
        // 添加一个齿轮
        levelData.components.Add(new ComponentData
        {
            type = "Gear",
            position = new float[] { 5f, 1f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        ProcessLevel(levelData, "简单");
    }
    
    /// <summary>
    /// 创建中等关卡
    /// </summary>
    private void CreateMediumLevel()
    {
        LevelData levelData = new LevelData
        {
            version = "1.0",
            levelName = levelName + " - 中等",
            startPosition = new float[] { 0f, 2f, 0f },
            endPosition = new float[] { 15f, 0.5f, 0f },
            components = new List<ComponentData>()
        };
        
        // 添加齿轮
        levelData.components.Add(new ComponentData
        {
            type = "Gear",
            position = new float[] { 5f, 1f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        // 添加杠杆
        levelData.components.Add(new ComponentData
        {
            type = "Lever",
            position = new float[] { 10f, 0.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        ProcessLevel(levelData, "中等");
    }
    
    /// <summary>
    /// 创建困难关卡
    /// </summary>
    private void CreateHardLevel()
    {
        LevelData levelData = new LevelData
        {
            version = "1.0",
            levelName = levelName + " - 困难",
            startPosition = new float[] { 0f, 2f, 0f },
            endPosition = new float[] { 20f, 0.5f, 0f },
            components = new List<ComponentData>()
        };
        
        // 添加多个齿轮
        levelData.components.Add(new ComponentData
        {
            type = "Gear",
            position = new float[] { 5f, 1f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        levelData.components.Add(new ComponentData
        {
            type = "Gear",
            position = new float[] { 10f, 1.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        // 添加杠杆
        levelData.components.Add(new ComponentData
        {
            type = "Lever",
            position = new float[] { 12f, 0.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        // 添加弹簧
        levelData.components.Add(new ComponentData
        {
            type = "Spring",
            position = new float[] { 15f, 0.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        ProcessLevel(levelData, "困难");
    }
    
    /// <summary>
    /// 创建齿轮测试关卡
    /// </summary>
    private void CreateGearTestLevel()
    {
        LevelData levelData = new LevelData
        {
            version = "1.0",
            levelName = levelName + " - 齿轮测试",
            startPosition = new float[] { 0f, 2f, 0f },
            endPosition = new float[] { 12f, 0.5f, 0f },
            components = new List<ComponentData>()
        };
        
        // 添加3个齿轮
        for (int i = 0; i < 3; i++)
        {
            levelData.components.Add(new ComponentData
            {
                type = "Gear",
                position = new float[] { 3f + i * 3f, 1f, 0f },
                rotation = new float[] { 0f, 0f, 0f },
                scale = new float[] { 1f, 1f, 1f }
            });
        }
        
        ProcessLevel(levelData, "齿轮测试");
    }
    
    /// <summary>
    /// 创建杠杆测试关卡
    /// </summary>
    private void CreateLeverTestLevel()
    {
        LevelData levelData = new LevelData
        {
            version = "1.0",
            levelName = levelName + " - 杠杆测试",
            startPosition = new float[] { 0f, 2f, 0f },
            endPosition = new float[] { 12f, 0.5f, 0f },
            components = new List<ComponentData>()
        };
        
        // 添加2个杠杆
        for (int i = 0; i < 2; i++)
        {
            levelData.components.Add(new ComponentData
            {
                type = "Lever",
                position = new float[] { 4f + i * 4f, 0.5f, 0f },
                rotation = new float[] { 0f, 0f, 0f },
                scale = new float[] { 1f, 1f, 1f }
            });
        }
        
        ProcessLevel(levelData, "杠杆测试");
    }
    
    /// <summary>
    /// 创建弹簧测试关卡
    /// </summary>
    private void CreateSpringTestLevel()
    {
        LevelData levelData = new LevelData
        {
            version = "1.0",
            levelName = levelName + " - 弹簧测试",
            startPosition = new float[] { 0f, 2f, 0f },
            endPosition = new float[] { 12f, 0.5f, 0f },
            components = new List<ComponentData>()
        };
        
        // 添加2个弹簧
        for (int i = 0; i < 2; i++)
        {
            levelData.components.Add(new ComponentData
            {
                type = "Spring",
                position = new float[] { 4f + i * 4f, 0.5f, 0f },
                rotation = new float[] { 0f, 0f, 0f },
                scale = new float[] { 1f, 1f, 1f }
            });
        }
        
        ProcessLevel(levelData, "弹簧测试");
    }
    
    /// <summary>
    /// 创建综合测试关卡
    /// </summary>
    private void CreateComprehensiveTestLevel()
    {
        LevelData levelData = new LevelData
        {
            version = "1.0",
            levelName = levelName + " - 综合测试",
            startPosition = new float[] { 0f, 2f, 0f },
            endPosition = new float[] { 25f, 0.5f, 0f },
            components = new List<ComponentData>()
        };
        
        // 齿轮1
        levelData.components.Add(new ComponentData
        {
            type = "Gear",
            position = new float[] { 5f, 1f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        // 杠杆1
        levelData.components.Add(new ComponentData
        {
            type = "Lever",
            position = new float[] { 8f, 0.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        // 弹簧1
        levelData.components.Add(new ComponentData
        {
            type = "Spring",
            position = new float[] { 12f, 0.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        // 齿轮2
        levelData.components.Add(new ComponentData
        {
            type = "Gear",
            position = new float[] { 15f, 1.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        // 杠杆2
        levelData.components.Add(new ComponentData
        {
            type = "Lever",
            position = new float[] { 18f, 0.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        // 弹簧2
        levelData.components.Add(new ComponentData
        {
            type = "Spring",
            position = new float[] { 22f, 0.5f, 0f },
            rotation = new float[] { 0f, 0f, 0f },
            scale = new float[] { 1f, 1f, 1f }
        });
        
        ProcessLevel(levelData, "综合测试");
    }
    
    /// <summary>
    /// 处理关卡（在场景中创建和/或保存为JSON）
    /// </summary>
    private void ProcessLevel(LevelData levelData, string suffix)
    {
        if (createInScene)
        {
            CreateLevelInScene(levelData, suffix);
        }
        
        if (saveAsJson)
        {
            SaveLevelAsJson(levelData, suffix);
        }
        
        Debug.Log($"✓ 测试关卡 '{levelData.levelName}' 创建完成！");
    }
    
    /// <summary>
    /// 在场景中创建关卡
    /// </summary>
    private void CreateLevelInScene(LevelData levelData, string suffix)
    {
        // 确保在Gameplay场景中
        if (EditorSceneManager.GetActiveScene().name != "Gameplay")
        {
            if (EditorUtility.DisplayDialog("切换场景", 
                "需要在Gameplay场景中创建关卡，是否切换到Gameplay场景？", 
                "是", "取消"))
            {
                string scenePath = "Assets/Scenes/Gameplay.unity";
                if (File.Exists(scenePath))
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
                else
                {
                    Debug.LogError($"场景文件不存在: {scenePath}");
                    return;
                }
            }
            else
            {
                return;
            }
        }
        
        // 创建关卡根对象
        GameObject levelRoot = new GameObject($"TestLevel_{suffix}_{levelId}");
        levelRoot.transform.position = Vector3.zero;
        
        // 创建起始点
        if (levelData.startPosition != null && levelData.startPosition.Length >= 3)
        {
            GameObject startPoint = new GameObject("StartPoint");
            startPoint.transform.parent = levelRoot.transform;
            startPoint.transform.position = new Vector3(
                levelData.startPosition[0],
                levelData.startPosition[1],
                levelData.startPosition[2]
            );
        }
        
        // 创建终点
        if (levelData.endPosition != null && levelData.endPosition.Length >= 3)
        {
            GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            goal.name = "Goal";
            goal.transform.parent = levelRoot.transform;
            goal.transform.position = new Vector3(
                levelData.endPosition[0],
                levelData.endPosition[1],
                levelData.endPosition[2]
            );
            goal.transform.localScale = new Vector3(1f, 1f, 1f);
            
            // 添加Goal标签（如果存在）
            if (LayerMask.NameToLayer("Goal") != -1)
            {
                goal.layer = LayerMask.NameToLayer("Goal");
            }
            
            // 添加触发器
            Collider collider = goal.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }
        
        // 创建组件
        foreach (var componentData in levelData.components)
        {
            GameObject componentObj = null;
            string prefabPath = "";
            
            switch (componentData.type)
            {
                case "Gear":
                    prefabPath = gearPrefabPath;
                    break;
                case "Lever":
                    prefabPath = leverPrefabPath;
                    break;
                case "Spring":
                    prefabPath = springPrefabPath;
                    break;
                default:
                    Debug.LogWarning($"未知的组件类型: {componentData.type}");
                    continue;
            }
            
            // 加载预制体
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                componentObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            }
            else
            {
                Debug.LogWarning($"预制体不存在: {prefabPath}，创建空对象");
                componentObj = new GameObject(componentData.type);
            }
            
            if (componentObj != null)
            {
                componentObj.transform.parent = levelRoot.transform;
                componentObj.name = $"{componentData.type}_{levelData.components.IndexOf(componentData)}";
                
                // 设置位置
                if (componentData.position != null && componentData.position.Length >= 3)
                {
                    componentObj.transform.position = new Vector3(
                        componentData.position[0],
                        componentData.position[1],
                        componentData.position[2]
                    );
                }
                
                // 设置旋转
                if (componentData.rotation != null && componentData.rotation.Length >= 3)
                {
                    componentObj.transform.rotation = Quaternion.Euler(
                        componentData.rotation[0],
                        componentData.rotation[1],
                        componentData.rotation[2]
                    );
                }
                
                // 设置缩放
                if (componentData.scale != null && componentData.scale.Length >= 3)
                {
                    componentObj.transform.localScale = new Vector3(
                        componentData.scale[0],
                        componentData.scale[1],
                        componentData.scale[2]
                    );
                }
            }
        }
        
        // 自动设置摄像机位置
        if (autoSetCamera)
        {
            SetCameraPosition(levelData);
        }
        
        // 标记场景为已修改
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        
        Debug.Log($"✓ 在场景中创建了测试关卡: {levelData.levelName}");
    }
    
    /// <summary>
    /// 设置摄像机位置
    /// </summary>
    private void SetCameraPosition(LevelData levelData)
    {
        // 查找场景中的主摄像机
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            // 如果没有主摄像机，查找所有摄像机
            Camera[] cameras = FindObjectsOfType<Camera>();
            if (cameras.Length > 0)
            {
                mainCamera = cameras[0];
            }
            else
            {
                // 如果没有摄像机，创建新的
                GameObject cameraObj = new GameObject("Main Camera");
                mainCamera = cameraObj.AddComponent<Camera>();
                cameraObj.tag = "MainCamera";
                cameraObj.AddComponent<AudioListener>();
                Debug.Log("创建了新的主摄像机");
            }
        }
        
        if (mainCamera == null)
        {
            Debug.LogWarning("无法找到或创建摄像机");
            return;
        }
        
        // 计算关卡的中心点和范围
        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.zero;
        
        if (levelData.startPosition != null && levelData.startPosition.Length >= 3)
        {
            startPos = new Vector3(
                levelData.startPosition[0],
                levelData.startPosition[1],
                levelData.startPosition[2]
            );
        }
        
        if (levelData.endPosition != null && levelData.endPosition.Length >= 3)
        {
            endPos = new Vector3(
                levelData.endPosition[0],
                levelData.endPosition[1],
                levelData.endPosition[2]
            );
        }
        
        // 计算关卡中心点
        Vector3 levelCenter = (startPos + endPos) / 2f;
        
        // 计算关卡范围
        float levelWidth = Mathf.Abs(endPos.x - startPos.x);
        float levelHeight = Mathf.Max(Mathf.Abs(endPos.y - startPos.y), 5f);
        
        // 根据关卡范围调整摄像机距离
        float adjustedDistance = Mathf.Max(cameraDistance, levelWidth * 0.8f);
        
        // 计算摄像机位置（在关卡中心的后上方）
        Vector3 cameraPosition = levelCenter;
        cameraPosition.y += cameraHeight;
        cameraPosition.z -= adjustedDistance;
        
        // 设置摄像机位置
        mainCamera.transform.position = cameraPosition;
        
        // 计算摄像机旋转（看向关卡中心）
        Vector3 direction = (levelCenter - cameraPosition).normalized;
        mainCamera.transform.rotation = Quaternion.LookRotation(direction);
        
        // 调整摄像机角度（俯视）
        Vector3 eulerAngles = mainCamera.transform.eulerAngles;
        eulerAngles.x = cameraAngle;
        mainCamera.transform.rotation = Quaternion.Euler(eulerAngles);
        
        // 调整视野以适应关卡（可选）
        if (levelWidth > 0)
        {
            // 使用正交摄像机或调整FOV
            if (mainCamera.orthographic)
            {
                // 正交摄像机：根据关卡宽度调整orthographicSize
                mainCamera.orthographicSize = Mathf.Max(levelWidth * 0.6f, 5f);
            }
            else
            {
                // 透视摄像机：根据距离和关卡宽度计算合适的FOV
                float requiredFOV = Mathf.Atan(levelWidth / (adjustedDistance * 2f)) * Mathf.Rad2Deg * 2f;
                mainCamera.fieldOfView = Mathf.Clamp(requiredFOV, 30f, 90f);
            }
        }
        
        // 选中摄像机以便查看
        Selection.activeGameObject = mainCamera.gameObject;
        SceneView.FrameLastActiveSceneView();
        
        Debug.Log($"✓ 摄像机已设置到位置: {cameraPosition}, 看向: {levelCenter}");
    }
    
    /// <summary>
    /// 保存关卡为JSON文件
    /// </summary>
    private void SaveLevelAsJson(LevelData levelData, string suffix)
    {
        // 确保Levels文件夹存在
        string levelsFolder = "Assets/Levels";
        if (!AssetDatabase.IsValidFolder(levelsFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Levels");
        }
        
        // 转换为JSON（Unity的JsonUtility不支持Dictionary，所以需要简化）
        // 创建一个简化的序列化版本
        string json = JsonUtility.ToJson(levelData, true);
        
        // 保存文件
        string fileName = $"TestLevel_{suffix}_{levelId}.json";
        string filePath = Path.Combine(levelsFolder, fileName);
        File.WriteAllText(filePath, json);
        
        AssetDatabase.Refresh();
        
        Debug.Log($"✓ 测试关卡已保存为JSON: {filePath}");
    }
    
    /// <summary>
    /// 创建所有预设关卡
    /// </summary>
    private void CreateAllPresetLevels()
    {
        int startId = levelId;
        
        levelId = startId;
        CreateSimpleLevel();
        
        levelId = startId + 1;
        CreateMediumLevel();
        
        levelId = startId + 2;
        CreateHardLevel();
        
        levelId = startId + 3;
        CreateGearTestLevel();
        
        levelId = startId + 4;
        CreateLeverTestLevel();
        
        levelId = startId + 5;
        CreateSpringTestLevel();
        
        levelId = startId + 6;
        CreateComprehensiveTestLevel();
        
        levelId = startId;
        
        Debug.Log($"✓ 已创建所有预设测试关卡（ID: {startId} - {startId + 6}）");
    }
    
    /// <summary>
    /// 清理场景中的测试关卡
    /// </summary>
    private void CleanupTestLevelsInScene()
    {
        if (!EditorUtility.DisplayDialog("确认清理", 
            "确定要删除场景中所有名称以'TestLevel_'开头的对象吗？", 
            "确定", "取消"))
        {
            return;
        }
        
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("TestLevel_"))
            {
                DestroyImmediate(obj);
                count++;
            }
        }
        
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log($"✓ 已清理 {count} 个测试关卡对象");
    }
}

