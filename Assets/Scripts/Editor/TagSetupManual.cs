using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 手动标签设置工具 - 直接修改TagManager.asset文件
/// 这是一个更可靠的方法，直接编辑ProjectSettings文件
/// </summary>
public class TagSetupManual
{
    [MenuItem("Tools/手动设置标签（可靠方法）")]
    public static void SetupTagsManually()
    {
        string tagManagerPath = "ProjectSettings/TagManager.asset";
        
        if (!File.Exists(tagManagerPath))
        {
            Debug.LogError("找不到TagManager.asset文件！");
            return;
        }
        
        // 读取文件
        string[] lines = File.ReadAllLines(tagManagerPath);
        System.Collections.Generic.List<string> newLines = new System.Collections.Generic.List<string>();
        
        bool inTagsSection = false;
        bool tagsAdded = false;
        string[] requiredTags = { "Ball", "Goal", "Checkpoint", "Hazard" };
        
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            
            // 检测tags部分开始
            if (line.Trim().StartsWith("m_Tags:"))
            {
                inTagsSection = true;
                newLines.Add(line);
                continue;
            }
            
            // 检测tags部分结束（遇到m_Layers或其他m_开头的属性）
            if (inTagsSection && line.Trim().StartsWith("m_") && !line.Contains("m_Tags"))
            {
                // 在tags部分结束前，检查并添加缺失的标签
                if (!tagsAdded)
                {
                    // 检查哪些标签已存在
                    System.Collections.Generic.HashSet<string> existingTags = new System.Collections.Generic.HashSet<string>();
                    for (int j = newLines.Count - 1; j >= 0; j--)
                    {
                        string prevLine = newLines[j];
                        if (prevLine.Trim().StartsWith("-"))
                        {
                            string tag = prevLine.Trim().Substring(1).Trim();
                            if (!string.IsNullOrEmpty(tag))
                            {
                                existingTags.Add(tag);
                            }
                        }
                        if (prevLine.Trim().StartsWith("m_Tags:"))
                        {
                            break;
                        }
                    }
                    
                    // 添加缺失的标签
                    foreach (string tag in requiredTags)
                    {
                        if (!existingTags.Contains(tag))
                        {
                            newLines.Add($"  - {tag}");
                            Debug.Log($"✓ 已添加标签: {tag}");
                        }
                        else
                        {
                            Debug.Log($"标签已存在: {tag}");
                        }
                    }
                    tagsAdded = true;
                }
                
                inTagsSection = false;
            }
            
            newLines.Add(line);
        }
        
        // 如果文件末尾还在tags部分，也要添加
        if (inTagsSection && !tagsAdded)
        {
            System.Collections.Generic.HashSet<string> existingTags = new System.Collections.Generic.HashSet<string>();
            for (int j = newLines.Count - 1; j >= 0; j--)
            {
                string prevLine = newLines[j];
                if (prevLine.Trim().StartsWith("-"))
                {
                    string tag = prevLine.Trim().Substring(1).Trim();
                    if (!string.IsNullOrEmpty(tag))
                    {
                        existingTags.Add(tag);
                    }
                }
                if (prevLine.Trim().StartsWith("m_Tags:"))
                {
                    break;
                }
            }
            
            foreach (string tag in requiredTags)
            {
                if (!existingTags.Contains(tag))
                {
                    newLines.Add($"  - {tag}");
                    Debug.Log($"✓ 已添加标签: {tag}");
                }
            }
        }
        
        // 写回文件
        try
        {
            File.WriteAllLines(tagManagerPath, newLines.ToArray());
            AssetDatabase.Refresh();
            Debug.Log("✓ 标签设置完成！请重新启动Unity或刷新项目。");
            Debug.Log("如果标签仍未出现，请手动添加：Edit -> Project Settings -> Tags and Layers");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"保存标签失败: {e.Message}");
            Debug.LogWarning("请手动添加标签：Edit -> Project Settings -> Tags and Layers");
        }
    }
}

