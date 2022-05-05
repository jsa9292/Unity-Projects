using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;

public class DetectPilot : MonoBehaviour
{
    public static DetectPilot instance;
    private Vector3 leftHand;
    private Vector3 rightHand;
    private Vector3 pilotPos;
    public float pilot_r;
    public float hand_r;
    public bool testing;
    public bool pilot_inside;
    public GameObject cell_prefab;
    public GameObject currentCell;
    public CellCtrl currentCell_CellCtrl;
    public float cell_y_offset;

    private com.rfilkov.components.SkeletonOverlayer skeleton;
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
        spawner = CellSpawner.instance;
        uac = UI_anim_control.instance;
        //currentCell = GameObject.Instantiate(cell_prefab);
    }

    private void Update()
    {
        leftHand = skeleton.leftHand;
        rightHand = skeleton.rightHand;
        pilotPos = skeleton.pilotPos;
        
        pilot_inside = (transform.position - pilotPos).magnitude < pilot_r;
        if (pilot_inside && uac.phase != 6)
        {
            uac.UserDetected();
        }
        else
        {
            if(uac.phase == 5) uac.End_phase();
        }

        cell_y_offset += uac.cellLoaded ? -0.05f : 0.1f;
        cell_y_offset = Mathf.Clamp(cell_y_offset, 0f, 3f);


    }
    private void LateUpdate()
    {
        if (currentCell)
        {
            currentCell.transform.position += Vector3.up * cell_y_offset;
            Debug.Log(currentCell.transform.position);
            if (attachToSpawner)
            {
                currentCell.transform.position = spawner.transform.position;
                currentCell.transform.localEulerAngles += Vector3.up * 10f * Time.deltaTime;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = pilot_inside ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, pilot_r);

        Gizmos.DrawSphere(leftHand, hand_r / 2);
        Gizmos.DrawSphere(rightHand, hand_r / 2);
        Gizmos.DrawSphere(pilotPos, hand_r / 2);
        
    }
    public void GetNewCell() {
        attachToSpawner = false;
        currentCell = Instantiate(cell_prefab);
        currentCell_CellCtrl = currentCell.GetComponent<CellCtrl>();
        currentCell_CellCtrl.Setup_Cell();
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
