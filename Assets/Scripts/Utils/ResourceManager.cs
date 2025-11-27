using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 资源管理器 - 管理AssetBundle和资源加载
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    
    [Header("资源设置")]
    [SerializeField] private string assetBundlePath = "AssetBundles";
    
    private Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();
    private Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 加载AssetBundle
    /// </summary>
    public void LoadAssetBundle(string bundleName, System.Action<AssetBundle> callback = null)
    {
        if (loadedBundles.ContainsKey(bundleName))
        {
            callback?.Invoke(loadedBundles[bundleName]);
            return;
        }
        
        StartCoroutine(LoadAssetBundleCoroutine(bundleName, callback));
    }
    
    /// <summary>
    /// 加载AssetBundle协程
    /// </summary>
    private IEnumerator LoadAssetBundleCoroutine(string bundleName, System.Action<AssetBundle> callback)
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, assetBundlePath, bundleName);
        
        AssetBundle bundle = null;
        
        #if UNITY_EDITOR
        // 编辑器模式：直接从Resources加载
        // 注意：在编辑器模式下，AssetBundle可能不存在，返回null
        yield return null;
        #else
        // 运行时：从AssetBundle加载
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
        yield return request;
        bundle = request.assetBundle;
        #endif
        
        if (bundle != null)
        {
            loadedBundles[bundleName] = bundle;
            callback?.Invoke(bundle);
        }
        else
        {
            Debug.LogError($"加载AssetBundle失败: {bundleName}");
            callback?.Invoke(null);
        }
    }
    
    /// <summary>
    /// 从AssetBundle加载预制体
    /// </summary>
    public void LoadPrefabFromBundle(string bundleName, string prefabName, System.Action<GameObject> callback)
    {
        string key = $"{bundleName}_{prefabName}";
        
        if (loadedPrefabs.ContainsKey(key))
        {
            callback?.Invoke(loadedPrefabs[key]);
            return;
        }
        
        LoadAssetBundle(bundleName, (bundle) =>
        {
            if (bundle != null)
            {
                GameObject prefab = bundle.LoadAsset<GameObject>(prefabName);
                if (prefab != null)
                {
                    loadedPrefabs[key] = prefab;
                    callback?.Invoke(prefab);
                }
                else
                {
                    Debug.LogError($"从AssetBundle加载预制体失败: {prefabName}");
                    callback?.Invoke(null);
                }
            }
            else
            {
                callback?.Invoke(null);
            }
        });
    }
    
    /// <summary>
    /// 从Resources加载预制体
    /// </summary>
    public GameObject LoadPrefabFromResources(string path)
    {
        string key = $"Resources_{path}";
        
        if (loadedPrefabs.ContainsKey(key))
        {
            return loadedPrefabs[key];
        }
        
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null)
        {
            loadedPrefabs[key] = prefab;
        }
        
        return prefab;
    }
    
    /// <summary>
    /// 卸载AssetBundle
    /// </summary>
    public void UnloadAssetBundle(string bundleName, bool unloadAllLoadedObjects = false)
    {
        if (loadedBundles.ContainsKey(bundleName))
        {
            loadedBundles[bundleName].Unload(unloadAllLoadedObjects);
            loadedBundles.Remove(bundleName);
        }
    }
    
    /// <summary>
    /// 清理所有资源
    /// </summary>
    public void ClearAllResources()
    {
        foreach (var bundle in loadedBundles.Values)
        {
            bundle.Unload(true);
        }
        
        loadedBundles.Clear();
        loadedPrefabs.Clear();
        
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
}

