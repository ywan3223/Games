using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSFXHandler : MonoBehaviour
{
    [Header("Mixers")]
    public AudioMixer audioMixer;

    [Header("Audio sources")]
    public AudioSource engineAudioSource;
    public AudioSource carJumpAudioSource;
    public AudioSource carJumpLandingAudioSource;

    float desiredEnginePitch = 0.5f;
    TopDownCarController topDownCarController;
    void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
    }
    void Start()
    {

    }
    void Update()
    {
        UpdateEngineSFX();
    }

    void UpdateEngineSFX()
    {
        float velocityMagnitude = topDownCarController.GetVelocityMagnitude();
        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, velocityMagnitude, Time.deltaTime * 10);
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);

        if (PauseMenu.GameIsPaused)
        {
            engineAudioSource.pitch *= .5f;
        }
    }
    public void PlayJumpSfx()
    {
        carJumpAudioSource.Play();
    }
    public void PlayLandingSfx()
    {
        carJumpLandingAudioSource.Play();
    }
}
