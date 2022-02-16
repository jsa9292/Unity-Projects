using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Italicizable : MonoBehaviour
{
    public float italic_progress = 0;
    public Vector3 italic_rot;
    public float tilt_speed;
    public Vector3 italic_scale;
    public float tilt_scale_speed;
    public bool on;

    // Update is called once per frame
    void Update()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        //if (clicked != null && clicked.transform.parent.name == "UI_Graphics") {
            if (on)
            {
                italic_progress += Time.deltaTime;
            }
            else
            {
                italic_progress -= Time.deltaTime;
            }
        //}
        italic_progress = Mathf.Clamp(italic_progress, 0f, 1f);

        transform.localEulerAngles = Vector3.Lerp(Vector3.zero, italic_rot, italic_progress);
        transform.localScale = Vector3.Lerp(Vector3.one, italic_scale, 1-Mathf.Sin(Mathf.PI / 2 + Mathf.PI/2*italic_progress));
    }
}
