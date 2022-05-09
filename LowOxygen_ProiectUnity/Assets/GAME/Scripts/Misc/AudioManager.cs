using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private float volumeModifier = 1;
    private bool inSpace = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        if (inSpace)
        {
            if(volumeModifier > 0)
            {
                volumeModifier -= Time.fixedDeltaTime;

                if(volumeModifier < 0)
                    volumeModifier = 0;
            }
        }
        else
        {
            if (volumeModifier < 1)
            {
                volumeModifier += Time.fixedDeltaTime;

                if (volumeModifier > 1)
                    volumeModifier = 1;
            }
        }
    }
    public void IsInSpace(bool newValue)
    {
        inSpace = newValue;
    }

    public void PlaySound(AudioClip clip, float volume, float pitch, bool loop)
    {
        GameObject clipHolder = new GameObject();
        clipHolder.AddComponent(typeof(AudioSource));

        AudioSource source = clipHolder.GetComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume * volumeModifier;
        source.pitch = pitch;
        source.loop = loop;

        source.Play();

        if (!loop)
            Destroy(clipHolder, source.clip.length / pitch + 0.1f); 
    }
}
