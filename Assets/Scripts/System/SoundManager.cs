using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer mixer;
    public AudioSource bgmSource;
    public AudioClip[] bgmClips;
    public AudioClip[] UIClips;

    public Slider bgmSlider, sfxSlider;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        BgSoundPlay(0);
    }

    public void BgmVolume(float size) // 배경음악 볼륨 설정
    {
        //mixer.SetFloat("Bgm",Mathf.Log10(size) * 20);
        mixer.SetFloat("Bgm", (size <= -20) ? -80f : size);

        //SaveDataManager.instance.bgmVolume = size;
    }
    public void SfxVolume(float size) // 효과음 볼륨 설정
    {
        mixer.SetFloat("Sfx", (size <= -30) ? -80f : size);
        mixer.SetFloat("Dice", (size <= -30) ? -80f : size - 10);

        //SaveDataManager.instance.sfxVolume = size;

        //mixer.SetFloat("Sfx",Mathf.Log10(size) * 20);
        //mixer.SetFloat("Dice",Mathf.Log10(size) * 20);
    }

    public void UISfxPlay(int _n) // 효과음 플레이
    {
        GameObject sfx = new GameObject(_n + "Sound");
        AudioSource audioSource = sfx.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = UIClips[_n];
        audioSource.Play();

        Destroy(sfx, UIClips[_n].length);
    }
    public void BgSoundPlay(int _n) // 배경음 플레이
    {
        bgmSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Bgm")[0];
        bgmSource.clip = bgmClips[_n];
        bgmSource.loop = true;
        bgmSource.volume = 0.1f;
        bgmSource.Play();
    }

}
