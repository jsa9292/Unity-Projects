using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellCtrl : MonoBehaviour
{
    
    public MeshRenderer ectoplasm_mr;
    public MeshRenderer nucleus_mr;
    public Mesh ectoplasm_mesh;
    public MCBlob mcb;

    public Canvas cv;
    public GameObject panel;
    public Text panel_title;
    public Text panel_content;

    private com.rfilkov.components.JointOverlayer[] jols;
    private float offsetMax = 0.3f;

    private static int cell_count;
    //Saving these
    public int cell_id = 0;
    private int[] freqs = new int[3];
    private int[] types = new int[3];
    private Quaternion[] qs = new Quaternion[3];
    private float[] dna_colors = new float[3];
    private float n_color;
    private float e_color;
    private float e_extrusion;
    private float axis_x;
    private float axis_y;
    private float axis_z;

    public string save_string = "";
    private com.rfilkov.components.SkeletonOverlayer skeleton;

    public void Setup_Cell() {
        cell_id = cell_count;
        cell_count += 1;
        skeleton = com.rfilkov.components.SkeletonOverlayer.instance;

        //audio
        types[0] = Random.Range(21, 63);
        types[1] = Random.Range(21, 63);
        types[2] = Random.Range(21, 63);
        //video
        qs[0] = Random.rotationUniform;
        qs[1] = Random.rotationUniform;
        qs[2] = Random.rotationUniform;

        float sensitivity = 10000000;
        dna_colors[0] = skeleton.features[9] * sensitivity; // Random.Range(0, 1f);
        dna_colors[1] = (skeleton.features[1])* sensitivity; // Random.Range(0, 1f);
        dna_colors[2] = (skeleton.features[2])* sensitivity; // Random.Range(0, 1f);
        n_color = (skeleton.features[3])* sensitivity; // Random.Range(0, 1f);
        e_color = (skeleton.features[4])* sensitivity; //
        e_extrusion = (skeleton.features[5])* sensitivity; // Random.Range(0, 1f);
        axis_x = (skeleton.features[6])* sensitivity; // Random.Range(0, 1f);
        axis_y = (skeleton.features[7])* sensitivity; // Random.Range(0, 1f);
        axis_z = (skeleton.features[8])*sensitivity; // Random.Range(0, 1f);

        //audio
        save_string += types[0].ToString() + ",";
        save_string += types[1].ToString() + ",";
        save_string += types[2].ToString() + ",";
        //video
        save_string += qs[0].ToString() + ",";
        save_string += qs[1].ToString() + ",";
        save_string += qs[2].ToString() + ",";
        save_string += dna_colors[0].ToString() + ",";
        save_string += dna_colors[1].ToString() + ",";
        save_string += dna_colors[2].ToString() + ",";
        save_string += n_color.ToString() + ",";
        save_string += e_color.ToString() + ",";
        save_string += e_extrusion.ToString() + ",";
        save_string += axis_x.ToString() + ",";
        save_string += axis_y.ToString() + ",";
        save_string += axis_z.ToString();

        Update_Cell_Graphics();
    }
    public void Update_Cell_Graphics() {
    
        panel_title.text = "Visitor #" + cell_id.ToString().PadLeft(4, '0');
        panel_content.text = "";

        AudioZone[] azs = nucleus_mr.transform.GetComponentsInChildren<AudioZone>();
        for (int i = 0; i < azs.Length; i++)
        {
            azs[i].soundType = types[i];
            azs[i].color = Color.HSVToRGB((Mathf.Cos(dna_colors[i]) + 1) / 2, 1f, 1f);
            azs[i].delay = i * 0.2f;
            azs[i].Start();
            azs[i].transform.position = nucleus_mr.transform.position + Vector3.up * 0.2f;
            nucleus_mr.transform.rotation *= qs[i];
        }
        nucleus_mr.material.SetColor("_Color", Color.HSVToRGB((Mathf.Cos(n_color)+1)/2, 1f, 1f));
        ectoplasm_mr.material.SetColor("_Color", Color.HSVToRGB((Mathf.Cos(e_color) + 1) / 2, 1f, 1f));
        ectoplasm_mr.material.SetFloat("_ExtrusionPoint", (Mathf.Cos(e_extrusion) + 1) / 10f + 0.01f);

        ectoplasm_mr.material.SetFloat("_Axis_x", (Mathf.Cos(axis_x) + 1) / 2);
        ectoplasm_mr.material.SetFloat("_Axis_y", (Mathf.Cos(axis_y) + 1) / 2);
        ectoplasm_mr.material.SetFloat("_Axis_z", (Mathf.Cos(axis_z) + 1) / 2);

        jols = GetComponentsInChildren<com.rfilkov.components.JointOverlayer>();
        for (int i = 1; i < jols.Length; i++)
        {
            jols[i].trackedJointInt = Random.Range(0, 23); // this is upto foot right without facials;
            jols[i].forwardOffset = Random.Range(-offsetMax, offsetMax);
            jols[i].horizontalOffset = Random.Range(-offsetMax, offsetMax);
            jols[i].verticalOffset = Random.Range(-offsetMax, offsetMax);
        }

    }

    public void ReadyForSpawning() {
        //disable jols
        jols = GetComponentsInChildren<com.rfilkov.components.JointOverlayer>();
        for (int i = 0; i < jols.Length; i++) {
            Destroy(jols[i]);
        }
        //disable mcblob
        ectoplasm_mesh = ectoplasm_mr.GetComponent<MeshFilter>().sharedMesh;
        Destroy(mcb);
        cv.enabled = false;

        gameObject.isStatic = true;
    }
}
