using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : SingletonMonoBehaviour <AudioManager>
{

    public List<AudioClip> BGMList;
    public List<AudioClip> SEList;
    public int MaxSE = 10;

    private AudioSource bgmSource = null;
    private List<AudioSource> seSources = null;
    private Dictionary<string, AudioClip> bgmDict = null;
    private Dictionary<string, AudioClip> seDict = null;
    private float bgmVolume = 1.0f;
    private float seVolume = 1.0f;
    private float saveBgmVolume = 1.0f;
    private float saveSeVolume = 1.0f;
    private bool noBGM = true;
    private bool noSE = true;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        //create listener
        if (FindObjectsOfType(typeof(AudioListener)).All(o => !((AudioListener)o).enabled))
        {
            this.gameObject.AddComponent<AudioListener>();
        }
        //create audio sources
        this.bgmSource = this.gameObject.AddComponent<AudioSource>();
        this.seSources = new List<AudioSource>();

        //create clip dictionaries
        this.bgmDict = new Dictionary<string, AudioClip>();
        this.seDict = new Dictionary<string, AudioClip>();

        Action<Dictionary<string, AudioClip>, AudioClip> addClipDict = (dict, c) => {
            if (!dict.ContainsKey(c.name))
            {
                dict.Add(c.name, c);
            }
        };

        this.BGMList.ForEach(bgm => addClipDict(this.bgmDict, bgm));
        this.SEList.ForEach(se => addClipDict(this.seDict, se));
    }

    //  効果音再生。
    public void PlaySE(string seName, float PlayTime = 0)
    {
        //  無ければ、not found
        if (!this.seDict.ContainsKey(seName)) throw new ArgumentException(seName + " not found", "seName");

        AudioSource source = this.seSources.FirstOrDefault(s => !s.isPlaying);
        if (source == null)
        {
            if (this.seSources.Count >= this.MaxSE)
            {
                Debug.Log("SE AudioSource is full");
                return;
            }

            source = this.gameObject.AddComponent<AudioSource>();
            this.seSources.Add(source);
        }
        source.volume = seVolume;
        source.clip = this.seDict[seName];
        source.time = PlayTime;
        source.Play();
    }

    //  効果音再生停止。
    public void StopSE()
    {
        this.seSources.ForEach(s => s.Stop());
    }

    //  BGM再生。
	public void PlayBGM(string bgmName, bool loop, float PlayTime = 0)
	{
		if (!this.bgmDict.ContainsKey(bgmName)) throw new ArgumentException(bgmName + " not found", "bgmName");
		if (this.bgmSource.clip == this.bgmDict[bgmName]) return;
		this.bgmSource.Stop();
		this.bgmSource.loop = loop;
		this.bgmSource.clip = this.bgmDict[bgmName];
        this.bgmSource.time = PlayTime;
        this.bgmSource.Play();
	}

    //  BGM再生停止。
    public void StopBGM()
    {
        this.bgmSource.Stop();
        this.bgmSource.clip = null;
    }

    public void SetVolumeBGM(float val)
    {
        saveBgmVolume = val;
        if (noBGM == true)
        {
            bgmVolume = saveBgmVolume;
        }
        this.bgmSource.volume = bgmVolume;
    }

    public void SetVolumSE(float val)
    {
        saveSeVolume = val;
        if (noSE == true)
        {
            AudioManager.Instance.PlaySE("動作音_1");
            seVolume = saveSeVolume;
        }
    }

    public void NoVolumeBGM(bool val)
    {
        if (val == false)
        {
            bgmVolume = 0.0f;
        }
        else
        {
            bgmVolume = saveBgmVolume;
        }
        this.bgmSource.volume = bgmVolume;
        noBGM = val;
    }

    public void NoVolumeSE(bool val)
    {
        if (val == false)
        {
            seVolume = 0.0f;
        }
        else
        {
            seVolume = saveSeVolume;
        }
        noSE = val;
    }
}