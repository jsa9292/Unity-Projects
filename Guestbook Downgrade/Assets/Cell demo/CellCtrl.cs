using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCtrl : MonoBehaviour
{
    public MCBlob mcb;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        MorphingOff();
    }

    // Update is called once per frame
    void Update()
    {
        Move(speed);
    }
    public void MorphingOff() {
        mcb.enabled = false;
    }
    public void Move(float speed) {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
