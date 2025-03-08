using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

    float playEnd;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
    }






    void SetAudioStats(Sound s, GameObject emitter)
    {


        if (emitter.GetComponent<AudioSource>() != null)
        {
            s.source = emitter.GetComponent<AudioSource>();
        }
        else
        {
            emitter.AddComponent<AudioSource>();
            s.source = emitter.GetComponent<AudioSource>();
            
        }

        s.source.clip = s.clip;
        s.source.loop = s.loop;

        if (s.spatialize)
        {
            s.source.spatialize = true;
            s.source.rolloffMode = AudioRolloffMode.Linear;
            s.source.spatialBlend = 1f;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;

        }

        s.source.outputAudioMixerGroup = mixerGroup;

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
       
        
    }
		
	public void Play(string sound, GameObject emitter) //for music
	{
        Sound s = Array.Find(sounds, item => item.name == sound);

        //if there there is no sound specified y the string, throw an error
        if (s == null)
        {
            Debug.Log("Sound: " + s.name + " not found! Did you spell it correctly?");
            return;
        }

        SetAudioStats(s, emitter);
        s.source.Play();
    }

    public void PlayOneShot(string sound, GameObject emitter) //For Sound Effects
    {

        Sound s = Array.Find(sounds, item => item.name == sound);

        //if there there is no sound specified by the string, throw an error
        if (s == null)
        {
            Debug.LogWarning("Sound: " + s.name + " not found! Did you spell it correctly?");
            return;
        }

        SetAudioStats(s, emitter);
        if (!s.source.isPlaying)
        {
            s.source.PlayOneShot(s.clip);
        }
    }

 





    
}
