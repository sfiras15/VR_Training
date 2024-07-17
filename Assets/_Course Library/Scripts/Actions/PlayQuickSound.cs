using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;
/// <summary>
/// Play a simple sounds using Play one shot with volume, and pitch
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayQuickSound : MonoBehaviour
{
    [Tooltip("The sound that is played")]
    public AudioClip sound = null;

    [Tooltip("The volume of the sound")]
    public float volume = 1.0f;

    [Tooltip("The range of pitch the sound is played at (-pitch, pitch)")]
    [Range(0, 1)] public float randomPitchVariance = 0.0f;

    private AudioSource audioSource = null;

    private float defaultPitch = 1.0f;

    private bool played = false;

    [Tooltip("Event to trigger when the audio finishes playing")]
    public UnityEvent OnSoundFinished = new UnityEvent();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (!played)
        {
            played = true;
            float randomVariance = UnityEngine.Random.Range(-randomPitchVariance, randomPitchVariance);
            randomVariance += defaultPitch;

            audioSource.pitch = randomVariance;
            audioSource.PlayOneShot(sound, volume);
            audioSource.pitch = defaultPitch;
            StartCoroutine(CheckIfSoundFinished());
        }
        
    }

    private IEnumerator CheckIfSoundFinished()
    {
        // Wait for the duration of the audio clip
        yield return new WaitForSeconds(sound.length);

        // Invoke the event when the audio has finished playing
        OnSoundFinished?.Invoke();
    }
    private void OnValidate()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

}
