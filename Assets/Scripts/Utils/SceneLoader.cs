using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 场景加载器 - 管理场景切换
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Header("加载设置")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text progressText;
    
    private static SceneLoader instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 加载场景
    /// </summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    
    /// <summary>
    /// 加载场景协程
    /// </summary>
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            
            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            
            if (progressText != null)
            {
                progressText.text = $"加载中... {Mathf.RoundToInt(progress * 100)}%";
            }
            
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// 重新加载当前场景
    /// </summary>
    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
}

