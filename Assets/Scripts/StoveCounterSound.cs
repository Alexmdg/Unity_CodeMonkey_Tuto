using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter counter;

    private AudioSource audioSource;

    private  void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        counter.OnStateChanged += OnStateChanged_PlaySound;
        audioSource.Stop();
    }

    private void OnStateChanged_PlaySound(object sender, StoveCounter.OnStateCHangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying;
        if (playSound)
            audioSource.Play();
        else
            audioSource.Stop();
    }
}
