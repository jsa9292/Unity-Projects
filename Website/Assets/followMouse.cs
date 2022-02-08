using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1f;
    }
    private Vector3 mousePosition;
    private float moveSpeed;
    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition += Vector3.forward * 5f;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = mousePosition;
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }
}
