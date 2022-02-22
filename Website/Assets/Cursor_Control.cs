using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Control : MonoBehaviour
{

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot;
    void OnMouseEnter()
    {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
