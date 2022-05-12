using System.Collections;
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
    public GameObject go;
    public Vector3 rand_dir;
    private float juice;
    private void Update()
    {
        if (count > 0)
        {
            timer += Time.deltaTime; 
            if (cell_finished && timer > interval)
            {
                timer -= interval;
                count--;
                go = GameObject.Instantiate(cell_finished, transform.position, DetectPilot.instance.currentCell.transform.rotation);
                if (sharingMesh) go.transform.GetChild(1).GetComponent<MeshFilter>().sharedMesh = sharedMesh;
                rand_dir = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
                juice = noiseMag;
                RuntimeText.WriteString(System.DateTime.Now.ToString()+","+transform.position+","+rand_dir.ToString() + "," + go.GetComponent<CellCtrl>().save_string);       
            }


        }
        else
        {
            cell_finished = null;
            timer = 0;
            count = 0;
            interval = 0;
        }

        if (go)
        {
            go.transform.Translate(rand_dir * juice * Time.deltaTime);
            go.transform.localEulerAngles += rand_dir* juice * Time.deltaTime;
            juice -= Time.deltaTime;
        }
    }
}
