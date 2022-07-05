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

    private SkeletonJointOverlayer[] sjols;
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
    public void Setup_Cell(float[] features) {
        cell_id = cell_count;
        cell_count += 1;

        float sensitivity = 100f;
        dna_colors[0] = features[9] * sensitivity; // Random.Range(0, 1f);
        dna_colors[1] = (features[1])* sensitivity; // Random.Range(0, 1f);
        dna_colors[2] = (features[2])* sensitivity; // Random.Range(0, 1f);
        n_color = (features[3])* sensitivity; // Random.Range(0, 1f);
        e_color = (features[4])* sensitivity; //
        e_extrusion = (features[5])* sensitivity; // Random.Range(0, 1f);
        axis_x = (features[6])* sensitivity; // Random.Range(0, 1f);
        axis_y = (features[7])* sensitivity; // Random.Range(0, 1f);
        axis_z = (features[8])*sensitivity; // Random.Range(0, 1f);

        //audio
        types[0] = ((int)Mathf.Floor(features[11]*sensitivity))%43+21;
        types[1] = ((int)Mathf.Floor(features[12] * sensitivity))%43+21;
        types[2] = ((int)Mathf.Floor(features[13] * sensitivity))%43+21;
        //Debug.Log(types[0].ToString()+","+ types[1].ToString()+","+types[2].ToString());
        //video
        qs[0] = Random.rotationUniform;
        qs[1] = Random.rotationUniform;
        qs[2] = Random.rotationUniform;

        sjols = GetComponentsInChildren<SkeletonJointOverlayer>();
        for (int i = 1; i < sjols.Length; i++)
        {
            sjols[i].trackedJointInt = (int) Mathf.Floor((features[i]*sensitivity)%23); // this is upto foot right without facials;
            sjols[i].offset = new Vector3(features[i], features[i + 1], features[i + 2]);
        }

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
        ectoplasm_mr.material.SetFloat("_ExtrusionPoint", (e_extrusion % 0.05f)+0.02f);

        ectoplasm_mr.material.SetFloat("_Axis_x", (axis_x % 1) );
        ectoplasm_mr.material.SetFloat("_Axis_y", (axis_y % 1) );
        ectoplasm_mr.material.SetFloat("_Axis_z", (axis_z % 1) );

    }

    public void ReadyForSpawning() {
        //disable jols
        sjols = GetComponentsInChildren<SkeletonJointOverlayer>();
        for (int i = 0; i < sjols.Length; i++) {
            Destroy(sjols[i]);
        }
        //disable mcblob
        ectoplasm_mesh = ectoplasm_mr.GetComponent<MeshFilter>().sharedMesh;
        Destroy(mcb);
        cv.enabled = false;

        gameObject.isStatic = true;
    }
}
