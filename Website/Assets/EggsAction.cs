using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsAction : MonoBehaviour
{
    private Vector3 target_pos;
    public float magnitude;
    public float noise_magnitude;
    private Rigidbody rb;
    private float timer;
    private Vector3 initialPos;
    private Vector3 initialRot;
    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass -= Vector3.up * 0.5f;
        target_pos = transform.parent.position;
        timer = Random.Range(0f, 15f);
        initialPos = transform.position;
        initialRot = transform.localEulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        Vector3 direction;
        if (timer % 15f < 12f)
            direction = (target_pos - transform.position).normalized;
        else direction = -Vector3.up;
        Vector3 noise = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        Vector3 forceSum = direction * magnitude + noise * noise_magnitude;

        rb.AddForce(forceSum);
    }
    private void OnEnable()
    {
        transform.position = initialPos;
        transform.localEulerAngles = initialRot;
    }
}
