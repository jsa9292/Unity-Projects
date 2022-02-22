using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    public GameObject desktop_canvas;
    public GameObject mobile_canvas;

    // Update is called once per frame
    void Update()
    {
        desktop_canvas.SetActive(Screen.width >= Screen.height);
        mobile_canvas.SetActive(Screen.width < Screen.height);
    }
}
