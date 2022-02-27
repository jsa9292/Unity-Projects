using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SymbolAction : MonoBehaviour
{
    private float timer;
    public float idle_rot_freq;
    public float idle_rot_mag;
    public float mouse_rot_mag;
    private Vector3 initial_rot;
    private Vector3 initial_mouse;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            initial_rot = transform.localEulerAngles;
            initial_mouse = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            initial_rot = transform.localEulerAngles;

        }
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (Input.GetMouseButton(0) && current == null)
        {
            Vector3 delta_mouse = initial_mouse - Input.mousePosition;
            Vector3 rot_from_mouse = new Vector3(-delta_mouse.y / Screen.height, delta_mouse.x / Screen.width, 0f);
            transform.localEulerAngles = initial_rot + rot_from_mouse * mouse_rot_mag;
        }
        else {
            transform.localEulerAngles +=  Vector3.up * Mathf.Sin(Time.realtimeSinceStartup*idle_rot_freq) * idle_rot_mag;
        }
    }
}
