using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Sine : MonoBehaviour
{
    public float mag;
    public float freq;
    public Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * freq) * mag + offset;
    }
}
