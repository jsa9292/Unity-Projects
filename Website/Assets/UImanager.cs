using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public RectTransform EmptyText;
    public Text currentDisplay;
    public Text prevDisplay;
    public Color invis;
    public Color vis;
    private float lerp_val;
    public float display_delay;
    public float vis_speed;
    public float invis_speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        lerp_val += Time.deltaTime;
        EmptyText.sizeDelta = new Vector2(364, (Screen.height - 200) - 196*3);
        if (currentDisplay != null) currentDisplay.color = Color.Lerp(invis, vis, lerp_val*vis_speed);
        if (prevDisplay != null)
        {
            prevDisplay.color = Color.Lerp(vis, invis, lerp_val*invis_speed);
            if(lerp_val > 1f) prevDisplay.enabled = false;
        }
    }
    public void MenuButtonClicked(Text targetText) {
        if (targetText == currentDisplay) return;
        int text_index = targetText.transform.GetSiblingIndex();
        EmptyText.SetSiblingIndex(text_index + 1);
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
    }
}
