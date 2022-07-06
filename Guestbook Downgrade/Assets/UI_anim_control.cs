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
    public Text orange_visitor_ko;
    public Text blue_visitor;
    public Text blue_visitor_ko;
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
        if (phase == 2 && kinectManager.user_tracked_dur >= 2f) LoadingUser();
    }
    //kinect detects user
    //go to stop 1
    public bool userDetected = false;
    public bool cellLoaded = false;
    public void UserDetected() {
        if (userDetected) return;
        //Debug.Log("User Detected");

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
        if (phase != 2 && loadingUser) return;
        anim.SetFloat("speed", 1f);
        loadingUser = true;

    }
    public void Start_anim()
    {
        anim.SetFloat("speed", 1f);
        //anim.StartPlayback();
    }

    public void SpawnCell()
    {
        //Debug.Log("Spawn");
        detectPilot.SpawnCell(1, 0f);
    }


    public void Start_phase() {
        anim.SetFloat("speed", 0f);
        cellLoaded = false;
        loadingUser = false;
        phase = 1;
        kinectManager.maxTrackedUsers = 5;
        LogVisualizer.instance.BottomLog("Start Phase");
    }
    public void Loadingbar_phase()
    {
        if (anim.GetFloat("speed") > 0f && phase != 6)
        {
            anim.SetFloat("speed", 0f);
            detectPilot.GetNewCell();
            phase = 2;
            string vn = detectPilot.currentCell_CellCtrl.cell_id.ToString().PadLeft(4, '0');
            string visitor_string = "HELLO,\nVISITOR #" + vn;
            loading_visitor.text = visitor_string;
            string visitor_string_ko = "안녕하세요,\n#" + vn + " 방문자님!";
            loading_visitor_ko.text = visitor_string_ko;
            string orange_string = "ANALZING\nVISITOR #" + vn;
            orange_visitor.text = orange_string;
            string orange_string_ko = "#" + vn + " 방문자님을\n분석중입니다!";
            orange_visitor_ko.text = orange_string_ko;
            string blue_string = "I'M THE SOFT CELL\nOF VISITOR #" + vn;
            blue_visitor.text = blue_string;
            string blue_string_ko = "저는 #" + vn + " 방문자님의\n소프트 셀입니다!";
            blue_visitor_ko.text = blue_string_ko;
            LogVisualizer.instance.BottomLog("Loading Bar");
        }
    }
    public void Orangebubble_phase() {
        if (anim.GetFloat("speed") > 0f && phase != 6)
        {
            anim.SetFloat("speed", 0f);
            cellLoaded = true;
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
            phase = 5;
        CancelInvoke();
        cellLoaded = false;
        userDetected = false;
        LogVisualizer.instance.BottomLog("End Phase");
        }
    }
    public void End_phase() {
        phase = 6;
        Start_anim();
        loadingUser = false;
        DetectPilot.instance.attachToSpawner = false;
        kinectManager.maxTrackedUsers = 0;
    }
}
