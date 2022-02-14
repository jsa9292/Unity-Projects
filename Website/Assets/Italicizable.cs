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
    private RectTransform this_rt;

    // Update is called once per frame
    void Update()
    {
        if (this_rt == null) this_rt = GetComponent<RectTransform>();
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        if (clicked != null && clicked.transform.parent.name == "UI_Graphics") {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                italic_progress += Time.deltaTime;
            }
            else
            {
                italic_progress -= Time.deltaTime;
            }
        }
        italic_progress = Mathf.Clamp(italic_progress, 0f, 1f);

        this_rt.eulerAngles = Vector3.Lerp(Vector3.zero, italic_rot, italic_progress * tilt_speed);
        this_rt.localScale = Vector3.Lerp(Vector3.one, italic_scale, italic_progress * tilt_scale_speed);
    }
}
