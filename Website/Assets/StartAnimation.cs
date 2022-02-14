using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartAnimation : MonoBehaviour
{
    public RectTransform target_rt;
    public float start_speed;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target_rt.position, Time.realtimeSinceStartup * start_speed);
        if (Time.timeSinceLevelLoad > 10f) this.enabled = false;
    }
}
