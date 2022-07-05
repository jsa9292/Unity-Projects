using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHeadCtrl : MonoBehaviour
{
    private com.rfilkov.components.SkeletonOverlayer skeleton;
    private com.rfilkov.components.SkeletonFromPast skeleton_past;
    private DetectPilot dp;
    private GameObject[] joints;
    public int trackedSkeleton;
    public Vector3 offset;
    public bool Pilot = false;
    private MeshRenderer mr;
    public Vector3[] headPos;
    public static SkeletonHeadCtrl instance;
    // Start is called before the first f
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        skeleton = com.rfilkov.components.SkeletonOverlayer.instance;
        skeleton_past = com.rfilkov.components.SkeletonFromPast.instance;
        dp = DetectPilot.instance;
        if (Pilot) mr = GetComponent<MeshRenderer>();
    }

    void LateUpdate()
    {
        if (skeleton_past.historyRepeating)
        {
            if (mr) mr.material.SetColor("_ASEOutlineColor", Color.black);
            trackedSkeleton = 0;
            if(!skeleton_past.showSkeleton) transform.position = headPos[trackedSkeleton] + offset;
            else transform.position = Vector3.zero;

        }
        else
        {
            if (mr) mr.material.SetColor("_ASEOutlineColor", Color.white);
            float min_dist2booth = 999;
            trackedSkeleton = -1;
            for (int i = 1; i < headPos.Length; i++) {
                float dist2booth = (Vector3.ProjectOnPlane(dp.transform.position, Vector3.up) - Vector3.ProjectOnPlane(headPos[i], Vector3.up)).magnitude;
                if (dist2booth < min_dist2booth) {
                    trackedSkeleton = i;
                    min_dist2booth = dist2booth;
                }
            }
            transform.position = headPos[trackedSkeleton] + offset;

        }
        if (Pilot) dp.pilotPos = transform.position;
    }
}
