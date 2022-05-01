using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBuilder : MonoBehaviour
{
    public Transform TrackFollower;
    public Transform FollowerFollower;
    public float node_r;
    public float threshold;
    public float speed;
    [Range(0,1f)]
    public float catchUp;
    public Transform[] Nodes;
    private Transform NextNode;
    public int start_i;
    public int current_i;
    // Start is called before the first frame update
    private void Start()
    {
        current_i = start_i;
        NextNode = Nodes[current_i];
        TrackFollower.position = NextNode.position;
        FollowerFollower.position = TrackFollower.position;
    }
    
    private void Update()
    {
        if ((TrackFollower.position - NextNode.position).magnitude < threshold)
        {
            current_i = (current_i + 1) % Nodes.Length;
            NextNode = Nodes[current_i];
        }
        TrackFollower.LookAt(NextNode,Vector3.up);
        TrackFollower.position += TrackFollower.forward * speed * Time.deltaTime;

        FollowerFollower.rotation = Quaternion.Lerp(FollowerFollower.rotation, TrackFollower.rotation,catchUp);
        FollowerFollower.position += (TrackFollower.position - FollowerFollower.position) * catchUp;
    }
    private void OnDrawGizmos()
    {
        if (Nodes.Length == 0)
        {
            Nodes = transform.GetComponentsInChildren<Transform>();
        }
        else if (Nodes.Length > 2)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(TrackFollower.position, Vector3.one*node_r);
            Vector3 line_start;
            Vector3 line_end;
            int line_end_i;
            for (int i = 0; i < Nodes.Length; i++)
            {
                line_start = Nodes[i].position;
                Gizmos.DrawWireSphere(line_start, node_r);

                line_end_i = (i + 1) % Nodes.Length;
                line_end = Nodes[line_end_i].position;
                Gizmos.DrawLine(line_start, line_end);

            }
        }
    }
}
