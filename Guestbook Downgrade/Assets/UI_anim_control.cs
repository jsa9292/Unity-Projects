using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_anim_control : MonoBehaviour
{
    public static UI_anim_control instance;

    public Animator anim;
    public AnimationClip ac;
    public int phase;
    public float spawning_dur = 0f;
    public float spawning_freq = 1f;
    public Camera cam;
    public Text orange_visitor;
    public Text blue_visitor;
    public Text loading_visitor;
    public Text loading_visitor_ko;
    private com.rfilkov.kinect.KinectManager kinectManager;
    private DetectPilot detectPilot;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Application.targetFrameRate = 60;
        ac = anim.runtimeAnimatorController.animationClips[0];
        kinectManager = com.rfilkov.kinect.KinectManager.Instance;
        detectPilot = DetectPilot.instance;
        phase = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (phase == 2 && kinectManager.user_tracked_dur > 2f) LoadingUser();
    }
    //kinect detects user
    //go to stop 1
    public bool userDetected = false;
    public bool cellLoaded = false;
    public void UserDetected() {
        if (userDetected) return;
        Debug.Log("User Detected");

        phase = 2;
        ac.SampleAnimation(gameObject, 0f);
        anim.SetFloat("speed", 1f);
        userDetected = true;
        kinectManager.user_tracked_dur = 0f;

    }
    //kinect loses user
    //go to stop 0
    public void UserUndetected() {
    }
    //kinect detected user for 2 seconds
    //loading bar is filled
    //go to stop 2
    //initiate nucleus
    public bool loadingUser = false;
    public void LoadingUser()
    {
        if (loadingUser) return;
        anim.SetFloat("speed", 1f);
        loadingUser = true;

    }
    public void Start_anim()
    {
        anim.SetFloat("speed", 1f);
        //anim.StartPlayback();
    }
    public void LoadCell()
    {

        spawning_dur = Random.Range(5, 10f);
        spawning_freq = Random.Range(1, 2f);
    }

    public void SpawnCell()
    {
        Debug.Log("Spawn");
        detectPilot.SpawnCell(1, 0f);
    }


    public void Start_phase() {
        anim.SetFloat("speed", 0f);
        cellLoaded = false;
        loadingUser = false;
        phase = 1;
        kinectManager.maxTrackedUsers = 1;
        LogVisualizer.instance.BottomLog("Start Phase");
    }
    public void Loadingbar_phase()
    {
        if (anim.GetFloat("speed") > 0f && phase != 6)
        {
            anim.SetFloat("speed", 0f);
            detectPilot.GetNewCell();
            phase = 2;
            string visitor_string = "HELLO,\nVISITOR #" + detectPilot.currentCell_CellCtrl.cell_id.ToString().PadLeft(4, '0'); ;
            orange_visitor.text = visitor_string;
            blue_visitor.text = visitor_string;
            loading_visitor.text = visitor_string;
            loading_visitor_ko.text = "안녕하세요,\n#" + detectPilot.currentCell_CellCtrl.cell_id.ToString().PadLeft(4, '0')+" 방문객님!";

            LogVisualizer.instance.BottomLog("Loading Bar");
        }
    }
    public void Orangebubble_phase() {
        if (anim.GetFloat("speed") > 0f && phase != 6)
        {
            anim.SetFloat("speed", 0f);
            cellLoaded = true;
            StartCoroutine("LoadCell");
            Invoke("Start_anim", 4f);
            phase = 3;
            LogVisualizer.instance.BottomLog("Orange Bubble");
        }
    }
    public void Bluebubble_phase() {
        if (anim.GetFloat("speed") > 0f && phase != 6)
        {
            anim.SetFloat("speed", 0f);
            InvokeRepeating("SpawnCell", 0, spawning_freq);
            Invoke("Start_anim", 3f);
            phase = 4;
            LogVisualizer.instance.BottomLog("Blue Bubble");
        }
    }
    public void Getout_phase() {
        if (anim.GetFloat("speed") > 0f && phase != 6)
        {
            anim.SetFloat("speed", 0f);
            phase = 5;
        }
    }
    public void End_phase() {
        CancelInvoke();
        phase = 6;
        Start_anim();
        cellLoaded = false;
        loadingUser = false;
        userDetected = false;
        DetectPilot.instance.attachToSpawner = false;
        kinectManager.maxTrackedUsers = 0;
        LogVisualizer.instance.BottomLog("End Phase");
    }
}
