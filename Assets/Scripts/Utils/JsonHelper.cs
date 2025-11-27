using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JSON辅助类 - 处理复杂JSON序列化
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// 序列化列表
    /// </summary>
    public static string ToJson<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = list;
        return JsonUtility.ToJson(wrapper);
    }
    
    /// <summary>
    /// 反序列化列表
    /// </summary>
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }
    
    /// <summary>
    /// 序列化字典（转换为列表）
    /// </summary>
    public static string DictionaryToJson<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
        foreach (var kvp in dict)
        {
            list.Add(kvp);
        }
        return ToJson(list);
    }
    
    [Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }
}

