using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineScaler : MonoBehaviour
{
    private SplineMesh.Spline spline { get => GetComponent<SplineMesh.Spline>(); }
    public float inc_speed;
    public float dec_speed;
    public float dist = 5f;
    public float heightScale;
    public List<float> hits;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spline.nodes.Count; i++)
        {
            hits[i] = 1f;
        }
    }
    private void Update()
    {
        for (int i = 0; i < spline.nodes.Count; i++)
        {
            Vector2 delta = (new Vector2(1 + hits[i]*heightScale, 1f) - spline.nodes[i].Scale) * Time.deltaTime;
            float speed = delta.x > 0 ? inc_speed : dec_speed;
            spline.nodes[i].Scale += delta* speed;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        for (int i = 0; i < spline.nodes.Count; i++)
        {
            //spline.nodes[i].Scale += new Vector2(1f, 0f);
            RaycastHit hit;
            Vector3 start = transform.position + spline.nodes[i].Position;
            if (Physics.Raycast(start, Vector3.up, out hit, dist)) {

                Debug.DrawRay(start, Vector3.up * hit.distance, Color.yellow);
                hits[i] = hit.distance;

            }
            else
            {
                Debug.DrawRay(start, Vector3.up * dist, Color.white);
                hits[i] = 0f;
            }
        }
    }
}
