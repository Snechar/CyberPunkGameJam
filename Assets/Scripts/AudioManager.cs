using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private AudioSource audioSource;

    public AudioClip[] clips;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip? getClip(string name) {
        Tracks? trackID = AudioManager.TracksFromString(name);
        if (trackID == null) {
            Debug.Log($"Failed to load track name {name}");
            return null;
        }
        return clips[(int)trackID];
    }

    public void Play(string name) {
        var clip = getClip(name);
        if (clip != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void Pause() {
        audioSource.Pause();
    }

    public void UnPause() {
        audioSource.UnPause();
    }

    public void Stop() {
        audioSource.Stop();
    }

    public void SetBGVolume(float value, float time = 0) {
        if (time < .001) {
            audioSource.volume = value;
        } else {
            StartCoroutine(VolumeOverTime(value, time));
        }
    }

    private IEnumerator VolumeOverTime(float target, float time) {
        var curVol = audioSource.volume;
        float elapsed = 0;
        while (elapsed < time) {
            var progress = elapsed / time;
            audioSource.volume = Mathf.Lerp(curVol, target, progress);
            yield return null;
            elapsed += Time.deltaTime;
        }
        audioSource.volume = target;
    }

    public static Tracks? TracksFromString(string str) {
        switch (str.ToLower()) {
            case "dead rain":
                return Tracks.DEAD_RAIN;
            case "remember yesterday":
                return Tracks.REMEMBER_YESTERDAY;
            case "the color purple":
                return Tracks.THE_COLOR_PURPLE;
        }
        Debug.Log($"Failed to find track named {str}");
        return null;
    }

    public void CrossfadeTo(string newTrack, float time) {
        StartCoroutine(CrossfadeOverTime(newTrack, time));
    }

    private IEnumerator CrossfadeOverTime(string newTrack, float time) {
        var initialVolume = audioSource.volume;
        var elapsed = 0f;
        var halfTime = time / 2;
        while (elapsed < halfTime) {
            var progress = elapsed / halfTime;
            audioSource.volume = Mathf.Lerp(initialVolume, 0, progress);
            yield return null;
            elapsed += Time.deltaTime;
        }

        audioSource.clip = getClip(newTrack);
        audioSource.Play();

        elapsed = 0;
        while (elapsed < halfTime) {
            var progress = elapsed / halfTime;
            audioSource.volume = Mathf.Lerp(0, initialVolume, progress);
            yield return null;
            elapsed += Time.deltaTime;
        }
        audioSource.volume = initialVolume;
    }

}

public enum Tracks {
    DEAD_RAIN = 0,
    REMEMBER_YESTERDAY = 1,
    THE_COLOR_PURPLE = 2,
}