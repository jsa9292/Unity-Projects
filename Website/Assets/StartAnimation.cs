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
        transform.position += (target_rt.position - transform.position) * start_speed * Time.deltaTime;
        
    }
}
