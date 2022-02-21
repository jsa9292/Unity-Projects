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
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass -= Vector3.up * 0.5f;
        target_pos = transform.parent.position;
        timer = Random.Range(0f, 15f);
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
        if (transform.position.y < -4f) forceSum = Vector3.up;
        if (transform.position.y > 4f) forceSum = -Vector3.up;
        if (transform.position.x < -2f) forceSum = Vector3.left;
        if (transform.position.x > 2f) forceSum = -Vector3.left;

        rb.AddForce(forceSum);
        rb.drag = (Mathf.Sin(Time.realtimeSinceStartup/10f)+1)/2f;
    }
}
