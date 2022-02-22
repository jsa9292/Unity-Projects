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

    public Light GraphicAssets_light;
    public List<GameObject> GraphicAssets;
    public RectTransform cursor;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas_size = canvas.sizeDelta;
        Reset();
        if (cursor != null)
        {
            Cursor.visible = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (cursor != null) cursor.position = Input.mousePosition;
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
            if (lerp_val > 1f && currentDisplay != prevDisplay) prevDisplay.transform.parent.parent.gameObject.SetActive(false);
        }
        if (currentDisplay != null)
        {
            currentDisplay.color = Color.Lerp(invis, vis, lerp_val * vis_speed);
            GraphicAssets_light.intensity = Mathf.Lerp(0, 1, lerp_val * vis_speed);
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
        Reset();
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
        pc.particle_off = false;

        for (int i = 0; i < GraphicAssets.Count; i++) {
            GraphicAssets[i].SetActive(false);
        }
        GraphicAssets_light.intensity = 0f;
    }
}
