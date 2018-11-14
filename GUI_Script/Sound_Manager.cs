using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sound_Manager : MonoBehaviour {

    public float bgmVolume;
    private float bgmOffVolume;
    public AudioSource bgmPlay;
    public AudioClip[] bgmClip;
    public bool isBgmOn;
    float nextTime;
    void Start () {
        bgmPlay.volume = 0f;
        bgmOffVolume = 0f;
        PlayBgmSound(bgmClip[0]);
        isBgmOn = true;
        nextTime = 1f;
    }
	
	void Update () {
        if (isBgmOn)
        {
            bgmPlay.volume = Mathf.Lerp(bgmPlay.volume, bgmVolume, Time.deltaTime * 1f);  
        }
        else
        {
            bgmPlay.volume = Mathf.Lerp(bgmPlay.volume, bgmOffVolume, Time.deltaTime * 1f);
        }
    }

    public void PlayBgmSound(AudioClip clip)
    {
        OffTheVolume();
        Invoke("OnTheVolume", 1f);
        bgmPlay.clip = clip;
        bgmPlay.Play();
    }

    public void PlayBgmSound(int clipID)
    {
        if(clipID < bgmClip.Length)
        {
            OffTheVolume();
            Invoke("OnTheVolume", 1f);
            bgmPlay.clip = bgmClip[clipID];
            bgmPlay.Play();
        }
    }

    public void OffTheVolume()
    {
        isBgmOn = false;
    }
    public void OnTheVolume()
    {
        isBgmOn = true;
    }

}
