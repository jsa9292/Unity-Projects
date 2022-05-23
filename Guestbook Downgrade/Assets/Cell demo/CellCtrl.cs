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

    public static int cell_count;
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

    public void Setup_Cell(float[] data) {
        cell_id = cell_count;
        cell_count += 1;
        skeleton = com.rfilkov.components.SkeletonOverlayer.instance;

        //audio
        types[0] = (int) data[7];
        types[1] = (int) data[8];
        types[2] = (int) data[9];
        //video
        qs[0] = new Quaternion(data[10], data[11], data[12], data[13]);
        qs[1] = new Quaternion(data[14], data[15], data[16], data[17]);
        qs[2] = new Quaternion(data[18], data[19], data[20], data[21]);
        
        dna_colors[0] = data[22]; // Random.Range(0, 1f);
        dna_colors[1] = data[23]; // Random.Range(0, 1f);
        dna_colors[2] = data[24]; // Random.Range(0, 1f);
        n_color = data[25]; // Random.Range(0, 1f);
        e_color = data[26]; //
        e_extrusion = data[27]; // Random.Range(0, 1f);
        axis_x = data[28]; // Random.Range(0, 1f);
        axis_y = data[29]; // Random.Range(0, 1f);
        axis_z = data[30]; // Random.Range(0, 1f);

        Update_Cell_Graphics();
    }
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

        float sensitivity = 1000f;
        dna_colors[0] = skeleton.features[9] * sensitivity; // Random.Range(0, 1f);
        dna_colors[1] = (skeleton.features[1])* sensitivity; // Random.Range(0, 1f);
        dna_colors[2] = (skeleton.features[2])* sensitivity; // Random.Range(0, 1f);
        n_color = (skeleton.features[3])* sensitivity; // Random.Range(0, 1f);
        e_color = (skeleton.features[4])* sensitivity; //
        e_extrusion = (skeleton.features[5])* sensitivity; // Random.Range(0, 1f);
        axis_x = (skeleton.features[6])* sensitivity; // Random.Range(0, 1f);
        axis_y = (skeleton.features[7])* sensitivity; // Random.Range(0, 1f);
        axis_z = (skeleton.features[8])*sensitivity; // Random.Range(0, 1f);

        ////audio
        //save_string += types[0].ToString() + ",";
        //save_string += types[1].ToString() + ",";
        //save_string += types[2].ToString() + ",";
        ////video
        //save_string += qs[0].ToString() + ",";
        //save_string += qs[1].ToString() + ",";
        //save_string += qs[2].ToString() + ",";
        //save_string += dna_colors[0].ToString() + ",";
        //save_string += dna_colors[1].ToString() + ",";
        //save_string += dna_colors[2].ToString() + ",";
        //save_string += n_color.ToString() + ",";
        //save_string += e_color.ToString() + ",";
        //save_string += e_extrusion.ToString() + ",";
        //save_string += axis_x.ToString() + ",";
        //save_string += axis_y.ToString() + ",";
        //save_string += axis_z.ToString();

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
        nucleus_mr.material.SetColor("_Color", Color.HSVToRGB((n_color % 1), 1f, 1f));
        ectoplasm_mr.material.SetColor("_Color", Color.HSVToRGB((e_color % 1), 1f, 1f));
        ectoplasm_mr.material.SetFloat("_ExtrusionPoint", (e_extrusion % 1)*2 + 0.01f);
        Debug.Log((e_color % 1 + 1) / 2);
        Debug.Log((e_color ));

        ectoplasm_mr.material.SetFloat("_Axis_x", (axis_x % 1) / 10f);
        ectoplasm_mr.material.SetFloat("_Axis_y", (axis_y % 1) / 10f);
        ectoplasm_mr.material.SetFloat("_Axis_z", (axis_z % 1) / 10f);

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
