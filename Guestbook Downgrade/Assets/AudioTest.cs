// MIT License
// 
// Copyright (c) 2021 Pixel Precision, LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioSource audioSource;

    public float amp = 0.75f;

    public float length = 3.0f;
    public float freq = 440.0f;
    public int sampsPerSec = 44100;

    public void Awake()
    {
        if (this.audioSource == null)
            this.audioSource = this.gameObject.AddComponent<AudioSource>();
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

    void SetAudio(float[] pcm, int sampsPerSec)
    {
        AudioClip ac = AudioClip.Create("", pcm.Length, 1, sampsPerSec, false);
        ac.SetData(pcm, 0);

        this.audioSource.Stop();
        this.audioSource.clip = ac;
        this.audioSource.time = 0.0f;
        this.audioSource.Play();
    }

    public void OnGUI()
    {
        GUILayout.Label($"Generation Length: {this.length} seconds");
        GUILayout.Label($"Tone Frequency : {this.freq} Hz");
        GUILayout.Label($"Audio SampleRate : {this.sampsPerSec} samples/second");
        GUILayout.Space(20.0f);

        GUILayout.Label("Audio Gen Amplitude");
        this.amp = GUILayout.HorizontalSlider(this.amp, 0.0f, 1.0f, GUILayout.Width(200.0f));

        if (GUILayout.Button("Generate Sine") == true)
        {
            float[] pcm = CreateSine(this.amp, this.length, this.freq, this.sampsPerSec);
            this.SetAudio(pcm, this.sampsPerSec);
        }

        if (GUILayout.Button("Generate Square") == true)
        {
            float[] pcm = CreateSquare(this.amp, this.length, this.freq, this.sampsPerSec);
            this.SetAudio(pcm, this.sampsPerSec);
        }

        if (GUILayout.Button("Generate Triangle") == true)
        {
            float[] pcm = CreateTriangle(this.amp, this.length, this.freq, this.sampsPerSec);
            this.SetAudio(pcm, this.sampsPerSec);
        }

        if (GUILayout.Button("Generate Sawtooth") == true)
        {
            float[] pcm = CreateSawtooth(this.amp, this.length, this.freq, this.sampsPerSec);
            this.SetAudio(pcm, this.sampsPerSec);
        }

        GUILayout.Space(20.0f);

        if (this.audioSource.clip != null)
        {
            if (this.audioSource.isPlaying == true)
            {
                if (GUILayout.Button("Stop") == true)
                {
                    this.audioSource.Stop();
                }
            }
            else
            {
                if (GUILayout.Button("Play") == true)
                {
                    this.audioSource.time = 0.0f;
                    this.audioSource.Play();
                }
            }
        }

        GUILayout.Space(20.0f);
        GUILayout.Label("Audio Source Volume");
        this.audioSource.volume = GUILayout.HorizontalSlider(this.audioSource.volume, 0.0f, 1.0f, GUILayout.Width(200.0f));
    }
}