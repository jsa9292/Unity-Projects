using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class EmissionManager : MonoBehaviour
{
    public ParticleSystem ps;
    public string savingFile;
    public Image img;
    public MeshRenderer mr;
    public FaceTrackerExample.FaceTrackerARExample fte;
    // Start is called before the first frame update
    void Start()
    {
        img.material = mr.material;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
