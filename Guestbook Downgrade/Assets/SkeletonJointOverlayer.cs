﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonJointOverlayer : MonoBehaviour
{
    private com.rfilkov.components.SkeletonOverlayer skeleton;
    private com.rfilkov.components.SkeletonFromPast skeleton_past;
    private DetectPilot detectPilot;
    private GameObject[] joints;
    public int trackedJointInt;
    public Vector3 offset;
    public bool Pilot = false;
    private MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        skeleton = com.rfilkov.components.SkeletonOverlayer.instance;
        skeleton_past = com.rfilkov.components.SkeletonFromPast.instance;
        detectPilot = DetectPilot.instance;
        if (Pilot) mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (skeleton_past.historyRepeating)
        {
            joints = skeleton_past.joints;
            if (mr) mr.material.SetColor("_ASEOutlineColor", Color.black);
        }
        else
        {
            joints = skeleton.joints;

            if (mr) mr.material.SetColor("_ASEOutlineColor", Color.white);
        }
        transform.position = joints[trackedJointInt].transform.position + offset;
        if (Pilot) detectPilot.pilotPos = transform.position;
    }
}
