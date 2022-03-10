using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndFollowTransform : MonoBehaviour
{
    // The target marker.
    public Transform target;
    public TrailRenderer tr;
    public Color inside;
    public Color outside;
    public Material m;
    public bool randgen;
    // Angular speed in radians per sec.
    public float speed = 1.0f;
    private float timer;
    public float idleSpeed;
    public float rotationBias;
    public float moveFreq;
    private void Start()
    {
        timer = Random.Range(-2f, 1f);
        m = tr.material;
        if (randgen) {
            tr.time = Random.Range(1f, 4f);
            speed = Random.Range(1f, 3f);
            idleSpeed = Random.Range(0.5f, 2f);
            rotationBias = Random.Range(-0.8f, 0.8f);
            moveFreq = Random.Range(0.5f, 2f);
            inside = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            m.SetColor("inside",inside);
        }
    }
    void Update()
    {
        timer += Time.deltaTime* moveFreq;
        // Draw a ray pointing at our target in
        //Debug.DrawRay(transform.position, newDirection, Color.red);

        float translateMagnitude = speed * Mathf.Sin(timer);
        if (translateMagnitude > rotationBias*speed) transform.position += transform.forward * Mathf.Clamp(translateMagnitude,idleSpeed,speed) * Time.deltaTime;
        else
        {
            // Determine which direction to rotate towards
            Vector3 targetDirection = target.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = speed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.position += transform.forward * idleSpeed * Time.deltaTime;
        }
    }
}
