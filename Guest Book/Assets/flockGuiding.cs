using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flockGuiding : MonoBehaviour
{
    public Vector3 velocity;
    public GameObject cell;
    public Material mat;
    public Color inside;
    public int cell_count;
    public float cellSpeed;
    public float cellRotationBias;
    public float cellMoveFreq;
    public float cellBodyLength;
    public bool randgen;
    // Start is called before the first frame update
    void Start()
    {

        mat = this.GetComponent<MeshRenderer>().material;
        inside = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        mat.SetColor("inside", inside);
        if (randgen) {
            cell_count = Random.Range(10,50);
            cellSpeed = Random.Range(1f, 3f);
            cellRotationBias = Random.Range(-0.8f, 0.8f);
            cellMoveFreq = Random.Range(0.5f, 2f);
            cellBodyLength = Random.Range(1f, 4f);
        }
        for (int i = 0; i < cell_count; i++)
        {
            Instantiate(cell, transform.position+Vector3.forward*i*0.01f, Quaternion.Euler(Vector3.forward*i*24f),transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=  velocity * Time.deltaTime;
    }
}
