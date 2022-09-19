using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public AudioClip landingClip;
    public AudioClip clickClip;
    public AudioClip climbingSound;
    public AudioClip pushingSound;

   
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

   public void Step()
   {
        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.PlayOneShot(clip);
   }

   public void LandingSound()
   {
        audioSource.PlayOneShot(landingClip);
   }

    public void ClickSound()
    {
        audioSource.PlayOneShot(clickClip);
    }

    public void ClimbSfx()
    {
        audioSource.PlayOneShot(climbingSound);
    }

    public void PushingSfx()
    {
        audioSource.PlayOneShot(pushingSound);
    }
}
