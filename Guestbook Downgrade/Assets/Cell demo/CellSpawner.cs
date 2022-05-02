﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    public GameObject cell_finished;
    public int count;
    public float timer;
    public float frequency;
    public float interval;
    public float move_mag;
    public static CellSpawner instance;
    public Mesh sharedMesh;
    public bool sharingMesh;
    public bool randRot;
    public float noiseMag;
    // Update is called once per frame
    void Start()
    {
        instance = this;
        timer = 0;
    }
    private void Update()
    {
        if (count > 0)
        {
            timer += Time.deltaTime;
            if (cell_finished && (timer > interval))
            {
                timer -= interval;
                count--;
                GameObject go = GameObject.Instantiate(cell_finished, transform.position+new Vector3(Random.Range(0, noiseMag),Random.Range(0, noiseMag),Random.Range(0, noiseMag)), randRot? Random.rotation:transform.rotation);
                if(sharingMesh) go.transform.GetChild(1).GetComponent<MeshFilter>().sharedMesh = sharedMesh;
                
            }
        }
        else
        {
            cell_finished = null;
            timer = 0;
            count = 0;
            interval = 0;
        }
        transform.position +=  Vector3.up * move_mag* Time.deltaTime * Mathf.Sin(frequency*Time.timeSinceLevelLoad);
    }
}