using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revertParentYrot : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, -transform.parent.localEulerAngles.y, 0);
    }
}
