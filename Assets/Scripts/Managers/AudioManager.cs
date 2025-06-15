using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBGM;
    private int BGMIndex;

    private bool canPlaySFX;

    protected override void Awake()
    {
        base.Awake();

        Invoke("AllowSFX", 1f);
    }

    private void Update()
    {
        if(!playBGM)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgm[BGMIndex].isPlaying)
            {
                PlayBGM(BGMIndex);
            }
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        BGMIndex = _bgmIndex;

        StopAllBGM();
        bgm[BGMIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        if (canPlaySFX == false)
        {
            return;
        }

        //Check if the player is within the minimum distance to play the SFX
        if (_source != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, _source.position) > sfxMinimumDistance)
        {
            return;
        }

        //Check if the SFX index is within the bounds of the array
        if (_sfxIndex < sfx.Length)
        {
            //Play at random volume pitch for variety
            sfx[_sfxIndex].pitch = Random.Range(0.8f, 1.2f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex)
    {
        sfx[_sfxIndex].Stop();
    }

    public void StopSFXWithTime(int _index)
    {
        StartCoroutine(DecreaseVolume(sfx[_index]));
    }

    private void AllowSFX()
    {
        canPlaySFX = true;
    }

    private IEnumerator DecreaseVolume(AudioSource _source)
    {
        float defaultVolume = _source.volume;

        while(_source.volume > .1f)
        {
            _source.volume -= _source.volume * 0.2f;
            yield return new WaitForSeconds(0.6f);

            if(_source.volume <= .1f)
            {
                _source.Stop();
                _source.volume = defaultVolume;
                break;
            }
        }
    }
}
