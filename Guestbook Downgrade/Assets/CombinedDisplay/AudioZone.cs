using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    public AudioSource audioSource;
    public MeshRenderer mr;
    public Color color;
    public float amp = 0.75f;

    public int sampsPerSec = 44100;
    public float length = 2.0f;
    public float freq = 440.0f;
    public int soundType = 0;
    public float delay;
    public AudioSource AS;
    private static AudioSource[] ASs = new AudioSource[0];
    public GameObject ASs_go;
    public void Start()
    {
        mr.material.color = color;
        if (ASs.Length == 0)
        {
            ASs_go = GameObject.Find("ASs");
            ASs = ASs_go.GetComponentsInChildren<AudioSource>();
        }
        if (AS) return;
        AS = ASs[soundType];
        //float[] pcm;
        //if (soundType == 0)
        //{
        //    pcm = CreateSine(this.amp, this.length, this.freq, this.sampsPerSec);
        //    ac = AudioClip.Create("", pcm.Length, 1, sampsPerSec, false);
        //    ac.SetData(pcm, 0);
        //}
        //else if (soundType == 1)
        //{
        //    pcm = CreateSquare(this.amp, this.length, this.freq, this.sampsPerSec);
        //    ac = AudioClip.Create("", pcm.Length, 1, sampsPerSec, false);
        //    ac.SetData(pcm, 0);
        //}
        //else if (soundType == 2)
        //{
        //    pcm = CreateSawtooth(this.amp, this.length, this.freq, this.sampsPerSec);
        //    ac = AudioClip.Create("", pcm.Length, 1, sampsPerSec, false);
        //    ac.SetData(pcm, 0);
        //}
        //else if (soundType == 3)
        //{

        //    pcm = CreateTriangle(this.amp, this.length, this.freq, this.sampsPerSec);
        //    ac = AudioClip.Create("", pcm.Length, 1, sampsPerSec, false);
        //    ac.SetData(pcm, 0);
        //}

    }
    private void OnTriggerEnter(Collider other)
    {
        Invoke("PlayNDie", delay);

    }
    private void PlayNDie() {
        GameObject go = new GameObject();
        AudioSource aud = go.AddComponent<AudioSource>();
        aud.clip = AS.clip;
        aud.time = 0.0f;
        aud.Play();
        GameObject.Destroy(go, aud.clip.length + 0.3f);

    }


    public static float[] CreateSine(float vel, float len, float freq, int samplesPerSec)
    {
        float tau = Mathf.PI * 2.0f;

        int samples = (int)(samplesPerSec * len);
        float[] ret = new float[samples];

        for (int i = 0; i < samples; ++i)
            ret[i] = Mathf.Sin((float)i / (float)samplesPerSec * freq * tau) * vel;

        return ret;
    }

    public static float[] CreateSquare(float vel, float len, float freq, int samplesPerSec)
    {
        int samples = (int)(samplesPerSec * len);
        float[] ret = new float[samples];

        for (int i = 0; i < samples; ++i)
        {
            float lam = (float)i / (float)samplesPerSec * freq % 1.0f;
            ret[i] = lam > 0.5f ? vel : -vel;
        }
        return ret;
    }

    public static float[] CreateSawtooth(float vel, float len, float freq, int samplesPerSec)
    {
        int samples = (int)(samplesPerSec * len);
        float[] ret = new float[samples];

        for (int i = 0; i < samples; ++i)
        {
            float lam = (float)i / (float)samplesPerSec * freq % 1.0f;
            ret[i] = (lam * 2.0f - 1.0f) * vel;
        }
        return ret;
    }

    public static float[] CreateTriangle(float vel, float len, float freq, int samplesPerSec)
    {
        int samples = (int)(samplesPerSec * len);
        float[] ret = new float[samples];

        for (int i = 0; i < samples; ++i)
        {
            float lam = (float)i / (float)samplesPerSec * freq % 1.0f;
            ret[i] =
                (lam < 0.5f) ?
                    (lam * 4 - 1) * vel :
                    (3.0f + lam * -4) * vel;
        }
        return ret;
    }


}
