using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 关卡编辑器 - 可视化关卡设计工具
/// </summary>
public class LevelEditor : MonoBehaviour
{
    [Header("部件库")]
    [SerializeField] private GameObject[] componentPrefabs; // 所有可放置的部件预制体
    [SerializeField] private Transform componentLibraryUI; // 部件库UI容器
    
    [Header("编辑设置")]
    [SerializeField] private LayerMask placementLayer; // 放置层
    [SerializeField] private float gridSize = 1f; // 网格大小
    [SerializeField] private bool snapToGrid = true; // 是否对齐网格
    
    [Header("撤销/重做")]
    [SerializeField] private int maxHistorySize = 50; // 最大历史记录数
    
    private GameObject selectedPrefab;
    private GameObject previewObject;
    private Camera editorCamera;
    private List<GameObject> placedObjects = new List<GameObject>();
    private Stack<EditorAction> undoStack = new Stack<EditorAction>();
    private Stack<EditorAction> redoStack = new Stack<EditorAction>();
    private bool isPlacing = false;
    
    // 编辑器状态
    public enum EditorMode
    {
        Place,      // 放置模式
        Select,     // 选择模式
        Connect,    // 连接模式
        Delete      // 删除模式
    }
    
    private EditorMode currentMode = EditorMode.Place;
    
    void Start()
    {
        InitializeEditor();
    }
    
    void Update()
    {
        HandleInput();
        UpdatePreview();
    }
    
    /// <summary>
    /// 初始化编辑器
    /// </summary>
    private void InitializeEditor()
    {
        editorCamera = Camera.main;
        if (editorCamera == null)
        {
            editorCamera = FindObjectOfType<Camera>();
        }
    }
    
    /// <summary>
    /// 处理输入
    /// </summary>
    private void HandleInput()
    {
        // 鼠标左键放置
        if (Input.GetMouseButtonDown(0) && isPlacing && selectedPrefab != null)
        {
            PlaceObject();
        }
        
        // 鼠标右键取消
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }
        
        // 撤销
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
        {
            Undo();
        }
        
