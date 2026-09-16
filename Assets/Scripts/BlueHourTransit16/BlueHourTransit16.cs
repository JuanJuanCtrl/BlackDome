using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnableClocksAndAudio : MonoBehaviour
{
    public GameObject clocks;        // Drag your Clocks object here
    public AudioSource audioSource;  // Drag AudioSource here
    public AudioClip tickingSFX;     // Assign ticking sound here

    void Start()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // Wait 1 second
        yield return new WaitForSeconds(1f);

        // Enable Clocks
        if (clocks != null)
        {
            clocks.SetActive(true);
        }

        // Play ticking sound
        if (audioSource != null && tickingSFX != null)
        {
            audioSource.clip = tickingSFX;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or ticking SFX not assigned!");
        }

        // Wait remaining 7 seconds (total 8)
        yield return new WaitForSeconds(7f);

        // Load scene 7
        SceneManager.LoadScene(7);
    }
}