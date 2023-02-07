﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private static MusicManager instance;
	private AudioSource audioSource;

    private bool isFading = false;

    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        instance = this;
    }
    

	public static void playMusic(AudioClip music){

		if (music != instance.audioSource.clip && instance.audioSource.isPlaying && !instance.isFading)
        {
            instance.isFading = true;
            Debug.Log("changing music");
            instance.StartCoroutine(fadeOutThenFadeIn(music, 1f));
        } else
        {
            Debug.Log("restarting same music");
            instance.StartCoroutine(fadeIn(music,1f));
        }
        if (!instance.audioSource.isPlaying)
        {
            instance.audioSource.clip = music;
            instance.audioSource.Play();
        }
	}
    

	private static IEnumerator fadeIn(AudioClip music, float fadeTime)
    {
        instance.isFading = false;
        instance.audioSource.clip = music;
        if (!instance.isFading && !instance.audioSource.isPlaying)
        {
            instance.audioSource.Play();
        }
        while (!instance.isFading && instance.audioSource.volume < 1f)
        {
            instance.audioSource.volume +=  Time.deltaTime / fadeTime;

            yield return null;
        }

    }

    private static IEnumerator fadeOutThenFadeIn(AudioClip music, float fadeTime)
    {
        while (instance.isFading && instance.audioSource.volume > 0.1)
        {
            instance.audioSource.volume -= Time.deltaTime / fadeTime;

            yield return null;
        }
        yield return null;
        if (instance.isFading) { 
            instance.StartCoroutine(fadeIn(music, fadeTime));
        }
    }
}