using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndFollowTransform : MonoBehaviour
{
    // The target marker.
    public Transform target;
    public TrailRenderer tr;
    // Angular speed in radians per sec.
    public float speed = 1.0f;
    private float timer;
    public float rotationBias;
    public float moveFreq;
    private void Start()
    {
        timer = Random.Range(-2f, 1f);
        flockGuiding fG = transform.parent.parent.GetComponent<flockGuiding>();
        tr.material = fG.mat;
        speed = fG.cellSpeed;
        rotationBias = fG.cellRotationBias;
        moveFreq = fG.cellMoveFreq;
        tr.time = fG.cellBodyLength;
    }
    void Update()
    {
        timer += Time.deltaTime* moveFreq;
        // Draw a ray pointing at our target in
        //Debug.DrawRay(transform.position, newDirection, Color.red);

        float translateMagnitude = speed * Mathf.Sin(timer);

        if (translateMagnitude > rotationBias * speed)
        {
            Vector3 pos_delta = transform.forward * Mathf.Clamp(translateMagnitude, 0, speed) * Time.deltaTime;
            transform.position += new Vector3(pos_delta.x,pos_delta.y,0);
        }
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
        }
    }
}
