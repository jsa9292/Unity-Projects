using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFade : MonoBehaviour
{
    private MeshRenderer mr;
    private float createTime = 0;
    public float protectTime = 0.02f;
    public float fadeTime = 1f;
    private float t = 0;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        createTime = Time.time + protectTime;
    }

    // Update is called once per frame
    void Update()
    {
        t = (Time.time - createTime)/fadeTime;
        if (t<=1)
        {
            Color c = mr.material.color;
            mr.material.color = new Color(c.r, c.g, c.b, 1-t);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
