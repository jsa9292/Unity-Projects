using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelControl : MonoBehaviour
{
    public Canvas cv;

    private void OnTriggerEnter(Collider other)
    {
        cv.enabled = true;
        cv.transform.LookAt(UI_anim_control.instance.cam.transform);

    }
    private void OnTriggerExit(Collider other)
    {
        cv.enabled = false;
    }
}
