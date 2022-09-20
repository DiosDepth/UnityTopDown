using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDSoundManager : Singleton<TDSoundManager>
{
    public bool BGMOn = true;
    public bool SFXOn = true;

    public AudioSource SFXPlayer;
    public AudioSource BGMPlayer;
    public override void Initialization()
    {
        base.Initialization();
        Debug.Log("TDSoundManager : " + instance.gameObject.name);
    }

    public void PlaySound(string path, Vector3 location = new Vector3(), bool loop = false)
    {
        if(SFXOn && path != "")
        {
            AudioClip clip = Resources.Load<AudioClip>(path);
            SFXPlayer.clip = clip;
            SFXPlayer.PlayOneShot(clip);
        }
    }
}
