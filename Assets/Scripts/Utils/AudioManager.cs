using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 音频管理器 - 管理游戏音效和背景音乐
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("音频设置")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private int maxSFXSources = 10;
    
    [Header("音量设置")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    
    private Queue<AudioSource> sfxSourcePool = new Queue<AudioSource>();
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        LoadAudioSettings();
    }
    
    /// <summary>
    /// 初始化音频源
    /// </summary>
    private void InitializeAudioSources()
    {
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
        }
        
        // 创建SFX对象池
        for (int i = 0; i < maxSFXSources; i++)
        {
            GameObject obj = new GameObject($"SFXSource_{i}");
            obj.transform.SetParent(transform);
            AudioSource source = obj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxSourcePool.Enqueue(source);
        }
    }
    
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }
    }
    
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        
        AudioSource source = GetSFXSource();
        if (source != null)
        {
            source.clip = clip;
            source.volume = volume * sfxVolume * masterVolume;
            source.Play();
            
            // 延迟归还到对象池
            StartCoroutine(ReturnSFXSourceAfterPlay(source, clip.length));
        }
    }
    
    /// <summary>
    /// 播放音效（通过名称）
    /// </summary>
    public void PlaySFX(string clipName, float volume = 1f)
    {
        if (audioClips.ContainsKey(clipName))
        {
            PlaySFX(audioClips[clipName], volume);
        }
        else
        {
            // 尝试从Resources加载
            AudioClip clip = Resources.Load<AudioClip>($"Audio/SFX/{clipName}");
            if (clip != null)
            {
                audioClips[clipName] = clip;
                PlaySFX(clip, volume);
            }
        }
    }
    
    /// <summary>
    /// 获取SFX音频源
    /// </summary>
    private AudioSource GetSFXSource()
    {
        if (sfxSourcePool.Count > 0)
        {
            return sfxSourcePool.Dequeue();
        }
        
        // 如果池为空，创建新的
        GameObject obj = new GameObject("SFXSource_Temp");
        obj.transform.SetParent(transform);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }
    
    /// <summary>
    /// 归还SFX音频源
    /// </summary>
    private System.Collections.IEnumerator ReturnSFXSourceAfterPlay(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        
        source.Stop();
        source.clip = null;
        sfxSourcePool.Enqueue(source);
    }
    
    /// <summary>
    /// 设置主音量
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
        SaveAudioSettings();
    }
    
    /// <summary>
    /// 设置音乐音量
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
        SaveAudioSettings();
    }
    
    /// <summary>
    /// 设置音效音量
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        SaveAudioSettings();
    }
    
    /// <summary>
    /// 更新音量
    /// </summary>
    private void UpdateVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
    
    /// <summary>
    /// 加载音频设置
    /// </summary>
    private void LoadAudioSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        UpdateVolumes();
    }
    
    /// <summary>
    /// 保存音频设置
    /// </summary>
    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
}

