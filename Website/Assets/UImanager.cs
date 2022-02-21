using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float display_delay;
    public float vis_speed;
    public float invis_speed;

    public GameObject GraphicAssets;

    // Start is called before the first frame update
    void Start()
    {
        //canvas_size = canvas.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        //resizing screen scale elements
        if (canvas.sizeDelta != canvas_size) {
            //SnakeImage.sizeDelta = new Vector2(canvas.sizeDelta.x / 2, 664*canvas.sizeDelta.x/2/1501);
            //EmptyText.sizeDelta = new Vector2(364, (canvas.sizeDelta.y - 200) - (110 * 3));
            canvas_size = canvas.sizeDelta;
        }
        //lerping fade in and out
        lerp_val += Time.deltaTime;
        if (prevDisplay != null)
        {
            prevDisplay.color = Color.Lerp(vis, invis, lerp_val * invis_speed);
            if (lerp_val > 1f) prevDisplay.transform.parent.gameObject.SetActive(false);
            GraphicAssets.gameObject.SetActive(false);
        }
        if (currentDisplay != null)
        {
            currentDisplay.color = Color.Lerp(invis, vis, lerp_val * vis_speed);
            GraphicAssets.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
            if (!mouse_held)
            {
                mouse_anchor = Input.mousePosition;
                mouse_held = true;
            }
            float mouse_y = -(Input.mousePosition - mouse_anchor).x / Screen.width;
            float mouse_x = (Input.mousePosition - mouse_anchor).y / Screen.height;
            GraphicAssets.transform.localEulerAngles = model_pos + new Vector3(mouse_x * 45f, mouse_y * 45f, 0f);
        }
        else
        {
            mouse_held = false;
            model_pos = GraphicAssets.transform.position;
        }

            if (GraphicAssets.gameObject.activeSelf)
        {
            GraphicAssets_active_time += Time.deltaTime / 2f;
            GraphicAssets.transform.localPosition = Vector3.Lerp(GraphicAssets_start, GraphicAssets_end, GraphicAssets_active_time);
        }
        else GraphicAssets_active_time = 0f;
    }
    private bool mouse_held = false;
    private Vector3 model_pos = Vector3.zero;
    private float GraphicAssets_active_time;
    private Vector3 mouse_anchor;
    private Vector3 GraphicAssets_start = new Vector3(0.5f,3f,2f);
    private Vector3 GraphicAssets_end = new Vector3(0.5f,0f,2f);
    public void MenuButtonClicked(TMP_Text targetText) {
        if (targetText == currentDisplay) return;
        //int text_index = targetText.transform.GetSiblingIndex();
        //EmptyText.SetSiblingIndex(text_index + 1);
        prevDisplay = currentDisplay;
        if (prevDisplay != null)
        {
            prevDisplay.raycastTarget = false;
        }
        currentDisplay = targetText;
        if (currentDisplay != null)
        {
            currentDisplay.enabled = true;
            currentDisplay.raycastTarget = true;
        }
        lerp_val = -display_delay;
        pc.particle_off = true;
        ActiveButton = targetText.name;
    }
    private string ActiveButton;
    public void Reset()
    {
        //EmptyText.SetSiblingIndex(0);
        prevDisplay = currentDisplay;
        if (prevDisplay != null)
        {
            prevDisplay.raycastTarget = false;
        }
        currentDisplay = null;
        lerp_val = -display_delay;
        pc.particle_off = false;
        GraphicAssets.gameObject.SetActive(false);
    }
}
