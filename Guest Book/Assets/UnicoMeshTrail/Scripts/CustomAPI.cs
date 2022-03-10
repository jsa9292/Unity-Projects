using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAPI : MonoBehaviour
{
    private MeshTrailDrawer mtd;
    public float intervalTime = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        mtd = GetComponent<MeshTrailDrawer>();
        StartCoroutine(IntervalDrawing());
    }

    // Update is called once per frame
    IEnumerator IntervalDrawing()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalTime);
            mtd.EndDrawing();
            yield return new WaitForSeconds(intervalTime);
            mtd.StartDrawing();
        }
    }
}
