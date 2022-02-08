using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSystem : MonoBehaviour
{
    public Transform Runner;
    public float speed;
    public GameObject NodeParent;
    public List<Vector3> SubNodes;
    private int SubNodeIndex;
    // Start is called before the first frame update
    void Start()
    {
        CreateNodes();
        //CreateSubNodes(10);
        //SmoothSubNodes(10);
        SubNodeIndex = 0;
    }

    private void Update()
    {
        Vector3 targetNode = SubNodes[SubNodeIndex];

        if ((Runner.position - targetNode).magnitude < 0.1f) SubNodeIndex += 1;
        if (SubNodeIndex == SubNodes.Count) SubNodeIndex = 0;
        Runner.LookAt(targetNode);
        Runner.position += Runner.forward * Time.deltaTime * speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Runner.position, Vector3.one);
        int nodeCount = SubNodes.Count;
        for (int i = 0; i < nodeCount; i++) {
            int start = i;
            int end = (i + 1) % nodeCount;
            Gizmos.DrawLine(SubNodes[start], SubNodes[end]);
            Gizmos.DrawLine(SubNodes[start], SubNodes[start]+Vector3.up*0.1f);
        }
    }
    void CreateNodes() {
        int childCount = NodeParent.transform.childCount;
        for (int i = 0; i < childCount; i++) {
            SubNodes.Add(NodeParent.transform.GetChild(i).position);
        }
        Debug.Log("Create Nodes.");
    } 
    void CreateSubNodes(int SubnodeDiv) {
        List<Vector3> SubNodes_temp = new List<Vector3>();
        for (int i = 0; i < SubNodes.Count; i++)
        {
            int start = i;
            int end = (i + 1)% SubNodes.Count;
            for(int j =0; j < SubnodeDiv; j++)
            {
                Vector3 SubnodeStart = SubNodes[start];
                Vector3 SubnodeEnd = SubNodes[end];

                Vector3 SubnodePos = Vector3.Lerp(SubnodeStart, SubnodeEnd, (j / (float)SubnodeDiv));
                SubNodes_temp.Add(SubnodePos);
            }
        }
        SubNodes = SubNodes_temp;
    }
    void SmoothSubNodes(int iterations) {
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < SubNodes.Count; j++)
            {
                int end = j+1;
                if (end == SubNodes.Count) end = 0;
                SubNodes[j] = SubNodes[j] + (SubNodes[end] - SubNodes[j]) * 0.1f;
            }
        }
    }
}
