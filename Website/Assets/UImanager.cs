using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UImanager : MonoBehaviour
{
    public RectTransform canvas;
    private Vector2 canvas_size;
    public ParticleControl pc;
    public TMP_Text currentDisplay;
    public TMP_Text prevDisplay;
    public Color invis;
    public Color vis;
    private float lerp_val;
    private float light_lerp_val;
    public float display_delay;
    public float vis_speed;
    public float invis_speed;

    public Light GraphicAssets_light;
    public List<GameObject> GraphicAssets;


    public Texture2D cursor;
    // Start is called before the first frame update
    void Start()
    {
        canvas_size = canvas.sizeDelta;
        //Reset();
        if(cursor != null) cursorSet(cursor);
    }
    void cursorSet(Texture2D tex)
    {
        CursorMode mode = CursorMode.ForceSoftware;
        float xspot = tex.width / 2;
        float yspot = tex.height / 2;
        Vector2 hotSpot = new Vector2(xspot, yspot);
        Cursor.SetCursor(tex, hotSpot, mode);
    }
    public bool ThisIsMobile;

    // Update is called once per frame
    void OnGUI()
    {
        
        //lerping fade in and out
        lerp_val += Time.deltaTime * vis_speed;
        light_lerp_val += Time.deltaTime * vis_speed;
        //if (prevDisplay != null)
        //{
        //    prevDisplay.color = Color.Lerp(vis, invis, lerp_val * invis_speed);
        //    if (lerp_val > 1f && currentDisplay != prevDisplay) prevDisplay.transform.parent.parent.gameObject.SetActive(false);

        //}
        if (currentDisplay != null)
        {
            currentDisplay.color = Color.Lerp(invis, vis, lerp_val);
            GraphicAssets_light.intensity = Mathf.Lerp(0, 1, light_lerp_val);
            if (light_lerp_val > -display_delay)
            {
                who_objs[0].SetActive(!thresed);
                who_objs[1].SetActive(thresed);
            }
        }
    }
    public void MenuButtonClicked(TMP_Text targetText) {
        currentDisplay = targetText;
        currentDisplay.enabled = true;
        currentDisplay.raycastTarget = true;
        
        pc.particle_off = true;
        ActiveButton = targetText.name;

    }
    private string ActiveButton;
    private void OnDisable()
    {
        //Reset();
    }
    public void Reset()
    {
        if (currentDisplay != null) prevDisplay = currentDisplay;
        if (prevDisplay != null)
        {
            prevDisplay.raycastTarget = false;
        }
        currentDisplay = null;
        lerp_val = -display_delay;
        light_lerp_val = -display_delay;
        pc.particle_off = false;

        for (int i = 0; i < GraphicAssets.Count; i++) {
            GraphicAssets[i].SetActive(false);
        }
        GraphicAssets_light.intensity = 0f;
    }
    public RectTransform who_text;
    public float who_text_switch_thres;
    public List<GameObject> who_objs;
    private bool thresed = false;
    public void checkScrollAndActivate() {
        if (thresed != who_text.localPosition.y > who_text_switch_thres)
        {
            thresed = who_text.localPosition.y > who_text_switch_thres;
            light_lerp_val = -display_delay;
        }
        
    }
}
