using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float depth;
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
        mousePosition += Vector3.forward * depth;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = mousePosition;
    }
}
