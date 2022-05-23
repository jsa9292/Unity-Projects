using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;

public class DetectPilot : MonoBehaviour
{
    public static DetectPilot instance;
    public Vector3 pilotPos;
    public bool pilot_inside;
    public float pilot_r;
    public float pilot_timer = 0f;
    public GameObject cell_prefab;
    public GameObject currentCell;
    public CellCtrl currentCell_CellCtrl;
    public float cell_y_offset;

    private com.rfilkov.components.SkeletonOverlayer skeleton;
    private com.rfilkov.components.SkeletonFromPast skeleton_past;
    private CellSpawner spawner;
    private KinectManager kinectManager;
    private UI_anim_control uac;
    public bool attachToSpawner;
    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        kinectManager = KinectManager.Instance;
        skeleton = com.rfilkov.components.SkeletonOverlayer.instance;
        skeleton_past = com.rfilkov.components.SkeletonFromPast.instance;
        spawner = CellSpawner.instance;
        uac = UI_anim_control.instance;
        //currentCell = GameObject.Instantiate(cell_prefab);
    }

    private void Update()
    {
        if (pilotPos == Vector3.zero) return;
        pilot_inside = (transform.position - pilotPos).magnitude < pilot_r;
        if (pilot_inside && uac.phase != 6)
        {
            uac.UserDetected();
            pilot_timer += Time.deltaTime;
        }
        else
        {
            if(uac.phase == 5) uac.End_phase();
            pilot_timer -= Time.deltaTime;
        }

        cell_y_offset += uac.cellLoaded ? -0.05f : 0.1f;
        cell_y_offset = Mathf.Clamp(cell_y_offset, 0f, 3f);


    }
    private void LateUpdate()
    {
        if (currentCell)
        {
            currentCell.transform.position += Vector3.up * cell_y_offset;
            //Debug.Log(currentCell.transform.position);
            if (attachToSpawner)
            {
                currentCell.transform.position = spawner.transform.position;
                currentCell.transform.localEulerAngles += Vector3.up * 10f * Time.deltaTime;
            }
        }
    }
    private float[] features;
    public void GetNewCell() {
        attachToSpawner = false;
        currentCell = Instantiate(cell_prefab);
        currentCell_CellCtrl = currentCell.GetComponent<CellCtrl>();
        if (skeleton_past.historyRepeating) features = skeleton_past.features;
        else features = skeleton.features;
        currentCell_CellCtrl.Setup_Cell(features);
    }
    public void SpawnCell(int count, float interval) {
        //parameters for spawning
        
        spawner.cell_finished = currentCell;
        currentCell_CellCtrl.ReadyForSpawning();
        attachToSpawner = true;
        spawner.sharedMesh = currentCell_CellCtrl.ectoplasm_mesh;
        spawner.count = count;
        spawner.interval = interval;
        
    }
}
