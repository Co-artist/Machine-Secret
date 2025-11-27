using UnityEngine;
using UnityEditor;

/// <summary>
/// 标签设置工具 - 自动添加项目所需的标签
/// </summary>
public class TagSetup
{
    [MenuItem("Tools/设置项目标签和层级")]
    public static void SetupTagsAndLayers()
    {
        int addedCount = 0;
        
        // 添加标签
        if (AddTag("Ball")) addedCount++;
        if (AddTag("Goal")) addedCount++;
        if (AddTag("Checkpoint")) addedCount++;
        if (AddTag("Hazard")) addedCount++;
        
        // 刷新AssetDatabase和标签系统
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("ProjectSettings/TagManager.asset"));
        
        if (addedCount > 0)
        {
            Debug.Log($"✓ 成功添加 {addedCount} 个标签！");
            Debug.Log("标签列表：Ball, Goal, Checkpoint, Hazard");
            Debug.Log("提示：如果仍有警告，请重新启动Unity或刷新项目");
        }
        else
        {
            Debug.Log("所有标签已存在，无需添加。");
        }
    }
    
    /// <summary>
    /// 添加标签（如果不存在）
    /// </summary>
    private static bool AddTag(string tag)
    {
        try
        {
            // 检查标签是否已存在
            if (TagExists(tag))
            {
                Debug.Log($"标签已存在: {tag}");
                return false;
            }
            
            // 获取TagManager
            UnityEngine.Object[] tagAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (tagAssets == null || tagAssets.Length == 0)
            {
                Debug.LogError("无法加载TagManager.asset，请手动添加标签");
                return false;
            }
            
            SerializedObject tagManager = new SerializedObject(tagAssets[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");
            
            if (tagsProp == null)
            {
                Debug.LogError("无法找到tags属性，请手动添加标签");
                return false;
            }
            
            // 检查是否还有空槽位
            bool foundEmpty = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty tagProp = tagsProp.GetArrayElementAtIndex(i);
                if (string.IsNullOrEmpty(tagProp.stringValue))
                {
                    tagProp.stringValue = tag;
                    foundEmpty = true;
                    break;
                }
            }
            
            // 如果没有空槽位，添加新元素
            if (!foundEmpty)
            {
                tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
                SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
                newTagProp.stringValue = tag;
            }
            
            tagManager.ApplyModifiedProperties();
            Debug.Log($"✓ 已添加标签: {tag}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"添加标签 {tag} 失败: {e.Message}");
            Debug.LogWarning($"请手动添加标签 '{tag}'：Edit -> Project Settings -> Tags and Layers");
            return false;
        }
    }
    
    /// <summary>
    /// 检查标签是否存在（不触发警告）
    /// </summary>
    private static bool TagExists(string tag)
    {
        try
        {
            // 直接检查TagManager，不使用CompareTag（避免触发警告）
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
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
}

