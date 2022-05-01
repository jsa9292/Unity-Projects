using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLook : MonoBehaviour
{
    public Transform Look_target;
    public Transform Follow_target;
    public Vector3 Pos_offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = Follow_target.position;
        transform.LookAt(Look_target);
        transform.position += transform.right*Pos_offset.x + transform.up*Pos_offset.y + transform.forward*Pos_offset.z;
    }
}
