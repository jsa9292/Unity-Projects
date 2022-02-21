using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartAnimation : MonoBehaviour
{
    public float start_speed;
    private RectTransform rt;
    // Update is called once per frame
    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    void FixedUpdate()
    {
        rt.localPosition /= 0.9f;
    }
}