        // 重做
        if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl))
        {
            Redo();
        }
        
        // 删除选中对象
        if (Input.GetKeyDown(KeyCode.Delete) && currentMode == EditorMode.Select)
        {
            DeleteSelected();
        }
    }
    
    /// <summary>
    /// 更新预览对象
    /// </summary>
    private void UpdatePreview()
    {
        if (!isPlacing || selectedPrefab == null) return;
        
        Ray ray = editorCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
        {
            Vector3 position = hit.point;
            
            // 网格对齐
            if (snapToGrid)
            {
                position = SnapToGrid(position);
            }
            
            if (previewObject == null)
            {
                previewObject = Instantiate(selectedPrefab);
                SetPreviewMaterial(previewObject);
            }
            
            previewObject.transform.position = position;
            
            // 旋转预览（鼠标滚轮）
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                previewObject.transform.Rotate(Vector3.up, scroll * 90f);
            }
        }
    }
    
    /// <summary>
    /// 网格对齐
    /// </summary>
    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }
    
    /// <summary>
    /// 设置预览材质（半透明）
    /// </summary>
    private void SetPreviewMaterial(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = 0.5f;
            mat.color = color;
            mat.SetFloat("_Mode", 3); // 透明模式
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
    }
    
    /// <summary>
    /// 放置对象
    /// </summary>
    private void PlaceObject()
    {
        if (previewObject == null) return;
        
        Vector3 position = previewObject.transform.position;
        Quaternion rotation = previewObject.transform.rotation;
        
        // 创建实际对象
        GameObject newObj = Instantiate(selectedPrefab, position, rotation);
        placedObjects.Add(newObj);
        
        // 记录操作
        EditorAction action = new EditorAction
        {
            type = EditorAction.ActionType.Place,
            targetObject = newObj
        };
        AddToHistory(action);
        
        // 清除预览
        Destroy(previewObject);
        previewObject = null;
    }
    
    /// <summary>
    /// 取消放置
    /// </summary>
    private void CancelPlacement()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
        isPlacing = false;
        selectedPrefab = null;
    }
    
    /// <summary>
    /// 选择部件预制体
    /// </summary>
    public void SelectComponent(int index)
    {
        if (index < 0 || index >= componentPrefabs.Length) return;
        
        selectedPrefab = componentPrefabs[index];
        isPlacing = true;
    }
    
    /// <summary>
    /// 撤销操作
    /// </summary>
    public void Undo()
    {
        if (undoStack.Count == 0) return;
        
        EditorAction action = undoStack.Pop();
        action.Undo();
        redoStack.Push(action);
    }
    
    /// <summary>
    /// 重做操作
    /// </summary>
    public void Redo()
    {
        if (redoStack.Count == 0) return;
        
        EditorAction action = redoStack.Pop();
        action.Redo();
        undoStack.Push(action);
    }
    
    /// <summary>
    /// 删除选中对象
    /// </summary>
    private void DeleteSelected()
    {
        // TODO: 实现选择系统
    }
    
    /// <summary>
    /// 添加到历史记录
    /// </summary>
    private void AddToHistory(EditorAction action)
    {
        undoStack.Push(action);
        if (undoStack.Count > maxHistorySize)
        {
            // 移除最旧的操作
            var temp = new Stack<EditorAction>();
            while (undoStack.Count > maxHistorySize - 1)
            {
                temp.Push(undoStack.Pop());
            }
            undoStack = temp;
        }
        redoStack.Clear(); // 清空重做栈
    }
    
    /// <summary>
    /// 保存关卡
    /// </summary>
    public void SaveLevel(string levelName)
    {
        LevelData levelData = new LevelData
        {
            version = "1.0",
            levelName = levelName,
            components = new List<ComponentData>()
        };
        
        foreach (var obj in placedObjects)
        {
            ComponentData compData = new ComponentData
            {
                type = obj.name.Replace("(Clone)", ""),
                position = new float[] { obj.transform.position.x, obj.transform.position.y, obj.transform.position.z },
                rotation = new float[] { obj.transform.eulerAngles.x, obj.transform.eulerAngles.y, obj.transform.eulerAngles.z },
                scale = new float[] { obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z }
            };
            
            levelData.components.Add(compData);
        }
        
        LevelManager.Instance?.SaveLevel(levelData);
    }
    
    /// <summary>
    /// 加载关卡
    /// </summary>
    public void LoadLevel(LevelData levelData)
    {
        // 清除现有对象
        foreach (var obj in placedObjects)
        {
            Destroy(obj);
        }
        placedObjects.Clear();
        
        // 加载部件
        foreach (var compData in levelData.components)
        {
            GameObject prefab = System.Array.Find(componentPrefabs, p => p.name == compData.type);
            if (prefab != null)
            {
                Vector3 pos = new Vector3(compData.position[0], compData.position[1], compData.position[2]);
                Vector3 rot = new Vector3(compData.rotation[0], compData.rotation[1], compData.rotation[2]);
                Vector3 scale = new Vector3(compData.scale[0], compData.scale[1], compData.scale[2]);
                
                GameObject obj = Instantiate(prefab, pos, Quaternion.Euler(rot));
                obj.transform.localScale = scale;
                placedObjects.Add(obj);
            }
        }
    }
    
    /// <summary>
    /// 编辑器操作类
    /// </summary>
    private class EditorAction
    {
        public enum ActionType
        {
            Place,
            Delete,
            Move,
            Rotate
        }
        
        public ActionType type;
        public GameObject targetObject;
        public Vector3 oldPosition;
        public Vector3 newPosition;
        public Quaternion oldRotation;
        public Quaternion newRotation;
        
        public void Undo()
        {
            switch (type)
            {
                case ActionType.Place:
                    if (targetObject != null)
                    {
                        targetObject.SetActive(false);
                    }
                    break;
                case ActionType.Delete:
                    if (targetObject != null)
                    {
                        targetObject.SetActive(true);
                    }
                    break;
                case ActionType.Move:
                    if (targetObject != null)
                    {
                        targetObject.transform.position = oldPosition;
                    }
                    break;
                case ActionType.Rotate:
                    if (targetObject != null)
                    {
                        targetObject.transform.rotation = oldRotation;
                    }
                    break;
            }
        }
        
        public void Redo()
        {
            switch (type)
            {
                case ActionType.Place:
                    if (targetObject != null)
                    {
                        targetObject.SetActive(true);
                    }
                    break;
                case ActionType.Delete:
                    if (targetObject != null)
                    {
                        targetObject.SetActive(false);
                    }
                    break;
                case ActionType.Move:
                    if (targetObject != null)
                    {
                        targetObject.transform.position = newPosition;
                    }
                    break;
                case ActionType.Rotate:
                    if (targetObject != null)
                    {
                        targetObject.transform.rotation = newRotation;
                    }
                    break;
            }
        }
    }
}

