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
        JamTrack? trackID = AudioManager.JamTrackFromString(name);
        if (trackID == null) {
            Debug.Log($"Failed to load track name {name}");
            return null;
        }
        return clips[(int)trackID];
    }

    public void Play(string name) {
        Play(getClip(name));
    }

    public void Play(JamTrack track) {
        Play(clips[(int)track]);
    }

    private void Play(AudioClip clip) {
        Debug.Log("AudioManager.Play");
        if (clip != null) {
            if (audioSource.clip != clip) {
                audioSource.clip = clip;
            } else {
                Debug.Log("AM.Play: clip was the same");
            }
            if (!audioSource.isPlaying) {
                audioSource.Play();
            } else {
                Debug.Log("AM.play: audioSource is already playing");
            }
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

    public static JamTrack? JamTrackFromString(string str) {
        switch (str.ToLower()) {
            case "dead rain":
                return JamTrack.DEAD_RAIN;
            case "remember yesterday":
                return JamTrack.REMEMBER_YESTERDAY;
            case "the color purple":
                return JamTrack.THE_COLOR_PURPLE;
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

// IDs here must match the order of the clips in AudioManager.clips
public enum JamTrack {
    DEAD_RAIN = 0,
    REMEMBER_YESTERDAY = 1,
    THE_COLOR_PURPLE = 2,
}