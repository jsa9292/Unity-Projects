using UnityEngine;
using System.Collections;
using com.rfilkov.kinect;
using System.IO;


namespace com.rfilkov.components
{
    /// <summary>
    /// SkeletonOverlayer overlays the the user's body joints and bones with spheres and lines.
    /// </summary>
    public class SkeletonFromPast : MonoBehaviour
    {
        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;
        public string filepath;
        [Tooltip("Game object used to overlay the joints.")]
        public GameObject jointPrefab;

        [Tooltip("Line object used to overlay the bones.")]
        public LineRenderer linePrefab;
        //public float smoothFactor = 10f;

        //public UnityEngine.UI.Text debugText;

        public GameObject[] joints = null;
        private LineRenderer[] lines = null;
        public float[] features;

        // initial body rotation
        private Quaternion initialRotation = Quaternion.identity;

        // background rectangle
        private Rect backgroundRect = Rect.zero;

        public static SkeletonFromPast instance;
        public Vector3 pilotPos;
        public bool historyRepeating;
        private KinectManager kinectManager;
        private UI_anim_control uac;
        private int jointsCount;
        private void Awake()
        {
            instance = this;
            features = new float[23];
        }
        private StreamReader sr;
        void Start()
        {

            jointsCount = 23;
            kinectManager = KinectManager.Instance;
            uac = UI_anim_control.instance;
            sr = new StreamReader(Application.streamingAssetsPath + "/" + filepath);
            if (jointPrefab)
                {
                    // array holding the skeleton joints
                    joints = new GameObject[jointsCount];

                    for (int i = 0; i < joints.Length; i++)
                    {
                        joints[i] = Instantiate(jointPrefab) as GameObject;
                        joints[i].transform.parent = transform;
                        joints[i].name = ((KinectInterop.JointType)i).ToString();
                        joints[i].SetActive(true);
                    }
                }

                // array holding the skeleton lines
                lines = new LineRenderer[jointsCount];

            // always mirrored
            initialRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            historyRepeating = false;
        }
        private string[] dataString;
        private string[] data;
        private string[] data_pos_string;
        private Vector3 data_pos;
        public Transform sensorTransform;
        private void FixedUpdate()
        {
            if (kinectManager.IsUserDetected(playerIndex)&& uac.phase == 1)
            {
                historyRepeating = false;
            }
            else
            {
                dataString = sr.ReadLine().Split(';');
                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i].transform.position = Vector3.zero;
                }
                for (int i = 0; i < dataString.Length; i++)
                {
                    data = dataString[i].Split(':');
                    try
                    {
                        int data_i = int.Parse(data[0]);
                        data_pos_string = data[1].Split(',');
                        float x = float.Parse(data_pos_string[0]);
                        float y = float.Parse(data_pos_string[1]);
                        float z = float.Parse(data_pos_string[2]);
                        data_pos = new Vector3(x, y, z);
                        if (sensorTransform)
                        {
                            data_pos = sensorTransform.TransformPoint(data_pos);
                        }
                        joints[data_i].transform.position = data_pos;

                        historyRepeating = true;


                    }
                    catch
                    {
                        //Debug.Log(data[0]);
                    }
                }
                for (int i = 0; i < 23; i++)
                {
                    if (lines[i] == null && linePrefab != null)
                    {
                        lines[i] = Instantiate(linePrefab) as LineRenderer;
                        lines[i].transform.parent = transform;
                        lines[i].gameObject.SetActive(false);
                    }

                    int jointParent = (int)kinectManager.GetParentJoint((KinectInterop.JointType)i);
                    Vector3 line_start = joints[i].transform.position;
                    Vector3 line_end = joints[jointParent].transform.position;
                    if (i == 1) pilotPos = line_end;
                    if (line_start != Vector3.zero && line_end != Vector3.zero)
                    {
                        lines[i].gameObject.SetActive(true);
                        features[i] = (line_start - line_end).magnitude;
                        lines[i].SetPosition(0, line_start);
                        lines[i].SetPosition(1, line_end);
                    }
                }
                if (sr.Peek() < 0)
                {
                    sr.Close();
                    historyRepeating = false;
                }
            }
        }


    }
}
