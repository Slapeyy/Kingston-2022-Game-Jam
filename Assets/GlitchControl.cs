using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchControl : MonoBehaviour
{
    public static GlitchControl Instance { get; private set; }

    public AudioClip glitch;
    public AudioClip mutate;
    public AudioSource source;

    public Coroutine glitchCor;
    public bool glitching = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMutate()
    {
        var a = Camera.main.GetComponent<Kino.AnalogGlitch>();
        a.verticalJump = 0f;
        a.scanLineJitter = 0f;
        a.horizontalShake = 0f;
        a.colorDrift = 0f;

        source.PlayOneShot(mutate, PlayerPrefs.GetFloat("Sound") * 0.3f);
        StartCoroutine(SingleMutate(a));

    }

    public void StartGlitching()
    {
        Camera.main.GetComponent<Kino.DigitalGlitch>().intensity = 0f;
        glitching = true;
        if (glitchCor != null) StopCoroutine(glitchCor);
        glitchCor = StartCoroutine(RandomGlitches());
    }

    public void StopGlitching()
    {
        Camera.main.GetComponent<Kino.DigitalGlitch>().intensity = 0f;
        glitching = false;
        StopCoroutine(glitchCor);
    }

    private IEnumerator SingleMutate(Kino.AnalogGlitch a)
    {
        a.scanLineJitter = 0.4f;
        a.horizontalShake = 0.4f;
        a.colorDrift = 0.4f;

        yield return new WaitForSeconds(0.4f);
        
        a.scanLineJitter = 0f;
        a.horizontalShake = 0f;
        a.colorDrift = 0f;
    }

    private IEnumerator RandomGlitches()
    {
        while (glitching)
        {
            float randomTime = Random.Range(0.1f, 1.5f);
            float randomIntensity = Random.Range(0.4f, 0.7f);
            float randomCD = Random.Range(1f, 5f);

            Camera.main.GetComponent<Kino.DigitalGlitch>().intensity = randomIntensity;
            source.PlayOneShot(glitch, PlayerPrefs.GetFloat("Sound") * 0.3f);
            yield return new WaitForSeconds(randomTime);
            Camera.main.GetComponent<Kino.DigitalGlitch>().intensity = 0f;

            yield return new WaitForSeconds(randomCD);
        }
    }
}
