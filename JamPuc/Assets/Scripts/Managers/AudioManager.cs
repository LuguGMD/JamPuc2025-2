using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    [SerializeField] private AudioSource m_musicSource;
    [SerializeField] private AudioSource m_sfxSource;

    [SerializeField] private List<AudioClip> m_musicList;
    [SerializeField] private List<AudioClip> m_sfxList;

    protected override void Awake()
    {
        base.Awake();
        if(m_instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(int music)
    {
        PlayMusic((Music)music);
    }

    public void PlayMusic(Music music)
    {
        AudioClip newMusic = m_musicList[(int)music];
        if (newMusic != m_musicSource.clip)
        {
            m_musicSource.Stop();
            m_musicSource.clip = newMusic;
            m_musicSource.Play();
        }
    }

    public void PlaySFX(int sfx)
    {
        PlaySFX((SFX)sfx);
    }

    public void PlaySFX(SFX sfx)
    {
        m_sfxSource.PlayOneShot(m_sfxList[(int)sfx]);
    }

}

public enum Music
{
    MainMenu,
    GamePlay,
    GameOver
}
public enum SFX
{
    ButtonClick,
}
