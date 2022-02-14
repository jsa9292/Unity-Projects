using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public Transform EmptyText;
    public Text currentDisplay;
    public Text prevDisplay;
    public Color invis;
    public Color vis;
    private float time_interact;
    public float display_delay;


    private RectTransform this_rt;
    public float tilt_speed;
    public float tilt_scale_speed;
    public bool italic_select;
    public Vector3 italic_rot;
    public Vector3 italic_scale;
    private float italic_progress;
    // Start is called before the first frame update
    void Start()
    {
        this_rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        time_interact += Time.deltaTime;
        if (currentDisplay != null) currentDisplay.color = Color.Lerp(invis, vis, time_interact);
        //if (prevDisplay != null) prevDisplay.color = Color.Lerp(vis, invis, time_interact);

        if (italic_select)
        {
            italic_progress += Time.deltaTime;
            this_rt.eulerAngles = Vector3.Lerp(Vector3.zero, italic_rot, italic_progress * tilt_speed);
            this_rt.localScale = Vector3.Lerp(Vector3.one, italic_scale, italic_progress * tilt_scale_speed);
        }
        else
        {
            italic_progress += Time.deltaTime;
            this_rt.eulerAngles = Vector3.Lerp(italic_rot, Vector3.zero, italic_progress * tilt_speed);
            this_rt.localScale = Vector3.Lerp(italic_scale, Vector3.one, italic_progress * tilt_scale_speed);
        }
    }
    public void MenuButtonClicked(Text targetText) {
        int text_index = targetText.transform.GetSiblingIndex();
        EmptyText.SetSiblingIndex(text_index + 1);
        prevDisplay = currentDisplay;
        currentDisplay = targetText;
        if (prevDisplay != null) prevDisplay.enabled=false;
        currentDisplay.enabled = true;
        time_interact = -display_delay;
    }
    public void italicUpdate()
    {
        italic_progress = 0;
        italic_select = true;
    }
}
