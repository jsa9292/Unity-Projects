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

    public List<GameObject> GraphicAssets;

    // Start is called before the first frame update
    void Start()
    {
        canvas_size = canvas.sizeDelta;
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
            if (lerp_val > 1f) prevDisplay.transform.parent.parent.gameObject.SetActive(false);
        }
        if (currentDisplay != null)
        {
            currentDisplay.color = Color.Lerp(invis, vis, lerp_val * vis_speed);
        }
    }
    public void MenuButtonClicked(TMP_Text targetText) {
        if (targetText == currentDisplay) return;
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
        if (currentDisplay != null) prevDisplay = currentDisplay;
        if (prevDisplay != null)
        {
            prevDisplay.raycastTarget = false;
        }
        currentDisplay = null;
        lerp_val = -display_delay;
        pc.particle_off = false;
    }
}
