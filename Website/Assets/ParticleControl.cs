using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    public ParticleSystem ps;
    private ParticleSystem.ShapeModule ps_sm;
    public bool particle_off;
    public float particle_duration;
    public float change_duration;
    public float change_speed;
    public float rotation_speed;
    public int scan_index;
    public List<Mesh> mesh_list;
    public List<Texture2D> texture_list;
    // Start is called before the first frame update
    void Start()
    {
        ps_sm = ps.shape;
    }

    private float timer;
    private bool changing_out;
    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += Vector3.up * rotation_speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > particle_duration || particle_off)
        {
            changing_out = true;
        }
        else {
            changing_out = false;
        }

        if(timer > particle_duration + change_duration && !particle_off)
        {
            timer = 0;
            scan_index += 1;
            scan_index = scan_index % 4;
            ps_sm.mesh = mesh_list[scan_index];
            setTexture(texture_list[scan_index]);

        } 
        


        if (changing_out) ps_sm.normalOffset += Time.deltaTime * change_speed;
        else ps_sm.normalOffset -= Time.deltaTime * change_speed;

        ps_sm.normalOffset = Mathf.Clamp(ps_sm.normalOffset, 0, change_duration * change_speed);
    }
    public void setTexture(Texture2D tex)
    {
        ps_sm.texture = tex;

    }
}
