using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_button : MonoBehaviour {

    public Material x_mat;
    public float x_fill;
    private void Start()
    {
        
    }
    private void Update()
    {
        x_mat.SetFloat("X_Fill", x_fill);
        //x_fill /= 2f;
    }

    private void OnCollisionStay(Collision col)
    {
        x_fill += 1f;
    }
}