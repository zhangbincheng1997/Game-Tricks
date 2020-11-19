using UnityEngine;

public class AudioMgr : UnitySingleton<AudioMgr>
{
    private AudioSource[] audioSource;
    private AudioSource musicSource;
    private AudioSource effectSource;

    public override void Awake()
    {
        // 继承父对象
        base.Awake();

        // 获取Audio组件
        audioSource = GetComponents<AudioSource>();
        // 第一个控制音乐
        musicSource = audioSource[0];
        // 第二个控制音效
        effectSource = audioSource[1];
    }

    // 音乐
    public void PlayMusic(string music)
    {
        musicSource.clip = Resources.Load<AudioClip>(music);
        musicSource.Play();
    }

    // 获取音乐大小
    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    // 控制音乐大小
    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    // 音效
    public void PlayEffect(string effect)
    {
        AudioClip clip = Resources.Load<AudioClip>(effect);
        effectSource.PlayOneShot(clip);
    }

    // 获取音效大小
    public float GetEffectVolume()
    {
        return effectSource.volume;
    }

    // 控制音效大小
    public void SetEffectVolume(float value)
    {
        effectSource.volume = value;
    }
}
