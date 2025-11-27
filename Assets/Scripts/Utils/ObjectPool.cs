using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 对象池 - 管理频繁创建和销毁的对象
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    
    public static ObjectPool Instance { get; private set; }
    
    [Header("对象池设置")]
    [SerializeField] private List<Pool> pools;
    
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        InitializePools();
    }
    
    /// <summary>
    /// 初始化对象池
    /// </summary>
    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    
    /// <summary>
    /// 从对象池获取对象
    /// </summary>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"对象池中不存在标签: {tag}");
            return null;
        }
        
        GameObject objectToSpawn;
        
        if (poolDictionary[tag].Count > 0)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }
        else
        {
            // 如果池为空，创建新对象
            Pool pool = pools.Find(p => p.tag == tag);
            if (pool != null)
            {
                objectToSpawn = Instantiate(pool.prefab);
            }
            else
            {
                return null;
            }
        }
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        // 调用对象的OnObjectSpawn方法（如果存在）
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        pooledObj?.OnObjectSpawn();
        
        return objectToSpawn;
    }
    
    /// <summary>
    /// 将对象返回对象池
    /// </summary>
    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"对象池中不存在标签: {tag}");
            Destroy(obj);
            return;
        }
        
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}

/// <summary>
/// 对象池接口
/// </summary>
public interface IPooledObject
{
    void OnObjectSpawn();
}

